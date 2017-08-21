using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Mylly
{
    /// <summary>
    /// Janne Kauppinen 2017.
    /// Copyrights: None.
    /// 
    /// Block luokka, joka pitää sisällään tietoa yhden pelialueen palasen tietoja. Kts. Mylly
    /// </summary>
    public class Block : DependencyObject
    {
        /// <summary>
        /// Enum, joka kertoo kenen pelinappulaa olio sisältää. 
        /// </summary>
        public enum BlockContent { None = 0, Player1 = 1, Player2 = 2}

        public Block()
        {

        }

        /// <summary>
        /// Dependency property, joka kertoo voiko liikkua vasemmalle.
        /// </summary>
        public bool LeftWay
        {
            get { return (bool)GetValue(LeftWayProperty); }
            set { SetValue(LeftWayProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Left.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LeftWayProperty =
            DependencyProperty.Register("LeftWay", typeof(bool), typeof(Block), new PropertyMetadata(false));

        /// <summary>
        /// Dependency property, joka kertoo voiko liikkua oikealle.
        /// </summary>
        public bool RightWay
        {
            get { return (bool)GetValue(RightWayProperty); }
            set { SetValue(RightWayProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Left.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RightWayProperty =
            DependencyProperty.Register("RightWay", typeof(bool), typeof(Block), new PropertyMetadata(false));

        /// <summary>
        /// Dependency property, joka kertoo voiko liikkua ylös.
        /// </summary>
        public bool UpWay
        {
            get { return (bool)GetValue(UpWayProperty); }
            set { SetValue(UpWayProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Left.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UpWayProperty =
            DependencyProperty.Register("UpWay", typeof(bool), typeof(Block), new PropertyMetadata(false));


        /// <summary>
        /// Dependency property, joka kertoo voiko liikkua vasemmalle.
        /// </summary>
        public bool DownWay
        {
            get { return (bool)GetValue(DownWayProperty); }
            set { SetValue(DownWayProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Left.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DownWayProperty =
            DependencyProperty.Register("DownWay", typeof(bool), typeof(Block), new PropertyMetadata(false));

        /// <summary>
        /// Dependency property, joka kertoo onko tässä blokissa jokin objekti (esim. pallo).
        /// </summary>
        public bool HasObject
        {
            get { return (bool)GetValue(HasObjectProperty); }
            set { SetValue(HasObjectProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Left.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HasObjectProperty =
            DependencyProperty.Register("HasObject", typeof(bool), typeof(Block), new PropertyMetadata(false));


        /// <summary>
        /// Dependency property, joka kertoo sen, onko blokissa nappula.
        /// </summary>
        public bool HasPiece
        {
            get { return (bool)GetValue(HasPieceProperty); }
            set { SetValue(HasPieceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HasPiece.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HasPieceProperty =
            DependencyProperty.Register("HasPiece", typeof(bool), typeof(Block), new PropertyMetadata(false));


        /// <summary>
        /// Dependency property, joka kertoo sen kuka "omistaa" blokin. Ts. tässä tapauksessa: kenen nappula on blokissa.
        /// </summary>
        public Player BlockOwner
        {
            get { return (Player)GetValue(BlockOwnerProperty); }
            set { SetValue(BlockOwnerProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BlockOwner.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BlockOwnerProperty =
            DependencyProperty.Register("BlockOwner", typeof(Player), typeof(Block), new PropertyMetadata(null));


        /// <summary>
        /// Dependencyproperty sille onko block valittavissa.
        /// </summary>
        public bool IsSelectable
        {
            get { return (bool)GetValue(IsSelectableProperty); }
            set { SetValue(IsSelectableProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsSelectable.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsSelectableProperty =
            DependencyProperty.Register("IsSelectable", typeof(bool), typeof(Block), new PropertyMetadata(false));


        /// <summary>
        /// Dependency property, joka kertoo käytännössä sen, onko blocking sisällä olevan tavara (nappula tässä tapauksessa) valittavissa.
        /// </summary>
        public bool IsContentSelectable
        {
            get { return (bool)GetValue(IsContentSelectableProperty); }
            set { SetValue(IsContentSelectableProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsContentSelectable.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsContentSelectableProperty =
            DependencyProperty.Register("IsContentSelectable", typeof(bool), typeof(Block), new PropertyMetadata(false));


        /// <summary>
        /// Dependency property, joka kertoo sen joiko sen sisältämää tavara (tässä nappula) valittu.
        /// </summary>
        public bool IsContentSelected
        {
            get { return (bool)GetValue(IsContentSelectedProperty); }
            set { SetValue(IsContentSelectedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsContentSelected.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsContentSelectedProperty =
            DependencyProperty.Register("IsContentSelected", typeof(bool), typeof(Block), new PropertyMetadata(false));


    }
}
