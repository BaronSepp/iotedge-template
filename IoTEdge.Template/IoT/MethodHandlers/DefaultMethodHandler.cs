using Microsoft.Azure.Devices.Client;
using Microsoft.Extensions.Logging;
using Prometheus;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace IoTEdge.Template.IoT.MethodHandlers;

/// <summary>
/// The default delegate that applies for all method endpoints.
/// If a delegate is already associated with the method, it
/// will be called, else the default delegate will be called.
/// </summary>
public sealed class DefaultMethodHandler : IMethodHandler
{
	private readonly Counter _unhandledMethodCounter;
	private readonly ILogger<DefaultMethodHandler> _logger;

	/// <summary>
	/// Public <see cref="DefaultMethodHandler"/> constructor, parameters resolved through <b>Dependency injection</b>.
	/// </summary>
	/// <param name="logger"><see cref="ILogger"/> resolved through <b>Dependency injection</b>.</param>
	/// <exception cref="ArgumentNullException">Thrown when any of the parameters could not be resolved.</exception>
	public DefaultMethodHandler(ILogger<DefaultMethodHandler> logger)
	{
		_unhandledMethodCounter = Metrics.CreateCounter("unhandled_method_counter", "Amount of unhandled methods received");
		_logger = logger ?? throw new ArgumentNullException(nameof(logger));
	}

	/// <inheritdoc cref="IMethodHandler.MethodName"/>
	public string MethodName => "*";


	/// <inheritdoc cref="IMethodHandler.Handle"/>
	public Task<MethodResponse> Handle(MethodRequest method, object userContext)
	{
		_unhandledMethodCounter.Inc();
		_logger.LogInformation("Unhandled method '{Method}' received with data '{Data}'.", method.Name, method.DataAsJson);

		var response = new MethodResponse(JsonSerializer.SerializeToUtf8Bytes("short and stout"), 418);
		return Task.FromResult(response);
	}
}
