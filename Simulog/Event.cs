using TED;
using TED.Interpreter;
using Time;

namespace Simulog {
    using static Variables;

    /// <summary>
    /// A 1-argument event (table predicate). When used as intended - with OccursWhen calls - this is an
    /// intensional/derived table and as such will have it's contents recomputed every tick.
    /// </summary>
    /// <typeparam name="T1">Type of the one argument (one column)</typeparam>
    public class Event<T1> : TablePredicate<T1> {
        /// <summary>Make a new Event predicate with the specified name and default arguments</summary>
        /// <param name="name">Name of the Event</param>
        /// <param name="arg1">Default variable of the first argument</param>
        public Event(string name, IColumnSpec<T1> arg1) : base(name, arg1) { Arg1 = arg1; }

        internal readonly IColumnSpec<T1> Arg1;

        private EventChronicle<T1>? _chronicle;
        
        // ReSharper disable once MemberCanBePrivate.Global
        /// <summary>Access (and create if not created) the event chronicle for this event.</summary>
        public EventChronicle<T1> Chronicle {
            get {
                _chronicle ??= new EventChronicle<T1>(this, time);
                return _chronicle;
            }
        }

        /// <summary>Establishes the logic for when an event occurs.</summary>
        /// <param name="conditions">Conditions under which the event should occur. When they conditions evaluate
        /// to True the values stored in the default variable will be added to the table this tick.</param>
        /// <returns>The same Event for method chaining.</returns>
        public Event<T1> OccursWhen(params Goal[] conditions) {
            If(conditions);
            return this;
        }
        /// <summary>Establishes the logic for side effects of when an event occurs.</summary>
        /// <param name="effects">List of Effects that this event occuring will cause.</param>
        /// <returns>The same Event for method chaining.</returns>
        public Event<T1> Causes(params Effect[] effects) {
            foreach (var e in effects) e.GenerateCode(DefaultGoal);
            return this;
        }

        /// <summary>Make a goal from this predicate with the specified argument value.</summary>
        /// <param name="arg1">Value to check for in the table.</param>
        public new EventGoal this[Term<T1> arg1] => new (this, arg1);

        /// <summary>Modified table goal for events - representing the application of arguments to an event.</summary>
        public class EventGoal : TableGoal<T1> {
            // ReSharper disable once SuggestBaseTypeForParameterInConstructor
            /// <summary>Create EventGoal for the given event and argument.</summary>
            /// <param name="e">Event to apply argument to.</param>
            /// <param name="arg1">Argument to apply to event.</param>
            public EventGoal(Event<T1> e, Term<T1> arg1) : base(e, arg1) { }

            /// <summary>Access this events chronicle to find occurrences of the event at the specified time.</summary>
            /// <param name="time">TimePoint to find occurrences of this event at.</param>
            /// <returns>Goal of finding the occurrences of EventGoal at the time passed in.</returns>
            public Goal At(Term<TimePoint> time) => 
                ((Event<T1>)TablePredicate).Chronicle[Arg1, time];
        }
    }

    /// <summary>
    /// A 2-argument event (table predicate). When used as intended - with OccursWhen calls - this is an
    /// intensional/derived table and as such will have it's contents recomputed every tick.
    /// </summary>
    /// <typeparam name="T1">Type of the first argument (column one)</typeparam>
    /// <typeparam name="T2">Type of the second argument (column two)</typeparam>
    public class Event<T1, T2> : TablePredicate<T1, T2> {
        /// <summary><inheritdoc cref="Event{T1}(string,IColumnSpec{T1})"/></summary>
        /// <param name="name">Name of the Event</param>
        /// <param name="arg1">Default variable of the first argument</param>
        /// <param name="arg2">Default variable of the second argument</param>
        public Event(string name, IColumnSpec<T1> arg1, IColumnSpec<T2> arg2) : base(name, arg1, arg2) {
            Arg1 = arg1;
            Arg2 = arg2;
        }

        internal readonly IColumnSpec<T1> Arg1;
        internal readonly IColumnSpec<T2> Arg2;

        private EventChronicle<T1, T2>? _chronicle;

        /// <inheritdoc cref="Event{T1}.Chronicle"/>
        public EventChronicle<T1, T2> Chronicle {
            get {
                _chronicle ??= new EventChronicle<T1, T2>(this, time);
                return _chronicle;
            }
        }
        
