FROM node:alpine as frontend-builder
WORKDIR '/webapp'
COPY src/webapp/package.json .
RUN npm install
COPY src/webapp .
RUN npm run build

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 as api-builder
WORKDIR '/api'
COPY src/api .
RUN dotnet publish -c Release -o publish

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR '/app'
COPY --from=api-builder /api/publish .
COPY --from=frontend-builder /webapp/build ./webapp

EXPOSE 8080

ENTRYPOINT ["dotnet", "EasyDashboard.Host.dll"]