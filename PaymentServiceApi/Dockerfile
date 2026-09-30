## stage 1 : build
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /source
COPY PaymentServiceApi/*.csproj ./PaymentServiceApi/
RUN dotnet restore ./PaymentServiceApi/PaymentServiceApi.csproj
COPY PaymentServiceApi/. ./PaymentServiceApi/
WORKDIR /source/PaymentServiceApi
RUN dotnet publish -c release -o /app --no-restore
## final stage : runtime
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "PaymentServiceApi.dll"]
