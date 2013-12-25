
using System.Collections.Generic;
namespace RecordExplorer
{
    public interface ITableNode : IExpandable, ICollapsable, IClickable
    {
    }

    public interface IExpandable
    {
        IList<ITableNode> GetChildren();
    }

    public interface ICollapsable
    {
        void RemoveChildren();
    }

    public interface IClickable
    {
        NodeInfo GetNodeInfo();
    }
}
