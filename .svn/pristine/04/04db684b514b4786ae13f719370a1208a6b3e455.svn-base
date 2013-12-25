using System;
using System.Collections.Generic;
using System.Windows.Forms;
using RecordExplorer.Data;

namespace RecordExplorer.Contracts
{
    public interface IMainForm
    {
        event EventHandler<MainFormEventArgs> NodeExpanding;

        event EventHandler<MainFormEventArgs> NodeSelected;

        event EventHandler<MainFormEventArgs> NodeCollapsing;

        void LoadChildNodes(TreeNode node, IList<TreeNode> nodes);

        void ShowRow(Dictionary<string, object> row);

        void ShowRows(IList<Dictionary<string, object>> rows);

        void ShowBusy();
    }
}
