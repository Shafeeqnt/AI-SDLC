@echo off
setlocal
echo Publishing GMS API...
dotnet publish Api\GMS.Api\GMS.Api.csproj -c Release -o Api\publish
if errorlevel 1 (
  echo Publish failed.
  exit /b 1
)
echo Publish completed successfully.
