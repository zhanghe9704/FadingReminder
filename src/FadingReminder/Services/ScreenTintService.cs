using System.Windows;
using System.Windows.Media;
using FadingReminder.Helpers;
using FadingReminder.Models;
using FadingReminder.Views;
using Color = System.Windows.Media.Color;
using Application = System.Windows.Application;

namespace FadingReminder.Services;

/// <summary>
/// Service for displaying screen tint overlays
/// </summary>
public class ScreenTintService
{
    private readonly SettingsManager _settingsManager;
    private readonly List<TintOverlay> _activeOverlays = new();

    public ScreenTintService(SettingsManager settingsManager)
    {
        _settingsManager = settingsManager;
    }

    /// <summary>
    /// Shows a tint overlay using current settings and custom message
    /// </summary>
    public void ShowTint()
    {
        ShowTint(null);
    }

    /// <summary>
    /// Shows a tint overlay with an optional override message (for reminders)
    /// </summary>
    /// <param name="overrideMessage">Optional message to override the default custom message</param>
    public void ShowTint(string? overrideMessage)
    {
        // Get current settings
        var settings = _settingsManager.Settings;

        // Parse the tint color
        Color tintColor = ColorHelper.FromHex(settings.TintColor);

        // Use override message if provided, otherwise use custom message from settings
        string displayMessage = overrideMessage ?? settings.CustomMessage;

        // Show overlay(s)
        if (settings.ShowOnAllMonitors)
        {
            ShowTintOnAllMonitors(tintColor, settings.TintOpacity, settings.TintDurationSeconds, displayMessage);
        }
        else
        {
            ShowTintOnPrimaryMonitor(tintColor, settings.TintOpacity, settings.TintDurationSeconds, displayMessage);
        }
    }

    /// <summary>
    /// Shows tint overlay on the primary monitor only
    /// </summary>
    private void ShowTintOnPrimaryMonitor(Color tintColor, double opacity, int durationSeconds, string customMessage)
    {
        Application.Current.Dispatcher.Invoke(() =>
        {
            var overlay = new TintOverlay(tintColor, opacity, durationSeconds, customMessage);
            overlay.Closed += (s, e) => _activeOverlays.Remove(overlay);
            _activeOverlays.Add(overlay);
            overlay.Show();
        });
    }

    /// <summary>
    /// Shows tint overlay on all monitors
    /// </summary>
    private void ShowTintOnAllMonitors(Color tintColor, double opacity, int durationSeconds, string customMessage)
    {
        Application.Current.Dispatcher.Invoke(() =>
        {
            var screens = System.Windows.Forms.Screen.AllScreens;

            foreach (var screen in screens)
            {
                var overlay = new TintOverlay(tintColor, opacity, durationSeconds, customMessage);
                overlay.Closed += (s, e) => _activeOverlays.Remove(overlay);

                // Position overlay on this specific screen
                overlay.Left = screen.Bounds.Left;
                overlay.Top = screen.Bounds.Top;
                overlay.Width = screen.Bounds.Width;
                overlay.Height = screen.Bounds.Height;

                _activeOverlays.Add(overlay);
                overlay.Show();
            }
        });
    }

    /// <summary>
    /// Closes all active tint overlays
    /// </summary>
    public void CloseAllTints()
    {
        Application.Current.Dispatcher.Invoke(() =>
        {
            var overlays = _activeOverlays.ToList();
            foreach (var overlay in overlays)
            {
                overlay.Close();
            }
            _activeOverlays.Clear();
        });
    }

    /// <summary>
    /// Gets the number of currently active overlays
    /// </summary>
    public int ActiveOverlayCount => _activeOverlays.Count;
}
