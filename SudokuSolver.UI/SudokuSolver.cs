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
        protected SmartSudokuPuzzle _puzzle;
        protected bool _boardEditable = false;
        public SudokuSolver()
        {
            InitializeComponent();

            WarmUp();
        }

        private void WarmUp()
        {
            lblTitle.Text = "Sudoku Solver Running Warm Up Test";

            ActionRecorder.Init();

            //string json = getJsonBoard("http://codecampcontest.azurewebsites.net/api/test");

            //_puzzle = JsonConvert.DeserializeObject<SmartSudokuPuzzle>(json);
            //solvePuzzle();
            //solveComplete();
        }

        private void btnGetNew_Click(object sender, EventArgs e)
        {
            string json = getJsonBoard("http://codecampcontest.azurewebsites.net/api/random");
            _puzzle = JsonConvert.DeserializeObject<SmartSudokuPuzzle>(json);
            ShowBoard();
        }

        private void reset()
        {
            if (grdBoard.Rows != null) grdBoard.Rows.Clear();
            if (grdBoard.Columns != null) grdBoard.Columns.Clear();
            lblStatus.Text = "";
            lblTime.Text = "";

            if (lstActions.Items != null) lstActions.Items.Clear();
            if (ActionRecorder.Actions != null) ActionRecorder.Actions.Clear();
        }

        private string getJsonBoard(string url)
        {
            HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse)wr.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string json = reader.ReadToEnd();

            reader.Close();
            //Console.WriteLine(json);
            Console.ReadLine();

            return json;
        }

        private void solvePuzzle() 
        {
            if (_boardEditable) createNewPuzzleFromBoard();
            _puzzle.Init();
            _puzzle.AddSolver(new PlaceFindingSolver());
            _puzzle.AddSolver(new OccupancyTheoremSolver());
            _puzzle.AddSolver(new EliminationSolver());

            _puzzle.Solve();
        }

        private void solveComplete(TimeSpan time)
        {
            solveComplete();
            lblTime.Text = time.TotalMilliseconds + " ms.";
        }

        private void solveComplete()
        {
            lblStatus.Text = "Solve Complete.";
            if (_puzzle.TileGroups.Any(x => !x.Solved)) lblStatus.Text += " Board is incomplete.";
            if (_puzzle.TileGroups.Any(x => !x.IsValid())) lblStatus.Text += " Board is invalid.";

            ShowBoard();
            lstActions.Items.AddRange(ActionRecorder.Actions.ToArray());
        }

        private void ShowBoard()
        {
            grdBoard.Columns.Clear();
            grdBoard.Rows.Clear();

            for (int col = 0; col < _puzzle.Board[0].Count; col++)
            {
                grdBoard.Columns.Add(col.ToString(), col.ToString());
            }
            _puzzle.Board.ForEach(delegate(List<SmartSudokuTile> row) { grdBoard.Rows.Add(row.ToArray<object>()); });
        }

        private void grdBoard_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (_boardEditable) return;

            var tile = ((SmartSudokuTile)grdBoard.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
            if (tile == null) return;

            lblTileInfo.Text = tile.PossibleValues == null || tile.PossibleValues.Count == 0 ? "No Possible Values" : string.Join(", ", tile.PossibleValues);
            if(tile.State == TileStates.Solved)
                lblTileInfo.Text += "\n" + tile.Reason;
        }

        private void btnCreateNew_Click(object sender, EventArgs e)
        {
            reset();
            for (int col = 0; col < 9; col++)
            {
                grdBoard.Columns.Add(col.ToString(), col.ToString());
            }
            grdBoard.Rows.Insert(0, 9);
            _boardEditable = true;
        }

        private void btnSolve_Click(object sender, EventArgs e)
        {
            DateTime begin = DateTime.UtcNow;

            solvePuzzle();

            DateTime end = DateTime.UtcNow;
            solveComplete(end - begin);
        }

        private void grdBoard_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (!_boardEditable) e.Cancel = true;
        }

        private void createNewPuzzleFromBoard()
        {
            _puzzle = new SmartSudokuPuzzle();
            _puzzle.Board = new List<List<SmartSudokuTile>>();
            foreach (DataGridViewRow row in grdBoard.Rows)
            {
                var puzRow = new List<SmartSudokuTile>();
                foreach (DataGridViewCell col in row.Cells)
                {
                    if (col.Value == null || string.IsNullOrWhiteSpace(col.Value.ToString()))
                        puzRow.Add(new SmartSudokuTile());
                    else
                        puzRow.Add(new SmartSudokuTile(int.Parse(col.Value.ToString())));
                }
                _puzzle.Board.Add(puzRow);
            }
            _puzzle.Init();
        }
    }
}

