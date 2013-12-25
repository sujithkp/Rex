using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace RecordExplorer.Business
{
    public class TableManager
    {
        private SqlConnection connection;
        private InformationSchema schemaManager;

        private string selectStmntFormat = "Select {0} from [{1}] where {2} = '{3}';";

        public TableManager()
        {
            var connectionString = "Data Source=.\\sqlexpress; Initial Catalog = flexilineweb;Integrated Security =SSPI";

            connection = new SqlConnection(connectionString);
            schemaManager = InformationSchema.Instance;
        }

        public Dictionary<string, string> GetColumnsValue(List<string> columns, string tableName, string column, string pkey)
        {
            connection.Open();

            var cols = string.Join(",", columns);
            var sqlStmnt = string.Format(selectStmntFormat, cols, tableName, column, pkey);

            var command = new SqlCommand(sqlStmnt, connection);
            var reader = command.ExecuteReader();

            var record = new Dictionary<string, string>();

            if (reader.Read())
            {
                foreach (var col in columns)
                {
                    record.Add(col, reader[col].ToString());
                }
            }

            connection.Close();
            return record;
        }

        public List<Dictionary<string, List<string>>> GetPrimaryKeyValues(string tableName, string conditionCol, string colvalue)
        {
            connection.Open();

            var result = new List<Dictionary<string, List<string>>>();
            var cols = new List<string>() { schemaManager.GetPrimaryKey(tableName) };
            var sqlStmnt = string.Format(selectStmntFormat, schemaManager.GetPrimaryKey(tableName), tableName, conditionCol, colvalue);

            var command = new SqlCommand(sqlStmnt, connection);
            SqlDataReader reader = null;

            try
            {
                reader = command.ExecuteReader();
            }
            catch (SqlException ex)
            {
                if (ex.Message.Contains("Invalid column name 'PKey'."))
                {
                    //command.CommandText = "Select * from " + tableName;
                    //reader = command.ExecuteReader()                        ;

                    //var cs = reader.GetSchemaTable().Columns;

                }

                connection.Close();
                return result;
            }

            while (reader.Read())
            {
                var record = new Dictionary<string, List<string>>();
                foreach (var col in cols)
                {
                    if (!record.Keys.Contains(col))
                        record.Add(col, new List<string>());

                    record[col].Add(reader[col].ToString());
                }

                result.Add(record);
            }

            connection.Close();
            return result;
        }

        public List<Dictionary<string, object>> GetRows(string tableName, string criCol, string criVal)
        {
            connection.Open();

            var result = new List<Dictionary<string, object>>();
            var sqlStmnt = string.Format(selectStmntFormat, "*", tableName, criCol, criVal);
            SqlDataReader reader = null;

            try
            {
                var command = new SqlCommand(sqlStmnt, connection);
                reader = command.ExecuteReader();
            }
            catch (Exception ex)
            {
                connection.Close();
                return result; ;
            }

            while (reader.Read())
            {
                var record = new Dictionary<string, object>();

                for (int i = 0; i < reader.FieldCount; i++)
                    record.Add(reader.GetName(i), reader[i]);

                result.Add(record);
            }

            connection.Close();
            return result;
        }
    }
}
