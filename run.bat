@echo off
setlocal
echo Running GMS API on port 5000...
dotnet run --project Api\GMS.Api\GMS.Api.csproj --launch-profile http
if errorlevel 1 (
  echo Run failed.
  exit /b 1
)
