using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Zad1
{
    public partial class Form1 : Form, 
        ISubscriber<TreeViewEventArgs>, 
        ISubscriber<EditPersonNotification>, 
        ISubscriber<NewPersonNotification>
    {
        public Form1(List<Person> Students, List<Person> Lecturers)
        {
            InitializeComponent();
            InitializeTreeView(Students, Lecturers);

            this.treeView1.AfterSelect += TreeViewClick;
            EventAggregator.Instance.AddSubscriber( (ISubscriber<TreeViewEventArgs>)this );
            EventAggregator.Instance.AddSubscriber( (ISubscriber<EditPersonNotification>)this );
            EventAggregator.Instance.AddSubscriber( (ISubscriber<NewPersonNotification>)this );
        }

        private void InitializeTreeView(List<Person> Students, List<Person> Lecturers)
        {
            foreach(var s in Students)
            {
                TreeNode sNode = new TreeNode(s.Surname + " " + s.Firstname);
                sNode.Tag = s;
                this.treeView1.Nodes["Studenci"].Nodes.Add(sNode);
            }

            foreach (var l in Lecturers)
            {
                TreeNode lNode = new TreeNode(l.Surname + " " + l.Firstname);
                lNode.Tag = l;
                this.treeView1.Nodes["Wykładowcy"].Nodes.Add(lNode);
            }
        }

        private void TreeViewClick(object sender, TreeViewEventArgs e)
        {
            EventAggregator.Instance.Publish(e);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        void ISubscriber<TreeViewEventArgs>.Handle(TreeViewEventArgs Notification)
        {
            if(Notification.Node.Tag == null)
            {
                ShowUserList();
            }
            else
            {
                ShowUser((Person)Notification.Node.Tag);
            }

        }

        public void ShowUser(Person person)
        {
            panel1.Controls.Clear();
            panel1.Controls.Add(new PersonDetails(person));
        }

        public void ShowUserList()
        {
            TreeNode Node = treeView1.SelectedNode;
            List<Person> people = new List<Person>();
            foreach (TreeNode user in Node.Nodes)
            {
                Person p = (Person)user.Tag;
                people.Add(p);
            }
            panel1.Controls.Clear();
            panel1.Controls.Add(new PersonList(people, Node.Name));
        }

        void ISubscriber<EditPersonNotification>.Handle(EditPersonNotification Notification)
        {
            // Refresh Student nodes
            foreach (TreeNode s in this.treeView1.Nodes["Studenci"].Nodes)
            {
                Person person = (Person)(s.Tag);
                s.Text = person.Surname + " " + person.Firstname;
            }
            // Refresh Wykladowca nodes
            foreach (TreeNode s in this.treeView1.Nodes["Wykładowcy"].Nodes)
            {
                Person person = (Person)(s.Tag);
                s.Text = person.Surname + " " + person.Firstname;
            }

            ShowUser((Person)this.treeView1.SelectedNode.Tag);
        }

        void ISubscriber<NewPersonNotification>.Handle(NewPersonNotification Notification)
        {
            if(this.treeView1.Nodes.ContainsKey(Notification.categoryName))
            {
                string nodeName = Notification.person.Surname + " " + Notification.person.Firstname;
                TreeNode newPerson = new TreeNode(nodeName);
                newPerson.Tag = Notification.person;
                this.treeView1.Nodes[Notification.categoryName].Nodes.Add(
                    newPerson
                    );

                ShowUserList();
            }
            else
            {
                throw new ArgumentException();
            }
        }
    }
}
