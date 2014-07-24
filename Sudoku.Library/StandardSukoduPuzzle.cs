using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cornfield.Sudoku.Library.Interfaces;

namespace Cornfield.Sudoku.Library
{
    public class StandardSukoduPuzzle<G, T>: SudokuPuzzle<G, T>, ISudokuPuzzle<G, T> where G: ISudokuTileGroup<T>, new() where T: ISudokuTile
    {
        public void InitTileGroups()
        {
            TileGroups = new List<G>();

            // Init Tile numbers, row groups, and col groups
            for (int row = 0; row < Board.Count; row++)
            {
                var rowGrp = new G() { Id = row, Type = GroupType.Row };
                for (int col = 0; col < Board[row].Count; col++)
                {
                    G colGrp;
                    if (TileGroups.Where(x => x.Id == Board.Count + col).Count() == 0)
                    {
                        colGrp = new G() { Id = Board.Count + col, Type = GroupType.Column };
                        TileGroups.Add(colGrp);
                    }
                    else
                        colGrp = TileGroups.First(x => x.Id == Board.Count + col);
                    rowGrp.AddTile(Board[row][col]);
                    colGrp.AddTile(Board[row][col]);

                    Board[row][col].XPos = col;
                    Board[row][col].YPos = row;
                }
                TileGroups.Add(rowGrp);
            }

            // Init Box Groups
            for (var index = 0; index < 9; index++)
            {
                var grp = new G() { Id = TileGroups.Count, Type = GroupType.Box };

                int startRow = (int)Math.Floor(index / 3.0) * 3;
                int startCol = (int)Math.Floor(index % 3.0) * 3;

                for (int row = startRow; row < startRow + 3; row++)
                {
                    for (int col = startCol; col < startCol + 3; col++)
                    {
                        grp.AddTile(Board[row][col]);
                    }
                }

                TileGroups.Add(grp);
            }
            
        }
    }
}
