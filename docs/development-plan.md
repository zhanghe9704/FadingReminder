# FadingReminder - Development Plan

## Overview
FadingReminder is a portable Windows 11 desktop application that provides periodic screen tinting and time-based reminder functionality with visual notifications.

## Core Requirements

### 1. Periodic Screen Tinting
- Tint the screen with a user-defined color (M) every N minutes
- Both color M and interval N are configurable by the user
- Non-intrusive, temporary overlay

### 2. Reminder System
- Support for reminders for "today" and "tomorrow"
- 10 reminder slots per day (20 total)
- Each reminder includes:
  - Time (HH:MM format)
  - Message text
- When triggered, display screen tint (color M) with the reminder message

### 3. Portability & Auto-Start
- No installation required (portable executable)
- All settings stored in application directory
- User can add to Windows auto-start
- Self-contained deployment

## Technology Stack

### Recommended: WPF with .NET 8
**Rationale:**
- Native Windows 11 support
- Excellent for screen overlays and transparency
- Self-contained deployment for true portability
- Rich UI capabilities with XAML
- Mature ecosystem and tooling

**Deployment:**
- .NET 8 with self-contained publish
- Single-file or folder deployment
- No external runtime dependencies

## Application Architecture

### Core Components

1. **ScreenTintService**
   - Creates fullscreen transparent overlay window
   - Applies color tint with configurable opacity
   - Auto-dismisses after duration or user interaction

2. **IntervalTimerService**
   - Manages N-minute periodic tinting
   - Runs on background thread
   - Configurable interval

3. **ReminderService**
   - Schedules and triggers reminders
   - Checks reminder times every minute
   - Handles date rollover (today → tomorrow)
   - Persists reminders to JSON

4. **SettingsManager**
   - Loads/saves application settings (JSON)
   - Manages color M and interval N
   - Stores tint duration and opacity
   - All data stored in application directory

5. **AutoStartManager**
   - Manages Windows registry entry for auto-start
   - Adds/removes from HKEY_CURRENT_USER\...\Run
   - Validates application path

6. **SystemTrayManager**
   - System tray icon and context menu
   - Quick access to settings and reminders
   - Application status indicator

### Data Models

```csharp
// AppSettings.cs
public class AppSettings
{
    public string TintColor { get; set; } // ARGB hex
    public int IntervalMinutes { get; set; } // N
    public int TintDurationSeconds { get; set; }
    public double TintOpacity { get; set; } // 0.0 - 1.0
    public bool AutoStartEnabled { get; set; }
}

// Reminder.cs
public class Reminder
{
    public Guid Id { get; set; }
    public TimeSpan Time { get; set; }
    public string Message { get; set; }
    public ReminderDay Day { get; set; } // Today or Tomorrow
    public bool IsEnabled { get; set; }
}

public enum ReminderDay
{
    Today,
    Tomorrow
}
```

## User Interface Design

### 1. System Tray
- **Icon:** Small app icon in system tray
- **Context Menu:**
  - Settings
  - Reminders
  - Enable/Disable Tinting
  - Exit

### 2. Settings Window
- **Tint Color (M):** Color picker control
- **Interval (N):** Numeric input (1-120 minutes)
- **Tint Duration:** 1-10 seconds
- **Tint Opacity:** Slider (20%-80%)
- **Auto-Start:** Checkbox
- **Save/Cancel buttons**

### 3. Reminders Window
- **Two sections:** Today | Tomorrow
- **Each section:**
  - 10 reminder slots
  - Time picker (HH:MM)
  - Message text box
  - Enable/disable checkbox
- **Save/Cancel buttons**

### 4. Tint Overlay Window
- **Fullscreen borderless window**
- **Topmost, transparent background**
- **Colored semi-transparent rectangle**
- **Auto-dismiss:** After duration or on click
- **For reminders:** Display message in center

### 5. Reminder Notification
- **Same tint overlay as periodic**
- **Message displayed in center**
- **Large, readable font**
- **White or contrasting text**
- **Dismiss on click or after duration**

## Development Phases

### Phase 1: Project Setup & Infrastructure (1-2 days)

**Tasks:**
- Create WPF .NET 8 project
- Set up project structure (Services, Models, Views, ViewModels)
- Configure for self-contained deployment
- Implement SettingsManager with JSON persistence
- Create basic SystemTrayManager
- Set up logging framework

**Deliverables:**
- Running WPF app with system tray icon
- Settings save/load functionality
- Basic project architecture

