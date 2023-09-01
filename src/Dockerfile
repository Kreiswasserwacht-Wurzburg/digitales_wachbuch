FROM mcr.microsoft.com/dotnet/sdk:7.0 as build-env

WORKDIR /app
COPY . ./
RUN dotnet restore digital-guard-book.sln
RUN dotnet publish -c Release -o out digital-guard-book.sln


FROM mcr.microsoft.com/dotnet/aspnet:7.0

WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT [ "dotnet", "DigitalGuardBook.dll" ]