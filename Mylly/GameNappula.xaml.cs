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

namespace Mylly
{
    /// <summary>
    /// Pelinappula komponentti. 
    /// </summary>
    public partial class GameNappula : UserControl
    {
        public GameNappula()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Dependencyproperty nappulan värin määritykseen. TODO_ käytetäänkö edes?
        /// </summary>
        public Brush NappulaColor
        {
            get { return (Brush)GetValue(NappulaColorProperty); }
            set { SetValue(NappulaColorProperty, value); }
        }



        public Brush SelectionColor
        {
            get { return (Brush)GetValue(SelectionColorProperty); }
            set { SetValue(SelectionColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectionColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectionColorProperty =
            DependencyProperty.Register("SelectionColor", typeof(Brush), typeof(GameNappula), new PropertyMetadata(new SolidColorBrush(Color.FromRgb(20,150,255))));



        // Using a DependencyProperty as the backing store for  NappulaColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NappulaColorProperty =
            DependencyProperty.Register("NappulaColor", 
                                         typeof(Brush), typeof(GameNappula), 
                                         new PropertyMetadata(new SolidColorBrush(Color.FromArgb(255,255,0,0))));

        /// <summary>
        /// Dependencyproperty, joka kertoo nappulatyypin. Ei ota kantaa siihen miten sitä käytetään. Itse xamlissa käytetään pelinappulan muodonmääritykseen.
        /// TODO: käytetäänkö edes.
        /// </summary>
        public int NappulaType
        {
            get { return (int)GetValue(NappulaTypeProperty); }
            set { SetValue(NappulaTypeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NappulaTypeProperty =
            DependencyProperty.Register("NappulaType", typeof(int), typeof(GameNappula), new PropertyMetadata(0));


        /// <summary>
        /// Dependency property, joka kertoo onko nappula valittu.
        /// </summary>
        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsSelected.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsSelectedProperty =
            DependencyProperty.Register("IsSelected", typeof(bool), typeof(GameNappula), new PropertyMetadata(false));

        /// <summary>
        /// Dependencyproperty, joka kertoo onko nappula valittavissa.
        /// </summary>
        public bool IsSelectable
        {
            get { return (bool)GetValue(IsSelectableProperty); }
            set { SetValue(IsSelectableProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsSelectable.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsSelectableProperty =
            DependencyProperty.Register("IsSelectable", typeof(bool), typeof(GameNappula), new PropertyMetadata(false));
    }
}