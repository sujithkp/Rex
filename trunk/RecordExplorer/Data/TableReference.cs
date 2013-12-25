
namespace RecordExplorer
{
    public class TableReferenceInfo
    {
        public TableReferenceInfo(string table, string column, string referenceTable,
            string referenceColumn, string constraintname)
        {
            this.Table = table;
            this.Column = column;
            this.ReferenceTable = referenceTable;
            this.ReferenceColumn = referenceColumn;
            this.ConstraintName = constraintname;
        }

        public string Table { get; private set; }

        public string Column { get; private set; }

        public string ReferenceTable { get; private set; }

        public string ReferenceColumn { get; private set; }

        public string ConstraintName { get; private set; }
    }
}
