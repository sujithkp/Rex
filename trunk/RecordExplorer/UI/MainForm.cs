using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using RecordExplorer.Business;
using RecordExplorer.Contracts;
using RecordExplorer.Data;

namespace RecordExplorer.UI
{
    public partial class MainForm : Form, IMainForm
    {
        public event EventHandler<MainFormEventArgs> NodeExpanding;

        public event EventHandler<MainFormEventArgs> NodeSelected;

        public event EventHandler<MainFormEventArgs> NodeCollapsing;

        private MainFormController controller;

        public MainForm()
        {
            InitializeComponent();

            controller = new MainFormController(this);
            ShowReady();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            tableNodeContainer.BeforeExpand += tableNodeContainer_BeforeExpand;
            tableNodeContainer.NodeMouseClick += tableNodeContainer_NodeMouseClick;
        }

        void tableNodeContainer_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            ShowBusy();

            if (NodeExpanding != null)
            {
                NodeExpanding(this, new MainFormEventArgs(e.Node as ITableNode));
                e.Cancel = true;
            }
        }

        private void tableNodeContainer_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();

            if (NodeSelected != null)
            {
                NodeSelected(this, new MainFormEventArgs(e.Node as ITableNode));
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private AddRecordDetail addRecordDetail = null;

        private void addRecordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            addRecordDetail = addRecordDetail ?? new AddRecordDetail(string.Empty, "0");

            addRecordDetail.DataSource = InformationSchema.Instance.GetTables();

            addRecordDetail = new AddRecordController().GetAddRecordDetail(addRecordDetail);

            if (addRecordDetail.IsNew)
            {
                tableNodeContainer.Nodes.Add(new TableNode(addRecordDetail.TableName,
                    InformationSchema.Instance.GetPrimaryKey(addRecordDetail.TableName), addRecordDetail.PKey));
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Record Explorer (Rex)" + Environment.NewLine
                + "Author: Sujith K P"
                + Environment.NewLine
                + "Version: " + Application.ProductVersion
                + Environment.NewLine
                + Environment.NewLine
                + "Note: Present version will not work with tables which has more than one key.", "About me.");
        }

        public void ShowRow(System.Collections.Generic.Dictionary<string, object> row)
        {
            dataGridView1.Columns.Add("Name", "Name");
            dataGridView1.Columns.Add("Value", "Value");

            dataGridView1.Columns[0].Width = 200;
            dataGridView1.Columns[1].Width = 600;

            foreach (var key in row.Keys)
            {
                var colval = row[key];

                if (colval.ToString().Length == 0)
                    colval = "null";

                dataGridView1.Rows.Add(key, colval);
            }
        }

        public void ShowRows(System.Collections.Generic.IList<System.Collections.Generic.Dictionary<string, object>> rows)
        {
            foreach (var key in rows.FirstOrDefault().Keys)
                dataGridView1.Columns.Add(key, key);

            foreach (var record in rows)
                dataGridView1.Rows.Add(record.Values.ToArray());

            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }

        public void LoadChildNodes(TreeNode node, System.Collections.Generic.IList<TreeNode> nodes)
        {
            tableNodeContainer.BeforeExpand -= tableNodeContainer_BeforeExpand;

            node.Nodes.Clear();
            node.Nodes.AddRange(nodes.ToArray());
            node.Expand();
            ShowReady();

            tableNodeContainer.BeforeExpand += tableNodeContainer_BeforeExpand;
        }

        public void ShowReady()
        {
            statusPanel.Hide();

            statusLbl.Text = "Ready";
        }

        public void ShowBusy()
        {
            statusPanel.Show();

            statusLbl.Text = "Busy";
        }
    }
}