        /// <inheritdoc cref="Event{T1}.OccursWhen"/>
        public Event<T1, T2> OccursWhen(params Goal[] conditions) {
            If(conditions);
            return this;
        }
        /// <inheritdoc cref="Event{T1}.Causes"/>
        public Event<T1, T2> Causes(params Effect[] effects) {
            foreach (var e in effects) e.GenerateCode(DefaultGoal);
            return this;
        }
        
        /// <summary>Make a goal from this predicate with the specified argument values.</summary>
        /// <param name="arg1">Value to check for in column one of the table.</param>
        /// <param name="arg2">Value to check for in column two of the table.</param>
        public new EventGoal this[Term<T1> arg1, Term<T2> arg2] => new(this, arg1, arg2);

        /// <inheritdoc cref="Event{T1}.EventGoal"/>
        public class EventGoal : TableGoal<T1, T2> {
            // ReSharper disable once SuggestBaseTypeForParameterInConstructor
            /// <summary>Create EventGoal for the given event and arguments.</summary>
            /// <param name="e">Event to apply arguments to.</param>
            /// <param name="arg1">Argument to apply to event (column one).</param>
            /// <param name="arg2">Argument to apply to event (column two).</param>
            public EventGoal(Event<T1, T2> e, Term<T1> arg1, Term<T2> arg2) : base(e, arg1, arg2) { }

            /// <inheritdoc cref="Event{T1}.EventGoal.At"/>
            public Goal At(Term<TimePoint> time) =>
                ((Event<T1, T2>)TablePredicate).Chronicle[Arg1, Arg2, time];
        }
    }

    /// <summary>
    /// A 3-argument event (table predicate). When used as intended - with OccursWhen calls - this is an
    /// intensional/derived table and as such will have it's contents recomputed every tick.
    /// </summary>
    /// <typeparam name="T1">Type of the first argument (column one)</typeparam>
    /// <typeparam name="T2">Type of the second argument (column two)</typeparam>
    /// <typeparam name="T3">Type of the third argument (column three)</typeparam>
    public class Event<T1, T2, T3> : TablePredicate<T1, T2, T3> {
        /// <summary><inheritdoc cref="Event{T1}(string,IColumnSpec{T1})"/></summary>
        /// <param name="name">Name of the Event</param>
        /// <param name="arg1">Default variable of the first argument</param>
        /// <param name="arg2">Default variable of the second argument</param>
        /// <param name="arg3">Default variable of the third argument</param>
        public Event(string name, IColumnSpec<T1> arg1, IColumnSpec<T2> arg2, IColumnSpec<T3> arg3) : 
            base(name, arg1, arg2, arg3) {
            Arg1 = arg1;
            Arg2 = arg2;
            Arg3 = arg3;
        }

        internal readonly IColumnSpec<T1> Arg1;
        internal readonly IColumnSpec<T2> Arg2;
        internal readonly IColumnSpec<T3> Arg3;

        private EventChronicle<T1, T2, T3>? _chronicle;

        // ReSharper disable once MemberCanBePrivate.Global
        /// <inheritdoc cref="Event{T1}.Chronicle"/>
        public EventChronicle<T1, T2, T3> Chronicle {
            get {
                _chronicle ??= new EventChronicle<T1, T2, T3>(this, time);
                return _chronicle;
            }
        }

        /// <inheritdoc cref="Event{T1}.OccursWhen"/>
        public Event<T1, T2, T3> OccursWhen(params Goal[] conditions) {
            If(conditions);
            return this;
        }
        /// <inheritdoc cref="Event{T1}.Causes"/>
        public Event<T1, T2, T3> Causes(params Effect[] effects) {
            foreach (var e in effects) e.GenerateCode(DefaultGoal);
            return this;
        }

        /// <summary>Make a goal from this predicate with the specified argument values.</summary>
        /// <param name="arg1">Value to check for in column one of the table.</param>
        /// <param name="arg2">Value to check for in column two of the table.</param>
        /// <param name="arg3">Value to check for in column three of the table.</param>
        public new EventGoal this[Term<T1> arg1, Term<T2> arg2, Term<T3> arg3] =>
            new(this, arg1, arg2, arg3);

