#!/bin/bash
# Build script for AceMapEditor.Core

set -e  # Exit on error

echo "========================================="
echo "  Building AceMapEditor.Core"
echo "========================================="

# Navigate to Core directory
cd "$(dirname "$0")/../Core"

# Clean previous builds
echo "Cleaning previous builds..."
dotnet clean

# Build Release
echo "Building Release configuration..."
dotnet build -c Release

# Check if build succeeded
if [ $? -eq 0 ]; then
    echo ""
    echo "========================================="
    echo "  Build Successful!"
    echo "========================================="
    echo "Output: bin/Release/netstandard2.1/AceMapEditor.Core.dll"
    echo ""

    # Copy DLL to Unity Package
    echo "Copying DLL to Unity Package..."
    mkdir -p ../Packages/com.acemapeditor.unity/Plugins
    cp bin/Release/netstandard2.1/AceMapEditor.Core.dll ../Packages/com.acemapeditor.unity/Plugins/
    cp bin/Release/netstandard2.1/AceMapEditor.Core.pdb ../Packages/com.acemapeditor.unity/Plugins/
    echo "✓ Copied to Packages/com.acemapeditor.unity/Plugins/"

    # Copy to Releases directory
    echo "Copying DLL to Releases directory..."
    mkdir -p ../Releases/netstandard2.1
    cp bin/Release/netstandard2.1/AceMapEditor.Core.dll ../Releases/netstandard2.1/
    cp bin/Release/netstandard2.1/AceMapEditor.Core.pdb ../Releases/netstandard2.1/
    echo "✓ Copied to Releases/netstandard2.1/"

    echo ""
    echo "========================================="
    echo "  All Done!"
    echo "========================================="
else
    echo ""
    echo "========================================="
    echo "  Build Failed!"
    echo "========================================="
    exit 1
fi
