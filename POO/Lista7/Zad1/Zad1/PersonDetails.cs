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
    public partial class PersonDetails : UserControl
    {
        Person _person;
        public PersonDetails(Person person)
        {
            InitializeComponent();

            this._person = person;
            InitializePersonDetails();

            this.button1.Click += EditPerson;
        }

        private void InitializePersonDetails()
        {
            this.lblSurname.Text = _person.Surname;
            this.lblFirstname.Text = _person.Firstname;
            this.lblBirthday.Text = _person.Birthdate;
            this.lblAddress.Text = _person.Address;
        }

        private void EditPerson(object sender, EventArgs e)
        {
            (new PersonForm(_person)).Show();
        }


    }
}
