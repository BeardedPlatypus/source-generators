# $1: The version to be published

# Pack the NuGet packages.
dotnet \
    pack ./BeardedPlatypus.SourceGenerators.Annotations/BeardedPlatypus.SourceGenerators.Annotations.csproj \
    -c Release \
    --include-source \
    --nologo

dotnet \
    pack ./BeardedPlatypus.SourceGenerators/BeardedPlatypus.SourceGenerators.csproj \
    -c Release \
    --include-source \
    --nologo

# Publish the NuGet packages to github.
dotnet \
    nuget push ./BeardedPlatypus.SourceGenerators.Annotations/bin/Release/BeardedPlatypus.SourceGenerators.Annotations.{$1}.nupkg \
    --api-key $GITHUB_TOKEN \
    --source "github"

dotnet \
    nuget push ./BeardedPlatypus.SourceGenerators.Annotations/bin/Release/BeardedPlatypus.SourceGenerators.Annotations.{$1}.symbols.nupkg \
    --api-key $GITHUB_TOKEN \
    --source "github"

dotnet \
    nuget push ./BeardedPlatypus.SourceGenerators/bin/Release/BeardedPlatypus.SourceGenerators.{$1}.nupkg \
    --api-key $GITHUB_TOKEN \
    --source "github"

dotnet \
    nuget push ./BeardedPlatypus.SourceGenerators/bin/Release/BeardedPlatypus.SourceGenerators.{$1}.symbols.nupkg \
    --api-key $GITHUB_TOKEN \
    --source "github"