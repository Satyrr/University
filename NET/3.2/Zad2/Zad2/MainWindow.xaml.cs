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

namespace Zad2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int elements;

        public MainWindow()
        {
            InitializeComponent();
            InitializeData();
            
        }

        private void InitializeData()
        {
            this.progressBar.Minimum = 0.0;
            this.progressBar.Maximum = 40.0;
            this.elements = 0;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            if (elements == 40) return;

            string element = this.textBox.Text;

            this.listView.Items.Add(element);
            this.progressBar.Value += 1.0;
            this.elements++;
            this.label.Content = String.Format("List capacity: {0}/40", this.elements);
            
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.listView.Items.Clear();
            this.progressBar.Value = 0.0;
            this.elements = 0;
            this.label.Content = "List capacity: 0/40";
        }
    }
}
