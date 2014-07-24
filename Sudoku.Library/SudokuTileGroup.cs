using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cornfield.Sudoku.Library.Interfaces;

namespace Cornfield.Sudoku.Library
{
    public enum GroupType { Row, Column, Box, Other }

    public class SudokuTileGroup<T> : ISudokuTileGroup<T> where T : ISudokuTile
    {
        public int Id { get; set; }
        public List<T> Tiles { get; protected set; }

        public GroupType Type { get; set; }
        
        public SudokuTileGroup()
        {
            Tiles = new List<T>();
        }

        public virtual void AddTile(T tile)
        {
            Tiles.Add(tile);
            tile.GroupIds.Add(this.Id);
        }

        public void Init() { }

        public bool IsValid() 
        {
            foreach (var tile in Tiles)
            {
                if (Tiles.Count(x => x.Value == tile.Value && x.Value != null) > 1) return false;
            }

            return true;
        }
    }
}
