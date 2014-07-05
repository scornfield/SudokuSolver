using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cornfield.SudokuSolver.Library.Interfaces;
using Cornfield.Sudoku.Library.Interfaces;
using Cornfield.Sudoku.Library;

namespace Cornfield.SudokuSolver.Library
{
    public class OccupancyTheoremSolver : ISudokuSolver
    {
        public void SolveGroup(SmartSudokuTileGroup group)
        {
            StaticSolveGroup(group);
        }

        public static void StaticSolveGroup(SmartSudokuTileGroup group)
        {
            foreach (var tile in group.Tiles.Where(x => x.State != TileStates.Solved))
            {
                // Find other tiles that have the exact same remaining possible tiles
                var matches = new List<SmartSudokuTile>() { tile };
                foreach (var otherTile in group.Tiles.Where(x => x.State != TileStates.Solved && tile != x))
                {
                    if (tile.PossibleValues.Count == otherTile.PossibleValues.Count && tile.PossibleValues.Intersect(otherTile.PossibleValues).Count() == tile.PossibleValues.Count) 
                        matches.Add(otherTile);
                }

                // If the number of matches equals the number of numbers in the set, then we have a preemtive set and can apply the occupancy theorem
                if (matches.Count == tile.PossibleValues.Count)
                {
                    //Console.WriteLine("Occupancy Theorem Solver: Processing {0} for Group {1}", string.Join(",", tile.PossibleValues), group.Id);
                    int affectedTileValues = 0;
                    foreach (var otherTile in group.Tiles.Where(x => x.State != TileStates.Solved && !matches.Contains(x)))
                    {
                        foreach (var num in tile.PossibleValues.Where(x => otherTile.PossibleValues.Contains(x)))
                        {
                            otherTile.RemovePossibleValue(num, false);
                            affectedTileValues++;
                        }
                    }
                    
                    if (!group.IsValid())
                        //Console.WriteLine("Occupancy Theorem Solver: Group {0} is now invalid", group.Id);
                    
                    // If this made any changes, raise the event to signal that this group has been updated
                    if(affectedTileValues > 0) group.OnTileGroupUpdated();
                }
            }

            // If any tile is down to its only remaining value now, set that value.
            foreach (var tile in group.Tiles.Where(x => x.State != TileStates.Solved))
            {
                tile.SetOnlyRemainingValue(string.Format("Occupancy Theorem Solver Group {0}", group.Id));
            }
        }
    }
}
 