#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app/Order

EXPOSE 5001
ENV ASPNETCORE_URLS="http://0.0.0.0:5001"
ENV MYURL=http://192.168.1.5:5000
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["OrderServer/OrderServer.csproj", "OrderServer/"]
RUN dotnet restore "OrderServer/OrderServer.csproj"
COPY . .
WORKDIR "/src/OrderServer"
RUN dotnet build "OrderServer.csproj" -c Release -o /app/Order/build

FROM build AS publish
RUN dotnet publish "OrderServer.csproj" -c Release -o /app/Order/publish /p:UseAppHost=false

FROM base as final
WORKDIR /app/Order
COPY --from=publish /app/Order/publish .
COPY /OrderServer/Order.Db /app/Order/Order.Db
COPY /MasterDb/orders.csv /app/Order/MasterDb/orders.csv
RUN chown -R 1000:1000 /app/Order/Order.Db
RUN chmod -R 777 /app/Order/MasterDb

USER 1000
ENTRYPOINT ["dotnet", "OrderServer.dll"]