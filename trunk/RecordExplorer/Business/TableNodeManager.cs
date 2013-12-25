using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using RecordExplorer.Business;

namespace RecordExplorer
{
    public class TableNodeManager
    {
        private static TableNodeManager _instance;

        public static TableNodeManager Instance
        {
            get
            {
                return (_instance = _instance ?? new TableNodeManager());
            }
        }

        private InformationSchema schema;
        private TableManager tableManager;

        private TableNodeManager()
        {
            schema = InformationSchema.Instance;
            tableManager = new TableManager();
        }

        public List<ITableNode> GetChildNodes(string parentName, string pkey)
        {
            var children = new List<ITableNode>();

            var childNodes = schema.GetChildTables(parentName);

            if (childNodes.Count > 0 && pkey != null)
            {
                var primaryCol = InformationSchema.Instance.GetPrimaryKey(parentName);
                var refValues = tableManager.GetColumnsValue(childNodes.Select(x => x.Column).ToList(), parentName,
                    primaryCol, pkey);

                if (refValues.Count > 0)
                {
                    foreach (var child in childNodes)
                        children.Add(new TableNode(child.Table, InformationSchema.Instance.GetPrimaryKey(child.Table)
                            , refValues[child.Column]));
                }
            }

            foreach (var child in schema.GetParentTables(parentName))
                children.Add(new TableNodeCollection(child.Table, child.Column, pkey));

            return children;
        }
    }
}
