using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using FadingReminder.Helpers;
using Color = System.Windows.Media.Color;

namespace FadingReminder.Views;

/// <summary>
/// Fullscreen tint overlay window
/// </summary>
public partial class TintOverlay : Window
{
    private DispatcherTimer? _dismissTimer;
    private readonly int _durationSeconds;

    public TintOverlay(Color tintColor, double opacity, int durationSeconds, string? message = null)
    {
        InitializeComponent();

        _durationSeconds = durationSeconds;

        // Set the tint color and opacity
        TintRectangle.Fill = new SolidColorBrush(tintColor);
        TintRectangle.Opacity = opacity;

        // If there's a message, show it
        if (!string.IsNullOrWhiteSpace(message))
        {
            MessageText.Text = message;
            MessageBorder.Visibility = Visibility.Visible;

            // Calculate contrasting color for text
            var backgroundColor = ColorHelper.FromHex("#E0FFFFFF");
            var textColor = ColorHelper.GetContrastingColor(backgroundColor);
            MessageTitle.Foreground = new SolidColorBrush(textColor);
            MessageText.Foreground = new SolidColorBrush(textColor);
        }
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        // Position the window to cover the entire screen
        PositionWindow();

        // Start the auto-dismiss timer
        StartDismissTimer();
    }

    private void PositionWindow()
    {
        // Get the primary screen dimensions
        var primaryScreen = System.Windows.Forms.Screen.PrimaryScreen;
        if (primaryScreen != null)
        {
            Left = primaryScreen.Bounds.Left;
            Top = primaryScreen.Bounds.Top;
            Width = primaryScreen.Bounds.Width;
            Height = primaryScreen.Bounds.Height;
        }
        else
        {
            // Fallback to WPF screen dimensions
            Left = 0;
            Top = 0;
            Width = SystemParameters.PrimaryScreenWidth;
            Height = SystemParameters.PrimaryScreenHeight;
        }
    }

    private void StartDismissTimer()
    {
        _dismissTimer = new DispatcherTimer
        {
            Interval = TimeSpan.FromSeconds(_durationSeconds)
        };
        _dismissTimer.Tick += (s, e) =>
        {
            _dismissTimer?.Stop();
            Close();
        };
        _dismissTimer.Start();
    }

    private void OverlayGrid_MouseDown(object sender, MouseButtonEventArgs e)
    {
        // User clicked - dismiss immediately
        _dismissTimer?.Stop();
        Close();
    }

    protected override void OnClosed(EventArgs e)
    {
        _dismissTimer?.Stop();
        _dismissTimer = null;
        base.OnClosed(e);
    }
}
