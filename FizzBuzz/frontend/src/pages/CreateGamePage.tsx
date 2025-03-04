import React, { useEffect, useState } from 'react';
import '../CSS/CreateGamePage.css'; 
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
        fetch("https://localhost:7178/api/Games") 
            .then(async (res) => {
                if (!res.ok) {
                    const text = await res.text();
                    throw new Error(`Error fetching games: ${text}`);
                }
                return res.json();
            })
            .then((data: Game[]) => setGames(data))
            .catch((err) => console.error(err));
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

  
    const handleSubmit = (e: React.FormEvent) => {
        e.preventDefault();

        fetch("https://localhost:7178/api/Games", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({
                name: gameName,
                author: authorName,
                minNumber,
                maxNumber,
                rules
            }),
        })
            .then(async (response) => {
                if (!response.ok) {
                    const text = await response.text();
                    alert(`Error creating game: ${text}`);
                    return;
                }
                //const data = await response.json();
                //alert(`Game created successfully! ID: ${data.gameId}`);

                
                const updatedGames = await fetch("https://localhost:7178/api/Games").then(res => res.json());
                setGames(updatedGames);

               
                setGameName("");
                setAuthorName("");
                setRules([{ divisor: 0, replacementText: "" }]);
                setMinNumber(1);
                setMaxNumber(100);
            })
            .catch(err => {
                console.error("Fetch error:", err);
                alert("Failed to create game. Check console for details.");
            });
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
