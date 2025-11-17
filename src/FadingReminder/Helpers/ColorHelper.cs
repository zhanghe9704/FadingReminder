using System.Windows.Media;

namespace FadingReminder.Helpers;

/// <summary>
/// Helper class for color conversion and manipulation
/// </summary>
public static class ColorHelper
{
    /// <summary>
    /// Converts a hex color string (ARGB) to a Color object
    /// </summary>
    /// <param name="hexColor">Hex color string (e.g., "#80FF0000")</param>
    /// <returns>Color object</returns>
    public static Color FromHex(string hexColor)
    {
        try
        {
            // Remove # if present
            hexColor = hexColor.TrimStart('#');

            // Ensure we have 8 characters (ARGB)
            if (hexColor.Length == 6)
            {
                // Add full opacity if only RGB provided
                hexColor = "FF" + hexColor;
            }

            if (hexColor.Length != 8)
            {
                throw new ArgumentException("Invalid hex color format");
            }

            byte a = Convert.ToByte(hexColor.Substring(0, 2), 16);
            byte r = Convert.ToByte(hexColor.Substring(2, 2), 16);
            byte g = Convert.ToByte(hexColor.Substring(4, 2), 16);
            byte b = Convert.ToByte(hexColor.Substring(6, 2), 16);

            return Color.FromArgb(a, r, g, b);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error parsing color '{hexColor}': {ex.Message}");
            // Return default color (semi-transparent navy blue)
            return Color.FromArgb(128, 0, 0, 128);
        }
    }

    /// <summary>
    /// Converts a Color object to hex string (ARGB)
    /// </summary>
    /// <param name="color">Color object</param>
    /// <returns>Hex color string (e.g., "#80FF0000")</returns>
    public static string ToHex(Color color)
    {
        return $"#{color.A:X2}{color.R:X2}{color.G:X2}{color.B:X2}";
    }

    /// <summary>
    /// Creates a Color with specified opacity
    /// </summary>
    /// <param name="color">Base color</param>
    /// <param name="opacity">Opacity (0.0 to 1.0)</param>
    /// <returns>Color with adjusted opacity</returns>
    public static Color WithOpacity(Color color, double opacity)
    {
        opacity = Math.Clamp(opacity, 0.0, 1.0);
        byte alpha = (byte)(opacity * 255);
        return Color.FromArgb(alpha, color.R, color.G, color.B);
    }

    /// <summary>
    /// Gets a contrasting color (black or white) for text display
    /// </summary>
    /// <param name="backgroundColor">Background color</param>
    /// <returns>Black or white color for best contrast</returns>
    public static Color GetContrastingColor(Color backgroundColor)
    {
        // Calculate luminance
        double luminance = (0.299 * backgroundColor.R + 0.587 * backgroundColor.G + 0.114 * backgroundColor.B) / 255;

        // Return white for dark backgrounds, black for light backgrounds
        return luminance > 0.5 ? Colors.Black : Colors.White;
    }
}
