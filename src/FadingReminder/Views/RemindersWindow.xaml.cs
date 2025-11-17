using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using FadingReminder.Helpers;
using FadingReminder.Models;
using FadingReminder.Services;
using TextBox = System.Windows.Controls.TextBox;
using CheckBox = System.Windows.Controls.CheckBox;
using Button = System.Windows.Controls.Button;
using MessageBox = System.Windows.MessageBox;
using Color = System.Windows.Media.Color;
using HorizontalAlignment = System.Windows.HorizontalAlignment;
using VerticalAlignment = System.Windows.VerticalAlignment;

namespace FadingReminder.Views;

/// <summary>
/// Reminders management window
/// </summary>
public partial class RemindersWindow : Window
{
    private readonly SettingsManager _settingsManager;
    private readonly ReminderService? _reminderService;
    private readonly List<ReminderSlot> _todaySlots = new();
    private readonly List<ReminderSlot> _tomorrowSlots = new();
    private const int MaxRemindersPerDay = 10;

    public RemindersWindow(SettingsManager settingsManager, ReminderService? reminderService = null)
    {
        InitializeComponent();

        _settingsManager = settingsManager;
        _reminderService = reminderService;

        InitializeReminderSlots();
        LoadReminders();
    }

    private void InitializeReminderSlots()
    {
        // Create 10 slots for today
        for (int i = 0; i < MaxRemindersPerDay; i++)
        {
            var slot = CreateReminderSlot(i + 1, ReminderDay.Today);
            _todaySlots.Add(slot);
            TodayRemindersPanel.Children.Add(slot.Panel);
        }

        // Create 10 slots for tomorrow
        for (int i = 0; i < MaxRemindersPerDay; i++)
        {
            var slot = CreateReminderSlot(i + 1, ReminderDay.Tomorrow);
            _tomorrowSlots.Add(slot);
            TomorrowRemindersPanel.Children.Add(slot.Panel);
        }
    }

