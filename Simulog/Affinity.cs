using System;
using TED;
using TED.Interpreter;
using Time;
using static TED.Language;

namespace Simulog {
    using static SimuLang;
    
    // ReSharper disable MemberCanBePrivate.Global
    public class Affinity<T1, T2> : TablePredicate<(T1, T2), T1, T2, int> 
        where T1 : IComparable<T1>, IEquatable<T1> where T2 : IComparable<T2>, IEquatable<T2> {
        // ReSharper disable StaticMemberInGenericType
        // ReSharper disable InconsistentNaming
        private static readonly Var<int> _setVal = (Var<int>)"setVal";
        private static readonly Var<int> _tempVal = (Var<int>)"tempVal";
        // ReSharper restore InconsistentNaming
        // ReSharper restore StaticMemberInGenericType

        public readonly Event<T1, T2, int> Changed;
        public readonly TablePredicate<(T1, T2)> Unchanged;

        internal static (T1, T2) NewTupleFunc(T1 main, T2 other) => (main, other);
        private Function<T1, T2, (T1, T2)> NewTuple => new($"New{Name}Tuple", NewTupleFunc) 
            {NameForCompilation = nameof(NewTupleFunc)};
        internal static int RegressToZeroImplementation(int value) => value == 0 ? 0 : value > 0 ? value - 1 : value + 1;
        private static Function<int, int> RegressToZero => new("RegressToZero", RegressToZeroImplementation) 
            {NameForCompilation = nameof(RegressToZeroImplementation)};

        public Affinity(string name, Var<(T1, T2)> pair, Var<T1> main, Var<T2> other, Var<int> value) :
            base(name, pair.Key, main.Indexed, other.Indexed, value) {
            Changed = Event($"{name}Changed", main.Indexed, other.Indexed, value);
            Add[pair, main, other, value].If(Changed, 
                !this[__, main, other, __], NewTuple[main, other, pair]);
            Set(pair, value, _setVal).If(Changed, 
                this[pair, main, other, _tempVal], _setVal == _tempVal + value);
            Unchanged = Predicate($"{name}Unchanged", pair).If(this, !Changed[main, other, __]);
        }

        public Affinity<T1, T2> Decay(float rate) {
            Set((Var<(T1, T2)>)DefaultVariables[0], (Var<int>)DefaultVariables[3], _setVal)
               .If(Unchanged, this, (Var<int>)DefaultVariables[3] != 0, 
                   Prob[rate], RegressToZero[(Var<int>)DefaultVariables[3], _setVal]);
            return this;
        }

        public Affinity<T1, T2> UpdateWhen(params Goal[] conditions) {
            Changed.OccursWhen(conditions);
            return this;
        }
        public Affinity<T1, T2> UpdateCauses(params Effect[] effects) {
            Changed.Causes(effects);
            return this;
        }

        public AffinityRelationship<T1, T2> Relationship(string name, Var<bool> state, int start, int end) => 
            new(name, this, state, start, end);
        
        public class AffinityGoal : TableGoal<(T1, T2), T1, T2, int> {
            public AffinityGoal(TablePredicate predicate, Term<(T1, T2)> pair,
                                Term<T1> main, Term<T2> other, Term<int> value)
                : base(predicate, pair, main, other, value) { }
        }
    }

