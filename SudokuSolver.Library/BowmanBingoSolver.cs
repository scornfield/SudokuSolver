using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cornfield.SudokuSolver.Library.Interfaces;
using Cornfield.Sudoku.Library.Interfaces;
using Cornfield.Sudoku.Library;

namespace Cornfield.SudokuSolver.Library
{
    // The Bowman Bingo Solver is a trial and error strategy that tries to eliminate possible values from tiles by guessing and then propagating the result until the
    // board has been solved or it has violated the sudoku condition.  If the condition is violated then the guessed value can be removed as a possibility, and we can 
    // potentially get down to the last possible value for a given tile, at which point we have a certain result.
    public class BowmanBingoSolver : ISudokuSolver
    {
        public SolverType Type { get; set; }

        public BowmanBingoSolver()
        {
            Type = SolverType.Puzzle;
        }

        public void Solve(SudokuPuzzleSolver puzzle)
        {
            // Start at the first tile in the board
            int y = 0, x = 0;
            
            // This is the number of possible values we're willing to guess at for a tile.  Start low and increase if we need to, but this will improve our odds.
            int numPossibleValues = 2;

            while (!puzzle.Solved)
            {
                var tile = puzzle.Board[y][x];
                // If this tile isn' already solved and meets our criteria for willingness to guess, guess at it
                if (tile.State != TileStates.Solved && tile.PossibleValues.Count == numPossibleValues)
                {
                    var initalPossibleValues = tile.PossibleValues.ToList();
                    foreach(var num in initalPossibleValues) 
                    {
                        
                        ActionRecorder.Record(string.Format("Bowman Bingo Solver: Guessing {0} as value for {1},{2} from {3}", num, tile.XPos, tile.YPos, string.Join(",", tile.PossibleValues)));
                            
                        // Guess a value and see what happens
                        tile.SetValue(num, "Bowman Bingo Solver", true);

                        // If that guess invalidated the puzzle, stop reset the board and remove that value as a possibility for this tile.
                        if (!puzzle.IsValid())
                        {
                            ActionRecorder.Record(string.Format("Bowman Bingo Solver: Value {0} for {1},{2} caused puzzle to become invalid.", num, tile.XPos, tile.YPos));

                            // We encountered a violation of the sudoku condition with this guess.  Reset the board and remove it from the tile's possible values and try the next number.
                            puzzle.ClearGuesses();
                            tile.RemovePossibleValue(num, false, "Bowman Bingo Eliminating Impossible Value");

                            // If removing this value reduced the tile to its last possible value and thus solved the tile, we can break out of this solver.
                            if (tile.State == TileStates.Solved)
                                return;

                            // If we aren't solved, then we need to inform our groups that our remaining values have changed so that they know to run other solvers later on.
                            foreach (var group in puzzle.TileGroups.Where(grp => tile.GroupIds.Contains(grp.Id)))
                            {
                                group.OnTileGroupUpdated();
                            }
                        }

                        // If that guess solved the puzzle, stop guessing.
                        //if (puzzle.Solved) return;

                        // This logic is a little weird, but if we didn't encounter a violation, we want to clear our guesses and try the next number in the possible values.
                        // In essence, we are actually hoping for violations so that we can remove possible values and hopefully reduce the possibilities to one certain value.
                        ActionRecorder.Record(string.Format("Bowman Bingo Solver: No violation encountered for guess of {0} for {1},{2}.", num, tile.XPos, tile.YPos));
                        puzzle.ClearGuesses();                        
                    }

                    // If we solved a tile with certainty or solved the puzzle, get out of here!
                    if (tile.State == TileStates.Solved)
                        return;

                    // Clear all guesses in the puzzle and try another tile
                    ActionRecorder.Record("Bowman Bingo Solver: Clearing guesses and trying the next tile for a certain move.");
                    puzzle.ClearGuesses();
                }

                // Move to the next column.
                x++;

                // If we've reached the end of our row, move to the next one and go back to the first column
                if (x == puzzle.Board[y].Count)
                {
                    y++;
                    x = 0;
                }

                // If we've reached the end of the board, then increase the number of possible values we're willing to guess at and move back to the beginning.
                if (y == puzzle.Board.Count)
                {
                    numPossibleValues++;
                    x = 0;
                    y = 0;
                }

                // If we've gone through the whole board for all possible values, get out of this solver
                if (numPossibleValues > puzzle.Board.Count) 
                    return;
            }
        }

        public void Solve(SudokuTileGroupSolver group)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return "Bowman Bingo Solver";
        }
    }
}
