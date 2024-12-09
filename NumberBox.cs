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
            // Wenn der Text markiert ist, ersetze ihn durch den neuen Text
            return currentText.Remove(selectionStart, selectionLength).Insert(selectionStart, addedText);

        // Wenn kein Text markiert ist, füge den neuen Text an der Cursorposition ein
        return currentText.Insert(selectionStart, addedText);
    }

    // Sorgt dafür, dass der Cursor immer ans Ende der Zahl springt
    protected void OnGotFocus(object sender, RoutedEventArgs e)
    {
        SelectionStart = Text.Length;
    }

    // Verhindert die Eingabe von ungültigen Zeichen
    protected void OnPreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        var updatedText = GetUpdatedText(e.Text);

        // Handled ist true wenn ein ungültiges Zeichen eingegeben wird, dementsprechend wird es nicht mehr weitergereicht
        e.Handled = !IsTextAllowed(updatedText);
    }

    // Einfügen aus Zwischenablage
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
