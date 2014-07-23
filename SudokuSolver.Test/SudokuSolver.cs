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

namespace Cornfield.SudokuSolver.Test
{
    public partial class SudokuSolver : Form
    {
        protected SudokuPuzzleSolver _puzzle;
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

            string json = getJsonBoard("http://codecampcontest.azurewebsites.net/api/test");

            _puzzle = JsonConvert.DeserializeObject<SudokuPuzzleSolver>(json);
            _puzzle.Solver = "Steven Cornfield";
            solvePuzzle();
            solveComplete();
        }

        private void GetNewPuzzle()
        {
            string json = getJsonBoard("http://codecampcontest.azurewebsites.net/api/random");
            _puzzle = JsonConvert.DeserializeObject<SudokuPuzzleSolver>(json);
            _puzzle.Solver = "Steven Cornfield";
        }

        private void btnGetNew_Click(object sender, EventArgs e)
        {
            GetNewPuzzle(); 
            lblTitle.Text = string.Format("Sudoku Puzzle #{0}", _puzzle.Id);
            reset();
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

        private string submitJsonBoard()
        {
            var request = (HttpWebRequest)WebRequest.Create("http://codecampcontest.azurewebsites.net/api/checkanswer");

            var data = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(new SerializablePuzzle(_puzzle)));

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

        private void solvePuzzle() 
        {
            if (_boardEditable) createNewPuzzleFromBoard();
            _puzzle.Init();
            _puzzle.AddSolver(new HiddenSingleSolver());
            _puzzle.AddSolver(new NakedMultipleSolver());
            _puzzle.AddSolver(new HiddenMultipleSolver());
            _puzzle.AddSolver(new BowmanBingoSolver());

            _puzzle.Solve();
        }

        private void solveComplete(TimeSpan time)
        {
            solveComplete();
            lblTime.Text = time.TotalMilliseconds + " ms.";
        }

        private void solveComplete()
        {
            if (_puzzle.Solved && _puzzle.IsValid())
            {
                submitJsonBoard();
            }

            lblStatus.Text = "Solve Complete.";
            if (_puzzle.TileGroups.Any(x => !x.Solved)) lblStatus.Text += " Board is incomplete.";
            if (!_puzzle.IsValid()) lblStatus.Text += " Board is invalid.";

            ShowBoard();
            lstActions.Items.AddRange(ActionRecorder.Actions.ToArray());

            txtJson.Text = JsonConvert.SerializeObject(new SerializablePuzzle(_puzzle));
        }

        private void ShowBoard()
        {
            grdBoard.Columns.Clear();
            grdBoard.Rows.Clear();

            for (int col = 0; col < _puzzle.Board[0].Count; col++)
            {
                grdBoard.Columns.Add(col.ToString(), col.ToString());
            }
            _puzzle.Board.ForEach(delegate(List<SudokuTileSolver> row) { grdBoard.Rows.Add(row.ToArray<object>()); });
        }

        private void grdBoard_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (_boardEditable) return;

            var tile = ((SudokuTileSolver)grdBoard.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
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
            _puzzle = new SudokuPuzzleSolver();
            _puzzle.Board = new List<List<SudokuTileSolver>>();
            foreach (DataGridViewRow row in grdBoard.Rows)
            {
                var puzRow = new List<SudokuTileSolver>();
                foreach (DataGridViewCell col in row.Cells)
                {
                    if (col.Value == null || string.IsNullOrWhiteSpace(col.Value.ToString()))
                        puzRow.Add(new SudokuTileSolver());
                    else
                        puzRow.Add(new SudokuTileSolver(int.Parse(col.Value.ToString())));
                }
                _puzzle.Board.Add(puzRow);
            }
            _puzzle.Init();
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                foreach (object row in lstActions.Items)
                {
                    sb.Append(row.ToString());
                    sb.AppendLine();
                }
                sb.Remove(sb.Length - 1, 1); // Just to avoid copying last empty row
                Clipboard.SetData(System.Windows.Forms.DataFormats.Text, sb.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnRunMany_Click(object sender, EventArgs e)
        {
            TimeSpan totalTime = new TimeSpan();
            int numPuzzles = 100;

            lblTitle.Text = string.Format("Running {0} Puzzles", numPuzzles);
            Refresh();
            int i = 0;

            for (i = 0; i < numPuzzles; i++)
            {
                ActionRecorder.Actions.Clear();
                DateTime begin = DateTime.UtcNow;
                GetNewPuzzle();
                //lblTitle.Text = string.Format("Sudoku Puzzle #{0}", _puzzle.Id);
                reset();
                //ShowBoard();

                solvePuzzle();

                DateTime end = DateTime.UtcNow;
                totalTime += end - begin;
                //solveComplete(end - begin);

                if (!_puzzle.Solved || !_puzzle.IsValid())
                {
                    ShowBoard();
                    solveComplete(totalTime);
                    MessageBox.Show("Incomplete or invalid puzzle.  Sorry...");
                    break;
                }
                else
                {
                    if (submitJsonBoard() != "true")
                    {
                        ShowBoard();
                        solveComplete(totalTime);
                        MessageBox.Show("Incomplete or invalid puzzle.  Sorry...");
                        break;
                    }
                }
            }

            lblTitle.Text = string.Format("Solved {0} Puzzles", i);
            lblStatus.Text = string.Format("Solved {0} puzzles in {1}ms.  {2}ms per puzzle.", i, totalTime.TotalMilliseconds, totalTime.TotalMilliseconds / i);
            
        }
    }
}

