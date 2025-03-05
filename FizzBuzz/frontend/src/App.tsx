import { Routes, Route,Link } from 'react-router-dom';
import CreateGamePage from './pages/GamePage';
import PlayGamePage from './pages/PlayGamePage';
import HomePage from './pages/HomePage';

function App() {
    return (
        <div>
            {/*<nav>*/}
            {/*    <Link to="/">Play Game</Link> | */}{/* Link to the default page */}
            <Link to="/" reloadDocument>
                Home
            </Link>
            {/*</nav>*/}

            <Routes>              
                <Route path="/" element={<HomePage />} />

                <Route path="/create-game" element={<CreateGamePage />} />
                <Route path="/play/:sessionId" element={<PlayGamePage />} />
            </Routes>
        </div>
    );
}

export default App;
