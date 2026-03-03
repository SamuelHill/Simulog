#if RELEASE_INTEGER_TIME

using TED;
using TED.Primitives;

namespace Time {
    /// <summary>
    /// Internal clock, keeps time during a simulation by ticking along with Simulator.Update
    /// </summary>
    public static class Clock {
        // ReSharper disable once MemberCanBePrivate.Global
        /// <summary> Simulation clock value (measured in 'ticks') </summary>
        public static uint ClockTick;
        // ReSharper disable once MemberCanBePrivate.Global
        /// <summary> Duration of the previous tick </summary>
        public const uint LastTickDuration = 1;

        /// <summary>TED variable to allow for referencing to the underlying clock datatype</summary>
        // ReSharper disable once InconsistentNaming
        public static readonly Var<uint> clockTicks = (Var<uint>)"clockTicks";

        /// <summary> Increments the internal clock by 1 </summary>
        public static void Tick() => ClockTick++;
        
        /// <summary> Creates a TimePoint from the current clock tick</summary>
        /// <returns> TimePoint representing the current clock tick</returns>
        public static TimePoint TimePoint() => new(ClockTick);
        
        /// <summary> TED Function to access the current clock tick as a TimePoint</summary>
        public static readonly Function<TimePoint> CurrentTimePoint = new($"Current{nameof(TimePoint)}", TimePoint, false)
            {NameForCompilation = nameof(TimePoint)};
        
        private static uint DecrementImplementation(uint input) => input - LastTickDuration;
        
        /// <summary> Decrements the value passed in by the last ticks duration (1) </summary>
        public static readonly Function<uint, uint> DecrementByLastTick = new("DecrementByLastTick", DecrementImplementation)
            {NameForCompilation = nameof(DecrementImplementation)};
        
        private static bool TimeWithinLastTick(TimePoint time) => time.Clock < ClockTick && time.Clock >= ClockTick - LastTickDuration;
        
        /// <summary> Checks whether or not the TimePoint has occured yet </summary>
        public static readonly PrimitiveTest<TimePoint> CheckTimeWithinLastTick = new("CheckTimeWithinLastTick", TimeWithinLastTick, false);
            // {NameForCompilation = nameof(TimeWithinLastTick)};
    }
}

#endif