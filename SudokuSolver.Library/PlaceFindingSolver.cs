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
        public void SolvePuzzle(SudokuPuzzle<SmartSudokuTileGroup, SmartSudokuTile> puzzle)
        {
            StaticSolvePuzzle(puzzle);
        }

        public void SolveGroup(SmartSudokuTileGroup group)
        {
            StaticSolveGroup(group);
        }

        public static void StaticSolvePuzzle(SudokuPuzzle<SmartSudokuTileGroup, SmartSudokuTile> puzzle)
        {
            foreach (var grp in puzzle.TileGroups)
            {
                StaticSolveGroup(grp);
            }
        }

        public static void StaticSolveGroup(SmartSudokuTileGroup group)
        {
            for (int num = 1; num < group.Tiles.Count; num++)
            {
                if (!group.AllPossibleValues.Contains(num)) continue;
                //if (!group.IsValid())
                    //Console.WriteLine("Group {0} is already invalid", group.Id);
                var possibleTiles = group.Tiles.Where(x => x.State != TileStates.Solved && x.PossibleValues.Contains(num)).ToList();
                if (possibleTiles.Count == 1)
                    possibleTiles[0].SetValue(num, TileConfidence.Certain, string.Format("Place Finding Solver Group {0}", group.Id));
                //if (!group.IsValid())
                    //Console.WriteLine("Group {0} is now invalid", group.Id);
            }
        }
    }
}
