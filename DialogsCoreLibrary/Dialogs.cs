namespace DialogsCoreLibrary;
public class Dialogs
{
    /// <summary>
    /// Dialog to ask a question
    /// </summary>
    /// <param name="caption">text for dialog caption</param>
    /// <param name="heading">text for dialog heading</param>
    /// <param name="yesText">text for yes button</param>
    /// <param name="noText">text for no button</param>
    /// <param name="defaultButton">specifies the default button for this dialog</param>
    /// <returns>true for yes button, false for no button</returns>
    public static bool Question(string caption, string heading, string yesText, string noText, DialogResult defaultButton)
    {

        TaskDialogButton yesButton = new(yesText) { Tag = DialogResult.Yes };
        TaskDialogButton noButton = new(noText) { Tag = DialogResult.No };

        TaskDialogButtonCollection buttons = new();

        if (defaultButton == DialogResult.Yes)
        {
            buttons.Add(yesButton);
            buttons.Add(noButton);
        }
        else
        {
            buttons.Add(noButton);
            buttons.Add(yesButton);
        }

        TaskDialogPage page = new()
        {
            Caption = caption,
            SizeToContent = true,
            Heading = heading,
            Icon = TaskDialogIcon.Information,
            Buttons = buttons
        };


        TaskDialogButton result = TaskDialog.ShowDialog(page);

        return (DialogResult)result.Tag! == DialogResult.Yes;

    }

}
