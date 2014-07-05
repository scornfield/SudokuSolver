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
        public void SolveGroup(SmartSudokuTileGroup group)
        {
            StaticSolveGroup(group);
        }

        public static void StaticSolveGroup(SmartSudokuTileGroup group)
        {

        }
    }
}
