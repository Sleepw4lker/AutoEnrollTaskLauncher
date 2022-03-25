@echo off

:: Should ensure that we know the Source Path
set WORKPATH=%~dp0
set WORKPATH=%WORKPATH:~0,-1%

set APPPATH=%PROGRAMFILES%\AutoEnrollTaskLauncher

echo Deleting old installation (if any)...
rmdir "%APPPATH%" /S /Q

echo Creating app directory...
md "%APPPATH%"

echo Installing app...
xcopy "%WORKPATH%\AutoEnrollTaskLauncher.exe" "%APPPATH%" /C /Y

echo De-registering scheduled tasks (if any)...
schtasks /Delete /F /TN "AutoEnrollTaskLauncher - System Task"
schtasks /Delete /F /TN "AutoEnrollTaskLauncher - User Task"

echo Registering scheduled tasks...
schtasks /Create /XML "%WORKPATH%\SystemTask.xml" /TN "AutoEnrollTaskLauncher - System Task"
schtasks /Create /XML "%WORKPATH%\UserTask.xml" /TN "AutoEnrollTaskLauncher - User Task"