# syntax=docker/dockerfile:1

# Create a stage for building the application.
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build

COPY . /source

WORKDIR /source/Squib.UserService.Host

ARG TARGETARCH

RUN --mount=type=cache,id=nuget,target=/root/.nuget/packages \
    dotnet publish -a ${TARGETARCH/amd64/x64} --use-current-runtime --self-contained false -o /app

# Create a new stage for running the application that contains the minimal
# runtime dependencies for the application.
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS final
WORKDIR /app

# Install ICU (International Components for Unicode) package for globalization support
RUN apk add --no-cache icu-libs

# Set environment variable to enable globalization
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

COPY --from=build /app .

ENTRYPOINT ["dotnet", "Squib.UserService.Host.dll"]
