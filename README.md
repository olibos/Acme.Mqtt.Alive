# Acme.Mqtt.Alive
## Why ?
To simplify device tracking in Home Automation and, of course, for **fun**!

## How ?
This tool will be installed as a Windows Service and simply set a MQTT topic to online when connected to home network and to offline when the device is shut down but also when it's not available: network issues, sleep, hibernate, ...

## Setup
Edit the appsettings.json and complete the MQTT section.
```json
{
  "Mqtt": {
    "Host": "MQTT_HOST",
    "UserName": "MQTT_USERNAME",
    "Password": "MQTT_PASSWORD",
    "Topic": "MQTT_TOPIC",
    "ActionTopic": "OPTIONAL_MQTT_ACTION_TOPIC"
  }
}
```
And publish the project:
```ps
dotnet publish --configuration RELEASE --self-contained -r win-x64 -p:PublishTrimmed=true
```

## Windows Installation

Run a shell with admin rights:
```ps
sc create "MQTT Alive" start=auto binpath="[path_to_exe_file😉]"
sc start "MQTT Alive"
```

## Home Assistant integration
```yaml
device_tracker:
  - platform: mqtt
    devices:
      my_pc: "monitor/mqtt/olivier"
    qos: 1
    payload_home: "online"
    payload_not_home: "offline"
    source_type: router
```

## Bonus
If you set ActionTopic variable in appsettings.json 
You can remotely bring the targeted machine to sleep / hibernate.

## Enjoy 🤖❗
Be creative with your automation...