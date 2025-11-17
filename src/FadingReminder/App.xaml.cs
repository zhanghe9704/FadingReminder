using System.Windows;
using FadingReminder.Services;

namespace FadingReminder;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private SystemTrayManager? _trayManager;
    private SettingsManager? _settingsManager;
    private AutoStartManager? _autoStartManager;
    private bool _startMinimized = false;

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        // Check for command line arguments
        if (e.Args.Length > 0)
        {
            foreach (var arg in e.Args)
            {
                if (arg.Equals("/silent", StringComparison.OrdinalIgnoreCase) ||
                    arg.Equals("-silent", StringComparison.OrdinalIgnoreCase))
                {
                    _startMinimized = true;
                }
            }
        }

        // Initialize managers
        InitializeApplication();

        // Prevent the app from showing the main window
        ShutdownMode = ShutdownMode.OnExplicitShutdown;
    }

    private void InitializeApplication()
    {
        // Initialize settings manager
        _settingsManager = new SettingsManager();
        _settingsManager.LoadSettings();
        _settingsManager.LoadReminders();

        // Initialize auto-start manager
        _autoStartManager = new AutoStartManager();

        // Initialize system tray
        _trayManager = new SystemTrayManager();
        _trayManager.SettingsClicked += OnSettingsClicked;
        _trayManager.RemindersClicked += OnRemindersClicked;
        _trayManager.ToggleTintingClicked += OnToggleTintingClicked;
        _trayManager.ExitClicked += OnExitClicked;

        // Update tray icon state
        _trayManager.UpdateTintingState(_settingsManager.Settings.IsTintingEnabled);

        // Show balloon tip on startup (optional)
        if (!_startMinimized)
        {
            _trayManager.ShowBalloonTip(
                "FadingReminder Started",
                "FadingReminder is running in the system tray.",
                System.Windows.Forms.ToolTipIcon.Info);
        }

        // TODO: Initialize other services (IntervalTimerService, ReminderService, etc.)
        // These will be added in Phase 2 and 3
    }

    private void OnSettingsClicked(object? sender, EventArgs e)
    {
        // TODO: Open settings window
        // Will be implemented in Phase 2
        MessageBox.Show("Settings window will be implemented in Phase 2", "FadingReminder",
            MessageBoxButton.OK, MessageBoxImage.Information);
    }

    private void OnRemindersClicked(object? sender, EventArgs e)
    {
        // TODO: Open reminders window
        // Will be implemented in Phase 3
        MessageBox.Show("Reminders window will be implemented in Phase 3", "FadingReminder",
            MessageBoxButton.OK, MessageBoxImage.Information);
    }

    private void OnToggleTintingClicked(object? sender, EventArgs e)
    {
        // Toggle tinting on/off
        if (_settingsManager != null)
        {
            _settingsManager.Settings.IsTintingEnabled = !_settingsManager.Settings.IsTintingEnabled;
            _settingsManager.SaveSettings();

            _trayManager?.UpdateTintingState(_settingsManager.Settings.IsTintingEnabled);

            string status = _settingsManager.Settings.IsTintingEnabled ? "enabled" : "disabled";
            _trayManager?.ShowBalloonTip(
                "FadingReminder",
                $"Screen tinting {status}",
                System.Windows.Forms.ToolTipIcon.Info);
        }
    }

    private void OnExitClicked(object? sender, EventArgs e)
    {
        // Clean up and exit
        Cleanup();
        Shutdown();
    }

    private void Cleanup()
    {
        _trayManager?.Dispose();

        // TODO: Dispose other services when they're added
        // (IntervalTimerService, ReminderService, etc.)
    }

    protected override void OnExit(ExitEventArgs e)
    {
        Cleanup();
        base.OnExit(e);
    }
}
