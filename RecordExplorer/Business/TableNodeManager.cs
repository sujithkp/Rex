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

        public List<TreeNode> GetChildNodes(string parentName, int? pkey)
        {
            var children = new List<TreeNode>();

            var childNodes = schema.GetChildTables(parentName);

            if (childNodes.Count > 0 && pkey.HasValue)
            {
                var refValues = tableManager.GetColumnsValue(childNodes.Select(x => x.Column).ToList(), parentName, "PKey", pkey.Value);

                if (refValues.Count > 0)
                {
                    foreach (var child in childNodes)
                        children.Add(new TableNode(child.Table, refValues[child.Column]) as TreeNode);
                }
            }

            foreach (var child in schema.GetParentTables(parentName))
                children.Add(new TableNodeCollection(child.Table, child.Column, pkey) as TreeNode);

            return children;
        }
    }
}
