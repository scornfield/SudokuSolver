using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cornfield.Sudoku.Library.Interfaces
{
    public interface ISudokuPuzzle<G, T> where G : ISudokuTileGroup<T> where T : ISudokuTile
    {
        List<List<T>> Board { get; set; }
        List<G> TileGroups { get; set; }
        int Id { get; set; }
        string Solver { get; set; }

        void PrintBoard();
    }
}
