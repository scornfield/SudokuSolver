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
    public class SudokuTileGroupSolver : SudokuTileGroup<SudokuTileSolver>, ISudokuTileGroup<SudokuTileSolver>
    {
        public List<int> AllPossibleValues = new List<int>();
        public EventHandler<TileGroupUpdatingEventArgs> TileGroupUpdated;
        public bool Solved { get { return AllPossibleValues.Count == 0; } }
        public SudokuTileGroupSolver() : base()
        {

        }

        // Initializes the tiles by seting up event handlers and possible values
        public new void Init()
        {
            // Add the TileSolved event handler to each tile in this group
            Tiles.ForEach(delegate(SudokuTileSolver tile) { tile.TileSolved += this.TileSolved; });
            
            // Initialize the Possible Values for the group and each tile
            for (int i = 1; i <= Tiles.Count; i++) { AllPossibleValues.Add(i); }
            Tiles.ForEach(delegate(SudokuTileSolver tile) { if(tile.State != TileStates.Solved) tile.PossibleValues = AllPossibleValues.ToList(); });
        }

        public void RecalcPossibleValues()
        {
            AllPossibleValues.Clear();
            for (int i = 1; i <= Tiles.Count; i++) { AllPossibleValues.Add(i); }

            AllPossibleValues.RemoveAll(x => Tiles.Any(t => t.State == TileStates.Solved && t.Value == x));
        }

        // This will handle the TileSolved event for each of the tiles by updating the remaining possible values for each tile in the group 
        // and alerting the board that this group has been updated
        public void TileSolved(object sender, TileSolvedEventArgs args)
        {
            int val = (int)args.Tile.Value;
            bool guess = ((SudokuTileSolver)args.Tile).Guessed;

            if (!UpdatePossibleValues(val, guess))
            {
                ActionRecorder.Record(string.Format("Group {0}: ERROR: While removing {1} from possible values for the group.", Id, val));
                return;
            }

            // The group is invalid now
            if (!IsValid())
            {
                ActionRecorder.Record(string.Format("Group {0}: ERROR: Group was invalidated by solving tile {1},{2}.", Id, args.Tile.XPos, args.Tile.YPos));
                return;
            } 

            OnTileGroupUpdated(guess);
        }

        // This gets called any time that a tile in this group is solved or has its remaining possible values updated
        public void OnTileGroupUpdated(bool guess = false)
        {
            // If this tile group isn't already completely solved, and this wasn't a guess, raise the event to alert the board that it's been updated
            if (Solved || guess) return;

            EventHandler<TileGroupUpdatingEventArgs> handler = TileGroupUpdated;
            if (handler != null)
                handler(this, new TileGroupUpdatingEventArgs(this));
        }

        // Update the possible values for this group and each of its tile to remove the given value
        // Returns true if all updates were successful and false if there were any errors (tiles reduced to no possible remaining values)
        public bool UpdatePossibleValues(int val, bool guess = false)
        {
            ActionRecorder.Record(string.Format("Group {0}: Removing Possible Value {1}.", Id, val));
            AllPossibleValues.Remove(val);

            // If the group is solved, stop here.
            if (Solved) return true;

            // Remove this value from the possible values of all other tiles that aren't already solved.
            bool isError = false;
            foreach (var tile in Tiles.Where(x => x.State != TileStates.Solved && x.PossibleValues.Contains(val)))
            {
                if (!tile.RemovePossibleValue(val, guess))
                    isError = true;
            }
            return !isError;
        }

        public override void AddTile(SudokuTileSolver tile) 
        {
            base.AddTile(tile);

            if (Type == GroupType.Row) tile.RowGroup = this;
            if (Type == GroupType.Column) tile.ColumnGroup = this;
            if (Type == GroupType.Box) tile.BoxGroup = this;
        }

        public override string ToString()
        {
            return string.Format("{0}: {1}, {2}", Id, IsValid() ? "Valid" : "Not Valid", Solved ? "Solved" : "Not Solved");
        }
    }
}
