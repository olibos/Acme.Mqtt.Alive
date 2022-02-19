namespace Acme.Mqtt.Alive;

using Microsoft.Extensions.Options;

using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Options;
using MQTTnet.Extensions.ManagedClient;

public sealed class WindowsBackgroundService : BackgroundService
{
    private readonly ILogger<WindowsBackgroundService> logger;
    private IOptions<MqttOptions> options;

    public WindowsBackgroundService(ILogger<WindowsBackgroundService> logger, IOptions<MqttOptions> options) => (this.logger, this.options) = (logger, options);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var mqtt = this.options.Value;
        var messageBuilder = new MqttApplicationMessageBuilder().WithTopic(mqtt.Topic).WithRetainFlag();
        var offlineMessage = messageBuilder.WithPayload("offline").Build();
        var onlineMesage = messageBuilder.WithPayload("online").Build();
        var options = new ManagedMqttClientOptionsBuilder()
            .WithAutoReconnectDelay(TimeSpan.FromSeconds(5))
            .WithClientOptions(new MqttClientOptionsBuilder()
                .WithClientId($"Acme.Mqtt.Alive-{Guid.NewGuid()}")
                .WithTcpServer(mqtt.Host)
                .WithCredentials(mqtt.UserName, mqtt.Password)
                .WithWillMessage(offlineMessage)
                .Build())
            .Build();
        using var mqttClient = new MqttFactory().CreateManagedMqttClient();
        mqttClient.ConnectedHandler = new MqttClientConnectedHandlerDelegate(e => mqttClient.PublishAsync(onlineMesage));
        await mqttClient.StartAsync(options);
        stoppingToken.WaitHandle.WaitOne();
        await mqttClient.PublishAsync(offlineMessage);
        await mqttClient.StopAsync();
    }
}