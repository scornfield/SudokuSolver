namespace Cornfield.SudokuSolver.UI
{
    partial class SudokuSolver
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.grdBoard = new System.Windows.Forms.DataGridView();
            this.btnGetNew = new System.Windows.Forms.Button();
            this.lblTileInfo = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lstActions = new System.Windows.Forms.ListBox();
            this.actionRecorderBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.lblTime = new System.Windows.Forms.ToolStripLabel();
            this.btnCreateNew = new System.Windows.Forms.Button();
            this.btnSolve = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.grdBoard)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.actionRecorderBindingSource)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // grdBoard
            // 
            this.grdBoard.AllowUserToAddRows = false;
            this.grdBoard.AllowUserToDeleteRows = false;
            this.grdBoard.AllowUserToResizeColumns = false;
            this.grdBoard.AllowUserToResizeRows = false;
            this.grdBoard.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.grdBoard.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdBoard.ColumnHeadersVisible = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.grdBoard.DefaultCellStyle = dataGridViewCellStyle1;
            this.grdBoard.Location = new System.Drawing.Point(12, 130);
            this.grdBoard.Name = "grdBoard";
            this.grdBoard.RowHeadersVisible = false;
            this.grdBoard.RowTemplate.Height = 28;
            this.grdBoard.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.grdBoard.Size = new System.Drawing.Size(704, 584);
            this.grdBoard.TabIndex = 0;
            this.grdBoard.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.grdBoard_CellBeginEdit);
            this.grdBoard.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdBoard_CellEnter);
            // 
            // btnGetNew
            // 
            this.btnGetNew.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGetNew.Location = new System.Drawing.Point(515, 817);
            this.btnGetNew.Name = "btnGetNew";
            this.btnGetNew.Size = new System.Drawing.Size(442, 77);
            this.btnGetNew.TabIndex = 3;
            this.btnGetNew.Text = "Get New Puzzle";
            this.btnGetNew.UseVisualStyleBackColor = true;
            this.btnGetNew.Click += new System.EventHandler(this.btnGetNew_Click);
            // 
            // lblTileInfo
            // 
            this.lblTileInfo.AutoSize = true;
            this.lblTileInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTileInfo.Location = new System.Drawing.Point(12, 717);
            this.lblTileInfo.Name = "lblTileInfo";
            this.lblTileInfo.Size = new System.Drawing.Size(102, 37);
            this.lblTileInfo.TabIndex = 7;
            this.lblTileInfo.Text = "label2";
            // 
            // lblTitle
            // 
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 26F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(12, 9);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(1420, 118);
            this.lblTitle.TabIndex = 8;
            this.lblTitle.Text = "Cornfield Sudoku Solver";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lstActions
            // 
            this.lstActions.FormattingEnabled = true;
            this.lstActions.ItemHeight = 20;
            this.lstActions.Location = new System.Drawing.Point(723, 131);
            this.lstActions.Name = "lstActions";
            this.lstActions.Size = new System.Drawing.Size(709, 584);
            this.lstActions.TabIndex = 11;
            // 
            // actionRecorderBindingSource
            // 
            this.actionRecorderBindingSource.DataSource = typeof(Cornfield.SudokuSolver.Library.ActionRecorder);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus,
            this.toolStripSeparator1,
            this.lblTime});
            this.toolStrip1.Location = new System.Drawing.Point(0, 956);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1444, 28);
            this.toolStrip1.TabIndex = 13;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // lblStatus
            // 
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(132, 25);
            this.lblStatus.Text = "toolStripLabel1";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 28);
            // 
            // lblTime
            // 
            this.lblTime.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(132, 25);
            this.lblTime.Text = "toolStripLabel2";
            // 
            // btnCreateNew
            // 
            this.btnCreateNew.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCreateNew.Location = new System.Drawing.Point(67, 817);
            this.btnCreateNew.Name = "btnCreateNew";
            this.btnCreateNew.Size = new System.Drawing.Size(442, 77);
            this.btnCreateNew.TabIndex = 14;
            this.btnCreateNew.Text = "Create New Puzzle";
            this.btnCreateNew.UseVisualStyleBackColor = true;
            this.btnCreateNew.Click += new System.EventHandler(this.btnCreateNew_Click);
            // 
            // btnSolve
            // 
            this.btnSolve.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSolve.Location = new System.Drawing.Point(963, 817);
            this.btnSolve.Name = "btnSolve";
            this.btnSolve.Size = new System.Drawing.Size(442, 77);
            this.btnSolve.TabIndex = 15;
            this.btnSolve.Text = "Solve Puzzle";
            this.btnSolve.UseVisualStyleBackColor = true;
            this.btnSolve.Click += new System.EventHandler(this.btnSolve_Click);
            // 
            // SudokuSolver
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1444, 984);
            this.Controls.Add(this.btnSolve);
            this.Controls.Add(this.btnCreateNew);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.lstActions);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.lblTileInfo);
            this.Controls.Add(this.btnGetNew);
            this.Controls.Add(this.grdBoard);
            this.Name = "SudokuSolver";
            this.Text = "SudokuSolver";
            ((System.ComponentModel.ISupportInitialize)(this.grdBoard)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.actionRecorderBindingSource)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView grdBoard;
        private System.Windows.Forms.Button btnGetNew;
        private System.Windows.Forms.Label lblTileInfo;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.ListBox lstActions;
        private System.Windows.Forms.BindingSource actionRecorderBindingSource;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel lblStatus;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripLabel lblTime;
        private System.Windows.Forms.Button btnCreateNew;
        private System.Windows.Forms.Button btnSolve;
    }
}

