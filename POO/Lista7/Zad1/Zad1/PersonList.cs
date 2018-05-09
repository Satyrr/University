using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Zad1
{
    public partial class PersonList : UserControl
    {
        string _categoryName;
        public PersonList(List<Person> PersonList, string CategoryName)
        {
            InitializeComponent();
            _categoryName = CategoryName;

            foreach (var p in PersonList)
            {
                AddPerson(p);
            }
        }

        public void AddPerson(Person Person)
        {
            ListViewItem PersonElement = new ListViewItem(
                new string[]
                {
                    Person.Surname,
                    Person.Firstname,
                    Person.Birthdate,
                    Person.Address
                }
                );
            this.listView1.Items.Add(PersonElement);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            (new PersonForm(_categoryName)).Show();
        }
    }
}
