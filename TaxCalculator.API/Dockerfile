# Use the official .NET SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and project files
COPY TaxCalculator.API.sln ./
COPY TaxCalculator.API/TaxCalculator.API.csproj ./TaxCalculator.API/

# Restore dependencies
RUN dotnet restore

# Copy the rest of the source code
COPY TaxCalculator.API/. ./TaxCalculator.API/

# Build the project
WORKDIR /src/TaxCalculator.API
RUN dotnet publish -c Release -o /app/publish

# Use the ASP.NET runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Expose port 8080 (optional: align with your dev environment)
EXPOSE 8080

# Run the application
ENTRYPOINT ["dotnet", "TaxCalculator.API.dll"]
