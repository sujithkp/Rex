using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using RecordExplorer.Data;

namespace RecordExplorer.Business
{
    public class InformationSchema
    {
        private string connectionString;
        private SqlConnection Connection;
        private string commandText = "SELECT f.name AS ForeignKey, OBJECT_NAME(f.parent_object_id) AS TableName, COL_NAME(fc.parent_object_id, fc.parent_column_id) AS ColumnName, OBJECT_NAME (f.referenced_object_id) AS ReferenceTableName, COL_NAME(fc.referenced_object_id, fc.referenced_column_id) AS ReferenceColumnName FROM sys.foreign_keys AS f INNER JOIN sys.foreign_key_columns AS fc ON f.OBJECT_ID = fc.constraint_object_id";
        private string primarykeyquery = "SELECT * FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE WHERE OBJECTPROPERTY(OBJECT_ID(constraint_name), 'IsPrimaryKey') = 1";

        private Dictionary<string, List<string>> primaryKeys;
        private List<TableReferenceInfo> tableReferences;

        private static InformationSchema _instance;

        private List<string> tables = null;

        private InformationSchema()
        {
            //TODO: I should get this from somewhere else.
            var connectionString = "Data Source=.\\sqlexpress; Initial Catalog = flexilineweb;Integrated Security =SSPI";

            this.Connection = new SqlConnection(this.connectionString = connectionString);
            Initialize();
        }

        private void Initialize()
        {
            this.Connection.Open();
            var command = new SqlCommand(this.commandText, this.Connection);
            var reader = command.ExecuteReader();

            tableReferences = new List<TableReferenceInfo>();

            while (reader.Read())
            {
                var fkeyname = reader["ForeignKey"] as string;
                var tableName = reader["TableName"] as string;

                var colName = reader["ColumnName"] as string;
                var reftableName = reader["ReferenceTableName"] as string;
                var refcolName = reader["ReferenceColumnName"] as string;

                tableReferences.Add(new TableReferenceInfo(tableName, colName, reftableName, refcolName, fkeyname));
            }

            reader.Close();
            this.Connection.Close();
        }

        public string GetPrimaryKey(string table)
        {
            if (primaryKeys != null)
                return primaryKeys[table].First();

            this.Connection.Open();
            var command = new SqlCommand(this.primarykeyquery, this.Connection);
            var reader = command.ExecuteReader();

            primaryKeys = new Dictionary<string, List<string>>();

            while (reader.Read())
            {
                var tablename = reader["TABLE_NAME"] as string;
                var column = reader["COLUMN_NAME"] as string;

                if (!primaryKeys.Keys.Contains(tablename))
                    primaryKeys[tablename] = new List<string>();

                primaryKeys[tablename].Add(column);
            }

            this.Connection.Close();
            return primaryKeys[table].First();
        }

        public List<String> GetTables()
        {
            return tables = tables ?? tableReferences.Select(x => x.Table)
                .Union(tableReferences.Select(x => x.ReferenceTable).ToList())
                .Distinct()
                .ToList();
        }

        public List<ForeignKeyInfo> GetChildTables(string tableName)
        {
            return this.tableReferences.Where(x => x.Table.Equals(tableName))
                .Select(x => new ForeignKeyInfo()
                {
                    Table = x.ReferenceTable,
                    Column = x.Column
                })
                .ToList();
        }

        public List<ForeignKeyInfo> GetParentTables(string tablename)
        {
            return this.tableReferences.Where(x => x.ReferenceTable.Equals(tablename))
                .Select(x => new ForeignKeyInfo()
                {
                    Table = x.Table,
                    Column = x.Column
                })
                .ToList();
        }

        public static InformationSchema Instance
        {
            get
            {
                return (_instance = _instance ?? new InformationSchema());
            }
        }



    }
}
