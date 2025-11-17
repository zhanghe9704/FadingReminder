using System.Drawing;
using System.Windows;
using System.Windows.Forms;
using Application = System.Windows.Application;

namespace FadingReminder.Services;

/// <summary>
/// Manages the system tray icon and context menu
/// </summary>
public class SystemTrayManager : IDisposable
{
    private NotifyIcon? _notifyIcon;
    private ContextMenuStrip? _contextMenu;

    public event EventHandler? SettingsClicked;
    public event EventHandler? RemindersClicked;
    public event EventHandler? ToggleTintingClicked;
    public event EventHandler? ExitClicked;

    private bool _isTintingEnabled = true;

    public SystemTrayManager()
    {
        InitializeTrayIcon();
    }

    private void InitializeTrayIcon()
    {
        // Create context menu
        _contextMenu = new ContextMenuStrip();

        var settingsItem = new ToolStripMenuItem("Settings");
        settingsItem.Click += (s, e) => SettingsClicked?.Invoke(this, EventArgs.Empty);

        var remindersItem = new ToolStripMenuItem("Reminders");
        remindersItem.Click += (s, e) => RemindersClicked?.Invoke(this, EventArgs.Empty);

        var separator1 = new ToolStripSeparator();

        var toggleTintingItem = new ToolStripMenuItem("Disable Tinting");
        toggleTintingItem.Click += (s, e) =>
        {
            _isTintingEnabled = !_isTintingEnabled;
            toggleTintingItem.Text = _isTintingEnabled ? "Disable Tinting" : "Enable Tinting";
            toggleTintingItem.Checked = !_isTintingEnabled;
            ToggleTintingClicked?.Invoke(this, EventArgs.Empty);
        };

        var separator2 = new ToolStripSeparator();

        var exitItem = new ToolStripMenuItem("Exit");
        exitItem.Click += (s, e) => ExitClicked?.Invoke(this, EventArgs.Empty);

        _contextMenu.Items.Add(settingsItem);
        _contextMenu.Items.Add(remindersItem);
        _contextMenu.Items.Add(separator1);
        _contextMenu.Items.Add(toggleTintingItem);
        _contextMenu.Items.Add(separator2);
        _contextMenu.Items.Add(exitItem);

        // Create notify icon
        _notifyIcon = new NotifyIcon
        {
            Icon = CreateDefaultIcon(),
            Text = "FadingReminder",
            Visible = true,
            ContextMenuStrip = _contextMenu
        };

        // Double-click to open settings
        _notifyIcon.DoubleClick += (s, e) => SettingsClicked?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Creates a default icon (colored circle) when no icon file is available
    /// </summary>
    private Icon CreateDefaultIcon()
    {
        try
        {
            // Try to load icon from resources
            string iconPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Icons", "app.ico");
            if (File.Exists(iconPath))
            {
                return new Icon(iconPath);
            }
        }
        catch
        {
            // Fall through to create default icon
        }

        // Create a simple default icon (16x16 blue circle)
        using var bitmap = new Bitmap(16, 16);
        using var g = Graphics.FromImage(bitmap);
        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
        g.Clear(Color.Transparent);
        g.FillEllipse(new SolidBrush(Color.FromArgb(0, 0, 128)), 2, 2, 12, 12);

        IntPtr hIcon = bitmap.GetHicon();
        return Icon.FromHandle(hIcon);
    }

    /// <summary>
    /// Updates the tray icon tooltip
    /// </summary>
    public void UpdateTooltip(string text)
    {
        if (_notifyIcon != null)
        {
            _notifyIcon.Text = text.Length > 63 ? text.Substring(0, 63) : text;
        }
    }

    /// <summary>
    /// Shows a balloon tip notification
    /// </summary>
    public void ShowBalloonTip(string title, string message, ToolTipIcon icon = ToolTipIcon.Info, int timeout = 3000)
    {
        _notifyIcon?.ShowBalloonTip(timeout, title, message, icon);
    }

    /// <summary>
    /// Updates the tinting enabled state in the context menu
    /// </summary>
    public void UpdateTintingState(bool isEnabled)
    {
        _isTintingEnabled = isEnabled;
        if (_contextMenu != null && _contextMenu.Items.Count > 3)
        {
            var toggleItem = _contextMenu.Items[3] as ToolStripMenuItem;
            if (toggleItem != null)
            {
                toggleItem.Text = isEnabled ? "Disable Tinting" : "Enable Tinting";
                toggleItem.Checked = !isEnabled;
            }
        }
    }

    public void Dispose()
    {
        if (_notifyIcon != null)
        {
            _notifyIcon.Visible = false;
            _notifyIcon.Dispose();
            _notifyIcon = null;
        }

        _contextMenu?.Dispose();
        _contextMenu = null;
    }
}
