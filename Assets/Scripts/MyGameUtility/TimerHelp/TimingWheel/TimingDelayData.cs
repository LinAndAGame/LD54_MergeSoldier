using System;
using Unity.VisualScripting;

namespace MyGameUtility {
    public struct TimingDelayData {
        public readonly DateTime       StartDateTime;
        public readonly DateTime       EndDateTime;

        public int TotalMs => (int)EndDateTime.Subtract(StartDateTime).TotalMilliseconds;

        public TimingDelayData(int years, int months, float days, float hours, float minutes, float seconds, int ms) {
            StartDateTime = DateTime.Now;
            EndDateTime   = DateTime.Now;
            EndDateTime   = EndDateTime.AddYears(years);
            EndDateTime   = EndDateTime.AddMonths(months);
            EndDateTime   = EndDateTime.AddDays(days);
            EndDateTime   = EndDateTime.AddHours(hours);
            EndDateTime   = EndDateTime.AddMinutes(minutes);
            EndDateTime   = EndDateTime.AddSeconds(seconds);
            EndDateTime   = EndDateTime.AddMilliseconds(ms);
        }

        public TimingDelayData(float minutes, float seconds) : this(0, 0, 0, 0, minutes, seconds, 0) { }
        public TimingDelayData(float minutes, float seconds, int ms) : this(0, 0, 0, 0, minutes, seconds, ms) { }
    }
}