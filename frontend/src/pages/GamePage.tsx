import React, { useEffect, useState } from 'react';
import '../CSS/GamePage.css';

interface Game {
    id: number;
    name: string;
    author: string;
    minNumber: number;
    maxNumber: number;
    
}

interface Rule {
    divisor: number;
    replacementText: string;
}

function CreateGamePage() {
    
    const [games, setGames] = useState<Game[]>([]);
    const [gameName, setGameName] = useState("");
    const [authorName, setAuthorName] = useState("");
    const [rules, setRules] = useState<Rule[]>([{ divisor: 0, replacementText: "" }]);
    const [minNumber, setMinNumber] = useState(1);
    const [maxNumber, setMaxNumber] = useState(100);

    
    useEffect(() => {
        const fetchGames = async () => {
            try {
                const response = await fetch("https://localhost:7178/api/Games");
                if (!response.ok) {
                    throw new Error(`HTTP error! status: ${response.status}`);
                }
                const data = await response.json();
                setGames(data);
            } catch (error) {
                console.error("Error fetching games:", error);
            }
        };

        fetchGames();
    }, []);

  
    const handleAddRule = () => {
        setRules([...rules, { divisor: 0, replacementText: "" }]);
    };

    const handleRuleChange = (index: number, field: keyof Rule, value: string) => {
        const updatedRules = [...rules];
        if (field === "divisor") {
            updatedRules[index].divisor = Number(value);
        } else {
            updatedRules[index].replacementText = value;
        }
        setRules(updatedRules);
    };

    const handleRemoveRule = (index: number) => {
        const updatedRules = [...rules];
        updatedRules.splice(index, 1);
        setRules(updatedRules);
    };

  
    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        try {
            const response = await fetch("https://localhost:7178/api/Games", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify({
                    name: gameName,
                    author: authorName,
                    rules: rules,
                }),
            });

            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }

            // Refresh the games list
            const updatedGames = await fetch("https://localhost:7178/api/Games").then(res => res.json());
            setGames(updatedGames);
            setGameName("");
            setAuthorName("");
            setRules([]);
        } catch (error) {
            console.error("Error creating game:", error);
        }
    };

    return (
        <div className="create-game-container">
            <h1>Create Your Own FizzBuzz Game!</h1>

            {}
            <div className="existing-games-container" style={{ marginBottom: "2rem" }}>
                <h2>Existing Games</h2>
                {games.length === 0 ? (
                    <p>No games found.</p>
                ) : (
                    <ul>
                        {games.map((g) => (
                            <li key={g.id}>
                                <strong>{g.name}</strong> by {g.author} <br />
                                Range: {g.minNumber} - {g.maxNumber}
                               
                            </li>
                        ))}
                    </ul>
                )}
            </div>

         
            <form className="game-form" onSubmit={handleSubmit}>
                <label className="form-label">Game Name</label>
                <input
                    type="text"
                    className="text-input"
                    value={gameName}
                    onChange={(e) => setGameName(e.target.value)}
                    placeholder="FooBooLoo"
                    required
                />

                <label className="form-label">Your Name</label>
                <input
                    type="text"
                    className="text-input"
                    value={authorName}
                    onChange={(e) => setAuthorName(e.target.value)}
                    placeholder="Game Creator"
                    required
                />

                <label className="form-label">Game Rules</label>
                <div className="rules-container">
                    {rules.map((rule, index) => (
                        <div key={index} className="rule-row">
                            <span>When divisible by</span>
                            <input
                                type="number"
                                className="small-input"
                                value={rule.divisor}
                                onChange={(e) => handleRuleChange(index, "divisor", e.target.value)}
                                placeholder="e.g. 3"
                                required
                            />
                            <span>say</span>
                            <input
                                type="text"
                                className="small-input"
                                value={rule.replacementText}
                                onChange={(e) => handleRuleChange(index, "replacementText", e.target.value)}
                                placeholder="e.g. Fizz"
                                required
                            />
                            {rules.length > 1 && (
                                <button
                                    type="button"
                                    className="remove-rule-btn"
                                    onClick={() => handleRemoveRule(index)}
                                >
                                    ✖
                                </button>
                            )}
                        </div>
                    ))}
                    <button
                        type="button"
                        className="add-rule-btn"
                        onClick={handleAddRule}
                    >
                        + Add Another Rule
                    </button>
                </div>

                <label className="form-label">Number Range</label>
                <div className="range-container">
                    <div>
                        <label>Minimum</label>
                        <input
                            type="number"
                            className="small-input"
                            value={minNumber}
                            onChange={(e) => setMinNumber(Number(e.target.value))}
                            required
                        />
                    </div>
                    <div>
                        <label>Maximum</label>
                        <input
                            type="number"
                            className="small-input"
                            value={maxNumber}
                            onChange={(e) => setMaxNumber(Number(e.target.value))}
                            required
                        />
                    </div>
                </div>

                <button type="submit" className="create-btn">
                    Create Game!
                </button>
            </form>
        </div>
    );
}

export default CreateGamePage;