### Phase 2: Screen Tinting Feature (2-3 days)

**Tasks:**
- Create TintOverlay window (fullscreen, transparent)
- Implement ScreenTintService
- Create IntervalTimerService
- Build Settings window UI
- Add color picker and interval controls
- Implement tint preview functionality
- Add configuration for duration and opacity

**Deliverables:**
- Working periodic screen tinting
- Settings UI for color M and interval N
- Configurable tint parameters

### Phase 3: Reminder System (3-4 days)

**Tasks:**
- Design Reminder data model
- Implement reminder persistence (JSON)
- Create ReminderService with scheduling
- Build Reminders management window
- Implement date rollover logic (midnight transition)
- Create reminder notification overlay
- Add reminder enable/disable functionality
- Handle reminder conflicts and validation

**Deliverables:**
- Functional reminder system (today & tomorrow)
- Reminders management UI
- Reminder notifications with screen tint

### Phase 4: Auto-Start & Portability (1-2 days)

**Tasks:**
- Implement AutoStartManager
- Add registry manipulation for auto-start
- Configure self-contained publish
- Test portable deployment
- Ensure all paths are relative to app directory
- Create startup argument handling (silent start)
- Add error handling for registry access

**Deliverables:**
- Working auto-start functionality
- True portable deployment
- No external dependencies

### Phase 5: Polish & Testing (2-3 days)

**Tasks:**
- UI/UX improvements and consistency
- Error handling and validation
- Performance optimization (minimize CPU/memory)
- Handle edge cases (midnight rollover, invalid colors, etc.)
- Multi-monitor support testing
- Windows 11 compatibility testing
- Create user documentation (README)
- Add tooltips and help text

**Deliverables:**
- Polished, production-ready application
- Comprehensive error handling
- User documentation

## Project Structure

```
FadingReminder/
├── src/
│   └── FadingReminder/
│       ├── App.xaml
│       ├── App.xaml.cs
│       ├── FadingReminder.csproj
│       │
│       ├── Services/
│       │   ├── ScreenTintService.cs
│       │   ├── IntervalTimerService.cs
│       │   ├── ReminderService.cs
│       │   ├── SettingsManager.cs
│       │   ├── AutoStartManager.cs
│       │   └── SystemTrayManager.cs
│       │
│       ├── Models/
│       │   ├── AppSettings.cs
│       │   └── Reminder.cs
│       │
│       ├── Views/
│       │   ├── SettingsWindow.xaml
│       │   ├── SettingsWindow.xaml.cs
│       │   ├── RemindersWindow.xaml
│       │   ├── RemindersWindow.xaml.cs
│       │   ├── TintOverlay.xaml
│       │   └── TintOverlay.xaml.cs
│       │
│       ├── ViewModels/
│       │   ├── SettingsViewModel.cs
│       │   └── RemindersViewModel.cs
│       │
│       ├── Helpers/
│       │   ├── ColorHelper.cs
│       │   └── DateTimeHelper.cs
│       │
│       └── Resources/
│           ├── Icons/
│           └── Styles/
│
├── docs/
│   ├── development-plan.md
│   └── user-guide.md
│
├── tests/
│   └── FadingReminder.Tests/
│
└── README.md
```

## Technical Specifications

### Screen Tinting
- **Color M:** ARGB format (supports transparency)
- **Interval N:** 1-120 minutes (default: 20 minutes)
- **Tint Duration:** 1-10 seconds (default: 3 seconds)
- **Opacity:** 20%-80% (default: 40%)
- **Display:** Fullscreen overlay on primary monitor (extend to all monitors option)
- **Dismissal:** Auto-dismiss after duration OR click to dismiss

### Reminders
- **Capacity:** 10 reminders for today, 10 for tomorrow
- **Time Format:** 24-hour (HH:MM) with validation
- **Message Length:** 1-200 characters
- **Persistence:** JSON file (reminders.json)
- **Date Rollover:** At midnight, "today" reminders clear, "tomorrow" becomes "today"
- **Notification:** Same tint color M, centered message display
- **Snooze:** Optional 5-minute snooze feature

### Settings Storage
- **Location:** Application directory (portable)
- **Format:** JSON files
  - `settings.json` - App configuration
  - `reminders.json` - Reminder data
- **Default Values:** Provided if files missing
- **Validation:** On load, with fallback to defaults

