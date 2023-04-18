# IoTEdge Template

A modern template for Azure IoT Edge featuring .NET7, Hosting, DI, Logging Abstractions and Metrics.

## Installation

```shell
git clone https://github.com/BaronSepp/iotedge-template
cd iotedge-template
dotnet new install . --force
```

## How to use

```shell
dotnet new iotedge -n <Name> -o <OutputPath>
```

> Currently it is required to manually change the module name in module.json

## Uninstall

```shell
cd iotedge-template
dotnet new uninstall .
```

## Features

### TwinHandler
Via the template's TwinHandler, developers are able to access the Desired Properties of the Module Twin.
The TwinHandler takes care of Desired Property updates and makes them accessible via the GetProperty method.

#### Access a Desired Property

Below snippet can be used to get a Desired Property named 'sqlConnectionString' of type string.
```c#
var sqlConnectionString = _twinHandler.GetProperty<string>("sqlConnectionString");
```

For reference, below Module Twin is used. Note that 'sqlConnectionString' is a Desired Property of type string.
```JSON
{
	"desired": {
		"sqlConnectionString": "value here",
		"sqlPassword": "value here",
		"listOfThings": [
			"thing1",
			"thing2"
		]
	}
}
```

#### Event handler
An EventHandler is also availabe to subscribe on when the Desired Properties are updated during application runtime.
Simply subscribe on it and write logic that needs to be executed when the Properties are updated, like restarting a background job for example.

```c#
_twinHandler.TwinUpdated += async (s, e) => await OnTwinUpdated();
```


### Environment variables
Environment Variables are used to setup variables before application runtime. For example, the LogLevel can be configured.
By default, the LogLevel is set on 'Information' in the appsettings.json.

```
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  },

  "ModuleClient": {
    "UpstreamProtocol": "AmqpTcp"
  },

  "Metric": {
    "HostName": "+",
    "Port": 9600,
    "Url": "metrics/",
    "UseHttps": false
  }
}
```

Via the deployment.template.(debug).json, it can be changed to another level.
```JSON
        "modules": {
          "Inimco.Facts.Modules.Sqlv2": {
            "version": "1.0.0",
            "type": "docker",
            "status": "running",
            "restartPolicy": "always",
            "settings": {
              "image": "${MODULEDIR<../Inimco.Facts.Modules.Sqlv2>}",
              "createOptions": {}
            },
            "env": {
              "Logging:LogLevel:Default": {
                "value": "Debug"
              }
            }
          }
        }
```
