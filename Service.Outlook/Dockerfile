#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Service.Outlook/Service.Outlook.csproj", "Service.Outlook/"]
COPY ["Global/Global.csproj", "Global/"]
RUN dotnet restore "Service.Outlook/Service.Outlook.csproj"
COPY . .
WORKDIR "/src/Service.Outlook"
RUN dotnet build "Service.Outlook.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Service.Outlook.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Service.Outlook.dll"]