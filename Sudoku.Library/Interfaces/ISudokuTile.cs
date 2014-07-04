using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cornfield.Sudoku.Library.Interfaces
{
    public interface ISudokuTile
    {
        int XPos { get; set; }
        int YPos { get; set; }
        int? Value { get; }
        List<int> GroupIds { get; }
    }
}
