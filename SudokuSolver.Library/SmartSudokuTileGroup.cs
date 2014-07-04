using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cornfield.Sudoku.Library;
using Cornfield.Sudoku.Library.Interfaces;
using Cornfield.SudokuSolver.Library.Interfaces;

namespace Cornfield.SudokuSolver.Library
{
    public class SmartSudokuTileGroup : SudokuTileGroup<SmartSudokuTile>, ISudokuTileGroup<SmartSudokuTile>
    {
        public HashSet<int> AllPossibleValues = new HashSet<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        public EventHandler<TileGroupUpdatingEventArgs> TileGroupUpdated;
        public bool InSolverQueue { get; set; }
        public SmartSudokuTileGroup() : base()
        {

        }

        public new void Init()
        {
            InSolverQueue = false;

            // Add the TileSolved event handler to each tile in this group
            Tiles.ForEach(delegate(SmartSudokuTile tile) { tile.TileSolved += this.TileSolved; });
        }

        public void Start()
        {
            // Fire the OnTileSolved on all of our initially filled in tiles
            Tiles.ForEach(delegate(SmartSudokuTile tile) { if (tile.State == TileStates.Solved) tile.OnTileSolved(); });
        }

        public void TileSolved(object sender, TileSolvedEventArgs args)
        {
            int val = (int)args.Tile.Value;

            //Console.WriteLine("{0},{1}: Solved.  Group {2} Updating.", args.Tile.XPos, args.Tile.YPos, Id);
            UpdatePossibleValues(val);

            // If this tile group isn't already in the queue, raise the event to add it
            if (!InSolverQueue)
            {
                EventHandler<TileGroupUpdatingEventArgs> handler = TileGroupUpdated;
                if (handler != null)
                    handler(this, new TileGroupUpdatingEventArgs(this));

                InSolverQueue = true;
            }   
        }

        public void UpdatePossibleValues(int val)
        {
            //Console.WriteLine("Group {0}: Removing Possible Value {1}.", Id, val);
            AllPossibleValues.Remove(val);

            // Remove this value from the possible values of all other tiles.
            foreach (var tile in Tiles)
            {
                if (tile.State == TileStates.Solved) continue;
                tile.RemovePossibleValue(val);
            } 
        }
    }
}
