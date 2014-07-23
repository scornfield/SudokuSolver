using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cornfield.SudokuSolver.Library
{
    public class SerializablePuzzle
    {
        public List<List<int>> Board { get; set; }
        public string Solver { get; set; }
        public int Id { get; set; }
        public SerializablePuzzle(SudokuPuzzleSolver puzzle)
        {
            Board = new List<List<int>>();
            foreach (var row in puzzle.Board)
            {
                // Assemble the board
                Board.Add(row.Select(x => (int)x.Value).ToList());
            }

            Solver = puzzle.Solver;
            Id = puzzle.Id;
        }
    }
}
