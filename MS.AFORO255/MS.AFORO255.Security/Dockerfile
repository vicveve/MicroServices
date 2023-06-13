ARG PROJECT_NAME=MS.AFORO255.Security

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
ARG PROJECT_NAME
ENV PROJECT=$PROJECT_NAME
WORKDIR /app
COPY . .
RUN dotnet publish ${PROJECT}.csproj -c release -o out

FROM mcr.microsoft.com/dotnet/aspnet:7.0
ARG PROJECT_NAME
ENV PROJECT=$PROJECT_NAME
WORKDIR /app
COPY --from=build-env /app/out .
ENV ASPNETCORE_URLS http://*:80
ENV ASPNETCORE_ENVIRONMENT docker
EXPOSE 80
ENTRYPOINT dotnet ${PROJECT}.dll