dotnet publish Action/MarkDigger.csproj -c Release -o Action/win-x64/ -r win-x64 -p:PublishSingleFile=true -p:PublishTrimmed=true --self-contained true /p:DebugType=None /p:IncludeNativeLibrariesForSelfExtract=true

if ($LASTEXITCODE -ne 0) {
  exit $LASTEXITCODE
}

dotnet publish Action/MarkDigger.csproj -c Release -o Action/linux-x64/ -r linux-x64 -p:PublishSingleFile=true -p:PublishTrimmed=true --self-contained true /p:DebugType=None /p:IncludeNativeLibrariesForSelfExtract=true

if ($LASTEXITCODE -ne 0) {
  exit $LASTEXITCODE
}

dotnet publish Action/MarkDigger.csproj -c Release -o Action/osx-x64/ -r osx-x64 -p:PublishSingleFile=true -p:PublishTrimmed=true --self-contained true /p:DebugType=None /p:IncludeNativeLibrariesForSelfExtract=true

if ($LASTEXITCODE -ne 0) {
  exit $LASTEXITCODE
}

# Copy-Item .\HighlighterExtension\PrismHighlighter\mcprism.js .\Action\linux-x64\
# Copy-Item .\HighlighterExtension\PrismHighlighter\mcprism.js .\Action\osx-x64\
# Copy-Item .\HighlighterExtension\PrismHighlighter\mcprism.js .\Action\win-x64\

# Copy-Item .\HighlighterExtension\PrismHighlighter\package.json .\Action\linux-x64\
# Copy-Item .\HighlighterExtension\PrismHighlighter\package.json .\Action\osx-x64\
# Copy-Item .\HighlighterExtension\PrismHighlighter\package.json .\Action\win-x64\