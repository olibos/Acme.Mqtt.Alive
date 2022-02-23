namespace Acme.Mqtt.Alive;
#nullable disable warnings
public record MqttOptions(string Host, string UserName, string Password, string Topic, string? ActionTopic)
{
    public MqttOptions()
        : this(default, default, default, default, default)
    { 
    }
}
#nullable restore warnings