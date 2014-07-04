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
        public List<ISudokuSolver> Solvers = new List<ISudokuSolver>();
        public HashSet<int> AllPossibleValues = new HashSet<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        public EventHandler<TileGroupUpdatingEventArgs> TileGroupUpdating;
        public SmartSudokuTileGroup() : base()
        {

        }

        public new void Init()
        {
            // Add the TileSolved event handler to each tile in this group
            Tiles.ForEach(delegate(SmartSudokuTile tile) { tile.TileSolved += this.TileSolved; });
        }

        public void Solve()
        {
            // Fire the OnTileSolved on all of our initially filled in tiles
            Tiles.ForEach(delegate(SmartSudokuTile tile) { if (tile.State == TileStates.Solved) tile.OnTileSolved(); });
        }

        public void AddSolver(ISudokuSolver solver)
        {
            Solvers.Add(solver);
        }

        public void TileSolved(object sender, TileSolvedEventArgs args)
        {
            int val = (int)args.Tile.Value;

            //Console.WriteLine("{0},{1}: Solved.  Group {2} Updating.", args.Tile.XPos, args.Tile.YPos, Id);
            UpdatePossibleValues(val);
            
            EventHandler<TileGroupUpdatingEventArgs> handler = TileGroupUpdating;
            if (handler != null)
                handler(this, new TileGroupUpdatingEventArgs(this));
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

        public void RunSolvers() 
        {
            //Console.WriteLine("Group {0} running solvers", Id);
            //if (!IsValid())
                //Console.WriteLine("Group {0} is invalid before Solvers", Id);
            foreach(var solver in Solvers) solver.SolveGroup(this);
            //if (!IsValid())
                //Console.WriteLine("Group {0} is invalid after Solvers", Id);
        }
    }
}
