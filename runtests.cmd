@ECHO OFF

SETLOCAL

SET SCRIPTDIR=%~dp0
SET SCRIPTDIR=%SCRIPTDIR:~0,-1%

SET CONFIG=Release
SET BASENAME=Cyotek.Data.Ini
SET RELDIR=tests\bin\%CONFIG%\
SET PRJFILE=tests\%BASENAME%.Tests.csproj
SET DLLNAME=%BASENAME%.Tests.dll
SET RESULTS=%SCRIPTDIR%\testresults

IF EXIST "%RESULTS%" RMDIR "%RESULTS%" /Q /S
TIMEOUT /T 2 /NOBREAK > NUL
MKDIR "%RESULTS%"

dotnet build %PRJFILE% --configuration %CONFIG%
IF %ERRORLEVEL% NEQ 0 GOTO :failed

dotnet test --no-restore --no-build --logger trx --logger html /p:Configuration=%CONFIG% --nologo --results-directory "%RESULTS%" /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura /p:CoverletOutput="%RESULTS%/"
IF %ERRORLEVEL% NEQ 0 GOTO :failed

ENDLOCAL

GOTO :eof

:failed
ECHO {0c}ERROR  : Test run failed{#}{\n}
EXIT /b 1

