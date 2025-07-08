#!/bin/bash

echo "🚀 Starting HR Management API..."

# Check if .NET is installed
if ! command -v dotnet &> /dev/null; then
    echo "❌ .NET SDK is not installed. Please install .NET 8.0 SDK first."
    exit 1
fi

# Check .NET version
DOTNET_VERSION=$(dotnet --version)
echo "✅ .NET version: $DOTNET_VERSION"

# Restore packages
echo "📦 Restoring packages..."
dotnet restore

# Build the solution
echo "🔨 Building solution..."
dotnet build HRManagement.sln

if [ $? -eq 0 ]; then
    echo "✅ Build successful!"
    
    # Run the application
    echo "🌐 Starting the API..."
    echo "📖 Swagger UI will be available at: https://localhost:7001"
    echo "🔗 API endpoints will be available at: https://localhost:7001/api/v1"
    echo ""
    echo "Press Ctrl+C to stop the application"
    echo ""
    
    dotnet run --project HRManagement.API.csproj
else
    echo "❌ Build failed. Please check the errors above."
    exit 1
fi 