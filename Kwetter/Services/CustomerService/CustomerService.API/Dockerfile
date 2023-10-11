# Use the official .NET 6 SDK as the base image
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

# Set the working directory inside the container
WORKDIR /kwetter

# Build the solution including the "Common" class library
RUN dotnet restore "Services/CustomerService/CustomerService.API/CustomerService.API.csproj"
RUN dotnet publish -c Release -o out

# Use a lightweight runtime image as the base image
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime

# Set the working directory inside the container
WORKDIR /kwetter

# Copy the built application from the build stage
COPY --from=build /app/out ./

# Expose port 8102 for Swagger UI
EXPOSE 8102

# Set the entry point for the container
ENTRYPOINT ["dotnet", "CustomerService.API.dll"]