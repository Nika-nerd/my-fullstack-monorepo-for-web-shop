
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src


COPY ["SafiShopAPI.csproj", "./"]
RUN dotnet restore "./SafiShopAPI.csproj"


COPY . .
RUN dotnet build "SafiShopAPI.csproj" -c Release -o /app/build


FROM build AS publish
RUN dotnet publish "SafiShopAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false


FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .


RUN mkdir -p wwwroot/uploads/products

ENTRYPOINT ["dotnet", "SafiShopAPI.dll"]