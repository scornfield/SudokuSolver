using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cornfield.Sudoku.Library.Interfaces;

namespace Cornfield.Sudoku.Library
{
    public class SudokuTile : ISudokuTile
    {
        public int XPos { get; set; }
        public int YPos { get; set; }
        protected int? _value = null;
        public virtual int? Value {
            get 
            {
                return _value;
            }
            protected set 
            {
                _value = value;
            }
        }

        public List<int> GroupIds { get; protected set; }
        public EventHandler<TileSolvedEventArgs> TileSolved;

        public SudokuTile() : this(null) { }

        public SudokuTile(int? val)
        {
            Value = val;
            GroupIds = new List<int>();
        }

        public static implicit operator SudokuTile(Int64 val)
        {
            SudokuTile tile = val == 0 ? new SudokuTile() :  new SudokuTile((int)val);
            return tile;
        }

        public override string ToString()
        {
            return string.Format("{0}", Value == null ? " " : Value.ToString());
        }
    }
}
