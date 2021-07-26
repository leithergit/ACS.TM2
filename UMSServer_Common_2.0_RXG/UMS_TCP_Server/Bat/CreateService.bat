%~d0 
cd %~dp0
cd..
sc create UMSTCPSERVICE binPath= "%cd%\UmsService.exe" start=auto
sc start UMSTCPSERVICE
pause()