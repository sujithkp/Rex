using System;

namespace RecordExplorer.Data
{
    public class MainFormEventArgs : EventArgs
    {
        public MainFormEventArgs(ITableNode node)
        {
            this.Node = node;
        }

        public ITableNode Node { get; private set; }
    }
}
