using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Zad2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            InitializeData();
        }

        private void InitializeData()
        {
            lblStatus.Text = "0/40";
            prbUsed.Style = ProgressBarStyle.Continuous;
        }

        private void toolStripSplitButton1_ButtonClick(object sender, EventArgs e)
        {

        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int nodeCount = treeView1.GetNodeCount(true);
            if (String.IsNullOrEmpty(tbxNewNode.Text)) return;
            if (nodeCount >= 40) return; 

            if (treeView1.SelectedNode == null)
                treeView1.Nodes.Add(tbxNewNode.Text);
            else
                treeView1.SelectedNode.Nodes.Add(tbxNewNode.Text);

            treeView1.SelectedNode = null;

            tbxNewNode.Text = "";

            lblStatus.Text = String.Format("{0}/40", nodeCount);

            prbUsed.Value = (int)(nodeCount * 2.5);
            if (nodeCount >= 3) prbUsed.ForeColor = Color.Yellow;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