    public class FloatAffinity<T1, T2> : TablePredicate<(T1, T2), T1, T2, float> 
        where T1 : IComparable<T1>, IEquatable<T1> where T2 : IComparable<T2>, IEquatable<T2> {
        // ReSharper disable StaticMemberInGenericType
        // ReSharper disable InconsistentNaming
        private static readonly Var<float> _setVal = (Var<float>)"setVal";
        private static readonly Var<float> _tempVal = (Var<float>)"tempVal";
        // ReSharper restore InconsistentNaming
        // ReSharper restore StaticMemberInGenericType

        public readonly Event<T1, T2, float> Change;
        public readonly TablePredicate<(T1, T2)> Unchanged;

        internal static (T1, T2) NewTupleFunc(T1 main, T2 other) => (main, other);
        private Function<T1, T2, (T1, T2)> NewTuple => new($"New{Name}Tuple", NewTupleFunc) 
            {NameForCompilation = nameof(NewTupleFunc)};
        internal static float RegressToZeroImplementation(float value) => value is < 1 and > -1 ? 0 : value > 0 ? value - 1 : value + 1;
        private static Function<float, float> RegressToZero => new("RegressToZero", RegressToZeroImplementation) 
            {NameForCompilation = nameof(RegressToZeroImplementation)};

        public FloatAffinity(string name, Var<(T1, T2)> pair, Var<T1> main, Var<T2> other, Var<float> value) :
            base(name, pair.Key, main.Indexed, other.Indexed, value) {
            Change = Event($"{name}Changed", main.Indexed, other.Indexed, value);
            Add[pair, main, other, value].If(Change, 
                !this[__, main, other, __], NewTuple[main, other, pair]);
            Set(pair, value, _setVal).If(Change, 
                this[pair, main, other, _tempVal], _setVal == _tempVal + value);
            Unchanged = Predicate($"{name}Unchanged", pair).If(this, !Change[main, other, __]);
        }

        public FloatAffinity<T1, T2> Decay(float rate) {
            Set((Var<(T1, T2)>)DefaultVariables[0], (Var<float>)DefaultVariables[3], _setVal)
               .If(Unchanged, this, (Var<float>)DefaultVariables[3] != 0,
                   Prob[rate], RegressToZero[(Var<float>)DefaultVariables[3], _setVal]);
            return this;
        }

        public FloatAffinity<T1, T2> UpdateWhen(params Goal[] conditions) {
            Change.OccursWhen(conditions);
            return this;
        }
        public FloatAffinity<T1, T2> UpdateCauses(params Effect[] effects) {
            Change.Causes(effects);
            return this;
        }

        public FloatAffinityRelationship<T1, T2> Relationship(string name, Var<bool> state, int start, int end) =>
            new(name, this, state, start, end);
        
        public class AffinityGoal : TableGoal<(T1, T2), T1, T2, float> {
            public AffinityGoal(TablePredicate predicate, Term<(T1, T2)> pair, 
                                Term<T1> main, Term<T2> other, Term<float> value)
                : base(predicate, pair, main, other, value) { }
        }
    }
    
    // ReSharper disable MemberCanBePrivate.Global
    public class GenericAffinity<T1, T2, T3> : TablePredicate<(T1, T2), T1, T2, T3> 
        where T1 : IComparable<T1>, IEquatable<T1> where T2 : IComparable<T2>, IEquatable<T2> where T3 : IComparable<T3>, IEquatable<T3> {
        // ReSharper disable StaticMemberInGenericType
        // ReSharper disable InconsistentNaming
        private static readonly Var<T3> _setVal = (Var<T3>)"setVal";
        // ReSharper restore InconsistentNaming
        // ReSharper restore StaticMemberInGenericType

        public readonly Event<T1, T2, T3> Changed;
        public readonly TablePredicate<(T1, T2)> Unchanged;
        
        internal static (T1, T2) NewTupleFunc(T1 main, T2 other) => (main, other);
        private Function<T1, T2, (T1, T2)> NewTuple => new($"New{Name}Tuple", NewTupleFunc) 
            {NameForCompilation = nameof(NewTupleFunc)};

        public GenericAffinity(string name, Var<(T1, T2)> pair, Var<T1> main, Var<T2> other, Var<T3> value) :
            base(name, pair.Key, main.Indexed, other.Indexed, value) {
            Changed = Event($"{name}Changed", main.Indexed, other.Indexed, value);
            Add[pair, main, other, value].If(Changed, 
                !this[__, main, other, __], NewTuple[main, other, pair]);
            Set(pair, value, _setVal).If(Changed, _setVal == value);
            Unchanged = Predicate($"{name}Unchanged", pair).If(this, !Changed[main, other, __]);
        }

        public GenericAffinity<T1, T2, T3> UpdateWhen(params Goal[] conditions) {
            Changed.OccursWhen(conditions);
            return this;
        }
        public GenericAffinity<T1, T2, T3> UpdateCauses(params Effect[] effects) {
            Changed.Causes(effects);
            return this;
        }

        public GenericAffinityRelationship<T1, T2, T3> Relationship(string name, Var<bool> state, T3 start, T3 end) => 
            new(name, this, state, start, end);
        
        public class AffinityGoal : TableGoal<(T1, T2), T1, T2, T3> {
            public AffinityGoal(TablePredicate predicate, Term<(T1, T2)> pair,
                                Term<T1> main, Term<T2> other, Term<T3> value)
                : base(predicate, pair, main, other, value) { }
        }
    }
}
