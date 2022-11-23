# IoTEdge Template

A modern template for Azure IoT Edge featuring .NET7, Hosting, DI, Logging Abstractions and Metrics.

## Installation

```shell
git clone https://github.com/BaronSepp/iotedge-template
cd iotedge-template
dotnet new --install .
```

## How to use

```shell
dotnet new iotedge -n <Name> -o <OutputPath>
```

> Currently it is required to manually change the module name in module.json

## Uninstall

```shell
cd iotedge-template
dotnet new --uninstall .
```
