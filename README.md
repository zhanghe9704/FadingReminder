# FadingReminder

A portable Windows 11 desktop application for periodic screen tinting and time-based reminders with visual notifications.

## Features

### Current Features

#### Phase 1 - Infrastructure ✅
- ✅ System tray integration
- ✅ Settings persistence (JSON-based)
- ✅ Auto-start functionality
- ✅ Portable deployment (no installation required)
- ✅ Basic application architecture

#### Phase 2 - Screen Tinting ✅
- ✅ Periodic screen tinting every N minutes
- ✅ Configurable tint color (M) with live preview
- ✅ Adjustable opacity (20%-80%)
- ✅ Configurable duration (1-10 seconds)
- ✅ Multi-monitor support
- ✅ Quick color selection buttons
- ✅ Test tint functionality
- ✅ Enable/disable tinting toggle
- ✅ Click-to-dismiss overlays

#### Phase 3 - Reminder System ✅
- ✅ 10 reminder slots for today
- ✅ 10 reminder slots for tomorrow
- ✅ Time-based scheduling (HH:MM format)
- ✅ Custom reminder messages (up to 200 chars)
- ✅ Screen tint with message when reminder triggers
- ✅ Enable/disable individual reminders
- ✅ Test reminder functionality
- ✅ Automatic date rollover at midnight
- ✅ Input validation and auto-formatting
- ✅ Persistent storage

### Coming Soon
- ⏳ **Phase 4**: Polish, enhancements, and optimization
- ⏳ **Phase 5**: Final testing and documentation

## System Requirements

- **Operating System**: Windows 10/11 (64-bit)
- **Framework**: .NET 8 Runtime (included in self-contained build)
- **Disk Space**: ~100 MB
- **Memory**: < 50 MB RAM

## Building the Application

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Windows 10/11
- Visual Studio 2022 (optional, for development)

### Build Instructions

#### Option 1: Using the Build Script (Recommended)
1. Double-click `build-portable.bat`
2. Wait for the build to complete
3. Find the output in `src/FadingReminder/bin/Release/net8.0-windows/win-x64/publish/`

#### Option 2: Using Command Line
```bash
cd src/FadingReminder
dotnet publish --configuration Release --runtime win-x64 --self-contained true
```

#### Option 3: Using Visual Studio
1. Open `src/FadingReminder/FadingReminder.csproj` in Visual Studio 2022
2. Set build configuration to **Release**
3. Right-click the project → **Publish**
4. Follow the publish wizard

## Running the Application

### First Run
1. Copy the entire `publish` folder to your desired location
2. Run `FadingReminder.exe`
3. The application will appear in the system tray (notification area)

### Command Line Arguments
- `/silent` - Start minimized without showing balloon tip

### System Tray Menu
- **Settings** - Configure tint color and interval (coming in Phase 2)
- **Reminders** - Manage your reminders (coming in Phase 3)
- **Disable/Enable Tinting** - Toggle periodic tinting on/off
- **Exit** - Close the application

## Configuration

All settings are stored in the application directory:
- `settings.json` - Application settings
- `reminders.json` - Reminder data

### Default Settings
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

## Auto-Start with Windows

### Enable Auto-Start
1. Open Settings from the system tray menu
2. Check "Start with Windows"
3. Click Save

The application will automatically start when you log in to Windows.

### Manual Registry Entry
If you prefer to set it up manually:
```
Key: HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run
Name: FadingReminder
Value: "C:\Path\To\FadingReminder.exe" /silent
```

## Uninstallation

Since this is a portable application:
1. Exit the application from the system tray
2. Delete the application folder
3. (Optional) Remove the registry entry if auto-start was enabled:
   - Open Registry Editor (regedit)
   - Navigate to: `HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run`
   - Delete the "FadingReminder" entry

## Project Structure

```
FadingReminder/
├── src/
│   └── FadingReminder/
│       ├── Services/           # Core services
│       │   ├── SettingsManager.cs
│       │   ├── SystemTrayManager.cs
│       │   ├── AutoStartManager.cs
│       │   ├── ScreenTintService.cs      (Phase 2)
│       │   ├── IntervalTimerService.cs   (Phase 2)
│       │   └── ReminderService.cs        (Phase 3)
│       ├── Models/             # Data models
│       │   ├── AppSettings.cs
│       │   └── Reminder.cs
│       ├── Views/              # XAML windows
│       ├── ViewModels/         # View models
│       └── Helpers/            # Utility classes
│           ├── ColorHelper.cs
│           └── DateTimeHelper.cs
├── docs/
│   └── development-plan.md     # Detailed development plan
├── build-portable.bat          # Build script
└── README.md                   # This file
```

## Development Status

### Phase 1: Infrastructure ✅ (COMPLETED)
- [x] Project setup
- [x] Basic architecture
- [x] Settings manager with JSON persistence
- [x] System tray integration
- [x] Auto-start manager
- [x] Build configuration

### Phase 2: Screen Tinting ✅ (COMPLETED)
- [x] Screen tint overlay window
- [x] Interval timer service
- [x] Settings UI for tint configuration
- [x] Color picker implementation
- [x] Multi-monitor support
- [x] Test tint functionality

### Phase 3: Reminder System ✅ (COMPLETED)
- [x] Reminder scheduling service
- [x] Reminders management UI
- [x] Reminder notifications with tint overlay
- [x] Date rollover logic
- [x] Test reminder functionality
- [x] Input validation and auto-formatting

### Phase 4: Polish & Testing ⏳ (Optional Enhancements)
- [ ] UI/UX improvements
- [ ] Additional multi-monitor features
- [ ] Performance optimization
- [ ] Comprehensive testing
- [ ] User documentation

## Troubleshooting

### Application doesn't start
- Make sure you have Windows 10/11 (64-bit)
- Check that all files from the `publish` folder are present
- Try running as Administrator

### System tray icon doesn't appear
- Check Windows notification area settings
- Make sure the application is running (check Task Manager)

### Settings not saving
- Ensure the application has write permissions to its directory
- Check if `settings.json` exists and is not read-only

### Auto-start not working
- Verify the registry entry exists
- Check that the path in the registry is correct
- Make sure the application hasn't been moved

## Contributing

This is currently in active development. Contributions are welcome!

## License

MIT License - See LICENSE file for details

## Support

For issues and feature requests, please use the GitHub issue tracker.

## Roadmap

See `docs/development-plan.md` for detailed development timeline and feature roadmap.

---

**Current Version**: 1.0.0-rc (Phase 3 - Release Candidate)
**Last Updated**: 2025-11-17
