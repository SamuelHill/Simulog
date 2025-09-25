using TED;
using Time;

namespace Simulog {
    /// <summary>Commonly used TED.Vars for types used in this simulator.</summary>
    /// <remarks>Variables will be lowercase for style/identification purposes.</remarks>
    public static class Variables {
        // ReSharper disable InconsistentNaming
        /// <summary> Boolean TED variable to represent the state of... </summary>
        public static readonly Var<bool> state = (Var<bool>)"state";
        /// <summary> Boolean TED variable to represent when something exists </summary>
        public static readonly Var<bool> exists = (Var<bool>)"exists";
        /// <summary> Integer TED variable to represent the count of something </summary>
        public static readonly Var<int> count = (Var<int>)"count";
        /// <summary> TimePoint TED variable to represent the current time </summary>
        public static readonly Var<TimePoint> time = (Var<TimePoint>)"time";
        /// <summary> TimePoint TED variable to represent a start time </summary>
        public static readonly Var<TimePoint> start = (Var<TimePoint>)"start";
        /// <summary> TimePoint TED variable to represent an end time </summary>
        public static readonly Var<TimePoint> end = (Var<TimePoint>)"end";
    }
}
