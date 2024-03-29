# Use the official .NET 6 SDK as the base image
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

# Set the working directory inside the container
WORKDIR ./Kwetter

# Copy the project files to the working directory
COPY Kwetter/Services/CustomerService/API ./
COPY Kwetter/Services/Common/API ./

# Build the solution including the "Common" class library
RUN dotnet restore "./CustomerService.API.csproj"
RUN dotnet publish -c Release -o out "./CustomerService.API.csproj"

# Use a lightweight runtime image as the base image
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime

# Set the working directory inside the container
WORKDIR ./Kwetter

# Copy the built application from the build stage
COPY --from=build ./Kwetter/out ./

# Expose port 8101 for Swagger UI
EXPOSE 8101

# Set the entry point for the container
ENTRYPOINT ["dotnet", "CustomerService.API.dll"]