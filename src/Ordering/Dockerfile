FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

WORKDIR /app

COPY ./src/Ordering ./Ordering

RUN dotnet restore ./Ordering/Ordering.Api/Ordering.Api.csproj
RUN dotnet publish ./Ordering/Ordering.Api -c Release -o Ordering.Api --runtime alpine-x64 --self-contained true


FROM mcr.microsoft.com/dotnet/aspnet:7.0-alpine AS runtime
WORKDIR /app
COPY --from=build /app/Ordering.Api ./Ordering.Api
ENTRYPOINT ["dotnet", "Ordering.Api/Ordering.Api.dll"]