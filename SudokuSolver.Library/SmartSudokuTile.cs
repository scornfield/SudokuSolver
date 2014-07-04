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
        Solved, Guessed, NoProgress
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
                _value = value;
                State = TileStates.Solved;
                OnTileSolved();
                if (PossibleValues != null)
                    PossibleValues.Clear();
                
            }
        }

        [JsonIgnore]
        public HashSet<int> PossibleValues { get; protected set; }

        [JsonIgnore]
        public TileStates State { get; protected set; }
        
        [JsonIgnore]
        public string Reason { get; set; }

        [JsonIgnore]
        private int? TempValue { get; set; }

        public SmartSudokuTile() : base()
        {
            PossibleValues = new HashSet<int>() {1, 2, 3, 4, 5, 6, 7, 8, 9};
            State = TileStates.NoProgress;
        }

        public SmartSudokuTile(int? val) : base(val)
        {
            State = TileStates.Solved;
            Reason = "Initialized";
        }

        public void SetValue(int val, TileConfidence conf, string reason = "Unknown")
        {
            //Console.WriteLine("{0},{1}: Setting Value {2} because {3}", XPos, YPos, val, reason);
            if (!PossibleValues.Contains(val)) 
            { 
                //Console.WriteLine("Don't do it!"); 
                return; 
            }


            Reason = reason;
            if (conf == TileConfidence.Certain)
                Value = val;
            else
                TempValue = val;
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

        public static implicit operator SmartSudokuTile(Int64 val)
        {
            SmartSudokuTile tile = val == 0 ? new SmartSudokuTile() : new SmartSudokuTile((int)val);
            return tile;
        }

        public void OnTileSolved()
        {
            //Console.WriteLine("{0},{1}: Firing Events", XPos, YPos);
            EventHandler<TileSolvedEventArgs> handler = TileSolved;
            if (handler != null)
                handler(this, new TileSolvedEventArgs(this));
            //Console.WriteLine("{0},{1}: Events Complete", XPos, YPos);
        }

        public void RemovePossibleValue(int val)
        {
            PossibleValues.Remove(val);
            
            if (PossibleValues.Count == 1)
            {
                SetValue(PossibleValues.ElementAt(0), TileConfidence.Certain, "Only Remaining Possibility");
                return;
            }

            if (PossibleValues.Count == 0)
                throw new Exception("No Remaining Possible Values for Tile");
        }

    }
}
