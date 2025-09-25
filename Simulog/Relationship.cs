using System;
using TED;
using TED.Interpreter;
using TED.Primitives;
using static TED.Language;

namespace Simulog {
    using static SimuLang;

    // ReSharper disable MemberCanBePrivate.Global
    public class Relationship<T1, T2> : TablePredicate<T1, T2, bool> 
        where T1 : IComparable<T1>, IEquatable<T1> where T2 : IComparable<T2>, IEquatable<T2> {
        public readonly Event<T1, T2> Start;
        public readonly Event<T1, T2> End;

        public Relationship(string name, Var<T1> main, Var<T2> other, Var<bool> state) :
            base(name, main.JointKey, other.JointKey, state.Indexed) {
            Start = Event($"{name}Start", main, other);
            End = Event($"{name}End", main, other);

            Add[main, other, true].If(Start, !this[main, other, __]);
            Set((main, other), state, false).If(End, this[main, other, true]);
            Set((main, other), state, true).If(Start, this[main, other, false]);
            // this.Colorize(state);
        }

        public Relationship<T1, T2> StartWhen(params Goal[] conditions) {
            Start.OccursWhen(conditions);
            return this;
        }
        public Relationship<T1, T2> StartCauses(params Effect[] effects) {
            Start.Causes(effects);
            return this;
        }

        public Relationship<T1, T2> EndWhen(params Goal[] conditions) {
            End.OccursWhen(conditions);
            return this;
        }
        public Relationship<T1, T2> EndCauses(params Effect[] effects) {
            End.Causes(effects);
            return this;
        }

        private RelationshipChronicle<T1, T2> _chronicle;
        
        public RelationshipChronicle<T1, T2> Chronicle {
            get {
                _chronicle ??= new RelationshipChronicle<T1, T2>(Name + "Chronicle", 
                    (Var<T1>)DefaultVariables[1], (Var<T2>)DefaultVariables[2]);
                _chronicle.StartWhen(Start);
                _chronicle.EndWhen(End);
                return _chronicle;
            }
        }

        public TableGoal<T1, T2, bool> this[Term<T1> main, Term<T2> other] => new(this, main, other, true);
    }
    
    public class ManyToOneRelationship<T1, T2>: TablePredicate<T1, T2, bool> 
        where T1 : IComparable<T1>, IEquatable<T1> where T2 : IComparable<T2>, IEquatable<T2> {
        public readonly Event<T1, T2> Start;
        public readonly Event<T1, T2> End;

        private readonly Definition<T1, T2> _relationshipFilterManyToOneStarts;

        public ManyToOneRelationship(string name, Var<T1> main, Var<T2> other, Var<bool> state) :
            base(name, main.JointKey, other.JointKey, state.Indexed) {
            Start = Event($"{name}Start", main, other);
            End = Event($"{name}End", main, other);
            var tempArg = (Var<T2>)"tempArg";
            _relationshipFilterManyToOneStarts = Definition("RelationshipFilterManyToOne", main, other).Is(
                tempArg != other, !this[main, other, true], !this[main, tempArg, true]);
            Add[main, other, true].If(Start, !this[main, other, __]);
            Set((main, other), state, false).If(End, this[main, other, true]);
            Set((main, other), state, true).If(Start, this[main, other, false]);
            // this.Colorize(state);
        }

        public ManyToOneRelationship<T1, T2> StartWhen(params Goal[] conditions) {
            Start.OccursWhen(And[conditions], _relationshipFilterManyToOneStarts);
            return this;
        }
        public ManyToOneRelationship<T1, T2> StartCauses(params Effect[] effects) {
            Start.Causes(effects);
            return this;
        }

        public ManyToOneRelationship<T1, T2> EndWhen(params Goal[] conditions) {
            End.OccursWhen(conditions);
            return this;
        }
        public ManyToOneRelationship<T1, T2> EndCauses(params Effect[] effects) {
            End.Causes(effects);
            return this;
        }

        private RelationshipChronicle<T1, T2> _chronicle;
        
        public RelationshipChronicle<T1, T2> Chronicle {
            get {
                _chronicle ??= new RelationshipChronicle<T1, T2>(Name + "Chronicle", 
                                                                 (Var<T1>)DefaultVariables[1], (Var<T2>)DefaultVariables[2]);
                _chronicle.StartWhen(Start);
                _chronicle.EndWhen(End);
                return _chronicle;
            }
        }

        public TableGoal<T1, T2, bool> this[Term<T1> main, Term<T2> other] => new(this, main, other, true);
    }
    
