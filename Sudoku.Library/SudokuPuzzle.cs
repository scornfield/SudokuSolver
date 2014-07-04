using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cornfield.Sudoku.Library.Interfaces;

namespace Cornfield.Sudoku.Library
{
    public class SudokuPuzzle<G, T> : ISudokuPuzzle<G, T> where G: ISudokuTileGroup<T> where T: ISudokuTile
    {
        public List<List<T>> Board { get; set; }
        public List<G> TileGroups { get; set; }
        public int Id { get; set; }
        public string Solver { get; set; }

        public void PrintBoard()
        {
            foreach (var row in Board)
            {
                //Console.WriteLine(string.Join(" ", row));
            }
        }
    }
}
