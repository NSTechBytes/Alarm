

# Alarm - Rainmeter Plugin

**Alarm** is a **Rainmeter** plugin that allows you to set a weekly alarm at a specific time and trigger an action when the alarm goes off. You can configure the alarm to repeat on certain days of the week and execute a Rainmeter command (bang) when it is triggered.

## Features

- Set an alarm time with a specific hour and minute.
- Choose which days of the week the alarm should go off (Sunday to Saturday).
- Alarm can be enabled or disabled via Rainmeter variables.
- Trigger a custom Rainmeter command (bang) when the alarm is triggered.
- Returns `1` when the alarm is triggered, and `0` otherwise.

## Installation

1. **Download** the latest release of **Alarm** from the [Releases page](https://github.com/NSTechBytes/Alarm/releases).
2. **Install** the plugin by copying `Alarm.dll` to your Rainmeter `Plugins` directory:
   - Default path:  
     `C:\Users\<YourUsername>\Documents\Rainmeter\Plugins\`

3. Once installed, you can use it in your Rainmeter skins.

## Usage

### 1. Create a Measure in your Rainmeter skin

In your `.ini` skin file, define a measure using the **Alarm** plugin:

```ini
[Rainmeter]
Update=1000
BackGroundMode=2
SolidColor=FFFFFF

[Variables]
Hour=11
Minute=48
Monday=1
Tuesday=1
Wednesday=1
Thursday=1
Friday=1
Saturday=1
Sunday=1
Alarm=1


[mSkinAlarm]
Measure=Plugin
Plugin=Alarm.dll
Hour=#Hour#
Minute=#Minute#
Monday=#Monday#
Tuesday=#Tuesday#
Wednesday=#Wednesday#
Thursday=#Thursday#
Friday=#Friday#
Saturday=#Saturday#
Sunday=#Sunday#
Alarm=#Alarm#
OnTriggerAlarm=[!EnableMeasure Alarm_Trigger][!ShowMeter MeterButton]

[Alarm_Trigger]
Measure=Calc
Formula=1
IfCondition=Alarm_Trigger = 1
iftrueaction=[Play Alarm1.wav]
ifconditionmode=1
UpdateDivider=5
Disabled=1


[TextDisplay]
Meter=STRING
MeasureName=mSkinAlarm
X=10
Y=10
FontColor=000000
Text="Alarm Status: %1" #Crlf#Alarm Will Ring #Hour#:#Minute#
FontSize=12
Antialias=1

[MeterButton]
Meter=String
Text=!Stop
W=100
H=40
X=50
Y=100
FontColor=FFFFFF
SolidColor=10,10,10
stringAlign = CenterCenter
Hidden=1
LeftMouseUpAction=[PlayStop][!DisableMeasure Alarm_Trigger][!HideMeter #CURRENTSECTION#][!UpdateMeter *][!Redraw]
```

In this example:
- `%1` will display `1` if the alarm is triggered, or `0` if it is not.
- The alarm is set for **7:30 AM**, and it will go off on **Sunday, Monday, Thursday, and Friday**.
- The `OnTriggerAlarm` variable defines the **bang** that will be executed when the alarm is triggered (e.g., `RunCommand`).


## Parameters

- **Hour**: Set the hour (0-23) for the alarm time.
- **Minute**: Set the minute (0-59) for the alarm time.
- **Sunday to Saturday**: Set `1` for the alarm to go off on that day, or `0` to disable it.
- **Alarm**: Set `1` to enable the alarm, or `0` to disable it.
- **OnTriggerAlarm**: Set the Rainmeter bang command to execute when the alarm is triggered.

## Troubleshooting

- **Alarm Not Triggering**: Ensure the correct values are set for the hour, minute, and days. Make sure the `Alarm` variable is set to `1`.
- **Wrong Day or Time**: Verify the system time matches the set alarm time and day of the week.

## Contributing

If you'd like to contribute to this project, please feel free to fork the repository and submit a pull request. Ensure that your code follows the existing style and includes tests where applicable.

### Bug Reports & Feature Requests

To report bugs or request new features, please visit the [Issues tab](https://github.com/NSTechBytes/Alarm/issues).

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
