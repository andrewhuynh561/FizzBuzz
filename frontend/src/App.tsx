
import './App.css'

import React, { useEffect, useState } from 'react';

function App() {
    const [data, setData] = useState<any[]>([]);
    const [error, setError] = useState<string>("");

    useEffect(() => {
        // Do a test GET request when the component mounts
        fetch("https://localhost:7178/WeatherForecast")
            .then(async (response) => {
                if (!response.ok) {
                    const message = await response.text();
                    throw new Error(message);
                }
                return response.json();
            })
            .then((json) => {
                setData(json);  // e.g. an array of forecasts
            })
            .catch((err) => {
                setError(err.message);
            });
    }, []);

    return (
        <div>
            <h1>Vite + React + .NET Test</h1>
            {error && <p style={{ color: 'red' }}>Error: {error}</p>}

            {data.length > 0 ? (
                <ul>
                    {data.map((forecast, idx) => (
                        <li key={idx}>
                            {forecast.date} - {forecast.temperatureC}°C - {forecast.summary}
                        </li>
                    ))}
                </ul>
            ) : (
                <p>No data yet or loading...</p>
            )}
        </div>
    );
}

export default App;