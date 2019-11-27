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
using static DialogLibrary.KarenDialogs;

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
            var ops = new DataOperations();
            var identifier = 0;
            var fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Dogma1.html");

            if (ops.InsertFileSimple(fileName, "Dogma1.html", ref identifier))
            {
                MessageBox.Show($"Id is {identifier}");
            }
            else
            {
                MessageBox.Show($"Failed: {ops.ExceptionMessage}");
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
            var ops = new DataOperations();
            var identifier = 2;
            var fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "extracted.html");

            if (ops.ReadFileFromDatabaseTableSimple(identifier, fileName))
            {
                MessageBox.Show("Success");
            }
            else
            {
                MessageBox.Show($"Failed: {ops.ExceptionMessage}");
            }
        }

        /// <summary>
        /// Simple example to add rows to our child table
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks>
        /// This has already been executed running this again would 
        /// duplicate the row. So we could run the following (super simple)
        /// 
        ///   SELECT id FROM EventAttachments WHERE FileBaseName = 'CPR'
        /// 
        /// To ensure it does not exists, of course we would do more conditions
        /// in the WHERE for a real app.
        /// </remarks>
        private void InsertToChildTableButton_Click(object sender, EventArgs e)
        {
            if (Question("You might want to read comments first, continue?"))
            {
                var ops = new DataOperations();

                var fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "EventFiles", "CPR_2.docx");

                // new identifier returned
                var identifier = 0;

                // existing row in parent table
                var eventIdentifier = 1;

                if (ops.InsertNewEvent(fileName, ref identifier, eventIdentifier))
                {
                    MessageBox.Show("Success");
                }
                else
                {
                    MessageBox.Show($"Failed: {ops.ExceptionMessage}");
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var courseName = "Entity Framework 101";
            label1.Text = courseName;

            var ops = new DataOperations();
            var id = ops.GetCourseIdentifier(courseName);

            dataGridView1.DataSource = ops.GetAttachmentsForEvent(id);
            dataGridView1.ExpandColumns();
        }
    }
}
