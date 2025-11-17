# FadingReminder - User Guide

A portable Windows 11 application that helps reduce eye strain and manage your schedule through periodic screen tinting and customizable time-based reminders.

## 📋 What Does It Do?

FadingReminder provides two main features:

1. **Periodic Screen Tinting**: Automatically tints your screen with a custom color every N minutes to remind you to take breaks (perfect for the 20-20-20 rule!)

2. **Time-Based Reminders**: Set up to 20 reminders (10 for today, 10 for tomorrow) that display a screen tint with your custom message at the scheduled time.

## 💻 System Requirements

- **Operating System**: Windows 10 or Windows 11 (64-bit)
- **For Building**: [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- **Disk Space**: ~100 MB
- **Memory**: Less than 50 MB RAM

## 🔨 How to Compile

### Method 1: Using the Build Script (Easiest)

1. Make sure you have [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) installed
2. Double-click `build-portable.bat` in the project root
3. Wait for the build to complete
4. Find the compiled application in:
   ```
   src/FadingReminder/bin/Release/net8.0-windows/win-x64/publish/
   ```

### Method 2: Using Command Line

1. Open Command Prompt or PowerShell
2. Navigate to the project directory:
   ```cmd
   cd path\to\FadingReminder
   ```
3. Build the application:
   ```cmd
   cd src\FadingReminder
   dotnet publish --configuration Release --runtime win-x64 --self-contained true
   ```
4. The compiled files will be in:
   ```
   bin\Release\net8.0-windows\win-x64\publish\
   ```

### Method 3: Using Visual Studio 2022

1. Open `src/FadingReminder/FadingReminder.csproj` in Visual Studio 2022
2. Set the build configuration to **Release**
3. Right-click the project → **Publish**
4. Follow the publish wizard to create the executable

## 📦 Installation

**Good News**: No installation required! FadingReminder is portable.

1. After building, copy the entire `publish` folder to any location you want
2. Run `FadingReminder.exe` from that folder
3. The application will start in your system tray (notification area)

## 🚀 How to Use

### First Time Setup

1. **Launch the Application**
   - Double-click `FadingReminder.exe`
   - The app will appear in your system tray (bottom-right corner, near the clock)
   - Look for the FadingReminder icon

2. **Access the Menu**
   - Right-click the system tray icon to see options:
     - Settings
     - Reminders
     - Enable/Disable Tinting
     - Exit

### Feature 1: Periodic Screen Tinting

This feature tints your entire screen with a color at regular intervals.

**To Configure:**

1. Right-click tray icon → **Settings**

2. **Choose Your Tint Color (M)**
   - Enter a hex color code (e.g., `#80FF0000` for semi-transparent red)
   - Or click one of the quick color buttons (Red, Green, Blue, Yellow, Orange, Purple, Navy)
   - See live preview in the color box

3. **Set the Interval (N)**
   - Enter how often to tint (1-120 minutes)
   - Default: 20 minutes
   - Example: Enter `20` for the 20-20-20 eye care rule

4. **Adjust Display Options**
   - Tint Duration: How long the tint stays (1-10 seconds)
   - Tint Opacity: How transparent it is (20%-80%)
   - Show on all monitors: Check this if you have multiple screens

5. **Test It**
   - Click "Test Tint Now" to preview how it will look
   - Adjust settings if needed

6. **Save**
   - Click "Save" to apply your settings
   - Tinting will begin at your set interval

**To Enable/Disable:**
- Right-click tray icon → "Enable Tinting" or "Disable Tinting"

**Example Configurations:**

**20-20-20 Eye Care Rule** (Every 20 minutes, look at something 20 feet away for 20 seconds)
- Color: Green `#8000FF00`
- Interval: 20 minutes
- Duration: 3 seconds

**Pomodoro Technique** (25-minute work intervals)
- Color: Red `#80FF0000`
- Interval: 25 minutes
- Duration: 5 seconds

### Feature 2: Time-Based Reminders

Set custom reminders that display a screen tint with your message at specific times.

**To Set Up Reminders:**

1. Right-click tray icon → **Reminders**

2. **Choose a Tab**
   - **Today**: For reminders today
   - **Tomorrow**: For reminders tomorrow

3. **For Each Reminder:**
   - ☑️ Check the box to enable it
   - 🕐 Enter the time in 24-hour format (e.g., `14:30` for 2:30 PM)
     - The time auto-formats as you type
   - 💬 Enter your reminder message (up to 200 characters)
   - 🧪 Optional: Click "Test" to preview how it will look

4. **Save**
   - Click "Save All" to save all reminders
   - Reminders will trigger at their scheduled times

**Example Reminders:**

**Work Schedule:**
- `09:00` - "Start work - Check emails"
- `10:00` - "Team standup meeting"
- `12:00` - "Lunch break"
- `14:00` - "Client presentation"
- `17:00` - "End of day review"

**Health Reminders:**
- `09:00` - "Morning vitamins"
- `12:00` - "Drink water"
- `15:00` - "Stretch break"
- `18:00` - "Evening medication"

**What Happens When a Reminder Triggers:**
1. Your screen tints with the configured color
2. Your reminder message appears in the center
3. The current time is displayed
4. Click anywhere to dismiss it

**Date Rollover:**
- At midnight (00:00), "tomorrow" reminders automatically become "today"
- Old reminders are cleared
- You can set new reminders each day

### Auto-Start with Windows

To make FadingReminder start automatically when you log in to Windows:

1. Right-click tray icon → **Settings**
2. Check ✅ "Start with Windows"
3. Click "Save"

The application will now start automatically in the background when you log in.

### Silent Start

If you want the app to start minimized without a notification:
- The auto-start feature automatically uses silent mode
- Or run manually with: `FadingReminder.exe /silent`

## ⚙️ Configuration Files

All settings are stored in the application directory as JSON files:

- `settings.json` - Your tint color, interval, and preferences
- `reminders.json` - Your reminder schedule

**You can edit these manually if needed**, but it's easier to use the UI.

Example `settings.json`:
```json
{
  "TintColor": "#80000080",
  "IntervalMinutes": 20,
  "TintDurationSeconds": 3,
  "TintOpacity": 0.4,
  "IsTintingEnabled": true,
  "AutoStartEnabled": false,
  "ShowOnAllMonitors": false,
  "StartMinimized": false
}
```

## 🎯 Usage Tips

1. **Finding the Right Interval**
   - Start with 20 minutes
   - Adjust based on your comfort
   - Shorter intervals (10-15 min) for intensive screen work
   - Longer intervals (30-45 min) for less demanding tasks

2. **Choosing Colors**
   - **Green/Blue**: Calming, good for eye strain reminders
   - **Orange/Yellow**: Gentle but noticeable
   - **Red**: More attention-grabbing for important reminders

3. **Opacity Settings**
   - 30-40%: Noticeable but not jarring
   - 50-60%: More prominent
   - 20%: Subtle, good for passive reminders

4. **Reminder Best Practices**
   - Don't set too many - 3-5 per day is usually enough
   - Use clear, action-oriented messages
   - Test reminders before saving to see how they look

## 🔧 Troubleshooting

### Application Won't Start
- ✅ Make sure you're running Windows 10 or 11 (64-bit)
- ✅ Check that all files from the `publish` folder are present
- ✅ Try running as Administrator (right-click → Run as administrator)

### System Tray Icon Doesn't Appear
- ✅ Check Windows notification area settings (Settings → Personalization → Taskbar)
- ✅ Look for hidden icons (click the ^ arrow in the system tray)
- ✅ Make sure the application is running (check Task Manager)

### Settings Not Saving
- ✅ Make sure the application folder is not read-only
- ✅ Check that you have write permissions to the folder
- ✅ Don't run from a network drive or protected location

### Reminders Not Triggering
- ✅ Verify the reminder is enabled (checkbox checked)
- ✅ Check the time format is correct (HH:MM, 24-hour)
- ✅ Make sure your computer isn't sleeping at the reminder time
- ✅ Reminders check every 30 seconds - timing accuracy is ±30 seconds

### Tint Doesn't Show on All Monitors
- ✅ Enable "Show on all monitors" in Settings
- ✅ Save settings and restart the application

### Auto-Start Not Working
- ✅ Check Settings → "Start with Windows" is checked
- ✅ Verify the registry entry exists:
  - Open Registry Editor (regedit)
  - Go to: `HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run`
  - Look for "FadingReminder" entry
- ✅ If you moved the application folder, disable and re-enable auto-start

## 🗑️ How to Uninstall

Since FadingReminder is portable (no installer):

1. Right-click tray icon → **Exit**
2. Delete the application folder
3. **Optional**: Remove auto-start entry
   - Open Registry Editor (regedit)
   - Navigate to: `HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run`
   - Delete the "FadingReminder" entry (if present)

## 📝 Quick Reference

### Keyboard Shortcuts
- There are no keyboard shortcuts - the app is controlled via the system tray menu

### Command Line Arguments
- `/silent` - Start minimized to tray without notification

### File Locations
- `settings.json` - Application settings
- `reminders.json` - Reminder data
- Both files are in the same folder as `FadingReminder.exe`

### Default Settings
- **Color**: Navy blue (`#80000080`)
- **Interval**: 20 minutes
- **Duration**: 3 seconds
- **Opacity**: 40%
- **Tinting**: Enabled
- **Auto-start**: Disabled

## ❓ Frequently Asked Questions

**Q: Can I have different tint colors for reminders and periodic tinting?**
A: Currently, both use the same tint color configured in Settings.

**Q: Can reminders repeat daily?**
A: Not automatically. You need to set them up each day, but tomorrow's reminders automatically move to today at midnight.

**Q: What happens if my computer is sleeping when a reminder should trigger?**
A: The reminder won't trigger. It will be skipped for that day.

**Q: Can I snooze a reminder?**
A: Not currently - you can click to dismiss it immediately.

**Q: How many reminders can I have?**
A: 10 for today and 10 for tomorrow (20 total).

**Q: Does this work on Windows 7 or 8?**
A: No, it requires Windows 10 or 11.

## 📞 Support

For issues, questions, or feature requests:
- Check the `docs/` folder for detailed documentation
- Review the troubleshooting section above
- Check the project repository for updates

## 📄 License

MIT License - Free to use, modify, and distribute.

---

**Version**: 1.0.0-rc (Release Candidate)
**Last Updated**: November 2025

## Quick Start Checklist

- [ ] Downloaded and compiled the application
- [ ] Application running in system tray
- [ ] Configured tint color and interval in Settings
- [ ] Tested tint to see how it looks
- [ ] Set up a few reminders for testing
- [ ] Tested a reminder to verify it works
- [ ] Configured auto-start (optional)
- [ ] Adjusted opacity and duration to preference

**Enjoy your FadingReminder experience! 🎉**
