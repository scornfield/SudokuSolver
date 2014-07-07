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
        NoProgress, Guessed, Solved
    }

    public enum TileConfidence
    {
        Certain, Guess
    }


    public class SmartSudokuTile: SudokuTile, ISudokuTile
    {
        

        public override int? Value
        {
            get
            {
                return _value ?? TempValue;
            }
            protected set
            {
                if (State == TileStates.Solved) 
                    Console.WriteLine("Already Solved");
                
                // Set the value
                _value = value;

                // If the tile is not null, then set it as solved and raise the TileSolved event.
                if (_value != null)
                {
                    State = TileStates.Solved;
                    if (PossibleValues != null)
                        PossibleValues.Clear();
                }
            }
        }

        [JsonIgnore]
        public List<int> PossibleValues { get; set; }

        private TileStates _state;
        [JsonIgnore]
        public TileStates State {
            get
            {
                return _state;
            } 
            protected set 
            {
                if (_state != value)
                {
                    _state = value;
                    OnTileSolved();
                }
            } 
        }
        
        [JsonIgnore]
        public string Reason { get; set; }

        [JsonIgnore]
        private int? TempValue { get; set; }

        // Initialize a new tile with no value.
        public SmartSudokuTile() : this(null)
        {
            State = TileStates.NoProgress;
        }

        // Initialize a new tile with an existing value.
        public SmartSudokuTile(int? val) : base(val)
        {
            if(val != null)
                Reason = "Initialized";
        }

        // Initialize the tile from an integer.  Used for deserializing the Json object.
        public static implicit operator SmartSudokuTile(Int64 val)
        {
            SmartSudokuTile tile = val == 0 ? new SmartSudokuTile() : new SmartSudokuTile((int)val);
            return tile;
        }

        private bool _solveHandled = false;

        public void SetValue(int val, TileConfidence conf, string reason = "Unknown")
        {
            ActionRecorder.Record(string.Format("{0},{1}: Setting Value {2} because {3}", XPos, YPos, val, reason));
            if (!PossibleValues.Contains(val)) 
                throw new Exception("Trying to set the tile's value to an invalid value");
            PossibleValues.Clear();

            // Set the reason for this value being set
            Reason = reason;

            // If this isn't a guess, then set the value, otherwise set the temporary value
            if (conf == TileConfidence.Certain)
                Value = val;
            else
            {
                State = TileStates.Guessed;
                TempValue = val;
            }
        }

        // When the tile is solved, this creates an event handler to alert all of its groups to the change
        public void OnTileSolved()
        {
            //Console.WriteLine("{0},{1}: Firing Events", XPos, YPos);
            EventHandler<TileSolvedEventArgs> handler = TileSolved;
            if (!_solveHandled && handler != null)
            {
                _solveHandled = true;
                handler(this, new TileSolvedEventArgs(this));
            }
                
            //Console.WriteLine("{0},{1}: Events Complete", XPos, YPos);
        }

        // Remove a value from the remaining possible values for this tile.
        public void RemovePossibleValue(int val, bool setValue = true)
        {
            // If this tile is already solved, stop here
            if (State == TileStates.Solved) return;

            ActionRecorder.Record(string.Format("{0},{1}: Removing {2} from possible values {3}", XPos, YPos, val, string.Join(",",PossibleValues)));

            // Remove the value from the possible values list
            PossibleValues.Remove(val);

            // If we are allowed to set the value now, then do it
            if (setValue)
            {
                CheckNakedSingle();
                return;
            }

            // If there are no possible values left for this tile, throw an exception
            if (PossibleValues.Count == 0)
                throw new Exception("No Remaining Possible Values for Tile");
        }

        // Check if there is only one remaining possible value for this tile and if there is, set that as its value.
        public void CheckNakedSingle(string reason = "Naked Single", TileConfidence confidence = TileConfidence.Certain)
        {
            if (PossibleValues.Count == 1)
            {
                SetValue(PossibleValues[0], confidence, reason);
                return;
            }
        }

        public void ClearGuess()
        {
            State = TileStates.NoProgress;
            if (TempValue != null)
                PossibleValues.Remove((int)TempValue);
            TempValue = null;
        }

        public void ConfirmValue()
        {
            Value = TempValue;
            TempValue = null;
            PossibleValues.Clear();
        }

    }
}