    public class OneToOneRelationship<T1, T2>: TablePredicate<T1, T2, bool> 
        where T1 : IComparable<T1>, IEquatable<T1> where T2 : IComparable<T2>, IEquatable<T2> {
        public readonly Event<T1, T2> Start;
        public readonly Event<T1, T2> End;

        private readonly Definition<T1, T2> _relationshipFilterOneToOneStarts;

        public OneToOneRelationship(string name, Var<T1> main, Var<T2> other, Var<bool> state) :
            base(name, main.JointKey, other.JointKey, state.Indexed) {
            Start = Event($"{name}Start", main, other);
            End = Event($"{name}End", main, other);
            var tempArg1 = (Var<T1>)"tempArg1";
            var tempArg2 = (Var<T2>)"tempArg2";
            _relationshipFilterOneToOneStarts = Definition("RelationshipFilterOneToOne", main, other).Is(
                tempArg1 != main, tempArg2 != other, !this[main, other, true], !this[main, tempArg2, true], !this[tempArg1, other, true]);
            Add[main, other, true].If(Start, !this[main, other, __]);
            Set((main, other), state, false).If(End, this[main, other, true]);
            Set((main, other), state, true).If(Start, this[main, other, false]);
            // this.Colorize(state);
        }

        public OneToOneRelationship<T1, T2> StartWhen(params Goal[] conditions) {
            Start.OccursWhen(And[conditions], _relationshipFilterOneToOneStarts);
            return this;
        }
        public OneToOneRelationship<T1, T2> StartCauses(params Effect[] effects) {
            Start.Causes(effects);
            return this;
        }

        public OneToOneRelationship<T1, T2> EndWhen(params Goal[] conditions) {
            End.OccursWhen(conditions);
            return this;
        }
        public OneToOneRelationship<T1, T2> EndCauses(params Effect[] effects) {
            End.Causes(effects);
            return this;
        }

        private RelationshipChronicle<T1, T2> _chronicle;
        
        public RelationshipChronicle<T1, T2> Chronicle {
            get {
                _chronicle ??= new RelationshipChronicle<T1, T2>(Name + "Chronicle", 
                                                                 (Var<T1>)DefaultVariables[1], (Var<T2>)DefaultVariables[2]);
                _chronicle.StartWhen(Start);
                _chronicle.EndWhen(End);
                return _chronicle;
            }
        }

        public TableGoal<T1, T2, bool> this[Term<T1> main, Term<T2> other] => new(this, main, other, true);
    }

