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
    public partial class PersonForm : Form
    {
        Person _person;
        string _categoryName;

        public PersonForm(string categoryName)
        {
            InitializeComponent();
            _categoryName = categoryName;

            this.Text = "Dodaj osobę";
        }

        public PersonForm(Person person)
        {
            InitializeComponent();
            this._person = person;

            this.Text = "Edytuj osobę";
            this.txtSurname.Text = person.Surname;
            this.txtFirstname.Text = person.Firstname;
            this.txtBirthday.Text = person.Birthdate;
            this.txtAddress.Text = person.Address; 
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(this._person == null)
            {
                EventAggregator.Instance.Publish(new NewPersonNotification()
                {
                    person = new Person(
                        this.txtSurname.Text,
                        this.txtFirstname.Text,
                        this.txtFirstname.Text,
                        this.txtAddress.Text
                    ),
                    categoryName = _categoryName
                });
            }
            else
            {
                _person.Surname = this.txtSurname.Text;
                _person.Firstname = this.txtFirstname.Text;
                _person.Birthdate = this.txtBirthday.Text;
                _person.Address = this.txtAddress.Text;
                EventAggregator.Instance.Publish(new EditPersonNotification());
            }

            this.Close();
        }
    }
}
