using System;
using TED.Compiler;
using Utilities;

namespace Time {
    /// <summary> Record of a point in time at the finest possible temporal resolution. </summary>
    public readonly struct TimePoint : IComparable<TimePoint>, IEquatable<TimePoint>, ISerializableValue<TimePoint> {
        // ReSharper disable once MemberCanBePrivate.Global
        /// <summary>Tick value within the simulation</summary>
        public readonly uint Clock;
        
        static TimePoint() => Compiler.DeclareExpressionGeneratorForType(
            typeof(TimePoint), (o, _) => ((TimePoint)o == Eschaton ? 
                                              "Time.TimePoint.Eschaton" : 
                                              $"new Time.TimePoint({((TimePoint)o).Clock})",
                                          null)!);

        /// <param name="clock">Tick value</param>
        public TimePoint(uint clock) => Clock = clock;

        // ReSharper disable once InconsistentNaming
        /// <summary> The last possible TimePoint (in this case uint.MaxValue) </summary>
        public static readonly TimePoint Eschaton = new(uint.MaxValue);
        private bool IsEschaton => Clock == uint.MaxValue;

        // *************************** Compare and Equality interfacing ***************************
        
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

        // ****************************************************************************************

        /// <returns>TimePoint as an int.</returns>
        public override string ToString() => IsEschaton ? "Has not occurred" : Clock.ToString();

        /// <summary> For use by CsvReader. Takes a string and converts it to a TimePoint.</summary>
        public static TimePoint FromString(string timePointString) {
            if (timePointString == "Has not occurred") return Eschaton;
            if (uint.TryParse(timePointString, out var i)) { return new TimePoint(i); }
            throw new InvalidOperationException($"Can't parse TimePoint from string: {timePointString}");
        }
    }
}
