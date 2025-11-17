# FadingReminder

**A simple Windows desktop app to remind you to rest your eyes and take breaks.**

FadingReminder periodically tints your screen with a customizable color overlay, displaying the current time and a personalized message. Perfect for the 20-20-20 rule (every 20 minutes, look at something 20 feet away for 20 seconds) or any regular break schedule.

## Features

- **Periodic Screen Tinting**: Automatically shows a color overlay at regular intervals
- **Custom Messages**: Personalize the reminder message shown with each tint
- **Current Time Display**: See the time prominently displayed on every reminder
- **Flexible Scheduling**: Set intervals from 1-120 minutes
- **Time-Based Reminders**: Schedule up to 10 reminders for today and 10 for tomorrow
- **Multi-Monitor Support**: Show tints on all monitors or just your primary screen
- **Customizable Appearance**: Choose tint color, opacity, and duration
- **Portable**: No installation required - runs from any folder

## Quick Start

1. **Download** the latest release or build from source (see [dev.md](dev.md))
2. **Run** `FadingReminder.exe`
3. **Configure** by right-clicking the system tray icon and selecting "Settings"
4. **Set up reminders** from the system tray menu

## Usage

### System Tray Icon

FadingReminder runs in your system tray (notification area). Right-click the icon to:
- Open **Settings** to configure tint color, interval, and custom message
- Manage **Reminders** for specific times
- **Enable/Disable** periodic tinting
- **Exit** the application

### Settings

- **Tint Color**: Choose from preset colors or enter a custom hex color (ARGB format)
- **Interval**: How often to show the tint (e.g., every 20 minutes)
- **Custom Message**: Personalize the message shown with the current time
- **Duration**: How long the tint stays visible (1-10 seconds)
- **Opacity**: How transparent/opaque the tint overlay is
- **Multi-Monitor**: Show tint on all screens or just primary
- **Auto-Start**: Launch with Windows

### Creating Reminders

1. Click the system tray icon and select "Reminders"
2. Set a time (e.g., "14:30" or "2:30 PM")
3. Enter your reminder message
4. Enable the reminder
5. Click Save

Reminders automatically roll over at midnight - tomorrow's reminders become today's.

## Example Use Cases

- **20-20-20 Rule**: Set 20-minute interval with message "Look 20 feet away for 20 seconds"
- **Hourly Breaks**: 60-minute interval with message "Time to stand up and stretch!"
- **Hydration**: Remind yourself to drink water every 30 minutes
- **Medication**: Set specific time-based reminders for daily medication
- **Meeting Prep**: Get a reminder 5 minutes before your scheduled meetings

## System Requirements

- Windows 10 or 11 (64-bit)
- ~100 MB disk space
- No additional software needed (self-contained)

## Links

- **User Guide**: See [USER-GUIDE.md](USER-GUIDE.md) for detailed instructions
- **Developer Documentation**: See [dev.md](dev.md) for build instructions and technical details
- **License**: MIT License (see LICENSE file)

## Support

For issues, questions, or feature requests, please use the GitHub issue tracker.

---

**Take care of your eyes. Take regular breaks. Stay healthy!** 👀
