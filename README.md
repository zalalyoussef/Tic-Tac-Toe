Tic-Tac-Toe Game with AI Decision-Making Capabilities
Project Overview
This project is a Tic-Tac-Toe game enhanced with AI-driven decision-making capabilities. The AI uses a decision-making algorithm to assess possible moves and determine optimal strategies, allowing the computer to play competitively against a human player. This project serves as an exploration into artificial intelligence and game theory, with an emphasis on tree-based decision-making models.

Features

Single-Player Mode with AI: The computer uses a tree-based decision-making algorithm to select its moves, simulating an intelligent opponent.
Minimax Algorithm with Depth Control: The AI evaluates possible game states using the minimax algorithm. Players can adjust the depth of the AI’s decision tree, balancing difficulty and computational time.
Evaluation Function: The AI assigns scores to end-game scenarios, aiming to maximize its advantage while minimizing potential losses from the opponent’s moves.
Score-Based Move Selection: The AI chooses moves based on a scoring system where:
+1000 points indicate a win for the AI.
0 points indicate a draw.
-1000 points indicate a loss to the opponent.
Interactive Gameplay: The application allows for both player-versus-player and player-versus-computer modes, with an option to simulate AI versus AI games.
Additional Options:
Option to display a tree of moves showing each possible game outcome.
Variable difficulty levels, making the AI’s decisions more challenging at higher depths.
Support for a larger, four-in-a-row grid game mode.
How It Works
The AI computes the optimal move by generating a game tree from the current state, evaluating all possible moves (nodes) to predict the game's outcome. Using minimax, the AI looks ahead to choose moves that maximize its chance of winning and minimize potential losses by predicting the opponent’s best possible responses.

Game Tree Generation: At each turn, the AI generates possible game states and assigns a score based on the outcome.
Minimax Algorithm: If it’s the AI’s turn, it chooses the move with the highest potential score. For the opponent’s turn, the AI anticipates the move with the lowest score to simulate realistic player responses.
Move Selection: The AI then makes a move that aligns with the highest cumulative score, improving its chances of winning.
Project Structure

src/: Contains the source code files.
docs/: Contains documentation and visuals explaining the decision-making process.
tests/: Test cases validating the AI’s move selection logic and various scenarios.
How to Run the Game
Instructions on how to clone the repository, install any dependencies, and run the game.

Future Enhancements

Integrating a GUI for an enhanced player experience.
Expanding the evaluation function for larger game boards or custom grid sizes.
