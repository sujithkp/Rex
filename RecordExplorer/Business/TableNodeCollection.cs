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

        public int RefColumnValue { get; private set; }

        public string TableName { get; private set; }

        public TableNodeCollection(string name, string refColumn, int? refColval)
            : base(name + "(s)")
        {
            this.TableName = name;
            this.RefColumn = refColumn;

            if (refColval.HasValue)
            {
                this.RefColumnValue = refColval.Value;
                this.Nodes.Add(string.Empty);
            }
        }

        public void LoadChildren()
        {
            this.Nodes.Clear();

            var list = new TableManager().GetPrimaryKeyValues(TableName, RefColumn, RefColumnValue);

            foreach (var dictionary in list)
            {
                foreach (var dictionaryEntry in dictionary)
                {
                    foreach (var record in dictionary[dictionaryEntry.Key])
                        this.Nodes.Add(new TableNode(TableName, record));
                }
            }
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
