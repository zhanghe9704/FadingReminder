using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using FadingReminder.Helpers;
using FadingReminder.Models;
using FadingReminder.Services;

namespace FadingReminder.Views;

/// <summary>
/// Settings window for configuring tint parameters
/// </summary>
public partial class SettingsWindow : Window
{
    private readonly SettingsManager _settingsManager;
    private readonly AutoStartManager _autoStartManager;
    private readonly ScreenTintService? _screenTintService;
    private readonly IntervalTimerService? _intervalTimerService;

    public SettingsWindow(
        SettingsManager settingsManager,
        AutoStartManager autoStartManager,
        ScreenTintService? screenTintService = null,
        IntervalTimerService? intervalTimerService = null)
    {
        InitializeComponent();

        _settingsManager = settingsManager;
        _autoStartManager = autoStartManager;
        _screenTintService = screenTintService;
        _intervalTimerService = intervalTimerService;

        LoadSettings();
    }

    private void LoadSettings()
    {
        var settings = _settingsManager.Settings;

        // Load values into UI
        ColorTextBox.Text = settings.TintColor;
        IntervalTextBox.Text = settings.IntervalMinutes.ToString();
        DurationTextBox.Text = settings.TintDurationSeconds.ToString();
        OpacitySlider.Value = settings.TintOpacity;
        ShowOnAllMonitorsCheckBox.IsChecked = settings.ShowOnAllMonitors;
        EnableTintingCheckBox.IsChecked = settings.IsTintingEnabled;
        AutoStartCheckBox.IsChecked = settings.AutoStartEnabled;
        StartMinimizedCheckBox.IsChecked = settings.StartMinimized;

        // Update color preview
        UpdateColorPreview();
    }

    private void SaveSettings()
    {
        // Validate inputs
        if (!ValidateInputs(out string errorMessage))
        {
            MessageBox.Show(errorMessage, "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        // Save settings
        var settings = _settingsManager.Settings;
        settings.TintColor = ColorTextBox.Text.Trim();
        settings.IntervalMinutes = int.Parse(IntervalTextBox.Text);
        settings.TintDurationSeconds = int.Parse(DurationTextBox.Text);
        settings.TintOpacity = OpacitySlider.Value;
        settings.ShowOnAllMonitors = ShowOnAllMonitorsCheckBox.IsChecked ?? false;
        settings.IsTintingEnabled = EnableTintingCheckBox.IsChecked ?? true;
        settings.AutoStartEnabled = AutoStartCheckBox.IsChecked ?? false;
        settings.StartMinimized = StartMinimizedCheckBox.IsChecked ?? false;

        _settingsManager.SaveSettings();

        // Restart interval timer if it exists and tinting is enabled
        if (_intervalTimerService != null)
        {
            if (settings.IsTintingEnabled)
            {
                _intervalTimerService.Restart();
            }
            else
            {
                _intervalTimerService.Stop();
            }
        }

        MessageBox.Show("Settings saved successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        Close();
    }

    private bool ValidateInputs(out string errorMessage)
    {
        errorMessage = string.Empty;

        // Validate color
        try
        {
            ColorHelper.FromHex(ColorTextBox.Text.Trim());
        }
        catch
        {
            errorMessage = "Invalid color format. Use ARGB hex format (e.g., #80FF0000)";
            return false;
        }

        // Validate interval
        if (!int.TryParse(IntervalTextBox.Text, out int interval) || interval < 1 || interval > 120)
        {
            errorMessage = "Interval must be between 1 and 120 minutes";
            return false;
        }

        // Validate duration
        if (!int.TryParse(DurationTextBox.Text, out int duration) || duration < 1 || duration > 10)
        {
            errorMessage = "Duration must be between 1 and 10 seconds";
            return false;
        }

        return true;
    }

    private void UpdateColorPreview()
    {
        try
        {
            Color color = ColorHelper.FromHex(ColorTextBox.Text.Trim());
            ColorPreview.Background = new SolidColorBrush(color);
        }
        catch
        {
            // Invalid color - show error color
            ColorPreview.Background = new SolidColorBrush(Colors.Gray);
        }
    }

    private void ColorTextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        UpdateColorPreview();
    }

    private void QuickColor_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.Tag is string colorHex)
        {
            ColorTextBox.Text = colorHex;
        }
    }

    private void NumericTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        // Only allow numeric input
        Regex regex = new Regex("[^0-9]+");
        e.Handled = regex.IsMatch(e.Text);
    }

    private void TestTintButton_Click(object sender, RoutedEventArgs e)
    {
        if (_screenTintService == null)
        {
            MessageBox.Show("Screen tint service not available", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        // Validate inputs first
        if (!ValidateInputs(out string errorMessage))
        {
            MessageBox.Show(errorMessage, "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        // Temporarily update settings for test
        var originalSettings = new AppSettings
        {
            TintColor = _settingsManager.Settings.TintColor,
            TintOpacity = _settingsManager.Settings.TintOpacity,
            TintDurationSeconds = _settingsManager.Settings.TintDurationSeconds,
            ShowOnAllMonitors = _settingsManager.Settings.ShowOnAllMonitors
        };

        _settingsManager.Settings.TintColor = ColorTextBox.Text.Trim();
        _settingsManager.Settings.TintOpacity = OpacitySlider.Value;
        _settingsManager.Settings.TintDurationSeconds = int.Parse(DurationTextBox.Text);
        _settingsManager.Settings.ShowOnAllMonitors = ShowOnAllMonitorsCheckBox.IsChecked ?? false;

        // Show test tint
        _screenTintService.ShowTint("Test Tint\n\nThis is how your tint will look.");

        // Restore original settings (don't save the test)
        _settingsManager.Settings.TintColor = originalSettings.TintColor;
        _settingsManager.Settings.TintOpacity = originalSettings.TintOpacity;
        _settingsManager.Settings.TintDurationSeconds = originalSettings.TintDurationSeconds;
        _settingsManager.Settings.ShowOnAllMonitors = originalSettings.ShowOnAllMonitors;
    }

    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        SaveSettings();
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void AutoStartCheckBox_Changed(object sender, RoutedEventArgs e)
    {
        // This will be handled when Save is clicked
        // Just update the UI immediately for better UX
        bool isChecked = AutoStartCheckBox.IsChecked ?? false;

        if (isChecked && !_autoStartManager.IsAutoStartEnabled())
        {
            if (!_autoStartManager.EnableAutoStart())
            {
                MessageBox.Show("Failed to enable auto-start. Please check permissions.", "Warning",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                AutoStartCheckBox.IsChecked = false;
            }
        }
        else if (!isChecked && _autoStartManager.IsAutoStartEnabled())
        {
            if (!_autoStartManager.DisableAutoStart())
            {
                MessageBox.Show("Failed to disable auto-start. Please check permissions.", "Warning",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                AutoStartCheckBox.IsChecked = true;
            }
        }
    }
}
