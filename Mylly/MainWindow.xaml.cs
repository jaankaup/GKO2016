using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Mylly
{
    /// <summary>
    /// Windows luokka. Katso ja ihmettele, ei ole juurikaan toimintalogiikkaa tässä luokassa. 
    /// Pelin toiminta logiikka on kokonaan viewmodel luokassa, ja itse viewmodel ja commandbindingit 
    /// ovat asetettu xamlissa.
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Palleroinen_BlockClicked(object sender, RoutedEventArgs e)
        {

        }

        private void Joopaooo(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("hähähähähä");
        }

        private void Ellipsi_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Ellipse ellipse = sender as Ellipse;
            if (ellipse == null) return;



            //Console.WriteLine((ellipse.Tag).GetType().Name);

            Block b = ellipse.Tag as Block;
            if (b == null) Console.WriteLine("no voeha että.");
        }
    }
}
