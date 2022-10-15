:: generate.bat
:: generating updater binary for windows.
::
:: Copyright (C) 2022 FurtherSystem Co.,Ltd. All rights reserved.
:: info@furthersystem.com
::
echo off

set CSHARP_COMPLIER_PATH="%WINDIR%\Microsoft.NET\Framework\v4.0.30319"
set GENERATE_ASSEMBLY=generate.exe
set SOURCE=src
set BINARY=bin
set PATH=%CSHARP_COMPLIER_PATH%;%PATH%
set DEBUG=

%DEBUG% del /s/q %GENERATE_ASSEMBLY%

%DEBUG% csc /out:%GENERATE_ASSEMBLY% %SOURCE%\updx.cs %SOURCE%\MiniJSON.cs 
%DEBUG% %GENERATE_ASSEMBLY% updx.json %BINARY% res

%DEBUG% del /s/q %GENERATE_ASSEMBLY%

pause