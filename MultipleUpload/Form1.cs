using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MultipleUpload
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            openFileDialog1.InitialDirectory = Path.Combine(AppDomain
                .CurrentDomain.BaseDirectory, "Files");
        }

        /// <summary>
        /// Ask for one or more files and insert into a table
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectFilesButton_Click(object sender, EventArgs e)
        {

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                var ops = new DataOperations();
                ops.OnLineHandler += OnLineHandler;
                ops.InsertFiles(openFileDialog1.FileNames.ToList());
            }
        }
        /// <summary>
        /// Notify the user that records have been added via an event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnLineHandler(object sender, InsertFileArgs args)
        {
            listBox1.Items.Add($"{args.Identifier} {args.FileName}");
            listBox1.SelectedIndex = listBox1.Items.Count - 1;
        }

    }
}
