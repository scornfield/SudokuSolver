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
    class BruteForceSolver : ISudokuSolver
    {
        public SolverType Type { get; set; }

        public BruteForceSolver()
        {
            Type = SolverType.Puzzle;
        }

        public void Solve(SmartSudokuPuzzle puzzle, TileConfidence confidence = TileConfidence.Certain)
        {
            
        }

        public void Solve(SmartSudokuTileGroup group, TileConfidence confidence = TileConfidence.Certain)
        {
            throw new NotImplementedException();
        }
    }
}
