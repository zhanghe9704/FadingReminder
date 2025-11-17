using System.IO;
using System.Text.Json;
using FadingReminder.Models;

namespace FadingReminder.Services;

/// <summary>
/// Manages application settings and reminder persistence using JSON files
/// </summary>
public class SettingsManager
{
    private readonly string _settingsFilePath;
    private readonly string _remindersFilePath;
    private readonly JsonSerializerOptions _jsonOptions;

    public AppSettings Settings { get; private set; }
    public ReminderCollection Reminders { get; private set; }

    public SettingsManager()
    {
        // Store settings in the application directory for portability
        string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
        _settingsFilePath = Path.Combine(appDirectory, "settings.json");
        _remindersFilePath = Path.Combine(appDirectory, "reminders.json");

        // Configure JSON serialization options
        _jsonOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNameCaseInsensitive = true
        };

        Settings = new AppSettings();
        Reminders = new ReminderCollection();
    }

    /// <summary>
    /// Loads settings from JSON file. Creates default settings if file doesn't exist.
    /// </summary>
    public void LoadSettings()
    {
        try
        {
            if (File.Exists(_settingsFilePath))
            {
                string json = File.ReadAllText(_settingsFilePath);
                var loadedSettings = JsonSerializer.Deserialize<AppSettings>(json, _jsonOptions);
                if (loadedSettings != null)
                {
                    Settings = loadedSettings;
                    return;
                }
            }

            // If file doesn't exist or deserialization failed, use defaults
            Settings = new AppSettings();
            SaveSettings(); // Save default settings
        }
        catch (Exception ex)
        {
            // Log error and use default settings
            Console.WriteLine($"Error loading settings: {ex.Message}");
            Settings = new AppSettings();
        }
    }

    /// <summary>
    /// Saves current settings to JSON file
    /// </summary>
    public void SaveSettings()
    {
        try
        {
            string json = JsonSerializer.Serialize(Settings, _jsonOptions);
            File.WriteAllText(_settingsFilePath, json);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving settings: {ex.Message}");
            throw;
        }
    }

    /// <summary>
    /// Loads reminders from JSON file. Creates empty collection if file doesn't exist.
    /// </summary>
    public void LoadReminders()
    {
        try
        {
            if (File.Exists(_remindersFilePath))
            {
                string json = File.ReadAllText(_remindersFilePath);
                var loadedReminders = JsonSerializer.Deserialize<ReminderCollection>(json, _jsonOptions);
                if (loadedReminders != null)
                {
                    Reminders = loadedReminders;

                    // Check for date rollover
                    CheckDateRollover();
                    return;
                }
            }

            // If file doesn't exist or deserialization failed, use empty collection
            Reminders = new ReminderCollection();
            SaveReminders(); // Save empty collection
        }
        catch (Exception ex)
        {
            // Log error and use empty collection
            Console.WriteLine($"Error loading reminders: {ex.Message}");
            Reminders = new ReminderCollection();
        }
    }

    /// <summary>
    /// Saves current reminders to JSON file
    /// </summary>
    public void SaveReminders()
    {
        try
        {
            Reminders.LastUpdateDate = DateTime.Today;
            string json = JsonSerializer.Serialize(Reminders, _jsonOptions);
            File.WriteAllText(_remindersFilePath, json);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving reminders: {ex.Message}");
            throw;
        }
    }

    /// <summary>
    /// Checks if the date has changed and performs rollover logic
    /// - Clears all "today" reminders
    /// - Moves "tomorrow" reminders to "today"
    /// - Clears "tomorrow" slots
    /// </summary>
    private void CheckDateRollover()
    {
        if (Reminders.LastUpdateDate < DateTime.Today)
        {
            // Date has changed - perform rollover
            var todayReminders = Reminders.Reminders
                .Where(r => r.Day == ReminderDay.Today)
                .ToList();

            var tomorrowReminders = Reminders.Reminders
                .Where(r => r.Day == ReminderDay.Tomorrow)
                .ToList();

            // Remove all today reminders
            foreach (var reminder in todayReminders)
            {
                Reminders.Reminders.Remove(reminder);
            }

            // Move tomorrow reminders to today and reset triggered flag
            foreach (var reminder in tomorrowReminders)
            {
                reminder.Day = ReminderDay.Today;
                reminder.HasTriggered = false;
                reminder.CreatedDate = DateTime.Today;
            }

            SaveReminders();
        }
    }

    /// <summary>
    /// Gets all reminders for a specific day
    /// </summary>
    public List<Reminder> GetRemindersForDay(ReminderDay day)
    {
        return Reminders.Reminders
            .Where(r => r.Day == day)
            .OrderBy(r => r.Time)
            .ToList();
    }

    /// <summary>
    /// Adds or updates a reminder
    /// </summary>
    public void SaveReminder(Reminder reminder)
    {
        var existing = Reminders.Reminders.FirstOrDefault(r => r.Id == reminder.Id);
        if (existing != null)
        {
            Reminders.Reminders.Remove(existing);
        }

        Reminders.Reminders.Add(reminder);
        SaveReminders();
    }

    /// <summary>
    /// Deletes a reminder by ID
    /// </summary>
    public void DeleteReminder(Guid id)
    {
        var reminder = Reminders.Reminders.FirstOrDefault(r => r.Id == id);
        if (reminder != null)
        {
            Reminders.Reminders.Remove(reminder);
            SaveReminders();
        }
    }

    /// <summary>
    /// Gets the number of reminders for a specific day
    /// </summary>
    public int GetReminderCountForDay(ReminderDay day)
    {
        return Reminders.Reminders.Count(r => r.Day == day);
    }
}
