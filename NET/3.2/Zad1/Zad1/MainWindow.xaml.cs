using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Zad1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void button_Copy_Click(object sender, RoutedEventArgs e)
        {
            string info;
            info = String.Format("{0}\n{1}\nStudia {2}\n", this.tbxName.Text, this.tbxAddress.Text,
                            this.cbxCycle.Text);

            if (chbComplem.IsChecked == true) info += "uzupełniające\n";
            if (chbDaily.IsChecked == true) info += "dzienne";

            MessageBox.Show(info);
        }
    }
}
