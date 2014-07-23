using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cornfield.SudokuSolver.Library
{
    public class TileGroupUpdatingEventArgs : EventArgs
    {
        public TileGroupUpdatingEventArgs(SudokuTileGroupSolver group)
        {
            Group = group;
        }

        public SudokuTileGroupSolver Group { get; set; }
    }
}
