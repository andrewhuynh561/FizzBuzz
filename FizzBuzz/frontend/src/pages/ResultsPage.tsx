import React, { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';

function ResultsPage() {
    const { sessionId } = useParams();
    const [correctCount, setCorrectCount] = useState(0);
    const [incorrectCount, setIncorrectCount] = useState(0);
    const [error, setError] = useState("");

    useEffect(() => {
        const fetchResults = async () => {
            if (!sessionId) return;
            try {
                const response = await fetch(`https://localhost:7178/api/Sessions/${sessionId}/results`);
                if (!response.ok) {
                    setError(`Error: ${await response.text()}`);
                    return;
                }
                const data = await response.json();
                setCorrectCount(data.correctCount);
                setIncorrectCount(data.incorrectCount);
            } catch (err) {
                setError(`Error: ${err}`);
            }
        };

        fetchResults();
    }, [sessionId]);

    return (
        <div>
            <h2>Results (Session {sessionId})</h2>
            {error && <p style={{ color: "red" }}>{error}</p>}
            <p>Correct: {correctCount}</p>
            <p>Incorrect: {incorrectCount}</p>
        </div>
    );
}

export default ResultsPage;