        /// <inheritdoc cref="Event{T1}.EventGoal"/>
        public class EventGoal : TableGoal<T1, T2, T3> {
            // ReSharper disable once SuggestBaseTypeForParameterInConstructor
            /// <summary><inheritdoc cref="Event{T1,T2}.EventGoal(Event{T1,T2},Term{T1},Term{T2})"/></summary>
            /// <param name="e">Event to apply arguments to.</param>
            /// <param name="arg1">Argument to apply to event (column one).</param>
            /// <param name="arg2">Argument to apply to event (column two).</param>
            /// <param name="arg3">Argument to apply to event (column three).</param>
            public EventGoal(Event<T1, T2, T3> e, Term<T1> arg1, Term<T2> arg2, Term<T3> arg3) :
                base(e, arg1, arg2, arg3) { }

            /// <inheritdoc cref="Event{T1}.EventGoal.At"/>
            public Goal At(Term<TimePoint> time) =>
                ((Event<T1, T2, T3>)TablePredicate).Chronicle[Arg1, Arg2, Arg3, time];
        }
    }

    /// <summary>
    /// A 4-argument event (table predicate). When used as intended - with OccursWhen calls - this is an
    /// intensional/derived table and as such will have it's contents recomputed every tick.
    /// </summary>
    /// <typeparam name="T1">Type of the first argument (column one)</typeparam>
    /// <typeparam name="T2">Type of the second argument (column two)</typeparam>
    /// <typeparam name="T3">Type of the third argument (column three)</typeparam>
    /// <typeparam name="T4">Type of the fourth argument (column four)</typeparam>
    public class Event<T1, T2, T3, T4> : TablePredicate<T1, T2, T3, T4> {
        /// <summary><inheritdoc cref="Event{T1}(string,IColumnSpec{T1})"/></summary>
        /// <param name="name">Name of the Event</param>
        /// <param name="arg1">Default variable of the first argument</param>
        /// <param name="arg2">Default variable of the second argument</param>
        /// <param name="arg3">Default variable of the third argument</param>
        /// <param name="arg4">Default variable of the fourth argument</param>
        public Event(string name, IColumnSpec<T1> arg1, IColumnSpec<T2> arg2, 
                     IColumnSpec<T3> arg3, IColumnSpec<T4> arg4) :
            base(name, arg1, arg2, arg3, arg4) {
            Arg1 = arg1;
            Arg2 = arg2;
            Arg3 = arg3;
            Arg4 = arg4;
        }

        internal readonly IColumnSpec<T1> Arg1;
        internal readonly IColumnSpec<T2> Arg2;
        internal readonly IColumnSpec<T3> Arg3;
        internal readonly IColumnSpec<T4> Arg4;

        private EventChronicle<T1, T2, T3, T4>? _chronicle;

        // ReSharper disable once MemberCanBePrivate.Global
        /// <inheritdoc cref="Event{T1}.Chronicle"/>
        public EventChronicle<T1, T2, T3, T4> Chronicle {
            get {
                _chronicle ??= new EventChronicle<T1, T2, T3, T4>(this, time);
                return _chronicle;
            }
        }

        /// <inheritdoc cref="Event{T1}.OccursWhen"/>
        public Event<T1, T2, T3, T4> OccursWhen(params Goal[] conditions) {
            If(conditions);
            return this;
        }
        /// <inheritdoc cref="Event{T1}.Causes"/>
        public Event<T1, T2, T3, T4> Causes(params Effect[] effects) {
            foreach (var e in effects) e.GenerateCode(DefaultGoal);
            return this;
        }

        /// <summary>Make a goal from this predicate with the specified argument values.</summary>
        /// <param name="arg1">Value to check for in column one of the table.</param>
        /// <param name="arg2">Value to check for in column two of the table.</param>
        /// <param name="arg3">Value to check for in column three of the table.</param>
        /// <param name="arg4">Value to check for in column four of the table.</param>
        public new EventGoal this[Term<T1> arg1, Term<T2> arg2, Term<T3> arg3, Term<T4> arg4] =>
            new(this, arg1, arg2, arg3, arg4);

