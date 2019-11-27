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
            this.InsertToChildTableButton = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // InsertSimpleButton
            // 
            this.InsertSimpleButton.Location = new System.Drawing.Point(12, 32);
            this.InsertSimpleButton.Name = "InsertSimpleButton";
            this.InsertSimpleButton.Size = new System.Drawing.Size(180, 52);
            this.InsertSimpleButton.TabIndex = 1;
            this.InsertSimpleButton.Text = "Insert simple";
            this.InsertSimpleButton.UseVisualStyleBackColor = true;
            this.InsertSimpleButton.Click += new System.EventHandler(this.InsertSimpleButton_Click);
            // 
            // SelectSimpleButton
            // 
            this.SelectSimpleButton.Location = new System.Drawing.Point(12, 90);
            this.SelectSimpleButton.Name = "SelectSimpleButton";
            this.SelectSimpleButton.Size = new System.Drawing.Size(180, 52);
            this.SelectSimpleButton.TabIndex = 2;
            this.SelectSimpleButton.Text = "Select simple";
            this.SelectSimpleButton.UseVisualStyleBackColor = true;
            this.SelectSimpleButton.Click += new System.EventHandler(this.SelectSimpleButton_Click);
            // 
            // InsertToChildTableButton
            // 
            this.InsertToChildTableButton.Location = new System.Drawing.Point(12, 149);
            this.InsertToChildTableButton.Name = "InsertToChildTableButton";
            this.InsertToChildTableButton.Size = new System.Drawing.Size(180, 52);
            this.InsertToChildTableButton.TabIndex = 3;
            this.InsertToChildTableButton.Text = "Insert relational";
            this.InsertToChildTableButton.UseVisualStyleBackColor = true;
            this.InsertToChildTableButton.Click += new System.EventHandler(this.InsertToChildTableButton_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(198, 32);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.Size = new System.Drawing.Size(335, 169);
            this.dataGridView1.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(195, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "label1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(567, 238);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.InsertToChildTableButton);
            this.Controls.Add(this.SelectSimpleButton);
            this.Controls.Add(this.InsertSimpleButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Insert files into SQL database";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button InsertSimpleButton;
        private System.Windows.Forms.Button SelectSimpleButton;
        private System.Windows.Forms.Button InsertToChildTableButton;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label label1;
    }
}

