@echo off
cls
setlocal ENABLEDELAYEDEXPANSION
set prompt=$G
set _count=0

if "%VSWHERE%"=="" set "VSWHERE=%ProgramFiles(x86)%\Microsoft Visual Studio\Installer\vswhere.exe"

if "%ADB%"=="" set "ADB=%ProgramFiles(x86)%\Android\android-sdk\platform-tools\adb.exe"

for /f "usebackq delims=" %%i in (`"%VSWHERE%" -prerelease -latest -property installationPath`) do (
  if exist "%%i\Common7\Tools\vsdevcmd.bat" (
    call "%%i\Common7\Tools\vsdevcmd.bat"
  )
)

for /f "tokens=1,*" %%G in ('"%ADB%" devices -l') do @call :AddAndDisplayDevice %%G "%%H"
if "%_count%" == "2" (
	call :DoIt 1
	exit /b 0
)

set /p _choice=Enter number from above menu:
if not defined _%_choice% @goto :BadSelection "%_choice%"
call :DoIt %_choice%

exit /b 0

:AddAndDisplayDevice
set input=%2
set input=%input:"=%
if "%_count%" == "0" (
	echo %*
	set /a _count+=1
) ELSE (
	echo %_count%. %*
	for %%a in ("%input::=" "%") do set "transponderId=%%~a"
	call :SetVar %_count% !transponderId!
	call :SetVar %_count%_description %2
	set /a _count+=1
)
exit /b 0

:BadSelection
rem Up to you whether to loop back and try again.
exit /b -1

:DoIt
set _Id=!_%1!
rem set _
rmdir AndroidStorage /s /q
mkdir AndroidStorage
cd AndroidStorage
echo copy storage from transponderId=%_Id%
"%ADB%" -t %_Id% exec-out run-as de.hdbw.apollo.client tar c files/ > data.tar
exit /b 0

:SetVar
set _%1=%2
exit /b 0
endlocal