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
    // The hidden single solver works by determining if there is only one possible tile left in a group that can accept a given value.  If this is the case, it sets the tile to that value.
    public class HiddenSingleSolver : ISudokuSolver
    {
        public SolverType Type { get; set; }

        public HiddenSingleSolver()
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
            // For each possible value in a group (1 to the number of tiles in the group)
            for (int num = 1; num <= group.Tiles.Count; num++)
            {
                // If this number has already been used in the group, move on to the next number
                if (!group.AllPossibleValues.Contains(num)) continue;

                // Get a list of tiles with this number in their possible values
                var possibleTiles = group.Tiles.Where(x => x.State != TileStates.Solved && x.PossibleValues.Contains(num)).ToList();

                // If there is only one tile that can accept this number, set its value
                if (possibleTiles.Count == 1)
                {
                    possibleTiles[0].SetValue(num, string.Format("Hidden Single Solver Group {0}", group.Id));
                }
                    
            }
        }

        public override string ToString()
        {
            return "Hidden Single Solver";
        }
    }
}
