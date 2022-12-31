namespace WindowsFormsApplication1
{
    partial class Form1
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
            this.InsertSimpleButton = new System.Windows.Forms.Button();
            this.SelectSimpleButton = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.FileContentsColumn = new System.Windows.Forms.DataGridViewImageColumn();
            this.FileNameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // InsertSimpleButton
            // 
            this.InsertSimpleButton.Location = new System.Drawing.Point(16, 39);
            this.InsertSimpleButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.InsertSimpleButton.Name = "InsertSimpleButton";
            this.InsertSimpleButton.Size = new System.Drawing.Size(240, 64);
            this.InsertSimpleButton.TabIndex = 1;
            this.InsertSimpleButton.Text = "Insert simple";
            this.InsertSimpleButton.UseVisualStyleBackColor = true;
            this.InsertSimpleButton.Click += new System.EventHandler(this.InsertSimpleButton_Click);
            // 
            // SelectSimpleButton
            // 
            this.SelectSimpleButton.Location = new System.Drawing.Point(16, 111);
            this.SelectSimpleButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.SelectSimpleButton.Name = "SelectSimpleButton";
            this.SelectSimpleButton.Size = new System.Drawing.Size(240, 64);
            this.SelectSimpleButton.TabIndex = 2;
            this.SelectSimpleButton.Text = "Select simple";
            this.SelectSimpleButton.UseVisualStyleBackColor = true;
            this.SelectSimpleButton.Click += new System.EventHandler(this.SelectSimpleButton_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.FileContentsColumn,
            this.FileNameColumn});
            this.dataGridView1.Location = new System.Drawing.Point(264, 39);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.RowTemplate.Height = 32;
            this.dataGridView1.Size = new System.Drawing.Size(447, 208);
            this.dataGridView1.TabIndex = 4;
            // 
            // FileContentsColumn
            // 
            this.FileContentsColumn.DataPropertyName = "FileContents";
            this.FileContentsColumn.HeaderText = "Contents";
            this.FileContentsColumn.MinimumWidth = 6;
            this.FileContentsColumn.Name = "FileContentsColumn";
            this.FileContentsColumn.Width = 125;
            // 
            // FileNameColumn
            // 
            this.FileNameColumn.DataPropertyName = "FileName";
            this.FileNameColumn.HeaderText = "Name";
            this.FileNameColumn.MinimumWidth = 6;
            this.FileNameColumn.Name = "FileNameColumn";
            this.FileNameColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.FileNameColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.FileNameColumn.Width = 125;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(756, 293);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.SelectSimpleButton);
            this.Controls.Add(this.InsertSimpleButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Insert images into SQL database";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button InsertSimpleButton;
        private System.Windows.Forms.Button SelectSimpleButton;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewImageColumn FileContentsColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn FileNameColumn;
    }
}

