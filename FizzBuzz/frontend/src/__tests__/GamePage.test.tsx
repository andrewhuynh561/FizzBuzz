import { render, screen, fireEvent } from '@testing-library/react';
import { BrowserRouter } from 'react-router-dom';
import CreateGamePage from '../pages/GamePage';

// Mock the fetch function
global.fetch = jest.fn();

describe('CreateGamePage', () => {
  beforeEach(() => {
    // Mock basic API response
    (global.fetch as jest.Mock).mockImplementation(() => 
      Promise.resolve({
        ok: true,
        json: () => Promise.resolve([
          {
            id: 1,
            name: 'Test Game',
            author: 'Test Author',
            rules: [{ divisor: 3, replacementText: 'Fizz' }]
          }
        ])
      })
    );
  });

  it('shows game creation form', () => {
    render(
      <BrowserRouter>
        <CreateGamePage />
      </BrowserRouter>
    );

    expect(screen.getByText('Create Your Own FizzBuzz Game!')).toBeInTheDocument();
    expect(screen.getByText('Game Name')).toBeInTheDocument();
    expect(screen.getByText('Your Name')).toBeInTheDocument();
  });

  it('creates new game', async () => {
    render(
      <BrowserRouter>
        <CreateGamePage />
      </BrowserRouter>
    );

    // Fill form
    const nameInput = screen.getByPlaceholderText('FooBooLoo');
    const authorInput = screen.getByPlaceholderText('Game Creator');
    
    fireEvent.change(nameInput, { target: { value: 'New Game' } });
    fireEvent.change(authorInput, { target: { value: 'New Author' } });

    // Submit
    fireEvent.click(screen.getByText('Create Game!'));
  });
}); 