FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

WORKDIR /app

COPY ./src/Ordering/Ordering.Contract ./Ordering/Ordering.Contract
COPY ./src/Gateway ./Gateway

RUN dotnet restore ./Gateway/Gateway.Api/Gateway.Api.csproj
RUN dotnet publish ./Gateway/Gateway.Api -c Release -o Gateway.Api --runtime alpine-x64 --self-contained true


FROM mcr.microsoft.com/dotnet/aspnet:7.0-alpine AS runtime
WORKDIR /app
COPY --from=build /app/Gateway.Api ./Gateway.Api
ENTRYPOINT ["dotnet", "Gateway.Api/Gateway.Api.dll"]