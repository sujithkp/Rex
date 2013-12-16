
namespace RecordExplorer
{
    public interface ITableNode : IExpandable, ICollapsable, IClickable
    {
    }

    public interface IExpandable
    {
        void LoadChildren();
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
