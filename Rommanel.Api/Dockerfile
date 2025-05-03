FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Rommanel.Api/Rommanel.Api.csproj", "Rommanel.Api/"]
COPY ["Rommanel.Domain/Rommanel.Domain.csproj", "Rommanel.Domain/"]
COPY ["Rommanel.Infra/Rommanel.Infra.csproj", "Rommanel.Infra/"]
COPY ["Rommanel.Interface/Rommanel.Interface.csproj", "Rommanel.Interface/"]

RUN dotnet restore "Rommanel.Api/Rommanel.Api.csproj"
COPY . .
WORKDIR "/src/Rommanel.Api"
RUN dotnet build "Rommanel.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Rommanel.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Rommanel.Api.dll"]