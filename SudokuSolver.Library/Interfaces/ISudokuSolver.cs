using Cornfield.Sudoku.Library;
using Cornfield.Sudoku.Library.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cornfield.SudokuSolver.Library.Interfaces
{
    public interface ISudokuSolver
    {
        void SolveGroup(SmartSudokuTileGroup group, TileConfidence confidence = TileConfidence.Certain);
    }
}
