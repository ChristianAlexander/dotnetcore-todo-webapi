FROM microsoft/aspnetcore-build:2.0

COPY . /build
WORKDIR /build/src
RUN dotnet publish -c Release -o ../../out

FROM microsoft/aspnetcore:2.0
WORKDIR /api
COPY --from=0 /build/out ./

EXPOSE 80
CMD dotnet TodoWebApi.dll
