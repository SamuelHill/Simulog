#if RELEASE_INTEGER_TIME

using System;
using TED;
using TED.Compiler;
using Utilities;

namespace Time {
    /// <summary> Record of a point in time at the finest possible temporal resolution. </summary>
    public readonly struct TimePoint : IComparable<TimePoint>, IEquatable<TimePoint>, ISerializableValue<TimePoint> {
        // ReSharper disable once MemberCanBePrivate.Global
        /// <summary>Clock tick value that this time point represents</summary>
        public readonly uint Clock;
        
        static TimePoint() => Compiler.DeclareExpressionGeneratorForType(
            typeof(TimePoint), (o, _) => ((TimePoint)o == Eschaton ? 
                                              "Time.TimePoint.Eschaton" : 
                                              $"new Time.TimePoint({((TimePoint)o).Clock})",
                                          // Second element of tuple is expected to be a string naming this expression
                                          // generator for caching, but null is also excepted in which case no caching
                                          // is done. Caching is only needed for linking outside data (tables) or
                                          // retaining state (RNG), neither of which is needed here.
                                          // ReSharper disable once NullableWarningSuppressionIsUsed
                                          null)!);

        /// <param name="clock">Tick value</param>
        public TimePoint(uint clock) => Clock = clock;

        // ReSharper disable once InconsistentNaming MemberCanBePrivate.Global
        /// <summary> The last possible TimePoint (in this case uint.MaxValue) </summary>
        public static readonly TimePoint Eschaton = new(uint.MaxValue);
        private bool IsEschaton => Clock == uint.MaxValue;

        #region Compare and Equality interfacing
        
        /// <inheritdoc />
        public int CompareTo(TimePoint other) => Clock.CompareTo(other.Clock);
        /// <inheritdoc />
        public bool Equals(TimePoint other) => Clock == other.Clock;
        /// <inheritdoc />
        public override bool Equals(object? obj) => obj is TimePoint other && Equals(other);
        /// <inheritdoc />
        public override int GetHashCode() => Clock.GetHashCode();

        /// <summary> Checks if both TimePoints represent the same clock tick </summary>
        /// <param name="t1">TimePoint 1</param>
        /// <param name="t2">TimePoint 2</param>
        /// <returns> true if both TimePoints represent the same clock tick </returns>
        public static bool operator ==(TimePoint t1, TimePoint t2) => t1.Equals(t2);
        /// <summary> Checks if both TimePoints represent different clock ticks </summary>
        /// <param name="t1">TimePoint 1</param>
        /// <param name="t2">TimePoint 2</param>
        /// <returns> true if both TimePoints represent different clock ticks </returns>
        public static bool operator !=(TimePoint t1, TimePoint t2) => !(t1 == t2);
        
        /// <summary> Checks if TimePoint t1s clock tick is the same as the value t2 </summary>
        /// <param name="t1">TimePoint</param>
        /// <param name="t2">number of clock ticks</param>
        /// <returns> true if TimePoint t1s clock tick is the same as the value t2 </returns>
        public static bool operator ==(TimePoint t1, uint t2) => t1.Clock == t2;
        /// <summary> Checks if TimePoint t1s clock tick is different from the value t2 </summary>
        /// <param name="t1">TimePoint</param>
        /// <param name="t2">number of clock ticks</param>
        /// <returns> true if TimePoint t1s clock tick is different from the value t2 </returns>
        public static bool operator !=(TimePoint t1, uint t2) => t1.Clock != t2;
        
        /// <summary> Checks if TimePoint t1 is greater (comes later in time) than t2 </summary>
        /// <param name="t1">TimePoint 1</param>
        /// <param name="t2">TimePoint 2</param>
        /// <returns> true if TimePoint t1 is greater (comes later in time) than t2 </returns>
        public static bool operator >(TimePoint t1, TimePoint t2) => t1.CompareTo(t2) == 1;
        /// <summary> Checks if TimePoint t1 is less (comes sooner in time) than t2 </summary>
        /// <param name="t1">TimePoint 1</param>
        /// <param name="t2">TimePoint 2</param>
        /// <returns> true if TimePoint t1 is less (comes sooner in time) than t2 </returns>
        public static bool operator <(TimePoint t1, TimePoint t2) => t1.CompareTo(t2) == -1;
        
        /// <summary> Checks if TimePoint t1s clock tick is greater than the value t2</summary>
        /// <param name="t1">TimePoint</param>
        /// <param name="t2">number of clock ticks</param>
        /// <returns> true if TimePoint t1s clock tick is greater than the value t2 </returns>
        public static bool operator >(TimePoint t1, uint t2) => t1.Clock.CompareTo(t2) == 1;
        /// <summary> Checks if TimePoint t1s clock tick is less than the value t2 </summary>
        /// <param name="t1">TimePoint</param>
        /// <param name="t2">number of clock ticks</param>
        /// <returns> true if TimePoint t1s clock tick is less than the value t2 </returns>
        public static bool operator <(TimePoint t1, uint t2) => t1.Clock.CompareTo(t2) == -1;
        
        /// <summary> Checks if TimePoint t1 is greater (comes later in time) than or equal to t2 </summary>
        /// <param name="t1">TimePoint 1</param>
        /// <param name="t2">TimePoint 2</param>
        /// <returns> true if TimePoint t1 is greater (comes later in time) than or equal to t2 </returns>
        public static bool operator >=(TimePoint t1, TimePoint t2) => t1.CompareTo(t2) != -1;
        /// <summary> Checks if TimePoint t1 is less (comes sooner in time) than or equal to t2 </summary>
        /// <param name="t1">TimePoint 1</param>
        /// <param name="t2">TimePoint 2</param>
        /// <returns> true if TimePoint t1 is less (comes sooner in time) than or equal to t2 </returns>
        public static bool operator <=(TimePoint t1, TimePoint t2) => t1.CompareTo(t2) != 1;
        
        /// <summary> Checks if TimePoint t1s clock tick is greater than or equal to the value t2</summary>
        /// <param name="t1">TimePoint</param>
        /// <param name="t2">number of clock ticks</param>
        /// <returns> true if TimePoint t1s clock tick is greater than or equal to the value t2 </returns>
        public static bool operator >=(TimePoint t1, uint t2) => t1.Clock.CompareTo(t2) != -1;
        /// <summary> Checks if TimePoint t1s clock tick is less than or equal to the value t2 </summary>
        /// <param name="t1">TimePoint</param>
        /// <param name="t2">number of clock ticks</param>
        /// <returns> true if TimePoint t1s clock tick is less than or equal to the value t2 </returns>
        public static bool operator <=(TimePoint t1, uint t2) => t1.Clock.CompareTo(t2) != 1;
        
        /// <summary> Add TimePoints creating a new point representing a clock tick that is the sum of each TimePoints clock ticks </summary>
        /// <param name="t1">TimePoint 1</param>
        /// <param name="t2">TimePoint 2</param>
        /// <returns> a TimePoint representing a clock tick that is the sum of each TimePoints clock ticks </returns>
        public static TimePoint operator +(TimePoint t1, TimePoint t2) => new(t1.Clock + t2.Clock);
        /// <summary> Subtract TimePoints creating a new point representing a clock tick that is the difference of the TimePoints clock ticks </summary>
        /// <param name="t1">TimePoint 1</param>
        /// <param name="t2">TimePoint 2</param>
        /// <returns> a TimePoint representing a clock tick that is the difference of the TimePoints clock ticks </returns>
        public static TimePoint operator -(TimePoint t1, TimePoint t2) => new(t1.Clock - t2.Clock);
        
        /// <summary> Add some number of clock ticks to a TimePoints clock ticks </summary>
        /// <param name="t1">TimePoint</param>
        /// <param name="t2">number of clock ticks</param>
        /// <returns> a TimePoint representing the time t2 number of clock ticks after (assuming positive t2) t1 </returns>
        public static TimePoint operator +(TimePoint t1, uint t2) => new(t1.Clock + t2);
        /// <summary> Subtract some number of clock ticks to a TimePoints clock ticks </summary>
        /// <param name="t1">TimePoint</param>
        /// <param name="t2">number of clock ticks</param>
        /// <returns> a TimePoint representing the time t2 number of clock ticks before (assuming positive t2) t1 </returns>
        public static TimePoint operator -(TimePoint t1, uint t2) => new(t1.Clock - t2);
        
        #endregion

        /// <summary> <inheritdoc cref="TimePoint.op_Addition(TimePoint,uint)" path="summary"/> </summary>
        public static Function<TimePoint, uint, TimePoint> AddToTimePoint = new(nameof(AddToTimePoint), AddToTimePointImplementation, false)
            {NameForCompilation = nameof(AddToTimePointImplementation)};
        
        /// <summary> <inheritdoc cref="TimePoint.op_Addition(TimePoint,uint)" path="summary"/> </summary>
        /// <param name="x"><inheritdoc cref="TimePoint.op_Addition(TimePoint,uint)" path="param[@name='t1']/node()"/></param>
        /// <param name="y">number of clock ticks</param>
        /// <returns>a TimePoint representing the time y number of clock ticks after (assuming positive y) x</returns>
        // This should work but doesn't seem to for me:
        // <param name="y"><inheritdoc cref="TimePoint.op_Addition(TimePoint,uint)" path="param[@name='t2']/node()"/></param>
        public static TimePoint AddToTimePointImplementation(TimePoint x, uint y) => x + y;

        /// <returns>TimePoint as an integer string with the text "Has not occurred" for Eschaton TimePoints.</returns>
        public override string ToString() => IsEschaton ? "Has not occurred" : Clock.ToString();

        /// <summary> For use by CsvReader. Takes a string and converts it to a TimePoint.</summary>
        public static TimePoint FromString(string timePointString) {
            if (timePointString == "Has not occurred") return Eschaton;
            if (uint.TryParse(timePointString, out var i)) return new TimePoint(i);
            throw new InvalidOperationException($"Can't parse TimePoint from string: {timePointString}");
        }
    }
}

#endif