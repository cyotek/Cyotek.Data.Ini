@ECHO OFF

SETLOCAL

SET CONFIG=Release
SET BASENAME=Cyotek.Data.Ini
SET RELDIR=src\bin\%CONFIG%\
SET PRJFILE=src\%BASENAME%.csproj
SET DLLNAME=%BASENAME%.dll

SET DISTDIR=dist\

SET DEPDIR=%DISTDIR%demo\

IF EXIST %RELDIR%*.nupkg  DEL /F %RELDIR%*.nupkg
IF EXIST %RELDIR%*.snupkg DEL /F %RELDIR%*.snupkg
IF EXIST %RELDIR%*.zip    DEL /F %RELDIR%*.zip
IF EXIST %DEPDIR%         RMDIR /Q /S %DEPDIR%
IF EXIST %DISTDIR%        RMDIR /Q /S %DISTDIR%

MKDIR %DISTDIR%

CALL runtests.cmd
IF %ERRORLEVEL% NEQ 0 GOTO :failed

CALL :buildpackage
IF %ERRORLEVEL% NEQ 0 GOTO :failed

ENDLOCAL

GOTO :eof

:buildfailed
:failed
ECHO ERROR: Build failed.
EXIT /b 1


:buildpackage
dotnet build %PRJFILE% --configuration %CONFIG%
IF %ERRORLEVEL% NEQ 0 EXIT /b %ERRORLEVEL%

PUSHD %RELDIR%

CALL cyotek-dual-sign-file.cmd net35\%DLLNAME%
CALL cyotek-dual-sign-file.cmd net40\%DLLNAME%
CALL cyotek-sign-file.cmd      net45\%DLLNAME%
CALL cyotek-sign-file.cmd      net452\%DLLNAME%
CALL cyotek-sign-file.cmd      net462\%DLLNAME%
CALL cyotek-sign-file.cmd      net472\%DLLNAME%
CALL cyotek-sign-file.cmd      net48\%DLLNAME%
CALL cyotek-sign-file.cmd      net6.0\%DLLNAME%
CALL cyotek-sign-file.cmd      netcoreapp3.1\%DLLNAME%
CALL cyotek-sign-file.cmd      netstandard2.1\%DLLNAME%

7z.exe a %BASENAME%.2.x.x.zip -r

POPD

MOVE %RELDIR%*.zip %DISTDIR%

dotnet pack %PRJFILE% --configuration Release --no-build
IF %ERRORLEVEL% NEQ 0 EXIT /b %ERRORLEVEL%

CALL cyotek-sign-package.cmd %RELDIR%*.nupkg
CALL cyotek-sign-package.cmd %RELDIR%*.snupkg

EXIT /b %ERRORLEVEL%
