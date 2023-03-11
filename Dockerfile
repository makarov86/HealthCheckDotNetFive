# Собираем приложение в отдельном образе
FROM ubuntu:22.04 AS dotnet_build_image
WORKDIR /src
RUN apt update && \
    apt install -y ca-certificates && \
    update-ca-certificates && \
    apt install -y dotnet-sdk-6.0

# Копируем из сходников пока только файлы sln и csproj, сохраняя структуру каталогов,
# это позволит на данном слое закешировать выкачивание всех нужных для приложения пакетов (dotnet restore)
COPY HealthCheckDotNetFive.sln .
COPY HealthCheckDotNetFive/HealthCheckDotNetFive.csproj ./HealthCheckDotNetFive/
RUN dotnet restore -r linux-musl-x64

# Теперь уже копируем все исходники
COPY . .
# Собираем бинарник, который будет выполняться без какого-либо рантайма dotnet
RUN dotnet publish -r linux-musl-x64 --self-contained true -p:PublishTrimmed=true --no-restore -c release --verbosity d -o /app

# Собираем итоговый рабочий образ
FROM amd64/alpine:3.16
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:8000 DOTNET_RUNNING_IN_CONTAINER=true DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=1
# Данные либы требуются для работы бинарных .Net приложений без рантайма
RUN apk add --no-cache \
        ca-certificates \
        krb5-libs \
        libgcc \
        libintl \
        libssl1.1 \
        libstdc++ \
        zlib
EXPOSE 8000
COPY --from=dotnet_build_image /app .
ENTRYPOINT ["/app/HealthCheckDotNetFive"]

