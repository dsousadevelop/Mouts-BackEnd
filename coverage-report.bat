@ECHO OFF

REM Install tools if not present
dotnet tool install --global coverlet.console
dotnet tool install --global dotnet-reportgenerator-globaltool

REM Clean and build solution
dotnet restore Ambev.DeveloperEvaluation.sln
dotnet build Ambev.DeveloperEvaluation.sln --configuration Release --no-restore

REM Run tests with coverage
dotnet test Ambev.DeveloperEvaluation.sln --no-restore --verbosity normal ^
/p:CollectCoverage=true ^
/p:CoverletOutputFormat=cobertura ^
/p:CoverletOutput=./TestResults/coverage.cobertura.xml ^
/p:Exclude="[*]*.Program%%2c[*]*.Startup%%2c[*]*.Migrations.*%%2c[*]*.DTOs.*%%2c[*]*.Requests.*%%2c[*]*.Responses.*%%2c[*]*.Profiles.*%%2c[*]*.Mapping.*%%2c[*]*.IoC.*%%2c[*]*.Configuration.*%%2c[*]*.Constants.*%%2c[*]*.Exceptions.*"

REM Generate coverage report
reportgenerator ^
-reports:"./tests/**/TestResults/coverage.cobertura.xml" ^
-targetdir:"./TestResults/CoverageReport" ^
-reporttypes:Html

REM Removing temporary files
rmdir /s /q bin 2>nul
rmdir /s /q obj 2>nul

echo.
echo Coverage report generated at TestResults/CoverageReport/index.html
pause
