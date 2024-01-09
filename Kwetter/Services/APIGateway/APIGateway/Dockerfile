# Use the official .NET 6 SDK as the base image
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

# Set the working directory inside the container
WORKDIR ./Kwetter

# Copy the project files to the working directory
COPY Kwetter/Services/APIGateway/APIGateway ./

# Build the solution including the "Common" class library
RUN dotnet restore "./Kwetter/Services/APIGateway/APIGateway/APIGateway.csproj"
RUN dotnet publish -c Release -o out "./Kwetter/Services/APIGateway/APIGateway/APIGateway.csproj"

# Use a lightweight runtime image as the base image
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime

# Set the working directory inside the container
WORKDIR ./Kwetter

# Copy the built application from the build stage
COPY --from=build ./Kwetter/out ./

# Expose port 8100 for Swagger UI
EXPOSE 8100

# Set the entry point for the container
ENTRYPOINT ["dotnet", "APIGateway.dll"]