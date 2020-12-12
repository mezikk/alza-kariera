@echo off
setlocal
set PropertiesFile=Config/properties.yaml
#set ProjectFile=AlzaKariera\AlzaKariera.csproj
set ProjectFile=%1
rem set TestApplication=kariera
set TestApplication=%2
rem set TestIsRemote=true
set TestIsRemote=%3
rem set TestBrowser=chrome
set TestBrowser=%4
rem nuget restore AlzaKariera.csproj
rem msbuild AlzaKariera.csproj -t:Clean,Compile,Build /p:OutputPath=c:\Temp\AlzaKariera
rem nunit3-console c:\Temp\AlzaKariera\AlzaKariera.dll
dotnet clean %ProjectFile%
dotnet build %ProjectFile%
dotnet test %ProjectFile%