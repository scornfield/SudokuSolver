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
        private List<SmartSudokuTileGroup> _groupUpdatedQueue = new List<SmartSudokuTileGroup>();
        private List<ISudokuSolver> _solvers;
        private int _solverIndex = 0;
        public List<int> _queueIndex = new List<int>();
        
        public bool Solved
        {
            get
            {
                return TileGroups.All(x => x.Solved);
            }
        }
        
        // Initialize the puzzle
        public void Init()
        {
            // Initialize the list of solvers
            _solvers = new List<ISudokuSolver>();

            // Initialize all of the groups to a standard puzzle
            InitTileGroups();

            // Initialize each group - this will wire up the event system for handling tile updates
            foreach (var group in TileGroups)
            {
                group.Init();
                group.TileGroupUpdated += AddGroupToQueue;
            }
        }

        // Add a custom solver to this puzzle.  Solvers should be added in order of increasing complexity.
        public void AddSolver(ISudokuSolver solver)
        {
            _solvers.Add(solver);
        }

        // Try to solve the puzzle
        public void Solve()
        {
            // Fire off the solved event on each already initialized tile to begin solving the puzzle
            foreach (var row in Board)
                foreach (var tile in row)
                    if(tile.State == TileStates.Solved)
                        tile.OnTileSolved();

            if (!Solved)
            {
                ActionRecorder.Record("All Tiles Initialized - Starting Solvers");
                // Run our custom solvers
                RunSolvers();
            }
        }

        // Run each of our solvers to try to complete the puzzle
        public void RunSolvers()
        {
            // We need to keep track of which parts of the queue each solver has processed
            int i = 0; 
            List<int> queueStartIndex = new List<int>();
            List<int> queueEndIndex = new List<int>();
            List<int> groupIdsProcessed = new List<int>();

            // Initialize the start and end counts for each solver to 0.
            for (int x = 0; x < _solvers.Count; x++)
            {
                queueStartIndex.Add(0);
                queueEndIndex.Add(0);
            }

            // Loop through our solvers and execute their solve methods.
            // Solvers should be added to the list in order of increasing complexity so that we run the most efficient solvers first and most often.
            while (_solverIndex < _solvers.Count && !Solved)
            {
                // Get the solver, store the index, and increment it.
                // The _solverIndex will get reset to 0 if any groups get updated while this is running.
                var solver = _solvers[_solverIndex];
                var curSolverIndex = _solverIndex;
                _solverIndex++;

                // If this is a puzzle solver, then pass the puzzle, otherwise run through the group queue.
                if (solver.Type == SolverType.Puzzle)
                {
                    // Run the solver on the puzzle
                    ActionRecorder.Record(string.Format("{0}: Processing Puzzle", solver.ToString()));
                    solver.Solve(this);
                }
                else
                {
                    // Initialize our counter to the correct starting index for this solver.
                    i = queueStartIndex[curSolverIndex];

                    // Store the current end index of the queue because groups will get added to it as the solver runs, but we don't want to process the newly added groups again.
                    queueEndIndex[curSolverIndex] = _groupUpdatedQueue.Count;

                    // Store the Ids of each group that we process so that we don't waste time processing a group more than once for the same solver.
                    groupIdsProcessed = new List<int>();

                    // Loop through each group in the queue
                    while (i < queueEndIndex[curSolverIndex])
                    {
                        // Get the current group, and increment our group index
                        var group = _groupUpdatedQueue[i];
                        i++;

                        // If the puzzle is solved or we have processed every group for this solver already, break out of the loop.
                        if (Solved || groupIdsProcessed.Count == TileGroups.Count) break;

                        // If this group is solved or we have already processed it during this iteration of this solver, move on to the next group.
                        else if (group.Solved || groupIdsProcessed.Contains(group.Id)) continue;

                        // Add this group to the list of groups processed on this iteration
                        groupIdsProcessed.Add(group.Id);

                        // Run the solver on the current group
                        ActionRecorder.Record(string.Format("{0}: Processing Group {1}", solver.ToString(), group.Id));
                        solver.Solve(group);
                    }

                    // Store the current index so that if this solver runs again we can pick up where we left off in the group queue
                    queueStartIndex[curSolverIndex] = i;
                }
            }
        }

        // Add a group to our solver queue to be processed
        public void AddGroupToQueue(object sender, TileGroupUpdatingEventArgs args)
        {
            ActionRecorder.Record(string.Format("Adding Group {0} to solver queue.", args.Group.Id));
            _solverIndex = 0;
            _groupUpdatedQueue.Add(args.Group);
        }

        public void ClearGuesses()
        {
            foreach(var row in Board) 
            {
                foreach(var tile in row) 
                {
                    tile.ClearGuesses();
                }
            }

            foreach (var group in TileGroups)
                group.RecalcPossibleValues();
        }
    }
}
