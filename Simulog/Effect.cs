using System;
using System.Linq;
using TED;
using TED.Interpreter;

namespace Simulog {

    /// <summary>Base class for effects of Event predicates</summary>
    public class Effect {
        private Goal[] _extraConditions = Array.Empty<Goal>();
        private readonly CodeGenerator _generator;

        private delegate void CodeGenerator(Effect effect, Goal eventCondition);

        private Effect(CodeGenerator generator) => _generator = generator;

        /// <summary>Calls underlying CodeGenerator function on the provided goal.</summary>
        /// <param name="goal">Conditions under which this effect should occur.</param>
        public void GenerateCode(TableGoal goal) => _generator(this, goal);

        /// <summary>Adds additional conditions to the effect occuring.</summary>
        /// <param name="extraConditions">Additional conditions under which the effect should occur.</param>
        /// <returns>The same Effect for method chaining.</returns>
        public Effect If(params Goal[] extraConditions) {
            _extraConditions = extraConditions;
            return this;
        }

        private Goal[] JoinExtraConditions(Goal g) => _extraConditions.Prepend(g).ToArray();

        /// <summary>Sets the "column" value for the row at "key" in table "t" to the "newValue".</summary>
        /// <param name="t">Table that this effect will set new values in.</param>
        /// <param name="key">Key of the table t that will be used to identify individual rows to update.</param>
        /// <param name="column">Location in table t of the value that will be updated.</param>
        /// <param name="newValue">New value to be set in a keys column.</param>
        /// <typeparam name="TKey">Type of the key column in table t.</typeparam>
        /// <typeparam name="TCol">Type of the column values with be updated in in table t.</typeparam>
        /// <returns>A set Effect that can be caused by various events.</returns>
        public static Effect Set<TKey, TCol>(TablePredicate t, Var<TKey> key, Var<TCol> column, Term<TCol> newValue) => 
            new((e, g) => { t.Set(key, column, newValue).If(e.JoinExtraConditions(g)); });

        /// <summary>
        /// Adds an entire "row" to a table using a table goal.
        /// (TableGoal must contain values for all columns in the table)
        /// </summary>
        /// <param name="row">TableGoal representing the new row to be added to the table.</param>
        /// <returns>An add Effect that can be caused by various events.</returns>
        public static Effect Add(TableGoal row) => new((e, g) => {
            row.TablePredicate.AddUntyped.GetGoal(row.Arguments).If(e.JoinExtraConditions(g));
        });
    }
}
