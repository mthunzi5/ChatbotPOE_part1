namespace CybersecurityAwarenessBot.Services;

public static class PromptHelper
{
    public static string ShowSingleLine(string title, string label, string defaultValue = "")
    {
        using var form = new Form
        {
            Width = 400,
            Height = 180,
            Text = title,
            FormBorderStyle = FormBorderStyle.FixedDialog,
            StartPosition = FormStartPosition.CenterParent,
            MinimizeBox = false,
            MaximizeBox = false
        };

        var labelControl = new Label
        {
            Text = label,
            Left = 12,
            Top = 12,
            Width = 360
        };

        var textBox = new TextBox
        {
            Left = 12,
            Top = 40,
            Width = 360,
            Text = defaultValue
        };

        var okButton = new Button
        {
            Text = "OK",
            DialogResult = DialogResult.OK,
            Left = 210,
            Width = 80,
            Top = 80
        };

        var cancelButton = new Button
        {
            Text = "Cancel",
            DialogResult = DialogResult.Cancel,
            Left = 295,
            Width = 80,
            Top = 80
        };

        form.Controls.Add(labelControl);
        form.Controls.Add(textBox);
        form.Controls.Add(okButton);
        form.Controls.Add(cancelButton);
        form.AcceptButton = okButton;
        form.CancelButton = cancelButton;

        return form.ShowDialog() == DialogResult.OK ? textBox.Text.Trim() : string.Empty;
    }
}
