using System;
using Time;
using TED;
using TED.Interpreter;
using TED.Tables;
using static TED.Language;

namespace Simulog {
    using static Variables;
    using static Clock;
    using static TimePoint;
    
    #if RELEASE_INTEGER_TIME
    
    public class Timer<T1> : TablePredicate<T1, uint, bool> {
        // Should these be tables or...?
        public readonly Definition<T1> NotOnTimer;
        public readonly Definition<T1> TimerFinished;
        public readonly GeneralIndex<(T1, uint, bool), bool> TimerStateIndex;

        private readonly Var<T1> _arg1;
        
        public Timer(string name, Var<T1> arg1) : base(name, arg1.Key, clockTicks, state.Indexed) {
            Overwrite = true;
            // Suppress nullable on IndexFor return type/casting as this table should always have an index
            // for the state column given the constructor base call above.
            // ReSharper disable once NullableWarningSuppressionIsUsed
            TimerStateIndex = (GeneralIndex<(T1, uint, bool), bool>)IndexFor(state, false)!;
            _arg1 = arg1.TypedVariable;
            NotOnTimer = Definition($"NotOn{name}Timer", _arg1).Is(!this[_arg1, __, true]);
            Set(_arg1, clockTicks, DecrementByLastTick[clockTicks]).If(this, clockTicks <= 0);
            Set(_arg1, state, false).If(this[arg1.TypedVariable, 0, true]);
            TimerFinished = Definition($"{name}TimerFinished", _arg1).Is(Set(_arg1, state)[_arg1, false]);
        }
        
        // ReSharper disable once UnusedMethodReturnValue.Global
        public Timer<T1> StartWhen(params Goal[] goals) {
            Add[_arg1, clockTicks, true].If(goals);
            return this;
        }
    }
    
    #endif
    
    #if RELEASE_FLOATING_POINT_TIME
    
    public class Timer<T1> : TablePredicate<T1, float, bool> {
        // Should these be tables or...?
        public readonly Definition<T1> NotOnTimer;
        public readonly Definition<T1> TimerFinished;
        public readonly GeneralIndex<(T1, float, bool), bool> TimerStateIndex;

        private readonly Var<T1> _arg1;
        
        public Timer(string name, Var<T1> arg1) : base(name, arg1.Key, clockTicks, state.Indexed) {
            Overwrite = true;
            // Suppress nullable on IndexFor return type/casting as this table should always have an index
            // for the state column given the constructor base call above.
            // ReSharper disable once NullableWarningSuppressionIsUsed
            TimerStateIndex = (GeneralIndex<(T1, float, bool), bool>)IndexFor(state, false)!;
            _arg1 = arg1.TypedVariable;
            NotOnTimer = Definition($"NotOn{name}Timer", _arg1).Is(!this[_arg1, __, true]);
            Set(_arg1, clockTicks, DecrementByLastTick[clockTicks]).If(this, clockTicks <= 0f);
            Set(_arg1, state, false).If(this[arg1.TypedVariable, 0, true]);
            TimerFinished = Definition($"{name}TimerFinished", _arg1).Is(Set(_arg1, state)[_arg1, false]);
        }
        
        // ReSharper disable once UnusedMethodReturnValue.Global
        public Timer<T1> StartWhen(params Goal[] goals) {
            Add[_arg1, clockTicks, true].If(goals);
            return this;
        }
    }
    
    #endif
    
    public class Scheduled<T1> : TablePredicate<T1, TimePoint, bool> {
        // Should these be tables or...?
        public readonly Definition<T1> NotOnTimer;
        public readonly Definition<T1> TimerFinished;
        public readonly GeneralIndex<(T1, TimePoint, bool), bool> TimerStateIndex;
        
        private readonly Var<T1> _arg1;

        public Scheduled(string name, Var<T1> arg1) : base(name, arg1.Key, time, state.Indexed) {
            Overwrite = true;
            _arg1 = arg1.TypedVariable;
            // Suppress nullable on IndexFor return type/casting as this table should always have an index
            // for the state column given the constructor base call above.
            // ReSharper disable once NullableWarningSuppressionIsUsed
            TimerStateIndex = (GeneralIndex<(T1, TimePoint, bool), bool>)IndexFor(state, false)!;
            Set(_arg1, state, false).If(this[_arg1, time, true], CheckTimeWithinLastTick[time]);
            TimerFinished = Definition($"{name}TimerFinished", _arg1).Is(Set(_arg1, state)[_arg1, false]);
            NotOnTimer = Definition($"NotOn{name}Timer", _arg1).Is(!this[_arg1, __, true]);
        }
        
        // ReSharper disable once UnusedMethodReturnValue.Global
        public Scheduled<T1> StartWhen(params Goal[] goals) {
            Add[_arg1, time, true].If(And[goals], AddToTimePoint[CurrentTimePoint, clockTicks, time]);
            return this;
        }
    }
}