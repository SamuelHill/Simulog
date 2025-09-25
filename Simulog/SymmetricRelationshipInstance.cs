using System;
using TED;
using TED.Primitives;

namespace Simulog {
    public class SymmetricRelationshipInstance<T> : IComparable<SymmetricRelationshipInstance<T>>, IEquatable<SymmetricRelationshipInstance<T>> 
        where T : IComparable<T>, IEquatable<T> {
        public readonly T Main;
        public readonly T Other;

        private SymmetricRelationshipInstance(T main, T other) {
            if (main.CompareTo(other) < 0) {
                Main = main;
                Other = other;
            } else {
                Main = other;
                Other = main;
            }
        }

        internal static SymmetricRelationshipInstance<T> NewRelationshipInstanceFunc(T main, T other) => new(main, other);
        public static Function<T, T, SymmetricRelationshipInstance<T>> NewRelationshipInstance =>
            new(nameof(NewRelationshipInstance), NewRelationshipInstanceFunc) {NameForCompilation = nameof(NewRelationshipInstanceFunc)};
        
        internal static bool RelationshipInstanceEqualsTupleFunc(SymmetricRelationshipInstance<T> symPairInst, 
                                                                 SymmetricTuple<T> symPair) => symPairInst.Equals(symPair);
        public static PrimitiveTest<SymmetricRelationshipInstance<T>, SymmetricTuple<T>> RelationshipInstanceEqualsTuple =>
            new(nameof(RelationshipInstanceEqualsTuple), RelationshipInstanceEqualsTupleFunc) 
                {CompilationName = nameof(RelationshipInstanceEqualsTupleFunc)};

        // *************************** Compare and Equality interfacing ***************************
        public int CompareTo(SymmetricRelationshipInstance<T> other) {
            var mainCompareTo = Main.CompareTo(other.Main);
            return mainCompareTo != 0 ? mainCompareTo : Other.CompareTo(other.Other);
        }
        
        public static bool operator ==(SymmetricRelationshipInstance<T> main, SymmetricRelationshipInstance<T> other) => 
            main is not null && main.Equals(other);
        public static bool operator !=(SymmetricRelationshipInstance<T> main, SymmetricRelationshipInstance<T> other) => !(main == other);
        public static bool operator ==(SymmetricRelationshipInstance<T> pairInstance, SymmetricTuple<T> pair) => 
            pairInstance is not null && pairInstance.Equals(pair);
        public static bool operator !=(SymmetricRelationshipInstance<T> pairInstance, SymmetricTuple<T> pair) => !(pairInstance == pair);
        
        public bool Equals(SymmetricRelationshipInstance<T> other) => other is not null && ReferenceEquals(this, other);
        private bool Equals(SymmetricTuple<T> pair) => Main.Equals(pair.Item1) && Other.Equals(pair.Item2);
        public override bool Equals(object obj) => obj is not null && ReferenceEquals(this, obj);
        public override int GetHashCode() => HashCode.Combine(Main, Other);

        // ****************************************************************************************

        public override string ToString() => $"{Main}, {Other}";
    }
}
