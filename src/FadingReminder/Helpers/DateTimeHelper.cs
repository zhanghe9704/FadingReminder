namespace FadingReminder.Helpers;

/// <summary>
/// Helper class for date and time operations
/// </summary>
public static class DateTimeHelper
{
    /// <summary>
    /// Parses a time string (HH:MM) to TimeSpan
    /// </summary>
    /// <param name="timeString">Time string in HH:MM format</param>
    /// <returns>TimeSpan object, or null if parsing fails</returns>
    public static TimeSpan? ParseTime(string timeString)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(timeString))
                return null;

            var parts = timeString.Split(':');
            if (parts.Length != 2)
                return null;

            if (int.TryParse(parts[0], out int hours) && int.TryParse(parts[1], out int minutes))
            {
                if (hours >= 0 && hours < 24 && minutes >= 0 && minutes < 60)
                {
                    return new TimeSpan(hours, minutes, 0);
                }
            }

            return null;
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Formats a TimeSpan to HH:MM string
    /// </summary>
    /// <param name="time">TimeSpan to format</param>
    /// <returns>Formatted time string (HH:MM)</returns>
    public static string FormatTime(TimeSpan time)
    {
        return $"{time.Hours:D2}:{time.Minutes:D2}";
    }

    /// <summary>
    /// Checks if a specific time has passed today
    /// </summary>
    /// <param name="time">Time to check</param>
    /// <returns>True if the time has already passed today</returns>
    public static bool HasTimePassed(TimeSpan time)
    {
        return DateTime.Now.TimeOfDay > time;
    }

    /// <summary>
    /// Gets the next occurrence of a specific time
    /// </summary>
    /// <param name="time">Target time</param>
    /// <returns>DateTime of next occurrence (today or tomorrow)</returns>
    public static DateTime GetNextOccurrence(TimeSpan time)
    {
        var today = DateTime.Today.Add(time);

        if (DateTime.Now < today)
        {
            return today;
        }
        else
        {
            return today.AddDays(1);
        }
    }

    /// <summary>
    /// Calculates time remaining until a specific time today
    /// </summary>
    /// <param name="time">Target time</param>
    /// <returns>TimeSpan remaining, or null if time has passed</returns>
    public static TimeSpan? GetTimeRemaining(TimeSpan time)
    {
        if (HasTimePassed(time))
            return null;

        return time - DateTime.Now.TimeOfDay;
    }

    /// <summary>
    /// Formats a duration in a human-readable format
    /// </summary>
    /// <param name="duration">Duration to format</param>
    /// <returns>Formatted string (e.g., "5m 30s", "1h 15m")</returns>
    public static string FormatDuration(TimeSpan duration)
    {
        if (duration.TotalHours >= 1)
        {
            return $"{(int)duration.TotalHours}h {duration.Minutes}m";
        }
        else if (duration.TotalMinutes >= 1)
        {
            return $"{(int)duration.TotalMinutes}m {duration.Seconds}s";
        }
        else
        {
            return $"{duration.Seconds}s";
        }
    }
}
