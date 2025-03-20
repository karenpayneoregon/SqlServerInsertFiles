using System.Windows.Forms;
namespace DialogLibrary
{
    public static class KarenDialogs
    {
        public static bool Question(string Text) => 
            (
                MessageBox.Show(Text, "Question",
                    MessageBoxButtons.YesNo, 
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2) == DialogResult.Yes);
    }
}
