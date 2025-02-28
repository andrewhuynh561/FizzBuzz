import { Routes, Route, Link } from 'react-router-dom';
import CreateGamePage from './pages/CreateGamePage';
import PlayGamePage from './pages/PlayGamePage';

function App() {
    return (
        <div>
            <nav>
                <Link to="/">Play Game</Link> | {/* Link to the default page */}
                <Link to="/create-game">Create Game</Link>
            </nav>

            <Routes>              
                <Route path="/" element={<PlayGamePage />} />
                <Route path="/create-game" element={<CreateGamePage />} />
                <Route path="/play/:sessionId" element={<PlayGamePage />} />
            </Routes>
        </div>
    );
}

export default App;
