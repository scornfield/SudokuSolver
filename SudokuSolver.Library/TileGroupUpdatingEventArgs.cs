using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cornfield.SudokuSolver.Library
{
    public class TileGroupUpdatingEventArgs : EventArgs
    {
        public TileGroupUpdatingEventArgs(SmartSudokuTileGroup group)
        {
            Group = group;
        }

        public SmartSudokuTileGroup Group { get; set; }
    }
}
