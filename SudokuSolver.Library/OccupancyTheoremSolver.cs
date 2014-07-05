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
    // The occupancy theorem works based on preemtive sets.  For any given group, if there are [n] tiles that share the same [n] possible values, 
    // and these are the only possible values for any of these tiles, we call this a preemtive set. 
    // No other tile in the group can be any of those values, so we remove those as possibilities.
    public class OccupancyTheoremSolver : ISudokuSolver
    {
        public void SolveGroup(SmartSudokuTileGroup group, TileConfidence confidence = TileConfidence.Certain)
        {
            StaticSolveGroup(group, confidence);
        }

        public static void StaticSolveGroup(SmartSudokuTileGroup group, TileConfidence confidence)
        {
            // For each unsolved tile in the group, try to apply the occupancy theorem.
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
                    ActionRecorder.Record(string.Format("Occupancy Theorem Solver: Processing {0} for Group {1}", string.Join(",", tile.PossibleValues), group.Id));
                    // Initialize our count of affected tiles so that we can tell if we make any changes
                    int affectedTileValues = 0;
                    
                    // For each other tile in the group that isn't solved or in our preemtive set, remove the possible values in our preemtive set
                    foreach (var otherTile in group.Tiles.Where(x => x.State != TileStates.Solved && !matches.Contains(x)))
                    {
                        foreach (var num in tile.PossibleValues.Where(x => otherTile.PossibleValues.Contains(x)))
                        {
                            ActionRecorder.Record(string.Format("Occupancy Theorem Solver: Removing {0} as a possible value from {1},{2}", num, otherTile.XPos, otherTile.YPos));
                            otherTile.RemovePossibleValue(num, false);
                            affectedTileValues++;
                        }
                    }
                    
                    //if (!group.IsValid()) Console.WriteLine("Occupancy Theorem Solver: Group {0} is now invalid", group.Id);
                    
                    // If this made any changes, raise the event to signal that this group has been updated
                    if(affectedTileValues > 0) group.OnTileGroupUpdated();
                }
            }

            // If any tile is down to its only remaining value now, set that value.
            foreach (var tile in group.Tiles.Where(x => x.State != TileStates.Solved))
            {
                tile.SetOnlyRemainingValue(string.Format("Occupancy Theorem Solver Group {0}", group.Id), confidence);
            }
        }

        public override string ToString()
        {
            return "Occupancy Theorem Solver";
        }
    }
}
 