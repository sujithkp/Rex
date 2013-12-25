using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using RecordExplorer.Business;
using RecordExplorer.Contracts;

namespace RecordExplorer.UI
{
    public class MainFormController
    {
        public IMainForm MainForm { get; set; }

        public MainFormController(IMainForm form)
        {
            MainForm = form;

            MainForm.NodeCollapsing += MainForm_NodeCollapsing;
            MainForm.NodeExpanding += MainForm_NodeExpanding;
            MainForm.NodeSelected += MainForm_NodeSelected;
        }

        void MainForm_NodeSelected(object sender, Data.MainFormEventArgs e)
        {
            var nodeInfo = (e.Node as IClickable).GetNodeInfo();

            if (nodeInfo == null) return;

            var worker = new BackgroundWorker();

            worker.DoWork += (obj, args) =>
            {
                args.Result = new TableManager().GetRows(nodeInfo.TableName, nodeInfo.CriteriaColumn, nodeInfo.CriteriaValue.ToString());
            };

            worker.RunWorkerCompleted += (obj, args) =>
            {
                var records = args.Result as List<Dictionary<string, object>>;

                if (!nodeInfo.IsCollection)
                {
                    if (records != null && records.Count > 0)
                        MainForm.ShowRow(records.FirstOrDefault());
                }
                else
                {
                    if (records != null && records.Count > 0)
                        MainForm.ShowRows(records);
                }
            };

            worker.RunWorkerAsync();
        }

        void MainForm_NodeExpanding(object sender, Data.MainFormEventArgs e)
        {
            var node = e.Node;

            var worker = new BackgroundWorker();

            worker.DoWork += (obj, args) =>
            {
                var tableNode = args.Argument as ITableNode;
                args.Result = tableNode.GetChildren();
            };

            worker.RunWorkerCompleted += (obj, args) =>
            {
                var children = new List<TreeNode>();
                (args.Result as IList<ITableNode>).ToList().ForEach(x => children.Add(x as TreeNode));
                MainForm.LoadChildNodes(node as TreeNode, children);
            };

            worker.RunWorkerAsync(node);
        }

        void MainForm_NodeCollapsing(object sender, Data.MainFormEventArgs e)
        {
            throw new NotImplementedException();
        }

    }
}
