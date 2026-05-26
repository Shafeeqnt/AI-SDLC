@echo off
setlocal
echo Building GMS solution...
dotnet build Api\GMS.sln
if errorlevel 1 (
  echo Build failed.
  exit /b 1
)
echo Build completed successfully.