    public class AffinityRelationship<T1, T2> : TablePredicate<T1, T2, bool> 
        where T1 : IComparable<T1>, IEquatable<T1> where T2 : IComparable<T2>, IEquatable<T2> {
        public readonly Event<T1, T2> Start;
        public readonly Event<T1, T2> End;

        internal static bool StartTransitionPointTest(int t, int value) => t >= 0 ? value > t : value < t;
        private static PrimitiveTest<int, int> StartTransitionPoint => 
            new(nameof(StartTransitionPoint), StartTransitionPointTest) {CompilationName = nameof(StartTransitionPointTest)};
        internal static bool EndTransitionPointTest(int t, int value) => t >= 0 ? value < t : value > t;
        private static PrimitiveTest<int, int> EndTransitionPoint =>
            new(nameof(EndTransitionPoint), EndTransitionPointTest) {CompilationName = nameof(EndTransitionPointTest)};

        public AffinityRelationship(string name, Affinity<T1, T2> affinity, Var<bool> state, int start, int end) :
            base(name, ((Var<T1>)affinity.DefaultVariables[1]).JointKey, 
                 ((Var<T2>)affinity.DefaultVariables[2]).JointKey, state.Indexed) {
            var main = (Var<T1>)affinity.DefaultVariables[1];
            var other = (Var<T2>)affinity.DefaultVariables[2];
            var value = (Var<int>)affinity.DefaultVariables[3];

            Add[main, other, true].If(affinity[__, main, other, value], 
                StartTransitionPoint[start, value], !this[main, other, __]);
            Set((main, other), state, false).If(affinity[__, main, other, value], 
                EndTransitionPoint[end, value], this[main, other, true]);
            Set((main, other), state, true).If(affinity[__, main, other, value], 
                StartTransitionPoint[start, value], this[main, other, false]);

            Start = Event($"{name}Start", main, other);
            Start.OccursWhen(Add);
            Start.OccursWhen(Set((main, other), state, true), this);
            End = Event($"{name}End", main, other);
            End.OccursWhen(Set((main, other), state, false), this);
            // this.Colorize(state);
        }

        public AffinityRelationship<T1, T2> StartCauses(params Effect[] effects) {
            Start.Causes(effects);
            return this;
        }
        public AffinityRelationship<T1, T2> EndCauses(params Effect[] effects) {
            End.Causes(effects);
            return this;
        }

        private RelationshipChronicle<T1, T2> _chronicle;
        
        public RelationshipChronicle<T1, T2> Chronicle {
            get {
                _chronicle ??= new RelationshipChronicle<T1, T2>(Name + "Chronicle", 
                    (Var<T1>)DefaultVariables[1], (Var<T2>)DefaultVariables[2]);
                _chronicle.StartWhen(Start);
                _chronicle.EndWhen(End);
                return _chronicle;
            }
        }

        public TableGoal<T1, T2, bool> this[Term<T1> main, Term<T2> other] => new(this, main, other, true);
    }

    public class FloatAffinityRelationship<T1, T2> : TablePredicate<T1, T2, bool> 
        where T1 : IComparable<T1>, IEquatable<T1> where T2 : IComparable<T2>, IEquatable<T2> {
        public readonly Event<T1, T2> Start;
        public readonly Event<T1, T2> End;

        internal static bool StartTransitionPointTest(float t, float value) => t >= 0 ? value > t : value < t;
        private static PrimitiveTest<float, float> StartTransitionPoint => 
            new(nameof(StartTransitionPoint), StartTransitionPointTest) {CompilationName = nameof(StartTransitionPointTest)};
        internal static bool EndTransitionPointTest(float t, float value) => t >= 0 ? value < t : value > t;
        private static PrimitiveTest<float, float> EndTransitionPoint =>
            new(nameof(EndTransitionPoint), EndTransitionPointTest) {CompilationName = nameof(EndTransitionPointTest)};

        public FloatAffinityRelationship(string name, FloatAffinity<T1, T2> affinity, Var<bool> state, float start, float end) :
            base(name, ((Var<T1>)affinity.DefaultVariables[1]).JointKey, 
                 ((Var<T2>)affinity.DefaultVariables[2]).JointKey, state.Indexed) {
            var main = (Var<T1>)affinity.DefaultVariables[1];
            var other = (Var<T2>)affinity.DefaultVariables[2];
            var value = (Var<float>)affinity.DefaultVariables[3];

            Add[main, other, true].If(affinity[__, main, other, value], 
                StartTransitionPoint[start, value], !this[main, other, __]);
            Set((main, other), state, false).If(affinity[__, main, other, value], 
                EndTransitionPoint[end, value], this[main, other, true]);
            Set((main, other), state, true).If(affinity[__, main, other, value], 
                StartTransitionPoint[start, value], this[main, other, false]);

            Start = Event($"{name}Start", main, other);
            Start.OccursWhen(Add);
            Start.OccursWhen(Set((main, other), state, true), this);
            End = Event($"{name}End", main, other);
            End.OccursWhen(Set((main, other), state, false), this);
            // this.Colorize(state);
        }

        public FloatAffinityRelationship<T1, T2> StartCauses(params Effect[] effects) {
            Start.Causes(effects);
            return this;
        }
        public FloatAffinityRelationship<T1, T2> EndCauses(params Effect[] effects) {
            End.Causes(effects);
            return this;
        }

        private RelationshipChronicle<T1, T2> _chronicle;
        
        public RelationshipChronicle<T1, T2> Chronicle {
            get {
                _chronicle ??= new RelationshipChronicle<T1, T2>(Name + "Chronicle", 
                    (Var<T1>)DefaultVariables[1], (Var<T2>)DefaultVariables[2]);
                _chronicle.StartWhen(Start);
                _chronicle.EndWhen(End);
                return _chronicle;
            }
        }

        public TableGoal<T1, T2, bool> this[Term<T1> main, Term<T2> other] => new(this, main, other, true);
    }
    
