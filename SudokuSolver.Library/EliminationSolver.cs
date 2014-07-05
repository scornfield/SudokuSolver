using Cornfield.SudokuSolver.Library.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cornfield.SudokuSolver.Library
{
    public class EliminationSolver : ISudokuSolver
    {
        public void SolveGroup(SmartSudokuTileGroup group, TileConfidence confidence = TileConfidence.Certain)
        {
            StaticSolveGroup(group, confidence);
        }

        public static void StaticSolveGroup(SmartSudokuTileGroup group, TileConfidence confidence)
        {
            if (group.Solved || group.AllPossibleValues.Count == group.Tiles.Where(x => x.Value != null).Count()) return;

            // For each unsolved tile in the group, try to apply the occupancy theorem.
            foreach (var tile in group.Tiles.Where(x => x.State != TileStates.Solved))
            {
                // Find all combinations of this tiles possible values
                var valueCombinations = tile.PossibleValues.GetAllCombos().Where(x => x.Count > 1).OrderByDescending(x => x.Count).ToList();

                foreach (var combo in valueCombinations)
                {
                    // Find other tiles that can be solved by the same combination of values
                    var matches = new List<SmartSudokuTile>() { tile };
                    foreach (var otherTile in group.Tiles.Where(x => x.State != TileStates.Solved && tile != x))
                    {
                        
                        if (otherTile.PossibleValues.Intersect(combo).Count() == combo.Count)
                            matches.Add(otherTile);
                    }

                    // If the number of matched tiles equals the number of numbers in the set, then we can eliminate all other possible values from these tiles, 
                    // and remove any values in this combination from all other tiles in the group
                    if (matches.Count == combo.Count)
                    {
                        // If any of the values in this combination can go into a tile that wasn't one of our matches, then we can't use this method
                        bool canProcess = true;
                        foreach (var num in combo)
                        {
                            if (group.Tiles.Where(x => x.State != TileStates.Solved && !matches.Contains(x) && x.PossibleValues.Contains(num)).Count() > 0)
                                canProcess = false;
                        }

                        // If this combination doesn't work, move onto the next one.
                        if (!canProcess) continue;

                        ActionRecorder.Record(string.Format("Elimination Solver: Processing {0} for Group {1}", string.Join(",", combo), group.Id));

                        // Initialize our count of affected tiles so that we can tell if we make any changes
                        int affectedTileValues = 0;

                        // Remove all other possible values from our matched tiles
                        foreach (var match in matches)
                        {
                            foreach (var num in match.PossibleValues.Where(x => !combo.Contains(x)).ToList())
                            {
                                ActionRecorder.Record(string.Format("Elimination Solver: Removing {0} as a possible value from {1},{2}", num, match.XPos, match.YPos));
                                match.RemovePossibleValue(num);
                                affectedTileValues++;
                            }
                        }

                        // If this made any changes, raise the event to signal that this group has been updated
                        if (affectedTileValues > 0) group.OnTileGroupUpdated();
                    }
                }
            }

            // If any tile is down to its only remaining value now, set that value.
            foreach (var tile in group.Tiles.Where(x => x.State != TileStates.Solved))
            {
                tile.SetOnlyRemainingValue(string.Format("Process of Elimination Solver Group {0}", group.Id), confidence);
            }
        }

        public override string ToString()
        {
            return "Elimination Solver";
        }
    }
}
