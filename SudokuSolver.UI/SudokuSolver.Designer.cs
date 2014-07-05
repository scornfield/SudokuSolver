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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.grdBoard = new System.Windows.Forms.DataGridView();
            this.btnNew = new System.Windows.Forms.Button();
            this.lblTime = new System.Windows.Forms.Label();
            this.lblTileInfo = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lstActions = new System.Windows.Forms.ListBox();
            this.actionRecorderBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.grdBoard)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.actionRecorderBindingSource)).BeginInit();
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
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.grdBoard.DefaultCellStyle = dataGridViewCellStyle2;
            this.grdBoard.Location = new System.Drawing.Point(12, 130);
            this.grdBoard.Name = "grdBoard";
            this.grdBoard.RowHeadersVisible = false;
            this.grdBoard.RowTemplate.Height = 28;
            this.grdBoard.Size = new System.Drawing.Size(704, 584);
            this.grdBoard.TabIndex = 0;
            this.grdBoard.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdBoard_CellEnter);
            // 
            // btnNew
            // 
            this.btnNew.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNew.Location = new System.Drawing.Point(484, 895);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(442, 77);
            this.btnNew.TabIndex = 3;
            this.btnNew.Text = "Get and Solve New Puzzle";
            this.btnNew.UseVisualStyleBackColor = true;
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // lblTime
            // 
            this.lblTime.AutoSize = true;
            this.lblTime.Location = new System.Drawing.Point(1144, 895);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(51, 20);
            this.lblTime.TabIndex = 5;
            this.lblTime.Text = "label1";
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
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus.Location = new System.Drawing.Point(12, 882);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(102, 37);
            this.lblStatus.TabIndex = 10;
            this.lblStatus.Text = "label2";
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
            // SudokuSolver
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1444, 984);
            this.Controls.Add(this.lstActions);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.lblTileInfo);
            this.Controls.Add(this.lblTime);
            this.Controls.Add(this.btnNew);
            this.Controls.Add(this.grdBoard);
            this.Name = "SudokuSolver";
            this.Text = "SudokuSolver";
            ((System.ComponentModel.ISupportInitialize)(this.grdBoard)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.actionRecorderBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView grdBoard;
        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.Label lblTime;
        private System.Windows.Forms.Label lblTileInfo;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.ListBox lstActions;
        private System.Windows.Forms.BindingSource actionRecorderBindingSource;
    }
}

