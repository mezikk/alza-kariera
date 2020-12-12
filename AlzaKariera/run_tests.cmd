@echo off
setlocal
set PropertiesFile=Config/properties.yaml
set ProjectFile=%1
rem nuget restore AlzaKariera.csproj
rem msbuild AlzaKariera.csproj -t:Clean,Compile,Build /p:OutputPath=c:\Temp\AlzaKariera
rem nunit3-console c:\Temp\AlzaKariera\AlzaKariera.dll
dotnet clean %ProjectFile%
rem dotnet build %ProjectFile%
dotnet test %ProjectFile%