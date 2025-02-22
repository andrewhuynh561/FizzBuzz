import React, { useEffect, useState } from 'react';
import './App.css';

interface Game {
    id: number;
    name: string;
    author: string;
    createdAt: string;
    // ... any other properties you might have
}

function App() {
    const [games, setGames] = useState<Game[]>([]);
    const [error, setError] = useState<string>("");

    useEffect(() => {
        // Example: Fetch a list of games from the .NET API
        fetch("https://localhost:7178/api/Games")
            .then(async (response) => {
                if (!response.ok) {
                    // If server returned an error code, read the message
                    const message = await response.text();
                    throw new Error(message);
                }
                return response.json();
            })
            .then((data: Game[]) => {
                // data should be an array of games
                setGames(data);
            })
            .catch((err) => {
                setError(err.message || "An error occurred while fetching games.");
            });
    }, []);

    return (
        <div className="App">
            <h1>FizzBuzz React App</h1>

            {error && <p style={{ color: 'red' }}>Error: {error}</p>}

            {games.length > 0 ? (
                <ul>
                    {games.map((game) => (
                        <li key={game.id}>
                            <strong>{game.name}</strong> by {game.author} (Created: {game.createdAt})
                        </li>
                    ))}
                </ul>
            ) : (
                <p>No games found or still loading...</p>
            )}
        </div>
    );
}

export default App;
