# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy csproj files and restore dependencies
COPY ["FoodVietAPI.Presentation/CleanFoodVietAPI.Presentation.csproj", "FoodVietAPI.Presentation/"]
COPY ["FoodVietAPI.Application/CleanFoodVietAPI.Application.csproj", "FoodVietAPI.Application/"]
COPY ["FoodVietAPI.Data/CleanFoodVietAPI.Data.csproj", "FoodVietAPI.Data/"]
RUN dotnet restore "FoodVietAPI.Presentation/CleanFoodVietAPI.Presentation.csproj"

# Copy the entire solution and publish
COPY . .
WORKDIR "/src/FoodVietAPI.Presentation"
RUN dotnet publish "CleanFoodVietAPI.Presentation.csproj" -c Release -o /app/publish

# Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80
ENTRYPOINT ["dotnet", "CleanFoodVietAPI.Presentation.dll"]
