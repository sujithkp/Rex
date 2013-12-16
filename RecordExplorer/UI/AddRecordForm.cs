using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RecordExplorer
{
    public partial class AddRecordForm : Form
    {
        public AddRecordForm()
        {
            InitializeComponent();
        }

        public string TableName { get; set; }

        public List<string> DataSource { get; set; }

        public int PKey { get; set; }

        private void AddRecordForm_Load(object sender, EventArgs e)
        {
            this.comboBox1.DataSource = DataSource;
            this.textBox1.Text = PKey.ToString();            
        }

        private void AddRecordForm_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.TableName = this.comboBox1.Text;
            this.PKey = int.Parse(this.textBox1.Text);
        }
    }
}
