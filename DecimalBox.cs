using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace Apk.App.Controls;

public partial class DecimalBox : NumberBox
{
    [GeneratedRegex(@"^[\d,]+$")]
    private static partial Regex DecimalRegex();

    public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register(
        nameof(Maximum),
        typeof(decimal?),
        typeof(DecimalBox),
        new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None));

    public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
        nameof(Value),
        typeof(decimal?),
        typeof(DecimalBox),
        new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    public decimal? Maximum
    {
        get { return (decimal?)GetValue(MaximumProperty); }
        set { SetValue(MaximumProperty, value); }
    }

    public decimal? Value
    {
        get { return (decimal?)GetValue(ValueProperty); }
        set { SetValue(ValueProperty, value); }
    }

    public DecimalBox()
    {
        TextChanged += (sender, e) => Value = Text?.ToDecimalOrNull();
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

        var isCtrlPressed = (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control;
        var delta = isCtrlPressed ? 0.1m : 1m;

        switch (e.Key)
        {
            // Wert + 1
            case Key.Up:
                currentValue += delta;

                if (Maximum == null || currentValue <= Maximum)
                    Text = currentValue.ToString();
                break;

            // Wert - 1
            case Key.Down when (currentValue - delta) >= 0:
                currentValue -= delta;
                Text = currentValue.ToString();
                break;

            // Beistrich einfügen
            case Key.Space:
                if (Text.Contains(',', StringComparison.Ordinal))
                    break;

                Text = GetUpdatedText(",");
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

        if (!DecimalRegex().IsMatch(text))
            return false;

        var value = text.ToDecimalOrNull();
        if (value == null)
            return false;

        if (Maximum != null && value > Maximum)
            return false;

        return true;
    }
}
