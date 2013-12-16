using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace RecordExplorer.Business
{
    public class TableManager
    {
        private SqlConnection connection;

        private string selectStmntFormat = "Select {0} from [{1}] where {2} = {3};";

        public TableManager()
        {
            var connectionString = "Data Source=tcp:oe57f6zbk5.database.windows.net,1433; Initial Catalog = FlexiLineWeb_Q;User ID=FlexiLineWeb_Q@oe57f6zbk5;Password=FlexiWeb_Q";

            connection = new SqlConnection(connectionString);
        }

        public Dictionary<string, int?> GetColumnsValue(List<string> columns, string tableName, string column, int pkey)
        {
            connection.Open();

            var cols = string.Join(",", columns);
            var sqlStmnt = string.Format(selectStmntFormat, cols, tableName, column, pkey);

            var command = new SqlCommand(sqlStmnt, connection);
            var reader = command.ExecuteReader();

            var record = new Dictionary<string, int?>();

            if (reader.Read())
            {
                foreach (var col in columns)
                {
                    record.Add(col, reader[col] as int?);
                }
            }

            connection.Close();
            return record;
        }

        public List<Dictionary<string, List<int>>> GetPrimaryKeyValues(string tableName, string conditionCol, int colvalue)
        {
            connection.Open();

            var result = new List<Dictionary<string, List<int>>>();
            var cols = new List<string>() { "PKey" };
            var sqlStmnt = string.Format(selectStmntFormat, "PKey", tableName, conditionCol, colvalue);

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
                var record = new Dictionary<string, List<int>>();
                foreach (var col in cols)
                {
                    if (!record.Keys.Contains(col))
                        record.Add(col, new List<int>());

                    record[col].Add((int)reader[col]);
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
