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
        private static string getJsonBoard(string url)
        {
            HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse)wr.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string json = reader.ReadToEnd();

            reader.Close();

            return json;
        }

        private static SudokuPuzzleSolver GetNewPuzzle()
        {
            SudokuPuzzleSolver puzzle;
            string json = getJsonBoard("http://codecampcontest.azurewebsites.net/api/random");

            puzzle = JsonConvert.DeserializeObject<SudokuPuzzleSolver>(json);
            puzzle.Solver = "Steven Cornfield";

            return puzzle;
        }

        private static string submitJsonBoard(SudokuPuzzleSolver puzzle)
        {
            if (!puzzle.Solved) return "unfinished";

            var request = (HttpWebRequest)WebRequest.Create("http://codecampcontest.azurewebsites.net/api/checkanswer");
            
            var data = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(new SerializablePuzzle(puzzle)));

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;

            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            var response = (HttpWebResponse)request.GetResponse();

            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

            return responseString;
        }

        private static void solvePuzzle(SudokuPuzzleSolver puzzle)
        {
            puzzle.Init();

            puzzle.AddSolver(new HiddenSingleSolver());
            puzzle.AddSolver(new NakedMultipleSolver());
            puzzle.AddSolver(new HiddenMultipleSolver());
            puzzle.AddSolver(new IntersectionRemovalSolver());
            puzzle.AddSolver(new BowmanBingoSolver());

            puzzle.Solve();
        }

        static void Main(string[] args)
        {
            while (true)
            {
                ActionRecorder.Init();
                var timer = new Stopwatch();

                var puzzle = GetNewPuzzle();

                timer.Start();
                solvePuzzle(puzzle);
                timer.Stop();

                var result = submitJsonBoard(puzzle);
                if (result == "unfinished") continue;
                
                Console.WriteLine(string.Format("Solved puzzle #{0} in {1}ms.  API Result: '{2}'.", puzzle.Id, timer.ElapsedMilliseconds, result));
                
                Console.ReadLine();
            }
        }
    }
}
