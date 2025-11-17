using System.Windows;
using FadingReminder.Services;
using FadingReminder.Views;
using Application = System.Windows.Application;

namespace FadingReminder;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private SystemTrayManager? _trayManager;
    private SettingsManager? _settingsManager;
    private AutoStartManager? _autoStartManager;
    private ScreenTintService? _screenTintService;
    private IntervalTimerService? _intervalTimerService;
    private ReminderService? _reminderService;
    private bool _startMinimized = false;

    // Track single instances of windows
    private SettingsWindow? _settingsWindow;
    private RemindersWindow? _remindersWindow;

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

        // Initialize screen tint service
        _screenTintService = new ScreenTintService(_settingsManager);

        // Initialize interval timer service
        _intervalTimerService = new IntervalTimerService(_settingsManager, _screenTintService);

        // Start the interval timer if tinting is enabled
        if (_settingsManager.Settings.IsTintingEnabled)
        {
            _intervalTimerService.Start();
        }

        // Initialize reminder service
        _reminderService = new ReminderService(_settingsManager, _screenTintService);
        _reminderService.Start();
    }

    private void OnSettingsClicked(object? sender, EventArgs e)
    {
        // Open settings window (only one instance allowed)
        if (_settingsManager != null && _autoStartManager != null)
        {
            // If window already exists and is open, bring it to focus
            if (_settingsWindow != null)
            {
                if (_settingsWindow.IsLoaded)
                {
                    _settingsWindow.Activate();
                    _settingsWindow.Focus();
                    return;
                }
            }

            // Create new window instance
            _settingsWindow = new SettingsWindow(
                _settingsManager,
                _autoStartManager,
                _screenTintService,
                _intervalTimerService);

            // Clear reference when window closes
            _settingsWindow.Closed += (s, args) => _settingsWindow = null;

            _settingsWindow.ShowDialog();
        }
    }

    private void OnRemindersClicked(object? sender, EventArgs e)
    {
        // Open reminders window (only one instance allowed)
        if (_settingsManager != null)
        {
            // If window already exists and is open, bring it to focus
            if (_remindersWindow != null)
            {
                if (_remindersWindow.IsLoaded)
                {
                    _remindersWindow.Activate();
                    _remindersWindow.Focus();
                    return;
                }
            }

            // Create new window instance
            _remindersWindow = new RemindersWindow(_settingsManager, _reminderService);

            // Clear reference when window closes
            _remindersWindow.Closed += (s, args) => _remindersWindow = null;

            _remindersWindow.ShowDialog();
        }
    }

    private void OnToggleTintingClicked(object? sender, EventArgs e)
    {
        // Toggle tinting on/off
        if (_settingsManager != null)
        {
            _settingsManager.Settings.IsTintingEnabled = !_settingsManager.Settings.IsTintingEnabled;
            _settingsManager.SaveSettings();

            _trayManager?.UpdateTintingState(_settingsManager.Settings.IsTintingEnabled);

            // Start or stop the interval timer
            if (_intervalTimerService != null)
            {
                if (_settingsManager.Settings.IsTintingEnabled)
                {
                    _intervalTimerService.Start();
                }
                else
                {
                    _intervalTimerService.Stop();
                }
            }

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
        // Stop and dispose reminder service
        _reminderService?.Dispose();

        // Stop and dispose interval timer
        _intervalTimerService?.Dispose();

        // Close any active tint overlays
        _screenTintService?.CloseAllTints();

        // Dispose tray manager
        _trayManager?.Dispose();
    }

    protected override void OnExit(ExitEventArgs e)
    {
        Cleanup();
        base.OnExit(e);
    }
}