        /// <inheritdoc cref="Event{T1}.EventGoal"/>
        public class EventGoal : TableGoal<T1, T2, T3, T4> {
            // ReSharper disable once SuggestBaseTypeForParameterInConstructor
            /// <summary><inheritdoc cref="Event{T1,T2}.EventGoal(Event{T1,T2},Term{T1},Term{T2})"/></summary>
            /// <param name="e">Event to apply arguments to.</param>
            /// <param name="arg1">Argument to apply to event (column one).</param>
            /// <param name="arg2">Argument to apply to event (column two).</param>
            /// <param name="arg3">Argument to apply to event (column three).</param>
            /// <param name="arg4">Argument to apply to event (column four).</param>
            public EventGoal(Event<T1, T2, T3, T4> e, Term<T1> arg1, Term<T2> arg2, 
                             Term<T3> arg3, Term<T4> arg4) :
                base(e, arg1, arg2, arg3, arg4) { }

            /// <inheritdoc cref="Event{T1}.EventGoal.At"/>
            public Goal At(Term<TimePoint> time) =>
                ((Event<T1, T2, T3, T4>)TablePredicate).Chronicle[Arg1, Arg2, Arg3, Arg4, time];
        }
    }

    /// <summary>
    /// A 5-argument event (table predicate). When used as intended - with OccursWhen calls - this is an
    /// intensional/derived table and as such will have it's contents recomputed every tick.
    /// </summary>
    /// <typeparam name="T1">Type of the first argument (column one)</typeparam>
    /// <typeparam name="T2">Type of the second argument (column two)</typeparam>
    /// <typeparam name="T3">Type of the third argument (column three)</typeparam>
    /// <typeparam name="T4">Type of the fourth argument (column four)</typeparam>
    /// <typeparam name="T5">Type of the fifth argument (column five)</typeparam>
    public class Event<T1, T2, T3, T4, T5> : TablePredicate<T1, T2, T3, T4, T5> {
        /// <summary><inheritdoc cref="Event{T1}(string,IColumnSpec{T1})"/></summary>
        /// <param name="name">Name of the Event</param>
        /// <param name="arg1">Default variable of the first argument</param>
        /// <param name="arg2">Default variable of the second argument</param>
        /// <param name="arg3">Default variable of the third argument</param>
        /// <param name="arg4">Default variable of the fourth argument</param>
        /// <param name="arg5">Default variable of the fifth argument</param>
        public Event(string name, IColumnSpec<T1> arg1, IColumnSpec<T2> arg2, IColumnSpec<T3> arg3,
                     IColumnSpec<T4> arg4, IColumnSpec<T5> arg5) :
            base(name, arg1, arg2, arg3, arg4, arg5) {
            Arg1 = arg1;
            Arg2 = arg2;
            Arg3 = arg3;
            Arg4 = arg4;
            Arg5 = arg5;
        }

        internal readonly IColumnSpec<T1> Arg1;
        internal readonly IColumnSpec<T2> Arg2;
        internal readonly IColumnSpec<T3> Arg3;
        internal readonly IColumnSpec<T4> Arg4;
        internal readonly IColumnSpec<T5> Arg5;

        private EventChronicle<T1, T2, T3, T4, T5>? _chronicle;

        // ReSharper disable once MemberCanBePrivate.Global
        /// <inheritdoc cref="Event{T1}.Chronicle"/>
        public EventChronicle<T1, T2, T3, T4, T5> Chronicle {
            get {
                _chronicle ??= new EventChronicle<T1, T2, T3, T4, T5>(this, time);
                return _chronicle;
            }
        }

        /// <inheritdoc cref="Event{T1}.OccursWhen"/>
        public Event<T1, T2, T3, T4, T5> OccursWhen(params Goal[] conditions) {
            If(conditions);
            return this;
        }
        /// <inheritdoc cref="Event{T1}.Causes"/>
        public Event<T1, T2, T3, T4, T5> Causes(params Effect[] effects) {
            foreach (var e in effects) e.GenerateCode(DefaultGoal);
            return this;
        }

        /// <summary>Make a goal from this predicate with the specified argument values.</summary>
        /// <param name="arg1">Value to check for in column one of the table.</param>
        /// <param name="arg2">Value to check for in column two of the table.</param>
        /// <param name="arg3">Value to check for in column three of the table.</param>
        /// <param name="arg4">Value to check for in column four of the table.</param>
        /// <param name="arg5">Value to check for in column five of the table.</param>
        public new EventGoal this[Term<T1> arg1, Term<T2> arg2, Term<T3> arg3, Term<T4> arg4, Term<T5> arg5] =>
            new(this, arg1, arg2, arg3, arg4, arg5);

