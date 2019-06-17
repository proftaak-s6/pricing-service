FROM mcr.microsoft.com/dotnet/core/sdk:2.2
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY Rekeningrijden.PricingService/*.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY Rekeningrijden.PricingService/ ./
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:2.2
WORKDIR /app


RUN echo "Europe/Amsterdam" > /etc/timezone

COPY --from=0 /app/out .
ENTRYPOINT ["dotnet", "Rekeningrijden.PricingService.dll"]