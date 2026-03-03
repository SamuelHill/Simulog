#if RELEASE_FLOATING_POINT_TIME

using TED;
using TED.Primitives;

namespace Time {
    /// <summary>
    /// Internal clock, keeps time during a simulation by ticking along with Simulator.Update
    /// </summary>
    public static class Clock {
        // ReSharper disable once MemberCanBePrivate.Global
        /// <summary>Simulation clock value (measured in 'ticks')</summary>
        public static float ClockTick;
        // ReSharper disable once MemberCanBePrivate.Global
        /// <summary> Duration of the previous tick </summary>
        public static float LastTickDuration;
        /// <summary> Number of ticks that have occured </summary>
        public static int TickCount;

        /// <summary>TED variable to allow for referencing to the underlying clock datatype</summary>
        // ReSharper disable once InconsistentNaming
        public static readonly Var<float> clockTicks = (Var<float>)"clockTicks";

        /// <summary> Increments the internal clock by tick duration </summary>
        public static void Tick(float tickDuration) {
            ClockTick += tickDuration;
            LastTickDuration = tickDuration;
            TickCount += 1;
        }

        /// <summary> Creates a TimePoint from the current clock tick</summary>
        /// <returns> TimePoint representing the current clock tick</returns>
        public static TimePoint TimePoint() => new(ClockTick);
        
        /// <summary> TED Function to access the current clock tick as a TimePoint</summary>
        public static readonly Function<TimePoint> CurrentTimePoint = new($"Current{nameof(TimePoint)}", TimePoint, false)
            {NameForCompilation = nameof(TimePoint)};
          
        private static float DecrementImplementation(float input) => input - LastTickDuration;
        
        /// <summary> Decrements the value passed in by the last ticks duration </summary>
        public static readonly Function<float, float> DecrementByLastTick = new("DecrementByLastTick", DecrementImplementation)
            {NameForCompilation = nameof(DecrementImplementation)};
        
        private static bool TimeWithinLastTick(TimePoint time) => time.Clock < ClockTick && time.Clock >= ClockTick - LastTickDuration;
        
        /// <summary> Checks whether or not the TimePoint has occured yet </summary>
        public static readonly PrimitiveTest<TimePoint> CheckTimeWithinLastTick = new("CheckTimeWithinLastTick", TimeWithinLastTick, false);
            // {NameForCompilation = nameof(TimeWithinLastTick)};
    }
}

#endif