@echo off

set version=%1

:: Build the nuget packages
dotnet ^
    pack ./BeardedPlatypus.SourceGenerators.Annotations/BeardedPlatypus.SourceGenerators.Annotations.csproj ^
    -c Release ^
    --include-source ^
    --nologo

dotnet ^
    pack ./BeardedPlatypus.SourceGenerators/BeardedPlatypus.SourceGenerators.csproj ^
    -c Release ^
    --include-source ^
    --nologo

:: Push the nuget packages
dotnet ^
    nuget push ./BeardedPlatypus.SourceGenerators.Annotations/bin/Release/BeardedPlatypus.SourceGenerators.Annotations.%version%.nupkg ^
    --api-key %GITHUB_TOKEN% ^
    --source "github"

dotnet ^
    nuget push ./BeardedPlatypus.SourceGenerators.Annotations/bin/Release/BeardedPlatypus.SourceGenerators.Annotations.%version%.symbols.nupkg ^
    --api-key %GITHUB_TOKEN% ^
    --source "github"

dotnet ^
    nuget push ./BeardedPlatypus.SourceGenerators/bin/Release/BeardedPlatypus.SourceGenerators.%version%.nupkg ^
    --api-key %GITHUB_TOKEN% ^
    --source "github"

dotnet ^
    nuget push ./BeardedPlatypus.SourceGenerators/bin/Release/BeardedPlatypus.SourceGenerators.%version%.symbols.nupkg ^
    --api-key %GITHUB_TOKEN% ^
    --source "github"