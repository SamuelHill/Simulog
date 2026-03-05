using TED;
using Time;
using Utilities;

namespace Simulog {
    using static Clock;
    using static ChronicleIndexing;

    /// <summary>
    /// 1-argument event chronicle (2 column table predicate). Base/Extensional table - stored not recalculated
    /// each tick - with every event occurrence timestamped.
    /// </summary>
    /// <typeparam name="T1">Type of the events one argument (one column)</typeparam>
    public class EventChronicle<T1> : TablePredicate<T1, TimePoint> {
        /// <summary>Make a new EventChronicle predicate with the specified event and default time argument</summary>
        /// <param name="table">Event to chronicle</param>
        /// <param name="time">Default variable of the TimePoint argument</param>
        public EventChronicle(Event<T1> table, Var<TimePoint> time) :
            base(table.Name + "Chronicle", (Var<T1>)table.Arg1, time) {
            Add.If(table, CurrentTimePoint[time]);
        }
    }
    
    /// <summary>
    /// 2-argument event chronicle (3 column table predicate). Base/Extensional table - stored not recalculated
    /// each tick - with every event occurrence timestamped.
    /// </summary>
    /// <typeparam name="T1">Type of the events first argument (column one)</typeparam>
    /// <typeparam name="T2">Type of the events second argument (column two)</typeparam>
    public class EventChronicle<T1, T2> : TablePredicate<T1, T2, TimePoint> {
        /// <inheritdoc cref="EventChronicle{T1}(Event{T1},Var{TimePoint})"/>
        public EventChronicle(Event<T1, T2> table, Var<TimePoint> time) :
            base(table.Name + "Chronicle", DemoteKey(table.Arg1), DemoteKey(table.Arg2), time) {
            Add.If(table, CurrentTimePoint[time]);
        }
    }

    /// <summary>
    /// 3-argument event chronicle (4 column table predicate). Base/Extensional table - stored not recalculated
    /// each tick - with every event occurrence timestamped.
    /// </summary>
    /// <typeparam name="T1">Type of the events first argument (column one)</typeparam>
    /// <typeparam name="T2">Type of the events second argument (column two)</typeparam>
    /// <typeparam name="T3">Type of the events third argument (column three)</typeparam>
    public class EventChronicle<T1, T2, T3> : TablePredicate<T1, T2, T3, TimePoint> {
        /// <inheritdoc cref="EventChronicle{T1}(Event{T1},Var{TimePoint})"/>
        public EventChronicle(Event<T1, T2, T3> table, Var<TimePoint> time) :
            base(table.Name + "Chronicle", DemoteKey(table.Arg1), 
                 DemoteKey(table.Arg2), DemoteKey(table.Arg3), time) {
            Add.If(table, CurrentTimePoint[time]);
        }
    }

    /// <summary>
    /// 4-argument event chronicle (5 column table predicate). Base/Extensional table - stored not recalculated
    /// each tick - with every event occurrence timestamped.
    /// </summary>
    /// <typeparam name="T1">Type of the events first argument (column one)</typeparam>
    /// <typeparam name="T2">Type of the events second argument (column two)</typeparam>
    /// <typeparam name="T3">Type of the events third argument (column three)</typeparam>
    /// <typeparam name="T4">Type of the events fourth argument (column four)</typeparam>
    public class EventChronicle<T1, T2, T3, T4> : TablePredicate<T1, T2, T3, T4, TimePoint> {
        /// <inheritdoc cref="EventChronicle{T1}(Event{T1},Var{TimePoint})"/>
        public EventChronicle(Event<T1, T2, T3, T4> table, Var<TimePoint> time) :
            base(table.Name + "Chronicle", DemoteKey(table.Arg1), DemoteKey(table.Arg2), 
                 DemoteKey(table.Arg3), DemoteKey(table.Arg4), time) {
            Add.If(table, CurrentTimePoint[time]);
        }
    }

