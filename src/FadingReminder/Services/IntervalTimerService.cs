using System.Windows.Threading;
using FadingReminder.Models;

namespace FadingReminder.Services;

/// <summary>
/// Service for managing periodic screen tinting at configured intervals
/// </summary>
public class IntervalTimerService : IDisposable
{
    private readonly SettingsManager _settingsManager;
    private readonly ScreenTintService _screenTintService;
    private DispatcherTimer? _timer;
    private bool _isRunning;

    public event EventHandler? TintTriggered;

    public bool IsRunning => _isRunning;

    public IntervalTimerService(SettingsManager settingsManager, ScreenTintService screenTintService)
    {
        _settingsManager = settingsManager;
        _screenTintService = screenTintService;
    }

    /// <summary>
    /// Starts the interval timer
    /// </summary>
    public void Start()
    {
        if (_isRunning)
        {
            return; // Already running
        }

        var settings = _settingsManager.Settings;

        if (!settings.IsTintingEnabled)
        {
            return; // Tinting is disabled
        }

        // Create and configure the timer
        _timer = new DispatcherTimer
        {
            Interval = TimeSpan.FromMinutes(settings.IntervalMinutes)
        };
        _timer.Tick += Timer_Tick;
        _timer.Start();

        _isRunning = true;

        Console.WriteLine($"Interval timer started: tinting every {settings.IntervalMinutes} minutes");
    }

    /// <summary>
    /// Stops the interval timer
    /// </summary>
    public void Stop()
    {
        if (!_isRunning)
        {
            return; // Not running
        }

        _timer?.Stop();
        _timer = null;
        _isRunning = false;

        Console.WriteLine("Interval timer stopped");
    }

    /// <summary>
    /// Restarts the timer with updated settings
    /// </summary>
    public void Restart()
    {
        Stop();
        Start();
    }

    /// <summary>
    /// Updates the timer interval without stopping it
    /// </summary>
    public void UpdateInterval(int intervalMinutes)
    {
        if (_timer != null && _isRunning)
        {
            _timer.Interval = TimeSpan.FromMinutes(intervalMinutes);
            Console.WriteLine($"Interval updated to {intervalMinutes} minutes");
        }
    }

    /// <summary>
    /// Manually triggers a tint (doesn't reset the timer)
    /// </summary>
    public void TriggerTintNow()
    {
        if (_settingsManager.Settings.IsTintingEnabled)
        {
            _screenTintService.ShowTint();
            TintTriggered?.Invoke(this, EventArgs.Empty);
        }
    }

    private void Timer_Tick(object? sender, EventArgs e)
    {
        // Check if tinting is still enabled
        if (!_settingsManager.Settings.IsTintingEnabled)
        {
            Stop();
            return;
        }

        // Trigger the tint
        _screenTintService.ShowTint();
        TintTriggered?.Invoke(this, EventArgs.Empty);

        Console.WriteLine($"Interval tint triggered at {DateTime.Now:HH:mm:ss}");
    }

    public void Dispose()
    {
        Stop();
    }
}
