namespace FadingReminder.Models;

/// <summary>
/// Application settings model for persisting user preferences
/// </summary>
public class AppSettings
{
    /// <summary>
    /// Tint color in ARGB hex format (e.g., "#80FF0000" for semi-transparent red)
    /// </summary>
    public string TintColor { get; set; } = "#80000080"; // Default: semi-transparent navy blue

    /// <summary>
    /// Interval in minutes for periodic screen tinting (N)
    /// </summary>
    public int IntervalMinutes { get; set; } = 20;

    /// <summary>
    /// Duration in seconds for how long the tint should be displayed
    /// </summary>
    public int TintDurationSeconds { get; set; } = 3;

    /// <summary>
    /// Opacity of the tint overlay (0.0 to 1.0)
    /// </summary>
    public double TintOpacity { get; set; } = 0.4;

    /// <summary>
    /// Whether periodic tinting is enabled
    /// </summary>
    public bool IsTintingEnabled { get; set; } = true;

    /// <summary>
    /// Whether the application should auto-start with Windows
    /// </summary>
    public bool AutoStartEnabled { get; set; } = false;

    /// <summary>
    /// Whether to show tint on all monitors or just primary
    /// </summary>
    public bool ShowOnAllMonitors { get; set; } = false;

    /// <summary>
    /// Whether to start the application minimized to tray
    /// </summary>
    public bool StartMinimized { get; set; } = false;

    /// <summary>
    /// Custom message to display on the tint overlay alongside the current time
    /// </summary>
    public string CustomMessage { get; set; } = "Time to rest your eyes!";
}
