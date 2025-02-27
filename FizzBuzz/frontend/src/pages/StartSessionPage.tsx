import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';

function StartSessionPage() {
    const [gameId, setGameId] = useState<number>(0);
    const [duration, setDuration] = useState<number>(60);
    const navigate = useNavigate();

    const handleStart = async () => {
        try {
            const response = await fetch("https://localhost:7178/api/Sessions/start", {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({ gameId, durationSeconds: duration }),
            });
            if (!response.ok) {
                console.error("Failed to start session");
                return;
            }
            const data = await response.json();
            // data.sessionId, data.endTime
            navigate(`/play/${data.sessionId}`);
        } catch (error) {
            console.error(error);
        }
    };

    return (
        <div>
            <h2>Start Session</h2>
            <div>
                <label>Game ID:</label>
                <input
                    type="number"
                    value={gameId}
                    onChange={(e) => setGameId(Number(e.target.value))}
                />
            </div>
            <div>
                <label>Duration (seconds):</label>
                <input
                    type="number"
                    value={duration}
                    onChange={(e) => setDuration(Number(e.target.value))}
                />
            </div>
            <button onClick={handleStart}>Start</button>
        </div>
    );
}

export default StartSessionPage;
