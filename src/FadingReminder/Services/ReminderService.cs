using System.Windows.Threading;
using FadingReminder.Models;

namespace FadingReminder.Services;

/// <summary>
/// Service for managing and triggering time-based reminders
/// </summary>
public class ReminderService : IDisposable
{
    private readonly SettingsManager _settingsManager;
    private readonly ScreenTintService _screenTintService;
    private DispatcherTimer? _checkTimer;
    private bool _isRunning;
    private DateTime _lastCheckedDate;

    public event EventHandler<Reminder>? ReminderTriggered;

    public bool IsRunning => _isRunning;

    public ReminderService(SettingsManager settingsManager, ScreenTintService screenTintService)
    {
        _settingsManager = settingsManager;
        _screenTintService = screenTintService;
        _lastCheckedDate = DateTime.Today;
    }

    /// <summary>
    /// Starts the reminder checking service
    /// </summary>
    public void Start()
    {
        if (_isRunning)
        {
            return; // Already running
        }

        // Create timer that checks every 30 seconds
        _checkTimer = new DispatcherTimer
        {
            Interval = TimeSpan.FromSeconds(30)
        };
        _checkTimer.Tick += CheckTimer_Tick;
        _checkTimer.Start();

        _isRunning = true;
        _lastCheckedDate = DateTime.Today;

        Console.WriteLine("Reminder service started");
    }

    /// <summary>
    /// Stops the reminder checking service
    /// </summary>
    public void Stop()
    {
        if (!_isRunning)
        {
            return; // Not running
        }

        _checkTimer?.Stop();
        _checkTimer = null;
        _isRunning = false;

        Console.WriteLine("Reminder service stopped");
    }

    private void CheckTimer_Tick(object? sender, EventArgs e)
    {
        // Check for date rollover
        CheckDateRollover();

        // Check for reminders that should trigger
        CheckReminders();
    }

    private void CheckDateRollover()
    {
        if (DateTime.Today > _lastCheckedDate)
        {
            // Date has changed - perform rollover
            Console.WriteLine($"Date rollover detected: {_lastCheckedDate:yyyy-MM-dd} -> {DateTime.Today:yyyy-MM-dd}");

            // Reload reminders to trigger the rollover logic in SettingsManager
            _settingsManager.LoadReminders();

            // Reset all "today" reminder triggered flags
            var todayReminders = _settingsManager.GetRemindersForDay(ReminderDay.Today);
            foreach (var reminder in todayReminders)
            {
                reminder.HasTriggered = false;
            }
            _settingsManager.SaveReminders();

            _lastCheckedDate = DateTime.Today;
        }
    }

    private void CheckReminders()
    {
        var now = DateTime.Now;
        var currentTime = now.TimeOfDay;

        // Get all enabled "today" reminders
        var todayReminders = _settingsManager.GetRemindersForDay(ReminderDay.Today)
            .Where(r => r.IsEnabled && !r.HasTriggered)
            .ToList();

        foreach (var reminder in todayReminders)
        {
            // Check if the reminder time has passed
            // We use a 1-minute window to account for the check interval
            var timeDiff = currentTime - reminder.Time;

            if (timeDiff >= TimeSpan.Zero && timeDiff <= TimeSpan.FromMinutes(1))
            {
                // Trigger the reminder
                TriggerReminder(reminder);
            }
        }
    }

    private void TriggerReminder(Reminder reminder)
    {
        Console.WriteLine($"Triggering reminder: {reminder.Message} at {reminder.Time}");

        // Mark as triggered to prevent duplicate notifications
        reminder.HasTriggered = true;
        _settingsManager.SaveReminders();

        // Show the tint with the reminder message and color (time is shown automatically by overlay)
        _screenTintService.ShowTint(reminder.Message, reminder.TintColor);

        // Raise event
        ReminderTriggered?.Invoke(this, reminder);
    }

    /// <summary>
    /// Manually triggers a reminder for testing
    /// </summary>
    public void TestReminder(Reminder reminder)
    {
        string message = $"TEST: {reminder.Message}";
        _screenTintService.ShowTint(message, reminder.TintColor);
    }

    /// <summary>
    /// Gets the next reminder that will trigger
    /// </summary>
    public Reminder? GetNextReminder()
    {
        var now = DateTime.Now.TimeOfDay;
        var todayReminders = _settingsManager.GetRemindersForDay(ReminderDay.Today)
            .Where(r => r.IsEnabled && !r.HasTriggered && r.Time > now)
            .OrderBy(r => r.Time)
            .ToList();

        if (todayReminders.Any())
        {
            return todayReminders.First();
        }

        // No more reminders today, check tomorrow
        var tomorrowReminders = _settingsManager.GetRemindersForDay(ReminderDay.Tomorrow)
            .Where(r => r.IsEnabled)
            .OrderBy(r => r.Time)
            .ToList();

        return tomorrowReminders.FirstOrDefault();
    }

    /// <summary>
    /// Gets a summary of active reminders
    /// </summary>
    public (int todayActive, int todayTriggered, int tomorrowActive) GetReminderSummary()
    {
        var todayReminders = _settingsManager.GetRemindersForDay(ReminderDay.Today);
        var tomorrowReminders = _settingsManager.GetRemindersForDay(ReminderDay.Tomorrow);

        int todayActive = todayReminders.Count(r => r.IsEnabled && !r.HasTriggered);
        int todayTriggered = todayReminders.Count(r => r.HasTriggered);
        int tomorrowActive = tomorrowReminders.Count(r => r.IsEnabled);

        return (todayActive, todayTriggered, tomorrowActive);
    }

    public void Dispose()
    {
        Stop();
    }
}
