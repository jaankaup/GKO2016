using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Collections.ObjectModel;

namespace Mylly
{
    /// <summary>
    /// Janne Kauppinen 2017.
    /// Copyrights: None.
    /// Pelaaja luokka myllypeliä varten.
    /// </summary>
    public class Player : DependencyObject
    {
        public Player()
        {

        }

        /// <summary>
        /// Dependency property "kädessä" oleville nappuloille.
        /// </summary>
        public ObservableCollection<GameNappula> UIPieces
        {
            get { return (ObservableCollection<GameNappula>)GetValue(UIPiecesProperty); }
            set { SetValue(UIPiecesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for UIPieces.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UIPiecesProperty =
            DependencyProperty.Register("UIPieces", typeof(ObservableCollection<GameNappula>), typeof(Player), new PropertyMetadata(null));

        

        /// <summary>
        /// Dependency property aloitusnappuloita varten.
        /// </summary>
        public int PieceCount
        {
            get { return (int)GetValue(PieceCountProperty); }
            set { SetValue(PieceCountProperty, value); }
        }


        /// <summary>
        /// Dependency property, joka pitää sisällään jotain int-tyyppistä dataa, joka puolestaan kertoo jotain 
        /// piirrettävän nappulan muodosta. Ei ota kantaa siihen mitä piirretään tai onko annetulla 
        /// int arvolla mitään järkevää vastinetta.
        /// </summary>
        public int PieceShapeId
        {
            get { return (int)GetValue(PieceShapeIdProperty); }
            set { SetValue(PieceShapeIdProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PieceShapeId.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PieceShapeIdProperty =
            DependencyProperty.Register("PieceShapeId", typeof(int), typeof(Player), new PropertyMetadata(0));



        // Using a DependencyProperty as the backing store for PieceCount.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PieceCountProperty =
            DependencyProperty.Register("PieceCount", typeof(int), typeof(Player), new PropertyMetadata(9));

        /// <summary>
        /// Dependency property pelaajan nimeä varten.
        /// </summary>
        public String PlayerName
        {
            get { return (String)GetValue(PlayerNameProperty); }
            set { SetValue(PlayerNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PlayerNameProperty =
            DependencyProperty.Register("PlayerName", typeof(String), typeof(Player), new PropertyMetadata("Erkki"));

        /// <summary>
        /// Dependency property pelaajan väriä varten.
        /// </summary>
        public Brush PlayerColor
        {
            get { return (Brush)GetValue(PlayerColorProperty); }
            set { SetValue(PlayerColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PlayerColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PlayerColorProperty =
            DependencyProperty.Register("PlayerColor", typeof(Brush), typeof(Player), 
                                         new PropertyMetadata(new SolidColorBrush(Color.FromRgb(255,255,0))));

        /// <summary>
        /// Dependency property pelaajan id:tä varten.
        /// </summary>
        public int PlayerId
        {
            get { return (int)GetValue(PlayerIdProperty); }
            set { SetValue(PlayerIdProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PlayerId.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PlayerIdProperty =
            DependencyProperty.Register("PlayerId", typeof(int), typeof(Player), new PropertyMetadata(0));

        /// <summary>
        /// Dependency property, joka kertoo onko pelaajan vuoro. ViewModel päättää kenen vuoro on, eli logiikka on 
        /// ViewModel luokassa. Tieto omasta vuorosta on pelaaja-oliolla.
        /// </summary>
        public bool HasTurn
        {
            get { return (bool)GetValue(HasTurnProperty); }
            set { SetValue(HasTurnProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HasTurn.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HasTurnProperty =
            DependencyProperty.Register("HasTurn", typeof(bool), typeof(Player), new PropertyMetadata(false));
    }
}