    private ReminderSlot CreateReminderSlot(int slotNumber, ReminderDay day)
    {
        var slot = new ReminderSlot();

        // Main panel
        var panel = new Border
        {
            BorderBrush = new SolidColorBrush(Color.FromRgb(200, 200, 200)),
            BorderThickness = new Thickness(1),
            CornerRadius = new CornerRadius(5),
            Padding = new Thickness(10),
            Margin = new Thickness(0, 5, 0, 5),
            Background = new SolidColorBrush(Colors.White)
        };

        var grid = new Grid();
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(40) }); // Enable checkbox
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(80) }); // Time
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }); // Message
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(60) }); // Test button

        // Enable checkbox
        var enableCheckBox = new CheckBox
        {
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalAlignment = HorizontalAlignment.Center,
            IsChecked = false
        };
        enableCheckBox.Checked += (s, e) => UpdateSlotState(slot);
        enableCheckBox.Unchecked += (s, e) => UpdateSlotState(slot);
        Grid.SetColumn(enableCheckBox, 0);
        grid.Children.Add(enableCheckBox);

        // Time input (HH:MM)
        var timeTextBox = new TextBox
        {
            Width = 60,
            VerticalAlignment = VerticalAlignment.Center,
            Text = "",
            MaxLength = 5,
            ToolTip = "HH:MM (24-hour format)"
        };
        timeTextBox.PreviewTextInput += TimeTextBox_PreviewTextInput;
        timeTextBox.TextChanged += (s, e) => FormatTimeInput(timeTextBox);
        Grid.SetColumn(timeTextBox, 1);
        grid.Children.Add(timeTextBox);

        // Message input
        var messageTextBox = new TextBox
        {
            Margin = new Thickness(10, 0, 0, 0),
            VerticalAlignment = VerticalAlignment.Center,
            MaxLength = 200,
            ToolTip = "Reminder message (max 200 characters)"
        };
        Grid.SetColumn(messageTextBox, 2);
        grid.Children.Add(messageTextBox);

        // Test button
        var testButton = new Button
        {
            Content = "Test",
            Width = 50,
            Height = 25,
            Margin = new Thickness(5, 0, 0, 0),
            Background = new SolidColorBrush(Color.FromRgb(33, 150, 243)),
            Foreground = new SolidColorBrush(Colors.White),
            BorderThickness = new Thickness(0),
            ToolTip = "Test this reminder"
        };
        testButton.Click += (s, e) => TestReminder(slot);
        Grid.SetColumn(testButton, 3);
        grid.Children.Add(testButton);

        panel.Child = grid;

        slot.Panel = panel;
        slot.EnableCheckBox = enableCheckBox;
        slot.TimeTextBox = timeTextBox;
        slot.MessageTextBox = messageTextBox;
        slot.TestButton = testButton;
        slot.Day = day;

        UpdateSlotState(slot);

        return slot;
    }

    private void UpdateSlotState(ReminderSlot slot)
    {
        bool isEnabled = slot.EnableCheckBox.IsChecked ?? false;
        slot.TimeTextBox.IsEnabled = isEnabled;
        slot.MessageTextBox.IsEnabled = isEnabled;
        slot.TestButton.IsEnabled = isEnabled;

        // Visual feedback
        slot.TimeTextBox.Opacity = isEnabled ? 1.0 : 0.5;
        slot.MessageTextBox.Opacity = isEnabled ? 1.0 : 0.5;
    }

    private void TimeTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        // Allow only digits and colon
        Regex regex = new Regex("[^0-9:]+");
        e.Handled = regex.IsMatch(e.Text);
    }

    private void FormatTimeInput(TextBox textBox)
    {
        // Auto-format time input (add colon after 2 digits)
        string text = textBox.Text.Replace(":", "");
        if (text.Length >= 2)
        {
            int cursorPosition = textBox.SelectionStart;
            textBox.Text = text.Insert(2, ":");
            textBox.SelectionStart = Math.Min(cursorPosition + (cursorPosition > 2 ? 1 : 0), textBox.Text.Length);
        }
    }

    private void LoadReminders()
    {
        // Load today's reminders
        var todayReminders = _settingsManager.GetRemindersForDay(ReminderDay.Today);
        for (int i = 0; i < Math.Min(todayReminders.Count, MaxRemindersPerDay); i++)
        {
            var reminder = todayReminders[i];
            var slot = _todaySlots[i];

            slot.ReminderId = reminder.Id;
            slot.EnableCheckBox.IsChecked = reminder.IsEnabled;
            slot.TimeTextBox.Text = DateTimeHelper.FormatTime(reminder.Time);
            slot.MessageTextBox.Text = reminder.Message;

            UpdateSlotState(slot);
        }

        // Load tomorrow's reminders
        var tomorrowReminders = _settingsManager.GetRemindersForDay(ReminderDay.Tomorrow);
        for (int i = 0; i < Math.Min(tomorrowReminders.Count, MaxRemindersPerDay); i++)
        {
            var reminder = tomorrowReminders[i];
            var slot = _tomorrowSlots[i];

            slot.ReminderId = reminder.Id;
            slot.EnableCheckBox.IsChecked = reminder.IsEnabled;
            slot.TimeTextBox.Text = DateTimeHelper.FormatTime(reminder.Time);
            slot.MessageTextBox.Text = reminder.Message;

            UpdateSlotState(slot);
        }
    }

    private void SaveReminders()
    {
        var reminders = new List<Reminder>();

        // Collect today's reminders
        foreach (var slot in _todaySlots)
        {
            if (slot.EnableCheckBox.IsChecked == true)
            {
                if (!ValidateSlot(slot, out string errorMessage))
                {
                    MessageBox.Show(errorMessage, "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var time = DateTimeHelper.ParseTime(slot.TimeTextBox.Text);
                if (time.HasValue)
                {
                    var reminder = new Reminder
                    {
                        Id = slot.ReminderId ?? Guid.NewGuid(),
                        Time = time.Value,
                        Message = slot.MessageTextBox.Text.Trim(),
                        Day = ReminderDay.Today,
                        IsEnabled = true,
                        HasTriggered = false,
                        CreatedDate = DateTime.Today
                    };
                    reminders.Add(reminder);
                }
            }
        }

        // Collect tomorrow's reminders
        foreach (var slot in _tomorrowSlots)
        {
            if (slot.EnableCheckBox.IsChecked == true)
            {
                if (!ValidateSlot(slot, out string errorMessage))
                {
                    MessageBox.Show(errorMessage, "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var time = DateTimeHelper.ParseTime(slot.TimeTextBox.Text);
                if (time.HasValue)
                {
                    var reminder = new Reminder
                    {
                        Id = slot.ReminderId ?? Guid.NewGuid(),
                        Time = time.Value,
                        Message = slot.MessageTextBox.Text.Trim(),
                        Day = ReminderDay.Tomorrow,
                        IsEnabled = true,
                        HasTriggered = false,
                        CreatedDate = DateTime.Today
                    };
                    reminders.Add(reminder);
                }
            }
        }

        // Save to settings manager
        _settingsManager.Reminders.Reminders = reminders;
        _settingsManager.SaveReminders();

        MessageBox.Show("Reminders saved successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        Close();
    }

    private bool ValidateSlot(ReminderSlot slot, out string errorMessage)
    {
        errorMessage = string.Empty;

        // Validate time
        var time = DateTimeHelper.ParseTime(slot.TimeTextBox.Text);
        if (!time.HasValue)
        {
            errorMessage = $"Invalid time format: {slot.TimeTextBox.Text}. Use HH:MM (24-hour format).";
            return false;
        }

        // Validate message
        if (string.IsNullOrWhiteSpace(slot.MessageTextBox.Text))
        {
            errorMessage = "Message cannot be empty for enabled reminders.";
            return false;
        }

        return true;
    }

    private void TestReminder(ReminderSlot slot)
    {
        if (_reminderService == null)
        {
            MessageBox.Show("Reminder service not available", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        if (!ValidateSlot(slot, out string errorMessage))
        {
            MessageBox.Show(errorMessage, "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        var time = DateTimeHelper.ParseTime(slot.TimeTextBox.Text);
        if (time.HasValue)
        {
            var reminder = new Reminder
            {
                Time = time.Value,
                Message = slot.MessageTextBox.Text.Trim(),
                Day = slot.Day
            };

            _reminderService.TestReminder(reminder);
        }
    }

    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        SaveReminders();
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }

    // Helper class to hold reminder slot controls
    private class ReminderSlot
    {
        public Border Panel { get; set; } = null!;
        public CheckBox EnableCheckBox { get; set; } = null!;
        public TextBox TimeTextBox { get; set; } = null!;
        public TextBox MessageTextBox { get; set; } = null!;
        public Button TestButton { get; set; } = null!;
        public ReminderDay Day { get; set; }
        public Guid? ReminderId { get; set; }
    }
}