    /// <summary>
    /// 5-argument event chronicle (6 column table predicate). Base/Extensional table - stored not recalculated
    /// each tick - with every event occurrence timestamped.
    /// </summary>
    /// <typeparam name="T1">Type of the events first argument (column one)</typeparam>
    /// <typeparam name="T2">Type of the events second argument (column two)</typeparam>
    /// <typeparam name="T3">Type of the events third argument (column three)</typeparam>
    /// <typeparam name="T4">Type of the events fourth argument (column four)</typeparam>
    /// <typeparam name="T5">Type of the events fifth argument (column five)</typeparam>
    public class EventChronicle<T1, T2, T3, T4, T5> : TablePredicate<T1, T2, T3, T4, T5, TimePoint> {
        /// <inheritdoc cref="EventChronicle{T1}(Event{T1},Var{TimePoint})"/>
        public EventChronicle(Event<T1, T2, T3, T4, T5> table, Var<TimePoint> time) :
            base(table.Name + "Chronicle", DemoteKey(table.Arg1), DemoteKey(table.Arg2),
                 DemoteKey(table.Arg3), DemoteKey(table.Arg4), DemoteKey(table.Arg5), time) {
            Add.If(table, CurrentTimePoint[time]);
        }
    }

    /// <summary>
    /// 6-argument event chronicle (7 column table predicate). Base/Extensional table - stored not recalculated
    /// each tick - with every event occurrence timestamped.
    /// </summary>
    /// <typeparam name="T1">Type of the events first argument (column one)</typeparam>
    /// <typeparam name="T2">Type of the events second argument (column two)</typeparam>
    /// <typeparam name="T3">Type of the events third argument (column three)</typeparam>
    /// <typeparam name="T4">Type of the events fourth argument (column four)</typeparam>
    /// <typeparam name="T5">Type of the events fifth argument (column five)</typeparam>
    /// <typeparam name="T6">Type of the events sixth argument (column six)</typeparam>
    public class EventChronicle<T1, T2, T3, T4, T5, T6> : TablePredicate<T1, T2, T3, T4, T5, T6, TimePoint> {
        /// <inheritdoc cref="EventChronicle{T1}(Event{T1},Var{TimePoint})"/>
        public EventChronicle(Event<T1, T2, T3, T4, T5, T6> table, Var<TimePoint> time) :
            base(table.Name + "Chronicle", DemoteKey(table.Arg1), DemoteKey(table.Arg2), DemoteKey(table.Arg3), 
                 DemoteKey(table.Arg4), DemoteKey(table.Arg5), DemoteKey(table.Arg6), time) {
            Add.If(table, CurrentTimePoint[time]);
        }
    }

    /// <summary>
    /// 7-argument event chronicle (8 column table predicate). Base/Extensional table - stored not recalculated
    /// each tick - with every event occurrence timestamped.
    /// </summary>
    /// <typeparam name="T1">Type of the events first argument (column one)</typeparam>
    /// <typeparam name="T2">Type of the events second argument (column two)</typeparam>
    /// <typeparam name="T3">Type of the events third argument (column three)</typeparam>
    /// <typeparam name="T4">Type of the events fourth argument (column four)</typeparam>
    /// <typeparam name="T5">Type of the events fifth argument (column five)</typeparam>
    /// <typeparam name="T6">Type of the events sixth argument (column six)</typeparam>
    /// <typeparam name="T7">Type of the events seventh argument (column seven)</typeparam>
    public class EventChronicle<T1, T2, T3, T4, T5, T6, T7> : TablePredicate<T1, T2, T3, T4, T5, T6, T7, TimePoint> {
        /// <inheritdoc cref="EventChronicle{T1}(Event{T1},Var{TimePoint})"/>
        public EventChronicle(Event<T1, T2, T3, T4, T5, T6, T7> table, Var<TimePoint> time) :
            base(table.Name + "Chronicle", DemoteKey(table.Arg1), DemoteKey(table.Arg2), DemoteKey(table.Arg3),
                 DemoteKey(table.Arg4), DemoteKey(table.Arg5), DemoteKey(table.Arg6), DemoteKey(table.Arg7), time) {
            Add.If(table, CurrentTimePoint[time]);
        }
    }
}