        /// <inheritdoc cref="Event{T1}.EventGoal"/>
        public class EventGoal : TableGoal<T1, T2, T3, T4, T5> {
            // ReSharper disable once SuggestBaseTypeForParameterInConstructor
            /// <summary><inheritdoc cref="Event{T1,T2}.EventGoal(Event{T1,T2},Term{T1},Term{T2})"/></summary>
            /// <param name="e">Event to apply arguments to.</param>
            /// <param name="arg1">Argument to apply to event (column one).</param>
            /// <param name="arg2">Argument to apply to event (column two).</param>
            /// <param name="arg3">Argument to apply to event (column three).</param>
            /// <param name="arg4">Argument to apply to event (column four).</param>
            /// <param name="arg5">Argument to apply to event (column five).</param>
            public EventGoal(Event<T1, T2, T3, T4, T5> e, Term<T1> arg1, Term<T2> arg2,
                             Term<T3> arg3, Term<T4> arg4, Term<T5> arg5) :
                base(e, arg1, arg2, arg3, arg4, arg5) { }

            /// <inheritdoc cref="Event{T1}.EventGoal.At"/>
            public Goal At(Term<TimePoint> time) =>
                ((Event<T1, T2, T3, T4, T5>)TablePredicate).Chronicle[
                    Arg1, Arg2, Arg3, Arg4, Arg5, time];
        }
    }

    /// <summary>
    /// A 6-argument event (table predicate). When used as intended - with OccursWhen calls - this is an
    /// intensional/derived table and as such will have it's contents recomputed every tick.
    /// </summary>
    /// <typeparam name="T1">Type of the first argument (column one)</typeparam>
    /// <typeparam name="T2">Type of the second argument (column two)</typeparam>
    /// <typeparam name="T3">Type of the third argument (column three)</typeparam>
    /// <typeparam name="T4">Type of the fourth argument (column four)</typeparam>
    /// <typeparam name="T5">Type of the fifth argument (column five)</typeparam>
    /// <typeparam name="T6">Type of the sixth argument (column six)</typeparam>
    public class Event<T1, T2, T3, T4, T5, T6> : TablePredicate<T1, T2, T3, T4, T5, T6> {
        /// <summary><inheritdoc cref="Event{T1}(string,IColumnSpec{T1})"/></summary>
        /// <param name="name">Name of the Event</param>
        /// <param name="arg1">Default variable of the first argument</param>
        /// <param name="arg2">Default variable of the second argument</param>
        /// <param name="arg3">Default variable of the third argument</param>
        /// <param name="arg4">Default variable of the fourth argument</param>
        /// <param name="arg5">Default variable of the fifth argument</param>
        /// <param name="arg6">Default variable of the sixth argument</param>
        public Event(string name, IColumnSpec<T1> arg1, IColumnSpec<T2> arg2, IColumnSpec<T3> arg3,
                     IColumnSpec<T4> arg4, IColumnSpec<T5> arg5, IColumnSpec<T6> arg6) :
            base(name, arg1, arg2, arg3, arg4, arg5, arg6) {
            Arg1 = arg1;
            Arg2 = arg2;
            Arg3 = arg3;
            Arg4 = arg4;
            Arg5 = arg5;
            Arg6 = arg6;
        }

        internal readonly IColumnSpec<T1> Arg1;
        internal readonly IColumnSpec<T2> Arg2;
        internal readonly IColumnSpec<T3> Arg3;
        internal readonly IColumnSpec<T4> Arg4;
        internal readonly IColumnSpec<T5> Arg5;
        internal readonly IColumnSpec<T6> Arg6;

        private EventChronicle<T1, T2, T3, T4, T5, T6>? _chronicle;

        // ReSharper disable once MemberCanBePrivate.Global
        /// <inheritdoc cref="Event{T1}.Chronicle"/>
        public EventChronicle<T1, T2, T3, T4, T5, T6> Chronicle {
            get {
                _chronicle ??= new EventChronicle<T1, T2, T3, T4, T5, T6>(this, time);
                return _chronicle;
            }
        }

