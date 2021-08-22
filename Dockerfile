FROM mcr.microsoft.com/dotnet/runtime:3.1-focal AS base
WORKDIR /app

# Creates a non-root user with an explicit UID and adds permission to access the /app folder
# For more info, please refer to https://aka.ms/vscode-docker-dotnet-configure-containers
RUN adduser -u 5678 --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser

FROM mcr.microsoft.com/dotnet/sdk:3.1-focal AS build
WORKDIR /src
COPY ["scheduled-ecs-dotnet.csproj", "./"]
RUN dotnet tool install --global dotnet-ef --version 3.1.0
ENV PATH="/root/.dotnet/tools:${PATH}"
RUN dotnet restore "scheduled-ecs-dotnet.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "scheduled-ecs-dotnet.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "scheduled-ecs-dotnet.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "scheduled-ecs-dotnet.dll"]
