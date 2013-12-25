using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using RecordExplorer.Business;

namespace RecordExplorer
{
    public class TableNodeCollection : TreeNode, ITableNode
    {
        public string RefColumn { get; private set; }

        public bool IsParent { get; private set; }

        public string RefColumnValue { get; private set; }

        public string TableName { get; private set; }

        public TableNodeCollection(string name, string refColumn, string refColval)
            : base(name + "(s)")
        {
            this.TableName = name;
            this.RefColumn = refColumn;

            if (!String.IsNullOrEmpty(refColval))
            {
                this.RefColumnValue = refColval;
                this.Nodes.Add(string.Empty);
            }
        }

        public IList<ITableNode> GetChildren()
        {
            var nodes = new List<ITableNode>();

            var list = new TableManager().GetPrimaryKeyValues(TableName, RefColumn, RefColumnValue);

            foreach (var dictionary in list)
            {
                foreach (var dictionaryEntry in dictionary)
                {
                    foreach (var record in dictionary[dictionaryEntry.Key])
                        nodes.Add(new TableNode(TableName, InformationSchema.Instance.GetPrimaryKey(TableName), record));
                }
            }

            return nodes;
        }

        public void RemoveChildren()
        {
            throw new NotImplementedException();
        }

        public NodeInfo GetNodeInfo()
        {
            return new NodeInfo()
            {
                IsCollection = true,
                TableName = this.TableName,
                CriteriaColumn = this.RefColumn,
                CriteriaValue = this.RefColumnValue,
            };
        }
    }
}
