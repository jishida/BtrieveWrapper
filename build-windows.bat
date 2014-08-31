@echo off

set msbuild=C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe

cd %~dp0

rd /q /s .\bin\release

%msbuild% BtrieveWrapper-windows-net_2_0.sln /t:Rebuild /p:Configuration=Release;Platform="Any CPU"
%msbuild% BtrieveWrapper-windows-net_2_0.sln /t:Rebuild /p:Configuration=Release;Platform=x64
%msbuild% BtrieveWrapper-windows-net_2_0.sln /t:Rebuild /p:Configuration=Release;Platform=x86
%msbuild% BtrieveWrapper-windows-net_3_5.sln /t:Rebuild /p:Configuration=Release;Platform="Any CPU"
%msbuild% BtrieveWrapper-windows-net_3_5.sln /t:Rebuild /p:Configuration=Release;Platform=x64
%msbuild% BtrieveWrapper-windows-net_3_5.sln /t:Rebuild /p:Configuration=Release;Platform=x86
%msbuild% BtrieveWrapper-windows-net_4_0.sln /t:Rebuild /p:Configuration=Release;Platform="Any CPU"
%msbuild% BtrieveWrapper-windows-net_4_0.sln /t:Rebuild /p:Configuration=Release;Platform=x64
%msbuild% BtrieveWrapper-windows-net_4_0.sln /t:Rebuild /p:Configuration=Release;Platform=x86

pause