using System;
using System.Runtime.InteropServices;
using Rainmeter;

namespace PluginAlarm
{
    internal class Measure
    {
        private int hour;
        private int minute;
        private bool[] days = new bool[7];  // Days of the week: Sunday = 0, Monday = 1, ..., Saturday = 6
        private bool alarmEnabled;
        private bool alarmTriggered = false;
        private string onTriggerAlarm;
        private API api;

        internal Measure() { }

        internal void Reload(Rainmeter.API api, ref double maxValue)
        {
            this.api = api;  // Assign the API instance to the class-level variable

            // Read values from Rainmeter variables dynamically
            hour = api.ReadInt("Hour", -1);  // #Hour# variable
            minute = api.ReadInt("Minute", -1);  // #Minute# variable

            // Read the days dynamically (1 = enabled, 0 = disabled)
            days[0] = api.ReadInt("Sunday", 0) == 1;  // #Sunday#
            days[1] = api.ReadInt("Monday", 0) == 1;  // #Monday#
            days[2] = api.ReadInt("Tuesday", 0) == 1;  // #Tuesday#
            days[3] = api.ReadInt("Wednesday", 0) == 1;  // #Wednesday#
            days[4] = api.ReadInt("Thursday", 0) == 1;  // #Thursday#
            days[5] = api.ReadInt("Friday", 0) == 1;  // #Friday#
            days[6] = api.ReadInt("Saturday", 0) == 1;  // #Saturday#

            // Alarm status dynamically (1 = enabled, 0 = disabled)
            alarmEnabled = api.ReadInt("Alarm", 0) == 1;  // #Alarm#
            onTriggerAlarm = api.ReadString("OnTriggerAlarm", ""); // OnTriggerAlarm bang command
        }

        internal double Update()
        {
            if (!alarmEnabled || hour < 0 || minute < 0)
            {
                alarmTriggered = false;
                return 0.0;  // No alarm, return 0
            }

            DateTime now = DateTime.Now;
            int currentDay = (int)now.DayOfWeek;  // Sunday = 0, Monday = 1, ..., Saturday = 6

            // Check if today is a valid alarm day
            bool isAlarmDay = days[currentDay];  // Use currentDay to index the days array

            // Check if the current time matches the alarm time
            if (isAlarmDay && now.Hour == hour && now.Minute == minute && !alarmTriggered)
            {
                TriggerAlarm();
                alarmTriggered = true;
            }
            else if (now.Minute != minute)  // Reset alarm trigger after the minute has passed
            {
                alarmTriggered = false;
            }

            return alarmTriggered ? 1.0 : 0.0;  // Return 1 if alarm triggered, 0 otherwise
        }

        private void TriggerAlarm()
        {
            // Only execute the OnTriggerAlarm bang when the alarm is triggered
            if (!string.IsNullOrEmpty(onTriggerAlarm))
            {
                api.Execute(onTriggerAlarm);  // Execute the OnTriggerAlarm bang
            }
        }
    }

    public static class Plugin
    {
        [DllExport]
        public static void Initialize(ref IntPtr data, IntPtr rm)
        {
            data = GCHandle.ToIntPtr(GCHandle.Alloc(new Measure()));
        }

        [DllExport]
        public static void Finalize(IntPtr data)
        {
            GCHandle.FromIntPtr(data).Free();
        }

        [DllExport]
        public static void Reload(IntPtr data, IntPtr rm, ref double maxValue)
        {
            Measure measure = (Measure)GCHandle.FromIntPtr(data).Target;
            measure.Reload(new Rainmeter.API(rm), ref maxValue);  // Pass the API instance here
        }

        [DllExport]
        public static double Update(IntPtr data)
        {
            Measure measure = (Measure)GCHandle.FromIntPtr(data).Target;
            return measure.Update();
        }
    }
}
