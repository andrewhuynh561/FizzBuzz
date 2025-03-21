FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY ["FizzBuzz/FizzBuzz.csproj", "FizzBuzz/"]
COPY ["FizzBuzz.Tests/FizzBuzz.Tests.csproj", "FizzBuzz.Tests/"]
RUN dotnet restore "FizzBuzz/FizzBuzz.csproj"

# Copy everything else and build
COPY . .
RUN dotnet build "FizzBuzz/FizzBuzz.csproj" -c Release -o /app/build

# Publish
FROM build AS publish
RUN dotnet publish "FizzBuzz/FizzBuzz.csproj" -c Release -o /app/publish

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
EXPOSE 80
EXPOSE 443
ENTRYPOINT ["dotnet", "FizzBuzz.dll"] 