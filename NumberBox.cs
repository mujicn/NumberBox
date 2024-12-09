using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Apk.App.Controls;
public abstract class NumberBox : TextBox
{
    protected NumberBox()
    {
        GotFocus += OnGotFocus;
        PreviewTextInput += OnPreviewTextInput;

        DataObject.AddPastingHandler(this, OnPaste);
    }

    protected abstract bool IsTextAllowed(string text);

    protected string GetUpdatedText(string addedText)
    {
        var currentText = Text;

        var selectionStart = SelectionStart;
        var selectionLength = SelectionLength;

        if (selectionLength > 0)
            // replace selected text
            return currentText.Remove(selectionStart, selectionLength).Insert(selectionStart, addedText);

        // add text at cursor position
        return currentText.Insert(selectionStart, addedText);
    }

    // ensures that the cursor is placed at the end
    protected void OnGotFocus(object sender, RoutedEventArgs e)
    {
        SelectionStart = Text.Length;
    }

    // prevents invalid characters from beeing entered
    protected void OnPreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        var updatedText = GetUpdatedText(e.Text);

        // Handled ist true wenn ein ungültiges Zeichen eingegeben wird, dementsprechend wird es nicht mehr weitergereicht
        e.Handled = !IsTextAllowed(updatedText);
    }

    // paste from clipboard
    protected void OnPaste(object sender, DataObjectPastingEventArgs e)
    {
        if (!e.DataObject.GetDataPresent(typeof(string)))
        {
            e.CancelCommand();
            return;
        }

        var pastedText = (string)e.DataObject.GetData(typeof(string));
        var updatedText = GetUpdatedText(pastedText);

        if (!IsTextAllowed(updatedText))
            e.CancelCommand();
    }
}
