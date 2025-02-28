import { useEffect, useState, useRef } from 'react';
import '../CSS/PlayGamePage.css'; // Optional: style as you wish

/********************************************
 * Interfaces matching your .NET models
 ********************************************/
interface GameRule {
    id?: number;
    divisor: number;
    replacementText: string;
}

interface Game {
    id: number;
    name: string;
    author: string;
    rules: GameRule[];
    // Possibly minNumber, maxNumber, etc. if you store them
}

interface StartSessionResponse {
    sessionId: number;
    endTime: string; // server might return an ISO date string
}

function PlayGamePage() {
    /********************************************
     * State for "select game" phase
     ********************************************/
    const [games, setGames] = useState<Game[]>([]);
    const [selectedGameId, setSelectedGameId] = useState<number | null>(null);
    const [durationSeconds, setDurationSeconds] = useState<number>(60);

    /********************************************
     * State for "session" phase
     ********************************************/
    const [sessionId, setSessionId] = useState<number | null>(null);
    const [endTime, setEndTime] = useState<Date | null>(null);
    const [timeLeft, setTimeLeft] = useState<number>(0);

    /********************************************
     * Gameplay: random number, user input, scoreboard
     ********************************************/
    const [currentNumber, setCurrentNumber] = useState<number | null>(null);
    const [userAnswer, setUserAnswer] = useState("");
    const [correctCount, setCorrectCount] = useState(0);
    const [incorrectCount, setIncorrectCount] = useState(0);

    /********************************************
     * Refs / Timers
     ********************************************/
    const timerRef = useRef<ReturnType<typeof setInterval> | null>(null);



    /********************************************
     * 1) Fetch list of games on mount
     ********************************************/
    useEffect(() => {
        fetch("https://localhost:7178/api/Games")
            .then(async (res) => {
                if (!res.ok) {
                    const text = await res.text();
                    throw new Error(`Error fetching games: ${text}`);
                }
                return res.json();
            })
            .then((data: Game[]) => {
                setGames(data);
                // Optionally auto-select the first game
                if (data.length > 0) {
                    setSelectedGameId(data[0].id);
                }
            })
            .catch((err) => {
                console.error(err);
                alert("Failed to load games. Check console for details.");
            });
    }, []);

    /********************************************
     * 2) Start session
     ********************************************/
    const handleStartSession = async () => {
        if (!selectedGameId) {
            alert("Please select a game first.");
            return;
        }

        try {
            const response = await fetch("https://localhost:7178/api/Sessions/start", {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({
                    gameId: selectedGameId,
                    durationSeconds: durationSeconds,
                }),
            });
            if (!response.ok) {
                const text = await response.text();
                throw new Error(`Start session error: ${text}`);
            }
            const data: StartSessionResponse = await response.json();

            // sessionId & endTime from server
            setSessionId(data.sessionId);
            const end = new Date(data.endTime); // parse ISO string
            setEndTime(end);

            // Initialize scoreboard
            setCorrectCount(0);
            setIncorrectCount(0);

            // Start countdown
            if (timerRef.current) clearInterval(timerRef.current);
            timerRef.current = setInterval(() => {
                updateTimeLeft(end);
            }, 1000);

            // Immediately update timeLeft
            updateTimeLeft(end);

            // Fetch the first random number
            fetchNextNumber(data.sessionId);

        } catch (error) {
            console.error(error);
            alert(String(error));
        }
    };

    /********************************************
     * 3) Update time left
     ********************************************/
    const updateTimeLeft = (end: Date) => {
        const now = new Date();
        const diffMs = end.getTime() - now.getTime(); // milliseconds
        const diffSec = Math.max(0, Math.floor(diffMs / 1000));
        setTimeLeft(diffSec);

        // If time is up, clear the interval
        if (diffSec <= 0 && timerRef.current) {
            clearInterval(timerRef.current);
            timerRef.current = null;
        }
    };

    /********************************************
     * 4) Fetch next random number from server
     ********************************************/
    const fetchNextNumber = async (sessionIdValue: number) => {
        try {
            const response = await fetch(
                `https://localhost:7178/api/Sessions/${sessionIdValue}/next-number`
            );
            if (!response.ok) {
                const text = await response.text();
                throw new Error(`Fetch next number error: ${text}`);
            }
            const data = await response.json();
            setCurrentNumber(data.number);
            setUserAnswer("");
        } catch (error) {
            console.error(error);
            alert(String(error));
        }
    };

    /********************************************
     * 5) Submit answer to server
     ********************************************/
    const handleSubmitAnswer = async () => {
        if (!sessionId || currentNumber === null) return;

        try {
            const response = await fetch(
                `https://localhost:7178/api/Sessions/${sessionId}/answer`,
                {
                    method: "POST",
                    headers: { "Content-Type": "application/json" },
                    body: JSON.stringify({
                        number: currentNumber,
                        userAnswer: userAnswer,
                    }),
                }
            );
            if (!response.ok) {
                const text = await response.text();
                throw new Error(`Submit answer error: ${text}`);
            }
            const data = await response.json();
            // data.correct => boolean, data.expected => string
            if (data.correct) {
                setCorrectCount((prev) => prev + 1);
            } else {
                setIncorrectCount((prev) => prev + 1);
            }

            // Fetch next random number
            fetchNextNumber(sessionId);
        } catch (error) {
            console.error(error);
            alert(String(error));
        }
    };

    /********************************************
     * Cleanup on unmount
     ********************************************/
    useEffect(() => {
        return () => {
            if (timerRef.current) {
                clearInterval(timerRef.current);
            }
        };
    }, []);

    /********************************************
     * Helper: Is the game running or is time up?
     ********************************************/
    const isGameActive = sessionId !== null && timeLeft > 0;

    /********************************************
     * Render
     ********************************************/
    // 1) If no session started, show "Select Game" & "Duration" & "Start"
    // 2) If session started, show the main game UI
    // 3) Display rules from the selected game
    const currentGame = games.find((g) => g.id === selectedGameId);

    return (
        <div className="play-game-page">
            <h1>Play a FizzBuzz-like Game</h1>

            {/* If session not started, let user pick game & duration */}
            {!sessionId && (
                <div className="setup-section">
                    <div>
                        <label>Choose Game:</label>
                        <select
                            value={selectedGameId ?? ""}
                            onChange={(e) => setSelectedGameId(Number(e.target.value))}
                        >
                            {games.map((g) => (
                                <option key={g.id} value={g.id}>
                                    {g.name} by {g.author}
                                </option>
                            ))}
                        </select>
                    </div>
                    <div>
                        <label>Duration (seconds):</label>
                        <input
                            type="number"
                            value={durationSeconds}
                            onChange={(e) => setDurationSeconds(Number(e.target.value))}
                        />
                    </div>
                    <button onClick={handleStartSession}>Start Game</button>
                </div>
            )}

            {/* If a game is selected, show its rules */}
            {currentGame && (
                <div className="rules-box">
                    <h2>Game Rules for {currentGame.name}</h2>
                    <ul>
                        {currentGame.rules.map((rule, idx) => (
                            <li key={idx}>
                                If divisible by {rule.divisor}, say "{rule.replacementText}"
                            </li>
                        ))}
                        <li>
                            If a number is divisible by multiple divisors, concatenate the words!
                        </li>
                    </ul>
                </div>
            )}

            {/* If session is started, show the game UI */}
            {sessionId && (
                <div className="gameplay-section">
                    <div className="timer-display">
                        {timeLeft > 0 ? (
                            <span>{timeLeft} seconds left</span>
                        ) : (
                            <span>Time's up!</span>
                        )}
                    </div>

                    <div className="number-box">
                        {currentNumber !== null ? currentNumber : "--"}
                    </div>

                    <div className="answer-area">
                        <input
                            type="text"
                            value={userAnswer}
                            onChange={(e) => setUserAnswer(e.target.value)}
                            placeholder="Your answer..."
                            disabled={!isGameActive}
                        />
                        <button onClick={handleSubmitAnswer} disabled={!isGameActive}>
                            Submit
                        </button>
                    </div>

                    <div className="scoreboard">
                        <div className="score correct">Correct: {correctCount}</div>
                        <div className="score incorrect">Incorrect: {incorrectCount}</div>
                    </div>
                </div>
            )}
        </div>
    );
}

export default PlayGamePage;
