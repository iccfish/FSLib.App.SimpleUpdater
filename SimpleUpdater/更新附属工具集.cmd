@echo off

cd /d "%~dp0\"
mkdir FSLib.App.SimpleUpdater\Utilities
copy FSLib.App.Utilities\bin\Release\net20\FSLib.App.Utilities.exe FSLib.App.SimpleUpdater\Utilities\Utilities_Net20.exe
copy FSLib.App.Utilities\bin\Release\net40\FSLib.App.Utilities.exe FSLib.App.SimpleUpdater\Utilities\Utilities_Net40.exe
copy FSLib.App.Utilities\bin\Release\net5.0-windows\FSLib.App.Utilities.exe FSLib.App.SimpleUpdater\Utilities\FSLib.App.Utilities.exe
copy FSLib.App.Utilities\bin\Release\net5.0-windows\FSLib.App.Utilities.dll FSLib.App.SimpleUpdater\Utilities\FSLib.App.Utilities.dll
copy FSLib.App.Utilities\bin\Release\net5.0-windows\FSLib.App.Utilities.runtimeconfig.json FSLib.App.SimpleUpdater\Utilities\FSLib.App.Utilities.runtimeconfig.json
copy _Tools\app.config FSLib.App.SimpleUpdater\Utilities\app.config

_Tools\FSLib.App.Utilities.exe zipfile FSLib.App.SimpleUpdater\Utilities\Utilities_Net20.exe
_Tools\FSLib.App.Utilities.exe zipfile FSLib.App.SimpleUpdater\Utilities\Utilities_Net40.exe
_Tools\FSLib.App.Utilities.exe zipfile FSLib.App.SimpleUpdater\Utilities\FSLib.App.Utilities.exe
_Tools\FSLib.App.Utilities.exe zipfile FSLib.App.SimpleUpdater\Utilities\FSLib.App.Utilities.dll
_Tools\FSLib.App.Utilities.exe zipfile FSLib.App.SimpleUpdater\Utilities\FSLib.App.Utilities.runtimeconfig.json
_Tools\FSLib.App.Utilities.exe zipfile FSLib.App.SimpleUpdater\Utilities\app.config

del FSLib.App.SimpleUpdater\Utilities\*.exe
del FSLib.App.SimpleUpdater\Utilities\*.dll
del FSLib.App.SimpleUpdater\Utilities\*.json
del FSLib.App.SimpleUpdater\Utilities\*.config