import React from 'react';
import { Routes, Route, Link } from 'react-router-dom';
import CreateGamePage from './pages/CreateGamePage';
import StartSessionPage from './pages/StartSessionPage';
import PlayGamePage from './pages/PlayGamePage';
import ResultsPage from './pages/ResultsPage';

function App() {
    return (
        <div>
            <nav>
                <Link to="/create-game">Create Game</Link> |
                <Link to="/start-session">Start Session</Link>
            </nav>

            <Routes>
                <Route path="/create-game" element={<CreateGamePage />} />
                <Route path="/start-session" element={<StartSessionPage />} />
                <Route path="/play/:sessionId" element={<PlayGamePage />} />
                <Route path="/results/:sessionId" element={<ResultsPage />} />
            </Routes>
        </div>
    );
}

export default App;
