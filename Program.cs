using Acme.Mqtt.Alive;

IHost host = Host.CreateDefaultBuilder(args)
    .UseWindowsService(options =>
    {
        options.ServiceName = "MQTT Alive";
    })
    .ConfigureServices(services =>
    {
        services.AddOptions<MqttOptions>().Configure<IConfiguration>((options, config) => config.GetSection("Mqtt").Bind(options));
        services.AddHostedService<WindowsBackgroundService>();
    })
    .Build();

await host.RunAsync();