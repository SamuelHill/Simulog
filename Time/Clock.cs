using TED;

namespace Time {
    /// <summary>
    /// Internal clock, keeps time during a simulation by ticking along with Simulator.Update
    /// </summary>
    public static class Clock {
        // ReSharper disable once MemberCanBePrivate.Global
        internal static uint ClockTick;
        
        /// <summary> Increments the internal clock by 1 </summary>
        public static void Tick() => ClockTick++;
        
        /// <summary> Creates a TimePoint from the current clock tick</summary>
        /// <returns> TimePoint representing the current clock tick</returns>
        public static TimePoint TimePoint() => new(ClockTick);
        
        /// <summary> TED Function to access the current clock tick as a TimePoint</summary>
        public static readonly Function<TimePoint> CurrentTimePoint = new($"Current{nameof(TimePoint)}", TimePoint, false)
            {NameForCompilation = nameof(TimePoint)};
    }
}
