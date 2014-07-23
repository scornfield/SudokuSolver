using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Cornfield.Sudoku.Library;
using Cornfield.SudokuSolver.Library;
using System.Diagnostics;

namespace Cornfield.SudokuSolver
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                HttpWebRequest wr = (HttpWebRequest)WebRequest.Create("http://codecampcontest.azurewebsites.net/api/random");
                HttpWebResponse response = (HttpWebResponse)wr.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());
                string json = reader.ReadToEnd();

                reader.Close();
                //Console.WriteLine(json);
                Console.ReadLine();

                TimeSpan begin = Process.GetCurrentProcess().TotalProcessorTime;

                StandardSukoduPuzzle<SudokuTileGroupSolver, SudokuTileSolver> puz = JsonConvert.DeserializeObject<StandardSukoduPuzzle<SudokuTileGroupSolver, SudokuTileSolver>>(json);
                puz.Solver = "Andrew Nguyen";
                puz.InitTileGroups();

                puz.PrintBoard();

                //Console.WriteLine();

                puz.TileGroups.ForEach(x => x.Init());

                TimeSpan end = Process.GetCurrentProcess().TotalProcessorTime;

                //Console.WriteLine("Measured time: " + (end - begin).TotalMilliseconds + " ms.");
                puz.PrintBoard();
                Console.ReadLine();
            }
        }
    }
}
