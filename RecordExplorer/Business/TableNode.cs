using System;
using System.Windows.Forms;

namespace RecordExplorer
{
    public class TableNode : TreeNode, ITableNode
    {
        public string Name { get; private set; }

        public int? PKey { get; private set; }

        public TableNode(string name, int? pkey)
            : base(name + "  (" + (pkey.HasValue ? pkey.ToString() : "null") + ")")
        {
            this.PKey = pkey;
            this.Name = name;

            if (pkey.HasValue)
                this.Nodes.Add(string.Empty);
        }

        public void LoadChildren()
        {
            this.Nodes.Clear();

            var childNodes = TableNodeManager.Instance.GetChildNodes(this.Name, PKey);
            var list = childNodes.ToArray() as TreeNode[];

            this.Nodes.AddRange(childNodes.ToArray() as TreeNode[]);
        }

        public void RemoveChildren()
        {

        }

        public NodeInfo GetNodeInfo()
        {
            if (this.PKey == null)
                return null;

            return new NodeInfo()
            {
                CriteriaColumn = "PKey",
                CriteriaValue = this.PKey.Value,
                TableName = this.Name,
            };
        }
    }
}
