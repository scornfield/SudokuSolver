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
    public class PlaceFindingSolver : ISudokuSolver
    {
        public void SolveGroup(SmartSudokuTileGroup group)
        {
            StaticSolveGroup(group);
        }

        public static void StaticSolveGroup(SmartSudokuTileGroup group)
        {
            
            for (int num = 1; num < group.Tiles.Count; num++)
            {
                if (!group.AllPossibleValues.Contains(num)) continue;

                var possibleTiles = group.Tiles.Where(x => x.State != TileStates.Solved && x.PossibleValues.Contains(num)).ToList();
                ////Console.WriteLine("Place Finding Solver: {0} Possible places for {1} in Group {2}", possibleTiles.Count, num, group.Id);

                if (possibleTiles.Count == 1)
                {
                    possibleTiles[0].SetValue(num, TileConfidence.Certain, string.Format("Place Finding Solver Group {0}", group.Id));
                }
                    
            }
        }
    }
}
