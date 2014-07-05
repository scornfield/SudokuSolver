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
        public List<int> AllPossibleValues = new List<int>();
        public EventHandler<TileGroupUpdatingEventArgs> TileGroupUpdated;
        public bool InSolverQueue { get; set; }
        public bool Solved { get { return AllPossibleValues.Count == 0; } }
        public SmartSudokuTileGroup() : base()
        {

        }

        // Initializes the tile by seting up event handlers and possible values
        public new void Init()
        {
            // Initialize the tile to not be in the queue yet
            InSolverQueue = false;

            // Add the TileSolved event handler to each tile in this group
            Tiles.ForEach(delegate(SmartSudokuTile tile) { tile.TileSolved += this.TileSolved; });
            
            // Initialize the Possible Values for the group and each tile
            for (int i = 1; i <= Tiles.Count; i++) { AllPossibleValues.Add(i); }
            Tiles.ForEach(delegate(SmartSudokuTile tile) { if(tile.State != TileStates.Solved) tile.PossibleValues = AllPossibleValues.ToList(); });
        }

        public void TileSolved(object sender, TileSolvedEventArgs args)
        {
            int val = (int)args.Tile.Value;

            //Console.WriteLine("{0},{1}: Solved.  Group {2} Updating.", args.Tile.XPos, args.Tile.YPos, Id);
            UpdatePossibleValues(val);

            OnTileGroupUpdated();
        }

        // This gets called any time that a tile in this group is solved or has its remaining possible values updated
        public void OnTileGroupUpdated()
        {
            // If this tile group isn't already in the queue or completely solved, raise the event to add it
            if (!InSolverQueue && !Solved)
            {
                EventHandler<TileGroupUpdatingEventArgs> handler = TileGroupUpdated;
                if (handler != null)
                    handler(this, new TileGroupUpdatingEventArgs(this));

                // Set this so the group knows it's already queued to avoid adding it more than once
                InSolverQueue = true;
            }
        }

        // Update the possible values for this group and each of its tile to remove the given value
        public void UpdatePossibleValues(int val)
        {
            ActionRecorder.Record(string.Format("Group {0}: Removing Possible Value {1}.", Id, val));
            //Console.WriteLine("Group {0}: Removing Possible Value {1}.", Id, val);
            AllPossibleValues.Remove(val);

            // Remove this value from the possible values of all other tiles that aren't already solved.
            foreach (var tile in Tiles.Where(x => x.State != TileStates.Solved))
            {
                tile.RemovePossibleValue(val);
            } 
        }

        public override string ToString()
        {
            return string.Format("{0}: {1}, {2}", Id, IsValid() ? "Valid" : "Not Valid", Solved ? "Solved" : "Not Solved");
        }
    }
}
