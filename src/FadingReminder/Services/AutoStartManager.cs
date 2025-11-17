using Microsoft.Win32;

namespace FadingReminder.Services;

/// <summary>
/// Manages Windows auto-start functionality via registry
/// </summary>
public class AutoStartManager
{
    private const string RunRegistryKey = @"Software\Microsoft\Windows\CurrentVersion\Run";
    private const string AppName = "FadingReminder";

    /// <summary>
    /// Checks if auto-start is currently enabled
    /// </summary>
    public bool IsAutoStartEnabled()
    {
        try
        {
            using var key = Registry.CurrentUser.OpenSubKey(RunRegistryKey, false);
            if (key == null) return false;

            var value = key.GetValue(AppName);
            return value != null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error checking auto-start: {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// Enables auto-start by adding registry entry
    /// </summary>
    public bool EnableAutoStart()
    {
        try
        {
            using var key = Registry.CurrentUser.OpenSubKey(RunRegistryKey, true);
            if (key == null)
            {
                Console.WriteLine("Failed to open registry key for writing");
                return false;
            }

            string exePath = Environment.ProcessPath ??
                           System.Reflection.Assembly.GetExecutingAssembly().Location;

            // Add /silent argument to start minimized
            string commandLine = $"\"{exePath}\" /silent";

            key.SetValue(AppName, commandLine);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error enabling auto-start: {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// Disables auto-start by removing registry entry
    /// </summary>
    public bool DisableAutoStart()
    {
        try
        {
            using var key = Registry.CurrentUser.OpenSubKey(RunRegistryKey, true);
            if (key == null) return false;

            if (key.GetValue(AppName) != null)
            {
                key.DeleteValue(AppName);
            }

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error disabling auto-start: {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// Toggles auto-start on or off
    /// </summary>
    public bool ToggleAutoStart(bool enable)
    {
        return enable ? EnableAutoStart() : DisableAutoStart();
    }

    /// <summary>
    /// Gets the current auto-start command line
    /// </summary>
    public string? GetAutoStartCommand()
    {
        try
        {
            using var key = Registry.CurrentUser.OpenSubKey(RunRegistryKey, false);
            if (key == null) return null;

            var value = key.GetValue(AppName);
            return value?.ToString();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting auto-start command: {ex.Message}");
            return null;
        }
    }
}
