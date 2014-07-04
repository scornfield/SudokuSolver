using Cornfield.Sudoku.Library;
using Cornfield.Sudoku.Library.Interfaces;
using Cornfield.SudokuSolver.Library;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cornfield.SudokuSolver.UI
{
    public partial class SudokuSolver : Form
    {
        public SudokuSolver()
        {
            InitializeComponent();

            /*HttpWebRequest wr = (HttpWebRequest)WebRequest.Create("http://codecampcontest.azurewebsites.net/api/test");
            HttpWebResponse response = (HttpWebResponse)wr.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string json = reader.ReadToEnd();

            reader.Close();
            Console.WriteLine(json);
            Console.ReadLine();

            DateTime begin = DateTime.UtcNow;

            SmartSudokuPuzzle puz = JsonConvert.DeserializeObject<SmartSudokuPuzzle>(json);
            lblTitle.Text = "Sudoku Solver Warming Up...";
            puz.Solver = "Andrew Nguyen";

            puz.Init();
            puz.AddSolver(new PlaceFindingSolver());

            puz.PrintBoard();

            puz.Solve();

            Console.WriteLine();
            ShowBoard(puz);



            DateTime end = DateTime.UtcNow;

            if (puz.TileGroups.Any(x => !x.IsValid())) txtResponseJson.Text = "Board is invalid";

            ShowBoard(puz);


            lblTime.Text = "Measured time: " + (end - begin).TotalMilliseconds + " ms.";
            puz.PrintBoard();
            Console.ReadLine();*/
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            HttpWebRequest wr = (HttpWebRequest)WebRequest.Create("http://codecampcontest.azurewebsites.net/api/random");
            HttpWebResponse response = (HttpWebResponse)wr.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string json = reader.ReadToEnd();

            reader.Close();
            Console.WriteLine(json);
            Console.ReadLine();

            DateTime begin = DateTime.UtcNow;

            SmartSudokuPuzzle puz = JsonConvert.DeserializeObject<SmartSudokuPuzzle>(json);
            lblTitle.Text = string.Format("Sudoku Puzzle #{0}", puz.Id);
            puz.Solver = "Andrew Nguyen";

            puz.Init();
            puz.AddSolver(new PlaceFindingSolver());

            puz.PrintBoard();

            puz.Solve();

            Console.WriteLine();
            ShowBoard(puz);



            DateTime end = DateTime.UtcNow;

            if (puz.TileGroups.Any(x => !x.IsValid())) txtResponseJson.Text = "Board is invalid";

            ShowBoard(puz);


            lblTime.Text = "Measured time: " + (end - begin).TotalMilliseconds + " ms.";
            puz.PrintBoard();
            Console.ReadLine();
        }

        private void ShowBoard(SudokuPuzzle<SmartSudokuTileGroup, SmartSudokuTile> puzzle)
        {
            grdBoard.Columns.Clear();
            grdBoard.Rows.Clear();

            for (int col = 0; col < puzzle.Board[0].Count; col++)
            {
                grdBoard.Columns.Add(col.ToString(), col.ToString());
            }
            puzzle.Board.ForEach(delegate(List<SmartSudokuTile> row) { grdBoard.Rows.Add(row.ToArray<object>()); });

            
        }

        private void grdBoard_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            var tile = ((SmartSudokuTile)grdBoard.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
            lblTileInfo.Text = tile.PossibleValues == null || tile.PossibleValues.Count == 0 ? "No Possible Values" : string.Join(", ", tile.PossibleValues);
            if(tile.State == TileStates.Solved)
                lblTileInfo.Text += "\n" + tile.Reason;
        }
    }
}

