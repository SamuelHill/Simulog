using TED.Interpreter;
using static TED.Tables.IndexMode;

namespace Utilities {
    /// <summary> Helper methods for properly handling joint keys and indices. </summary>
    public static class ChronicleIndexing {
        /// <summary> Demotes a key to an index (while maintaining appropriate joins)</summary>
        /// <param name="columnSpec">Column to be demoted</param>
        /// <typeparam name="T">Type of the column</typeparam>
        /// <returns>Demoted ColumnSpec</returns>
        public static IColumnSpec<T> DemoteKey<T>(IColumnSpec<T> columnSpec) => 
            columnSpec.IndexMode == Key ? MaintainJoint(columnSpec) : columnSpec;

        private static IColumnSpec<T> MaintainJoint<T>(IColumnSpec<T> columnSpec) =>
            columnSpec.JointPartial ? columnSpec.TypedVariable.JointIndexed : columnSpec.TypedVariable.Indexed;
    }
}
