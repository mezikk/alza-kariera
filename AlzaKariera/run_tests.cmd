@echo off
setlocal
set PropertiesFile=Config/properties.yaml
set ProjectFile=AlzaKariera\AlzaKariera.csproj
rem set ProjectFile=%1
set TestApplication=kariera
rem set TestApplication=%2
set TestIsRemote=false
rem set TestIsRemote=%3
set TestBrowser=chrome
rem set TestBrowser=%4
rem nuget restore AlzaKariera.csproj
rem msbuild AlzaKariera.csproj -t:Clean,Compile,Build /p:OutputPath=c:\Temp\AlzaKariera
rem nunit3-console c:\Temp\AlzaKariera\AlzaKariera.dll
dotnet clean %ProjectFile%
dotnet build %ProjectFile%
dotnet test %ProjectFile%