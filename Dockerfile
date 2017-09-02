FROM microsoft/aspnetcore-build:2.0

COPY . /build
WORKDIR /build
RUN dotnet publish -c Release -o ../out
ENV PORT=80

FROM microsoft/aspnetcore:2.0
WORKDIR /api
COPY --from=0 /build/src/out ./

EXPOSE 80
CMD ASPNETCORE_URLS="http://*:$PORT" dotnet TodoWebApi.dll
