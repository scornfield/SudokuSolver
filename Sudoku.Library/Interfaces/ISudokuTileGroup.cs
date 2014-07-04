using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cornfield.Sudoku.Library.Interfaces
{
    public interface ISudokuTileGroup<T> where T: ISudokuTile
    {
        int Id { get; set; }
        List<T> Tiles { get; }

        void AddTile(T tile);
        void Init();

        bool IsValid();
    }
}
