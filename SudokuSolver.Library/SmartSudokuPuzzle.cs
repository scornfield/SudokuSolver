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
    public class SmartSudokuPuzzle : StandardSukoduPuzzle<SmartSudokuTileGroup, SmartSudokuTile>, ISudokuPuzzle<SmartSudokuTileGroup, SmartSudokuTile>
    {
        private List<SmartSudokuTileGroup> _solverQueue = new List<SmartSudokuTileGroup>();
        private List<ISudokuSolver> _solvers = new List<ISudokuSolver>();
        
        // Initialize the puzzle
        public void Init()
        {
            // Initialize all of the groups to a standard puzzle
            InitTileGroups();

            // Initialize each group - this will wire up the event system for handling tile updates
            foreach (var group in TileGroups)
            {
                group.Init();
                group.TileGroupUpdated += AddGroupToQueue;
            }
        }

        // Add a custom solver to this puzzle
        public void AddSolver(ISudokuSolver solver)
        {
            _solvers.Add(solver);
        }

        // Try to solve the puzzle
        public void Solve()
        {
            // Start each groups self-solving events
            foreach (var group in TileGroups)
            {
                group.Start();
            }

            // Run our custom solvers
            RunSolvers();
        }

        // Run each solver on every group in the queue
        public void RunSolvers()
        {
            // Loop through our solvers and process each group in the queue
            // TODO: Try to get it to do all of the more efficient solvers first.
            while (_solverQueue.Count > 0)
            {
                var group = _solverQueue[0];
                _solverQueue.RemoveAt(0);
                group.InSolverQueue = false;
                foreach (var solver in _solvers)
                {
                    if (group.Solved) break;
                    //Console.WriteLine("Group {0}: Running {1}", group.Id, solver.ToString());
                    
                    solver.SolveGroup(group);
                }
                //if(!group.IsValid()) Console.WriteLine("Group {0} is now invalid", _solverQueue[0].Id);
                
            }
        }

        // Add a group to our solver queue to be processed
        public void AddGroupToQueue(object sender, TileGroupUpdatingEventArgs args)
        {
            _solverQueue.Add(args.Group);
        }
    }
}