        /// <inheritdoc cref="Event{T1}.OccursWhen"/>
        public Event<T1, T2, T3, T4, T5, T6> OccursWhen(params Goal[] conditions) {
            If(conditions);
            return this;
        }
        /// <inheritdoc cref="Event{T1}.Causes"/>
        public Event<T1, T2, T3, T4, T5, T6> Causes(params Effect[] effects) {
            foreach (var e in effects) e.GenerateCode(DefaultGoal);
            return this;
        }

        /// <summary>Make a goal from this predicate with the specified argument values.</summary>
        /// <param name="arg1">Value to check for in column one of the table.</param>
        /// <param name="arg2">Value to check for in column two of the table.</param>
        /// <param name="arg3">Value to check for in column three of the table.</param>
        /// <param name="arg4">Value to check for in column four of the table.</param>
        /// <param name="arg5">Value to check for in column five of the table.</param>
        /// <param name="arg6">Value to check for in column six of the table.</param>
        public new EventGoal this[Term<T1> arg1, Term<T2> arg2, Term<T3> arg3, Term<T4> arg4,
                                  Term<T5> arg5, Term<T6> arg6] =>
            new(this, arg1, arg2, arg3, arg4, arg5, arg6);

        /// <inheritdoc cref="Event{T1}.EventGoal"/>
        public class EventGoal : TableGoal<T1, T2, T3, T4, T5, T6> {
            // ReSharper disable once SuggestBaseTypeForParameterInConstructor
            /// <summary><inheritdoc cref="Event{T1,T2}.EventGoal(Event{T1,T2},Term{T1},Term{T2})"/></summary>
            /// <param name="e">Event to apply arguments to.</param>
            /// <param name="arg1">Argument to apply to event (column one).</param>
            /// <param name="arg2">Argument to apply to event (column two).</param>
            /// <param name="arg3">Argument to apply to event (column three).</param>
            /// <param name="arg4">Argument to apply to event (column four).</param>
            /// <param name="arg5">Argument to apply to event (column five).</param>
            /// <param name="arg6">Argument to apply to event (column six).</param>
            public EventGoal(Event<T1, T2, T3, T4, T5, T6> e, Term<T1> arg1, Term<T2> arg2,
                             Term<T3> arg3, Term<T4> arg4, Term<T5> arg5, Term<T6> arg6) :
                base(e, arg1, arg2, arg3, arg4, arg5, arg6) { }

            /// <inheritdoc cref="Event{T1}.EventGoal.At"/>
            public Goal At(Term<TimePoint> time) =>
                ((Event<T1, T2, T3, T4, T5, T6>)TablePredicate).Chronicle[
                    Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, time];
        }
    }

    /// <summary>
    /// A 7-argument event (table predicate). When used as intended - with OccursWhen calls - this is an
    /// intensional/derived table and as such will have it's contents recomputed every tick.
    /// </summary>
    /// <typeparam name="T1">Type of the first argument (column one)</typeparam>
    /// <typeparam name="T2">Type of the second argument (column two)</typeparam>
    /// <typeparam name="T3">Type of the third argument (column three)</typeparam>
    /// <typeparam name="T4">Type of the fourth argument (column four)</typeparam>
    /// <typeparam name="T5">Type of the fifth argument (column five)</typeparam>
    /// <typeparam name="T6">Type of the sixth argument (column six)</typeparam>
    /// <typeparam name="T7">Type of the seventh argument (column seven)</typeparam>
    public class Event<T1, T2, T3, T4, T5, T6, T7> : TablePredicate<T1, T2, T3, T4, T5, T6, T7> {
        /// <summary><inheritdoc cref="Event{T1}(string,IColumnSpec{T1})"/></summary>
        /// <param name="name">Name of the Event</param>
        /// <param name="arg1">Default variable of the first argument</param>
        /// <param name="arg2">Default variable of the second argument</param>
        /// <param name="arg3">Default variable of the third argument</param>
        /// <param name="arg4">Default variable of the fourth argument</param>
        /// <param name="arg5">Default variable of the fifth argument</param>
        /// <param name="arg6">Default variable of the sixth argument</param>
        /// <param name="arg7">Default variable of the seventh argument</param>
        public Event(string name, IColumnSpec<T1> arg1, IColumnSpec<T2> arg2, IColumnSpec<T3> arg3,
                     IColumnSpec<T4> arg4, IColumnSpec<T5> arg5, IColumnSpec<T6> arg6, IColumnSpec<T7> arg7) :
            base(name, arg1, arg2, arg3, arg4, arg5, arg6, arg7) {
            Arg1 = arg1;
            Arg2 = arg2;
            Arg3 = arg3;
            Arg4 = arg4;
            Arg5 = arg5;
            Arg6 = arg6;
            Arg7 = arg7;
        }

