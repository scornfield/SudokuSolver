using Cornfield.Sudoku.Library;
using Cornfield.Sudoku.Library.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cornfield.SudokuSolver.Library.Interfaces
{
    public enum SolverType { Puzzle, Group }

    public interface ISudokuSolver
    {
        SolverType Type { get; set; }   
        void Solve(SmartSudokuPuzzle puzzle);
        void Solve(SmartSudokuTileGroup group);
    }
}
