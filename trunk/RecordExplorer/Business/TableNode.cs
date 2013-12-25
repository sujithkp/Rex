using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace RecordExplorer
{
    public class TableNode : TreeNode, ITableNode
    {
        public string Name { get; private set; }

        public string PKey { get; private set; }

        public string CriCol { get; private set; }

        public TableNode(string name, string criCol, string pkey)
            : base(name + "  (" + (pkey.Length > 0 ? pkey : "null") + ")")
        {
            this.PKey = pkey;
            this.Name = name;
            this.CriCol = criCol;

            if (pkey.Length > 0)
                this.Nodes.Add(string.Empty);
        }

        public IList<ITableNode> GetChildren()
        {
            return TableNodeManager.Instance.GetChildNodes(this.Name, PKey);
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
                CriteriaColumn = this.CriCol,
                CriteriaValue = this.PKey,
                TableName = this.Name,
            };
        }
    }
}
