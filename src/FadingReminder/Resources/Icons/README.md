# Application Icons

## Optional Custom Icon

### app.ico (Optional)
- **Size**: 256x256 pixels (multi-resolution .ico file recommended)
- **Format**: ICO file
- **Usage**: Application icon (system tray uses auto-generated icon)
- **Current Status**: Not included - application uses default Windows icon

## Creating a Custom Icon (Optional)

If you want a custom application icon, you can create one using:
- Online tools: https://www.favicon-generator.org/
- GIMP (with ICO plugin)
- Photoshop
- IconWorkshop

## How to Add a Custom Icon

1. Create or download an icon file named `app.ico`
2. Place it in this directory (`src/FadingReminder/Resources/Icons/`)
3. Uncomment this line in `FadingReminder.csproj`:
   ```xml
   <ApplicationIcon>Resources\Icons\app.ico</ApplicationIcon>
   ```
4. Rebuild the application

## Default Behavior

The application works perfectly fine without a custom icon:
- **Application executable**: Uses default Windows application icon
- **System tray**: Auto-generates a simple blue circle icon (see SystemTrayManager.cs)
