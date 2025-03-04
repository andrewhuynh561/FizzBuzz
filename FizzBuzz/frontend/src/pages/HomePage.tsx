import React from 'react';
import { useNavigate } from 'react-router-dom';
import '../CSS/HomePage.css'; 

function HomePage() {
    const navigate = useNavigate();

    // Handlers to navigate to other pages
    const handleCreateGame = () => {
        navigate('/create-game'); 
    };

    const handlePlayGame = () => {
        navigate('/play/12'); // asumming navigate to play with sesion 12

    };

    return (
        <div className="home-container">
            <h1 className="home-title">FIZZ BUZZER</h1>

            <p className="home-description">
                FizzBuzz! In this game, you’ll replace numbers with quirky words—like ‘Foo’
                and ‘Boo’—whenever certain divisibility rules apply. If a number matches multiple rules, stack the words together!
                It’s a lighthearted way to sharpen your number sense and challenge your mind, all while having a blast :)).
            </p>

            <div className="boxes-container">
                {/*<div className="box box-foo">Foo</div>*/}
                {/*<div className="box box-boo">Boo</div>*/}
                <button className="box box-foo" onClick={handleCreateGame}>
                    Create Game
                </button>
                <button className="box box-boo" onClick={handlePlayGame}>
                    Play Game
                </button>
            </div>

  
        </div>
    );
}

export default HomePage;
