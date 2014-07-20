using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cornfield.Sudoku.Library;
using Cornfield.Sudoku.Library.Interfaces;
using Newtonsoft.Json;

namespace Cornfield.SudokuSolver.Library
{
    public enum TileStates
    {
        NoProgress, Solved
    }

    public class SmartSudokuTile: SudokuTile, ISudokuTile
    {
        public override int? Value
        {
            get
            {
                return _value;
            }
            protected set
            {
                // TODO: After sufficient testing, remove this to increase efficiency
                if (State == TileStates.Solved)
                    throw new Exception("Trying to change value when the tile is already solved");
                
                // Set the value
                _value = value;

                // If the value is not null, then set the tile as Solved and clear the PossibleValues 
                if (_value != null)
                {
                    try
                    {
                        State = TileStates.Solved;
                        if (PossibleValues != null)
                        {
                            if (Guessed)
                                TentativelyRemovedPossibleValues.AddRange(PossibleValues);
                            PossibleValues.Clear();
                        }
                    }
                    catch (SudokuConditionViolatedException ex)
                    {
                        ActionRecorder.Record(ex.Message);

                        ClearGuesses();

                        throw;
                    }
                }
            }
        }

        [JsonIgnore]
        public List<int> PossibleValues { get; set; }

        [JsonIgnore]
        public List<int> TentativelyRemovedPossibleValues { get; set; }

        private TileStates _state;
        [JsonIgnore]
        public TileStates State {
            get
            {
                return _state;
            } 
            protected set 
            {
                // Don't do anything if our state isn't actually changing
                if (_state != value)
                {
                    _state = value;

                    // If the tile is now solved, then raise the OnTileSolved event.
                    if(_state == TileStates.Solved)
                        OnTileSolved();
                }
            } 
        }
        
        [JsonIgnore]
        public string Reason { get; set; }

        [JsonIgnore]
        public bool Guessed { get; protected set; }

        // Initialize a new tile with no value.
        public SmartSudokuTile() : this(null)
        {
            State = TileStates.NoProgress;
        }

        // Initialize a new tile with an existing value.
        public SmartSudokuTile(int? val) : base(val)
        {
            Guessed = false;
            TentativelyRemovedPossibleValues = new List<int>();

            if(val != null)
                Reason = "Initialized";
        }

        // Initialize the tile from an integer.  Used for deserializing the Json object.
        public static implicit operator SmartSudokuTile(Int64 val)
        {
            SmartSudokuTile tile = val == 0 ? new SmartSudokuTile() : new SmartSudokuTile((int)val);
            return tile;
        }

        // Set the value for the tile and the reason that it was set to this value
        public void SetValue(int val, string reason = "Unknown", bool guess = false)
        {
            ActionRecorder.Record(string.Format("{0},{1}: Setting Value {2} because {3}", XPos, YPos, val, reason));

            // TODO: After sufficient testing, remove this to increase efficiency
            if (!PossibleValues.Contains(val)) 
                throw new Exception("Trying to set the tile's value to an invalid value");

            // Set the reason for this value being set
            Reason = reason;

            // Set whether this is a guess or not
            Guessed = guess;

            // Set the value
            Value = val;
        }

        // Clear out all state information associated with a guessed value
        public void ClearValue()
        {
            _value = null;
            Reason = null;
            State = TileStates.NoProgress;
            _solveHandled = false;
            Guessed = false;
        }

        // This bool gets flipped when the tile is solved so that we don't raise our events more than once
        private bool _solveHandled = false;

        // When the tile is solved, this creates an event handler to alert all of its groups to the change
        public void OnTileSolved()
        {
            EventHandler<TileSolvedEventArgs> handler = TileSolved;

            // Make sure that we only handle this event once
            if (!_solveHandled && handler != null)
            {
                _solveHandled = true;
                handler(this, new TileSolvedEventArgs(this));
            }
        }

        // Remove a value from the remaining possible values for this tile.
        public void RemovePossibleValue(int val, bool guess = false, string reason = "Naked Single")
        {
            // If this tile is already solved, stop here
            if (State == TileStates.Solved) return;

            var strPossibleValues = string.Join(",", PossibleValues);

            // Remove the value from the possible values list
            if (PossibleValues.Remove(val))
            {
                ActionRecorder.Record(string.Format("{0},{1}: Removing {2} from possible values {3}", XPos, YPos, val, strPossibleValues));

                Guessed = guess;

                // If this removal was trigged by a guess, then remember the value just in case
                if (guess)
                    TentativelyRemovedPossibleValues.Add(val);

                // If there are no possible values left for this tile, throw an exception
                if (PossibleValues.Count == 0)
                    throw new SudokuConditionViolatedException(string.Format("No Remaining Possible Values for Tile {0},{1}", XPos, YPos));

                // Check if we're down to our last possible option
                CheckNakedSingle(reason, guess); 
            }
  
        }

        // Restore possible values that were eliminated by guessing
        public void ClearGuesses()
        {
            // If we haven't guessed that this tile then don't try to clear it
            if (!Guessed) return;

            ClearValue();
            PossibleValues.AddRange(TentativelyRemovedPossibleValues);
            TentativelyRemovedPossibleValues.Clear();
        }

        // Check if there is only one remaining possible value for this tile and if there is, set that as its value.
        public void CheckNakedSingle(string reason = "Naked Single", bool guess = false)
        {
            if (PossibleValues.Count == 1)
            {
                SetValue(PossibleValues[0], reason, guess);
                return;
            }
        }
    }
}
