# Use the official .NET 8 SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy the csproj file and restore dependencies
COPY ["PharmacyBackend.csproj", "./"]
RUN dotnet restore "PharmacyBackend.csproj"

# Copy the rest of the code and build it
COPY . .
WORKDIR "/app/"

# IMPORTANT: Install EF Core tools and run migrations BEFORE publishing
RUN dotnet tool install --global dotnet-ef
ENV PATH="$PATH:/root/.dotnet/tools"

# Now build and publish the app
RUN dotnet publish "PharmacyBackend.csproj" -c Release -o /app/publish

# Use the official .NET 8 runtime image to run the app
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Tell the app to listen on the port provided by Render
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "PharmacyBackend.dll"]
