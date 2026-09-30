## stage 1 : build 
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /source
COPY OrderServiceApi/*.csproj ./OrderServiceApi/
RUN dotnet restore ./OrderServiceApi/OrderServiceApi.csproj
COPY OrderServiceApi/. ./OrderServiceApi/
WORKDIR /source/OrderServiceApi
RUN dotnet publish -c release -o /app --no-restore
## final stage : runtime
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "OrderServiceApi.dll"]