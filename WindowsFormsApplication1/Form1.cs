using Newtonsoft.Json;
using System;
using System.Data;
using System.IO;
using System.Windows.Forms;
using WindowsFormsApplication1.Classes;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Simple example of inserting a file into a table
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InsertSimpleButton_Click(object sender, EventArgs e)
        {
            var fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "age.png");

            var (success, identifier, bytes, exception) = DataOperations.InsertFileSimple(fileName);
            if (success)
            {
                var source = (DataTable)dataGridView1.DataSource;
                source.Rows.Add(identifier,bytes, Path.GetFileName(fileName));
                MessageBox.Show($"Id is {identifier}");
            }
            else
            {
                MessageBox.Show($"Failed: {exception.Message}");
            }
           
        }
        /// <summary>
        /// Extract file by id using know record id that will always
        /// exists unless someone trying this code deletes the record.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectSimpleButton_Click(object sender, EventArgs e)
        {

            var identifier = 1;
            var fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"blub1{DateTime.Now.Millisecond}.png");
            var (success, exception) = DataOperations.ReadFileFromDatabaseTableSimple(identifier, fileName);
            if (success)
            {
                MessageBox.Show($"Success, extracted as blub1{DateTime.Now.Millisecond}.png");
            }
            else
            {
                MessageBox.Show($"Failed: {exception}");
            }
        }
        
        private void Form1_Load(object sender, EventArgs e)
        {
            dataGridView1.DataSource = DataOperations.GetAttachmentsForEvent();
            dataGridView1.ExpandColumns();
        }
    }
}
