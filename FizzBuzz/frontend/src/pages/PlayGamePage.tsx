import React, { useEffect, useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';

function PlayGamePage() {
    const { sessionId } = useParams();
    const navigate = useNavigate();

    const [currentNumber, setCurrentNumber] = useState<number | null>(null);
    const [userAnswer, setUserAnswer] = useState("");
    const [feedback, setFeedback] = useState<string>("");

    const fetchNextNumber = async () => {
        if (!sessionId) return;
        try {
            const response = await fetch(`https://localhost:7178/api/Sessions/${sessionId}/next-number`);
            if (!response.ok) {
                const text = await response.text();
                setFeedback(`Error: ${text}`);
                return;
            }
            const data = await response.json();
            setCurrentNumber(data.number);
            setUserAnswer("");
            setFeedback("");
        } catch (error) {
            setFeedback(`Error fetching next number: ${error}`);
        }
    };

    const submitAnswer = async () => {
        if (!sessionId || currentNumber == null) return;
        try {
            const response = await fetch(`https://localhost:7178/api/Sessions/${sessionId}/answer`, {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({ number: currentNumber, userAnswer }),
            });
            if (!response.ok) {
                const text = await response.text();
                setFeedback(`Error: ${text}`);
                return;
            }
            const data = await response.json();
            if (data.correct) {
                setFeedback(`Correct! (Expected: ${data.expected})`);
            } else {
                setFeedback(`Incorrect! (Expected: ${data.expected})`);
            }
        } catch (error) {
            setFeedback(`Error submitting answer: ${error}`);
        }
    };

    const endGame = () => {
        if (sessionId) {
            navigate(`/results/${sessionId}`);
        }
    };

    useEffect(() => {
        fetchNextNumber();
    }, []);

    return (
        <div>
            <h2>Play Game (Session {sessionId})</h2>
            <div>
                {currentNumber !== null ? (
                    <p>Random Number: <strong>{currentNumber}</strong></p>
                ) : (
                    <p>No number yet.</p>
                )}
            </div>
            <div>
                <input
                    placeholder="Your Answer"
                    value={userAnswer}
                    onChange={(e) => setUserAnswer(e.target.value)}
                />
                <button onClick={submitAnswer}>Submit</button>
            </div>
            <div>
                <button onClick={fetchNextNumber}>Next Number</button>
                <button onClick={endGame}>End Session</button>
            </div>
            <p>{feedback}</p>
        </div>
    );
}

export default PlayGamePage;
