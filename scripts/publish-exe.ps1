param(
    [string]$configuration = "Release",
    [string]$runtime = "win-x64",
    [string]$output = "publish"
)

$proj = "./WinFormsApp1.csproj"

# Publish a single-file self-contained exe
dotnet publish $proj -c $configuration -r $runtime /p:PublishSingleFile=true /p:PublishTrimmed=false /p:SelfContained=true -o $output

if ($LASTEXITCODE -ne 0) {
    Write-Error "Publish failed"
    exit $LASTEXITCODE
}

Write-Output "Published to $output"