@echo off
REM Build script for creating portable FadingReminder application

echo ========================================
echo Building FadingReminder (Portable)
echo ========================================
echo.

REM Navigate to project directory
cd /d "%~dp0src\FadingReminder"

echo Cleaning previous builds...
dotnet clean --configuration Release
if errorlevel 1 goto error

echo.
echo Building self-contained executable...
dotnet publish --configuration Release --runtime win-x64 --self-contained true -p:PublishSingleFile=false -p:PublishReadyToRun=true -p:IncludeNativeLibrariesForSelfExtract=true
if errorlevel 1 goto error

echo.
echo ========================================
echo Build completed successfully!
echo ========================================
echo.
echo Output location:
echo %~dp0src\FadingReminder\bin\Release\net8.0-windows\win-x64\publish\
echo.
echo Copy the entire 'publish' folder to your desired location.
echo The application is now portable and ready to use.
echo.

goto end

:error
echo.
echo ========================================
echo Build FAILED! Check the error messages above.
echo ========================================
echo.
pause
exit /b 1

:end
pause