    public class GenericAffinityRelationship<T1, T2, T3> : TablePredicate<T1, T2, bool> 
        where T1 : IComparable<T1>, IEquatable<T1> where T2 : IComparable<T2>, IEquatable<T2> where T3 : IComparable<T3>, IEquatable<T3> {
        public readonly Event<T1, T2> Start;
        public readonly Event<T1, T2> End;

        internal static bool TransitionOnEquals(T3 t, T3 value) => t.Equals(value);
        private static PrimitiveTest<T3, T3> StartTransitionPoint => 
            new(nameof(StartTransitionPoint), TransitionOnEquals) {CompilationName = nameof(TransitionOnEquals)};
        private static PrimitiveTest<T3, T3> EndTransitionPoint =>
            new(nameof(EndTransitionPoint), TransitionOnEquals) {CompilationName = nameof(TransitionOnEquals)};

        public GenericAffinityRelationship(string name, GenericAffinity<T1, T2, T3> affinity, Var<bool> state, T3 start, T3 end) :
            base(name, ((Var<T1>)affinity.DefaultVariables[1]).JointKey, 
                 ((Var<T2>)affinity.DefaultVariables[2]).JointKey, state.Indexed) {
            var main = (Var<T1>)affinity.DefaultVariables[1];
            var other = (Var<T2>)affinity.DefaultVariables[2];
            var value = (Var<T3>)affinity.DefaultVariables[3];

            Add[main, other, true].If(affinity[__, main, other, value], 
                StartTransitionPoint[start, value], !this[main, other, __]);
            Set((main, other), state, false).If(affinity[__, main, other, value], 
                EndTransitionPoint[end, value], this[main, other, true]);
            Set((main, other), state, true).If(affinity[__, main, other, value], 
                StartTransitionPoint[start, value], this[main, other, false]);

            Start = Event($"{name}Start", main, other);
            Start.OccursWhen(Add);
            Start.OccursWhen(Set((main, other), state, true), this);
            End = Event($"{name}End", main, other);
            End.OccursWhen(Set((main, other), state, false), this);
            // this.Colorize(state);
        }

        public GenericAffinityRelationship<T1, T2, T3> StartCauses(params Effect[] effects) {
            Start.Causes(effects);
            return this;
        }
        public GenericAffinityRelationship<T1, T2, T3> EndCauses(params Effect[] effects) {
            End.Causes(effects);
            return this;
        }

        private RelationshipChronicle<T1, T2> _chronicle;
        
        public RelationshipChronicle<T1, T2> Chronicle {
            get {
                _chronicle ??= new RelationshipChronicle<T1, T2>(Name + "Chronicle", 
                    (Var<T1>)DefaultVariables[1], (Var<T2>)DefaultVariables[2]);
                _chronicle.StartWhen(Start);
                _chronicle.EndWhen(End);
                return _chronicle;
            }
        }

        public TableGoal<T1, T2, bool> this[Term<T1> main, Term<T2> other] => new(this, main, other, true);
    }
}
