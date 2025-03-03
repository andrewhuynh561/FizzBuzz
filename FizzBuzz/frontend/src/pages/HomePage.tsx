import React from 'react';
import { useNavigate } from 'react-router-dom';
import '../CSS/HomePage.css'; // We'll define CSS in a separate file

function HomePage() {
    const navigate = useNavigate();

    // Handlers to navigate to other pages
    const handleCreateGame = () => {
        navigate('/create-game'); // or whichever route you use
    };

    const handlePlayGame = () => {
        navigate('/play'); // or whichever route you use
    };

    return (
        <div className="home-container">
            <h1 className="home-title">FIZZ BUZZER</h1>

            <p className="home-description">
                Lorem ipsum dolor sit amet, consectetur adipiscing elit.
                In this game, you’ll replace numbers with words like "Foo" or "Boo"
                based on divisibility rules. Combine words if multiple rules apply!
            </p>

            <div className="boxes-container">
                <div className="box box-foo">Foo</div>
                <div className="box box-boo">Boo</div>
            </div>

            <div className="buttons-container">
                <button className="nav-button" onClick={handleCreateGame}>
                    Create Game
                </button>
                <button className="nav-button" onClick={handlePlayGame}>
                    Play Game
                </button>
            </div>
        </div>
    );
}

export default HomePage;
