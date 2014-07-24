using Cornfield.Sudoku.Library;
using Cornfield.Sudoku.Library.Interfaces;
using Cornfield.SudokuSolver.Library.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cornfield.SudokuSolver.Library
{
    // The intersection removal solver works on the basis that if any one number occurs twice or three times in just one unit (any row, column or box),
    // then we can remove that number from the intersection of another aligning unit.
    public class IntersectionRemovalSolver : ISudokuSolver
    {
        public SolverType Type { get; set; }

        public IntersectionRemovalSolver()
        {
            Type = SolverType.Group;
        }

        public void Solve(SudokuPuzzleSolver puzzle)
        {
            
        }

        public void Solve(SudokuTileGroupSolver group)
        {
            SudokuTileGroupSolver compareGroup;
            int affectedTiles = 0;
            // Check if this group has two or three tiles that can accept a value, and if they are aligned
            foreach (var num in group.AllPossibleValues.ToList())
            {
                var matches = group.Tiles.Where(x => x.State != TileStates.Solved && x.PossibleValues.Contains(num));

                // If there's not 2 or 3 matches for this number, try the next one
                if (matches.Count() > 3 || matches.Count() < 2) continue;

                // If this is a box, check for alignment on the column and row.
                if (group.Type == GroupType.Box) 
                {
                    // Check if we are aligned on the column
                    compareGroup = matches.ElementAt(0).ColumnGroup;
                    if (matches.All(x => x.ColumnGroup.Id == compareGroup.Id))
                    {
                        ActionRecorder.Record(string.Format("Group {0}: Found {1} pointing tiles in column group {2}.", group.Id, matches.Count(), compareGroup.Id));
                        // Remove this possible value from the rest of the column, because it has to be in this box.
                        foreach (var tile in compareGroup.Tiles.Where(x => x.State != TileStates.Solved && x.PossibleValues.Contains(num)))
                        {
                            if (!matches.Contains(tile))
                            {
                                tile.RemovePossibleValue(num, "Intersection Removal Solver");
                                affectedTiles++;
                            }
                        }

                        // Since we did this, no need to check for other group alignments
                        continue;
                    }

                    // Check if we are aligned on the row
                    compareGroup = matches.ElementAt(0).RowGroup;
                    if (matches.All(x => x.RowGroup.Id == compareGroup.Id))
                    {
                        ActionRecorder.Record(string.Format("Group {0}: Found {1} pointing tiles in row group {2}.", group.Id, matches.Count(), compareGroup.Id));
                        // Remove this number from the rest of the row, because it has to be in this box.
                        foreach (var tile in compareGroup.Tiles.Where(x => x.State != TileStates.Solved && x.PossibleValues.Contains(num)))
                        {
                            if (!matches.Contains(tile))
                            {
                                tile.RemovePossibleValue(num, "Intersection Removal Solver");
                                affectedTiles++;
                            }
                        }

                        // Since we did this, no need to check for other group alignments
                        continue;
                    }
                }

                // If this is a column or row, check if all of our matches are in the same box.
                else if (group.Type == GroupType.Column || group.Type == GroupType.Row)
                {
                    // Check if all of our matches are in the same box
                    compareGroup = matches.ElementAt(0).BoxGroup;
                    if (matches.All(x => x.BoxGroup.Id == compareGroup.Id))
                    {
                        ActionRecorder.Record(string.Format("Group {0}: Found {1} pointing tiles in box group {2}.", group.Id, matches.Count(), compareGroup.Id));
                        // Remove this number from the rest of the box, because it has to be in this column or row.
                        foreach (var tile in compareGroup.Tiles.Where(x => x.State != TileStates.Solved && x.PossibleValues.Contains(num)))
                        {
                            if (!matches.Contains(tile))
                            {
                                tile.RemovePossibleValue(num, "Intersection Removal Solver");
                                affectedTiles++;
                            }
                        }
                    }
                }
            }

            // Raise the event to add this group to the solver queue if we did anything useful.
            if (affectedTiles > 0) group.OnTileGroupUpdated();
        }

        public override string ToString()
        {
            return "Intersection Removal Solver";
        }
    }
}