        internal readonly IColumnSpec<T1> Arg1;
        internal readonly IColumnSpec<T2> Arg2;
        internal readonly IColumnSpec<T3> Arg3;
        internal readonly IColumnSpec<T4> Arg4;
        internal readonly IColumnSpec<T5> Arg5;
        internal readonly IColumnSpec<T6> Arg6;
        internal readonly IColumnSpec<T7> Arg7;

        private EventChronicle<T1, T2, T3, T4, T5, T6, T7>? _chronicle;
        
        // ReSharper disable once MemberCanBePrivate.Global
        /// <inheritdoc cref="Event{T1}.Chronicle"/>
        public EventChronicle<T1, T2, T3, T4, T5, T6, T7> Chronicle {
            get {
                _chronicle ??= new EventChronicle<T1, T2, T3, T4, T5, T6, T7>(this, time);
                return _chronicle;
            }
        }

        /// <inheritdoc cref="Event{T1}.OccursWhen"/>
        public Event<T1, T2, T3, T4, T5, T6, T7> OccursWhen(params Goal[] conditions) {
            If(conditions);
            return this;
        }
        /// <inheritdoc cref="Event{T1}.Causes"/>
        public Event<T1, T2, T3, T4, T5, T6, T7> Causes(params Effect[] effects) {
            foreach (var e in effects) e.GenerateCode(DefaultGoal);
            return this;
        }

        /// <summary>Make a goal from this predicate with the specified argument values.</summary>
        /// <param name="arg1">Value to check for in column one of the table.</param>
        /// <param name="arg2">Value to check for in column two of the table.</param>
        /// <param name="arg3">Value to check for in column three of the table.</param>
        /// <param name="arg4">Value to check for in column four of the table.</param>
        /// <param name="arg5">Value to check for in column five of the table.</param>
        /// <param name="arg6">Value to check for in column six of the table.</param>
        /// <param name="arg7">Value to check for in column seven of the table.</param>
        public new EventGoal this[Term<T1> arg1, Term<T2> arg2, Term<T3> arg3, Term<T4> arg4,
                                  Term<T5> arg5, Term<T6> arg6, Term<T7> arg7] => 
            new(this, arg1, arg2, arg3, arg4, arg5, arg6, arg7);

        /// <inheritdoc cref="Event{T1}.EventGoal"/>
        public class EventGoal : TableGoal<T1, T2, T3, T4, T5, T6, T7> {
            // ReSharper disable once SuggestBaseTypeForParameterInConstructor
            /// <summary><inheritdoc cref="Event{T1,T2}.EventGoal(Event{T1,T2},Term{T1},Term{T2})"/></summary>
            /// <param name="e">Event to apply arguments to.</param>
            /// <param name="arg1">Argument to apply to event (column one).</param>
            /// <param name="arg2">Argument to apply to event (column two).</param>
            /// <param name="arg3">Argument to apply to event (column three).</param>
            /// <param name="arg4">Argument to apply to event (column four).</param>
            /// <param name="arg5">Argument to apply to event (column five).</param>
            /// <param name="arg6">Argument to apply to event (column six).</param>
            /// <param name="arg7">Argument to apply to event (column seven).</param>
            public EventGoal(Event<T1, T2, T3, T4, T5, T6, T7> e, Term<T1> arg1, Term<T2> arg2, 
                             Term<T3> arg3, Term<T4> arg4, Term<T5> arg5, Term<T6> arg6, Term<T7> arg7) : 
                base(e, arg1, arg2, arg3, arg4, arg5, arg6, arg7) { }

            /// <inheritdoc cref="Event{T1}.EventGoal.At"/>
            public Goal At(Term<TimePoint> time) =>
                ((Event<T1, T2, T3, T4, T5, T6, T7>)TablePredicate).Chronicle[
                    Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, time];
        }
    }
}
