FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build

run echo $PORT

WORKDIR /server
COPY . .
RUN dotnet restore "./src/Wordki.Api/Wordki.Api.csproj"
RUN dotnet build "./src/Wordki.Api/Wordki.Api.csproj" -c Release -o /app

##########################################################
# Build tests project
##########################################################


FROM build AS integration-tests
WORKDIR /server/tests/Wordki.Tests.E2E
COPY ./build/common/wait /wait
RUN chmod +x /wait

RUN dotnet build "Wordki.Tests.E2E.csproj"


##########################################################
# Publish application
##########################################################

FROM build AS pre-release

RUN dotnet publish "./src/Wordki.Api/Wordki.Api.csproj" -c Release -o /app

##########################################################
# Run app
##########################################################

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS local

WORKDIR /app

COPY --from=pre-release /app .
ENTRYPOINT dotnet Wordki.Api.dll


##########################################################
# Run app
##########################################################

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS release

WORKDIR /app

COPY --from=pre-release /app .
ENV ASPNETCORE_ENVIRONMENT=Production
CMD ASPNETCORE_URLS=http://*:$PORT dotnet Wordki.Api.dll