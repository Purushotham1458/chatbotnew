FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY src/Domain/Domain.csproj Domain/
COPY src/Application/Application.csproj Application/
COPY src/Infrastructure/Infrastructure.csproj Infrastructure/
COPY src/WebAPI/WebAPI.csproj WebAPI/
RUN dotnet restore WebAPI/WebAPI.csproj

COPY src/Domain/ Domain/
COPY src/Application/ Application/
COPY src/Infrastructure/ Infrastructure/
COPY src/WebAPI/ WebAPI/
RUN dotnet publish WebAPI/WebAPI.csproj -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "WebAPI.dll"]
