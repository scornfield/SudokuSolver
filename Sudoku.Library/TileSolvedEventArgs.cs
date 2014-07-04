using Cornfield.Sudoku.Library.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cornfield.Sudoku.Library
{
    public class TileSolvedEventArgs : EventArgs
    {
        public TileSolvedEventArgs(ISudokuTile tile)
        {
            Tile = tile;
        }

        public ISudokuTile Tile { get; set; }
    }
}
