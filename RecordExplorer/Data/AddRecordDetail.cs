using System;
using System.Collections.Generic;

namespace RecordExplorer.Data
{
    public class AddRecordDetail
    {
        public AddRecordDetail(string tablename, int pkey)
        {
            this.TableName = tablename;
            this.PKey = pkey;
        }

        public String TableName { get; private set; }

        public int PKey { get; private set; }

        public bool IsNew { get; set; }

        public List<String> DataSource { get; set; }
    }
}
