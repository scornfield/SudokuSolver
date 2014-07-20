using Cornfield.SudokuSolver.Library.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cornfield.SudokuSolver.Library
{
    // The strategy of naked multiples works by finding the tiles that share the same possible solutions.  For any set of tiles, 
    // if the number of possible solutions is equal to the number of tiles in the set, then none of the other tiles in the group 
    // can use these values and they can be removed from the other tiles' possible solutions.
    public class NakedMultipleSolver : ISudokuSolver
    {
        public SolverType Type { get; set; }

        public NakedMultipleSolver()
        {
            Type = SolverType.Group;
        }

        public void Solve(SmartSudokuPuzzle puzzle)
        {
            throw new NotImplementedException();
        }

        public void Solve(SmartSudokuTileGroup group)
        {
            StaticSolveGroup(group);
        }

        public static void StaticSolveGroup(SmartSudokuTileGroup group)
        {
            // If the group is solved or the number of remaining values equals the number of open tiles, then this solver won't help
            if (group.Solved || group.AllPossibleValues.Count == group.Tiles.Where(x => x.Value != null).Count()) return;

            // Find all combinations of this groups possible values
            var valueCombinations = group.AllPossibleValues.GetAllCombos().Where(x => x.Count > 1 && x.Count <= 4).OrderByDescending(x => x.Count).ToList();

            // For each combination, search for hidden multiples
            foreach (var combo in valueCombinations)
            {
                // If this combo is all of the remaining tiles for this group, then there's nothing to be done here.
                if (combo.Count == group.AllPossibleValues.Count) continue;

                // Find tiles that can only be solved by the values in this combination
                var matches = new List<SmartSudokuTile>();
                foreach (var tile in group.Tiles.Where(x => x.State != TileStates.Solved))
                {
                    if (combo.ContainsAll(tile.PossibleValues))
                        matches.Add(tile);
                }

                // If the number of matched tiles equals the number of numbers in the set, then we can eliminate the values in this combination from all other tiles in the group.
                if (matches.Count == combo.Count)
                {
                    // If each number wasn't in at least one of our matched tiles, then don't proceed with this combination
                    if (!combo.All(x => matches.Any(y => y.PossibleValues.Contains(x)))) continue;

                    ActionRecorder.Record(string.Format("Naked Multiple Solver: Found {0} for Group {1}", string.Join(",", combo), group.Id));

                    // Initialize our count of affected tiles so that we can tell if we make any changes
                    int affectedTileValues = 0;

                    // Remove all other possible values from our matched tiles
                    foreach (var match in matches)
                    {
                        foreach (var num in match.PossibleValues.Where(x => !combo.Contains(x)).ToList())
                        {
                            ActionRecorder.Record(string.Format("Naked Multiple Solver: Removing {0} as a possible value from {1},{2}", num, match.XPos, match.YPos));
                            match.RemovePossibleValue(num);
                            affectedTileValues++;
                        }
                    }

                    // If this made any changes, raise the event to signal that this group has been updated
                    if (affectedTileValues > 0) group.OnTileGroupUpdated();
                }
            }

            // If any tile is down to its only remaining value now, set that value.
            foreach (var tile in group.Tiles.Where(x => x.State != TileStates.Solved))
            {
                tile.CheckNakedSingle(string.Format("Naked Multiple Solver Group {0}", group.Id));
            }
        }

        public override string ToString()
        {
            return "Naked Multiple Solver";
        }
    }
}
