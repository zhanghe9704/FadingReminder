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
    private readonly bool _useExplicitPositioning;

    public TintOverlay(Color tintColor, double opacity, int durationSeconds, string customMessage)
    {
        InitializeComponent();

        _durationSeconds = durationSeconds;
        _useExplicitPositioning = false; // Auto-position to primary screen

        // Set the tint color and opacity
        TintRectangle.Fill = new SolidColorBrush(tintColor);
        TintRectangle.Opacity = opacity;

        // Set current time
        CurrentTimeText.Text = DateTime.Now.ToString("h:mm tt");

        // Set custom message
        MessageText.Text = string.IsNullOrWhiteSpace(customMessage)
            ? "Time to rest your eyes!"
            : customMessage;
    }

    /// <summary>
    /// Constructor for explicitly positioned overlay (used for multi-monitor)
    /// </summary>
    public TintOverlay(Color tintColor, double opacity, int durationSeconds, string customMessage,
                       int left, int top, int width, int height)
        : this(tintColor, opacity, durationSeconds, customMessage)
    {
        _useExplicitPositioning = true;
        Left = left;
        Top = top;
        Width = width;
        Height = height;
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        // Position the window to cover the entire screen (only if not explicitly positioned)
        if (!_useExplicitPositioning)
        {
            PositionWindow();
        }

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
