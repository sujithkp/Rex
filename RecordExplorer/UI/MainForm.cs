using System;
using System.Linq;
using System.Windows.Forms;
using RecordExplorer.Business;
using RecordExplorer.Data;
using RecordExplorer.UI;

namespace RecordExplorer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //  tableNodeContainer.Nodes.Add(new TableNode("LoanRequest", 1406));
            tableNodeContainer.BeforeExpand += tableNodeContainer_BeforeExpand;
        }

        void tableNodeContainer_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            statusLbl.Text = "Working..";

            (e.Node as IExpandable).LoadChildren();

            ShowStatusAsDone();
        }

        private void ShowStatusAsDone()
        {
            statusLbl.Text = "Ready.";
        }

        private void tableNodeContainer_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            statusLbl.Text = "Working..";

            var nodeInfo = (e.Node as IClickable).GetNodeInfo();

            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();

            if (nodeInfo == null) return;

            if (nodeInfo.IsCollection)
            {
                var records = new TableManager().GetRows(nodeInfo.TableName, nodeInfo.CriteriaColumn, nodeInfo.CriteriaValue.ToString());

                var cols = records.FirstOrDefault();

                if (cols == null)
                {
                    ShowStatusAsDone();
                    return;
                }

                foreach (var key in cols.Keys)
                    dataGridView1.Columns.Add(key, key);

                foreach (var record in records)
                    dataGridView1.Rows.Add(record.Values.ToArray());

                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            }
            else
            {
                var record = new TableManager().GetRows(nodeInfo.TableName, nodeInfo.CriteriaColumn, nodeInfo.CriteriaValue.ToString());
                if (record.Count == 0)
                {
                    ShowStatusAsDone();
                    return;
                }

                dataGridView1.Columns.Add("Name", "Name");
                dataGridView1.Columns.Add("Value", "Value");

                dataGridView1.Columns[0].Width = 200;
                dataGridView1.Columns[1].Width = 600;

                foreach (var key in record.First().Keys)
                {
                    var colval = record.First()[key];

                    if (colval.ToString().Length == 0)
                        colval = "null";

                    dataGridView1.Rows.Add(key, colval);
                }
            }

            ShowStatusAsDone();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private AddRecordDetail addRecordDetail = null;

        private void addRecordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            addRecordDetail = addRecordDetail ?? new AddRecordDetail(string.Empty, 0);

            addRecordDetail.DataSource = InformationSchema.Instance.GetTables();

            addRecordDetail = new AddRecordController().GetAddRecordDetail(addRecordDetail);

            if (addRecordDetail.IsNew)
            {
                tableNodeContainer.Nodes.Add(new TableNode(addRecordDetail.TableName, addRecordDetail.PKey));
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
                + "Note: Present version will not work with tables which has primary key other than PKey", "About me.");
        }
    }
}
