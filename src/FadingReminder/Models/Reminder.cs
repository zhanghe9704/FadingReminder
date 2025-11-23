namespace FadingReminder.Models;

/// <summary>
/// Represents a single reminder with time and message
/// </summary>
public class Reminder
{
    /// <summary>
    /// Unique identifier for the reminder
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Time of day when the reminder should trigger (HH:MM)
    /// </summary>
    public TimeSpan Time { get; set; }

    /// <summary>
    /// Message to display when the reminder triggers
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Which day this reminder is for (Today or Tomorrow)
    /// </summary>
    public ReminderDay Day { get; set; }

    /// <summary>
    /// Whether this reminder is enabled
    /// </summary>
    public bool IsEnabled { get; set; } = true;

    /// <summary>
    /// Whether this reminder has already been triggered today
    /// (used to prevent duplicate triggering)
    /// </summary>
    public bool HasTriggered { get; set; } = false;

    /// <summary>
    /// The date this reminder was created (for tracking purposes)
    /// </summary>
    public DateTime CreatedDate { get; set; } = DateTime.Today;

    /// <summary>
    /// Custom tint color for this reminder in ARGB hex format (e.g., "#80FF0000")
    /// If null or empty, the global settings color will be used
    /// </summary>
    public string? TintColor { get; set; }
}

/// <summary>
/// Enumeration for which day a reminder belongs to
/// </summary>
public enum ReminderDay
{
    Today,
    Tomorrow
}

/// <summary>
/// Container for all reminders
/// </summary>
public class ReminderCollection
{
    /// <summary>
    /// List of all reminders (both today and tomorrow)
    /// </summary>
    public List<Reminder> Reminders { get; set; } = new();

    /// <summary>
    /// The date when reminders were last updated
    /// (used for detecting date rollover)
    /// </summary>
    public DateTime LastUpdateDate { get; set; } = DateTime.Today;
}
