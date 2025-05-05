FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src
COPY ["src/VQM.Web.Host/VQM.Web.Host.csproj", "src/VQM.Web.Host/"]
COPY ["src/VQM.Web.Core/VQM.Web.Core.csproj", "src/VQM.Web.Core/"]
COPY ["src/VQM.Application/VQM.Application.csproj", "src/VQM.Application/"]
COPY ["src/VQM.Core/VQM.Core.csproj", "src/VQM.Core/"]
COPY ["src/VQM.EntityFrameworkCore/VQM.EntityFrameworkCore.csproj", "src/VQM.EntityFrameworkCore/"]
WORKDIR "/src/src/VQM.Web.Host"
RUN dotnet restore 

WORKDIR /src
COPY ["src/VQM.Web.Host", "src/VQM.Web.Host"]
COPY ["src/VQM.Web.Core", "src/VQM.Web.Core"]
COPY ["src/VQM.Application", "src/VQM.Application"]
COPY ["src/VQM.Core", "src/VQM.Core"]
COPY ["src/VQM.EntityFrameworkCore", "src/VQM.EntityFrameworkCore"]
WORKDIR "/src/src/VQM.Web.Host"
RUN dotnet publish -c Release -o /publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0
EXPOSE 80
WORKDIR /app
COPY --from=build /publish .
ENTRYPOINT ["dotnet", "VQM.Web.Host.dll"]
