using Cornfield.Sudoku.Library;
using Cornfield.Sudoku.Library.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cornfield.SudokuSolver.Library
{
    public class SmartSudokuPuzzle : StandardSukoduPuzzle<SmartSudokuTileGroup, SmartSudokuTile>, ISudokuPuzzle<SmartSudokuTileGroup, SmartSudokuTile>
    {
        private List<SmartSudokuTileGroup> _updateQueue = new List<SmartSudokuTileGroup>();

        public void Init()
        {
            InitTileGroups();
            foreach (var group in TileGroups)
            {
                group.Init();
                group.AddSolver(new PlaceFindingSolver());
                group.TileGroupUpdating += AddGroupToQueue;
            }
        }

        public void Solve()
        {
            foreach (var group in TileGroups)
            {
                group.Solve();
            }
            
            while (_updateQueue.Count > 0)
            {
                _updateQueue[0].RunSolvers();
                _updateQueue.RemoveAt(0);
            }
        }

        public void AddGroupToQueue(object sender, TileGroupUpdatingEventArgs args)
        {
            if (!_updateQueue.Contains(args.Group))
                _updateQueue.Add(args.Group);
        }
    }
}
