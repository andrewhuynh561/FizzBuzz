import { render, screen } from '@testing-library/react';
import { BrowserRouter } from 'react-router-dom';
import PlayGamePage from '../pages/PlayGame';

describe('PlayGamePage', () => {
  it('renders game selection screen', () => {
    render(
      <BrowserRouter>
        <PlayGamePage />
      </BrowserRouter>
    );

    // Check for main UI elements
    expect(screen.getByText('Play a FizzBuzz Game')).toBeInTheDocument();
    expect(screen.getByText('Choose Game:')).toBeInTheDocument();
    expect(screen.getByText('Duration (seconds):')).toBeInTheDocument();
    expect(screen.getByText('Start Game')).toBeInTheDocument();
  });
}); 