# 1. Build (Derleme) Aşaması
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app

# Önce sadece csproj dosyasını kopyalayıp bağımlılıkları indiriyoruz
COPY *.csproj ./
RUN dotnet restore

# Tüm dosyaları kopyalayıp projeyi yayına hazırlıyoruz
COPY . ./
RUN dotnet publish -c Release -o out

# 2. Çalıştırma Aşaması (Daha hafif bir imaj)
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .

# Render'ın varsayılan olarak dinlediği portu ayarlıyoruz
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

# Uygulamayı başlatıyoruz (dll adı csproj adınızla aynı olur)
ENTRYPOINT ["dotnet", "YarimKalanlar.dll"]