### Auto-Start
- **Method:** Registry key in HKCU\Software\Microsoft\Windows\CurrentVersion\Run
- **Key Name:** "FadingReminder"
- **Value:** Full path to executable with /silent argument
- **Silent Start:** Minimize to tray on startup
- **Permissions:** User-level (no admin required)

### Portability
- **Deployment:** Self-contained .NET 8 publish
- **Size:** Target <100MB
- **Dependencies:** All bundled
- **Data:** Relative paths, app directory only
- **Uninstall:** Simply delete folder + optional registry cleanup

## Key Implementation Details

### 1. Screen Overlay Implementation
```csharp
// TintOverlay.xaml.cs
- WindowStyle = None
- ResizeMode = NoResize
- Topmost = true
- AllowsTransparency = true
- WindowState = Maximized
- Background = Transparent
- ShowInTaskbar = false
```

### 2. Timer Implementation
- Use System.Threading.Timer for interval-based tinting
- Dispatcher.Invoke for UI thread marshalling
- Proper disposal and cancellation token support

### 3. Reminder Scheduling
- Background service checking every 30-60 seconds
- Compare current time with reminder times
- Mark reminders as triggered to prevent duplicates
- Handle system sleep/wake scenarios

### 4. Date Rollover Logic
- Monitor date changes (DateTime.Date comparison)
- On date change:
  - Clear all "today" reminders
  - Move "tomorrow" to "today"
  - Clear "tomorrow" slots
- Persist changes immediately

### 5. Multi-Monitor Support
- Detect primary monitor
- Option to show tint on all monitors
- Use System.Windows.Forms.Screen for monitor info

## Development Timeline

| Phase | Duration | Tasks |
|-------|----------|-------|
| Phase 1 | 1-2 days | Project setup, infrastructure |
| Phase 2 | 2-3 days | Screen tinting feature |
| Phase 3 | 3-4 days | Reminder system |
| Phase 4 | 1-2 days | Auto-start & portability |
| Phase 5 | 2-3 days | Polish & testing |
| **Total** | **10-14 days** | **Complete application** |

## Success Criteria

- [ ] Application runs without installation
- [ ] Screen tints every N minutes with color M
- [ ] User can configure tint color and interval via UI
- [ ] User can create 10 reminders for today
- [ ] User can create 10 reminders for tomorrow
- [ ] Reminders trigger at specified time with screen tint and message
- [ ] Application can be added to Windows auto-start
- [ ] All settings persist between application restarts
- [ ] Portable deployment (single folder, no external dependencies)
- [ ] Minimal resource usage (<50MB RAM, <1% CPU idle)
- [ ] Clean, intuitive Windows 11-style UI
- [ ] Proper error handling and validation
- [ ] Multi-monitor support
- [ ] Date rollover works correctly at midnight

## Risk Mitigation

### Potential Challenges

1. **Screen Overlay Z-Order Issues**
   - Risk: Overlay not appearing on top
   - Mitigation: Use Topmost window property, test with various apps

2. **Permission Issues for Registry**
   - Risk: Auto-start fails without admin rights
   - Mitigation: Use HKCU (user-level) registry, handle gracefully

3. **Time Synchronization**
   - Risk: Reminders miss trigger time
   - Mitigation: Check frequently, handle system time changes

4. **Deployment Size**
   - Risk: Self-contained .NET app too large
   - Mitigation: Use trimming and single-file publish

5. **Background Service Performance**
   - Risk: High CPU/memory usage
   - Mitigation: Efficient timers, proper disposal, testing

## Future Enhancements (Post-MVP)

- Snooze functionality for reminders
- Recurring reminders (daily, weekly)
- Multiple color profiles
- Sound notifications
- Tint patterns (fade in/out animations)
- Import/export settings
- Cloud sync (optional)
- Statistics (tint history, reminder completion)
- Custom tint shapes (not just fullscreen)
- Hotkey support

## Resources & References

- **WPF Documentation:** https://docs.microsoft.com/en-us/dotnet/desktop/wpf/
- **.NET 8 Self-Contained Deployment:** https://docs.microsoft.com/en-us/dotnet/core/deploying/
- **Windows Registry for Auto-Start:** HKCU\Software\Microsoft\Windows\CurrentVersion\Run
- **Color Picker Control:** Extended WPF Toolkit or custom implementation

## Conclusion

This development plan provides a structured approach to building FadingReminder, a portable Windows 11 application with screen tinting and reminder functionality. The phased approach ensures steady progress while maintaining code quality and user experience standards.
