using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace Apk.App.Controls;

public partial class IntegerBox : NumberBox
{
    [GeneratedRegex(@"^[\d]+$")]
    private static partial Regex IntegerRegex();

    public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register(
        nameof(Maximum),
        typeof(int?),
        typeof(DecimalBox),
        new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None));

    public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
        nameof(Value),
        typeof(int?),
        typeof(DecimalBox),
        new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    public int? Maximum
    {
        get { return (int?)GetValue(MaximumProperty); }
        set { SetValue(MaximumProperty, value); }
    }

    public int? Value
    {
        get { return (int?)GetValue(ValueProperty); }
        set { SetValue(ValueProperty, value); }
    }

    public IntegerBox()
    {
        Text = Value?.ToString();
        TextChanged += (sender, e) => Value = Text?.ToIntOrNull();
        PreviewKeyDown += OnPreviewKeyDown;
    }

    protected override void OnInitialized(EventArgs e)
    {
        base.OnInitialized(e);

        // initiale Befüllung des eigentlichen Text-Properties der TextBox
        Text = Value?.ToString();
    }

    // Zusatzfunktionen über Tastatureingaben
    private void OnPreviewKeyDown(object sender, KeyEventArgs e)
    {
        var currentValue = Value;
        if (currentValue == null)
            return;

        switch (e.Key)
        {
            // Wert + 1
            case Key.Up:
                currentValue += 1;

                if (Maximum == null || currentValue <= Maximum)
                    Text = currentValue.ToString();
                break;

            // Wert - 1
            case Key.Down when (currentValue - 1) >= 0:
                currentValue -= 1;
                Text = currentValue.ToString();
                break;

            default: return;
        }

        e.Handled = true;
        SelectionStart = Text.Length;
    }

    // Prüft die eingegebenen Zeichen per Regex
    protected override bool IsTextAllowed(string text)
    {
        text = text?.Trim();

        if (!IntegerRegex().IsMatch(text))
            return false;

        var value = text.ToIntOrNull();
        if (value == null)
            return false;

        if (Maximum != null && value > Maximum)
            return false;

        return true;
    }
}
