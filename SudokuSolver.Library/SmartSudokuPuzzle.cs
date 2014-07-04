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
            // Loop until we run out of groups in the queue
            while (_solverQueue.Count > 0)
            {
                foreach (var solver in _solvers)
                {
                    _solverQueue[0].InSolverQueue = false;
                    solver.SolveGroup(_solverQueue[0]);
                }
                _solverQueue.RemoveAt(0);
            }
        }

        // Add a group to our solver queue to be processed
        public void AddGroupToQueue(object sender, TileGroupUpdatingEventArgs args)
        {
            _solverQueue.Add(args.Group);
        }
    }
}
