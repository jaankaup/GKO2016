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
using System.Windows.Navigation;
using System.Configuration;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Mylly
{
    /// <summary>
    /// Janne Kauppinen 2017.
    /// Copyrights: None.
    /// Yksinkertainen Settings ikkuna myllypeliä varten. Tällä hetkellä ikkuna ei skaalaudu, ja tämä johtuu valitettavasti ohjelmoijan kiireestä/laiskuudesta.
    /// TODO: Jos jää aikaa, niin laita kaikki controllit skaalautumaan.
    /// </summary>
    public partial class SettingsWindow : Window
    {
        /// <summary>
        /// Routed command enterin painallukselle.
        /// </summary>
        public static RoutedCommand EnterCommand = new RoutedCommand();

        public SettingsWindow()
        {
            InitializeComponent();

            // Asetetaan enter nappulan commandille pikanappula, eli enter.
            EnterCommand.InputGestures.Add(new KeyGesture(Key.Enter));

            // Haetaan asetukset ja asetetaan ne tämän SettingsWindow luokan dependency propertyihin.
            var P1Col = Properties.Settings.Default.P1Color;
            Player1Color = Color.FromArgb(P1Col.A, P1Col.R, P1Col.G, P1Col.B);
            var P2Col = Properties.Settings.Default.P2Color;
            Player2Color = Color.FromArgb(P2Col.A, P2Col.R, P2Col.G, P2Col.B);
            Player1Name = Properties.Settings.Default.P1Name;
            Player2Name = Properties.Settings.Default.P2Name;
            Player1Shape = Properties.Settings.Default.P1Shape;
            Player2Shape = Properties.Settings.Default.P2Shape;
            var BCol = Properties.Settings.Default.BoardColor;
            GameBoardColor = Color.FromArgb(BCol.A, BCol.R, BCol.G, BCol.B);
            CurrentMap = Properties.Settings.Default.MapType;

            // Laitetaan tänne eri muotovaihtoehdot.
            var shapes = new List<string>();
            shapes.Add("Ympyrä");
            shapes.Add("Kolmio");
            shapes.Add("Neliö");

            // Asetetaan muotovaihtoehdot dependency propertyyn, jotta voidaan sitten bindailla niihin xamlista.
            Muodot = shapes;

            // Luodaan eri kenttämahdollisuudet.
            var maps = new List<string>();
            maps.Add("Original");
            maps.Add("Custom map");

            Mapit = maps;
        }

        /// <summary>
        /// Dependency property pelilaudan tyyppiä varten.
        /// </summary>
        public List<string> Mapit
        {
            get { return (List<string>)GetValue(MapitProperty); }
            set { SetValue(MapitProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MapitProperty =
            DependencyProperty.Register("Mapit", typeof(List<string>), typeof(SettingsWindow), new PropertyMetadata(null));

        /// <summary>
        /// Dependency property tämän hetkiselle pelikentälle.
        /// </summary>
        public string CurrentMap
        {
            get { return (string)GetValue(CurrentMapProperty); }
            set { SetValue(CurrentMapProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurrentMap.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentMapProperty =
            DependencyProperty.Register("CurrentMap", typeof(string), typeof(SettingsWindow), new PropertyMetadata(null));



        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MyPropertyProperty =
            DependencyProperty.Register("MyProperty", typeof(List<string>), typeof(SettingsWindow), new PropertyMetadata(null));


        /// <summary>
        /// Dependency property eri muodoille.
        /// </summary>
        public List<string> Muodot
        {
            get { return (List<string>)GetValue(MuodotProperty); }
            set { SetValue(MuodotProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Muodot.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MuodotProperty =
            DependencyProperty.Register("Muodot", typeof(List<string>), typeof(SettingsWindow), new PropertyMetadata(null));


        /// <summary>
        /// Dependency property pelaajan 1 nimelle.
        /// </summary>
        public String Player1Name
        {
            get { return (String)GetValue(Player1NameProperty); }
            set { SetValue(Player1NameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Player1Name.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Player1NameProperty =
            DependencyProperty.Register("Player1Name", typeof(String), typeof(SettingsWindow), new PropertyMetadata("Player1"));


        /// <summary>
        /// Dependency property pelaajan 2 nimelle.
        /// </summary>
        public String Player2Name
        {
            get { return (String)GetValue(Player2NameProperty); }
            set { SetValue(Player2NameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Player2Name.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Player2NameProperty =
            DependencyProperty.Register("Player2Name", typeof(String), typeof(SettingsWindow), new PropertyMetadata("Player2"));

        /// <summary>
        /// Dependency property 1 pelaajan värille.
        /// </summary>
        public Color Player1Color
        {
            get { return (Color)GetValue(Player1ColorProperty); }
            set { SetValue(Player1ColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Player1Color.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Player1ColorProperty =
            DependencyProperty.Register("Player1Color", typeof(Color), typeof(SettingsWindow), new PropertyMetadata(Color.FromRgb(255,0,0)));


        /// <summary>
        /// Dependency property 2 pelaajan värille.
        /// </summary>
        public Color Player2Color
        {
            get { return (Color)GetValue(Player2ColorProperty); }
            set { SetValue(Player2ColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Player2Color.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Player2ColorProperty =
            DependencyProperty.Register("Player2Color", typeof(Color), typeof(SettingsWindow), new PropertyMetadata(Color.FromRgb(0,255,0)));


        /// <summary>
        /// Dependency property 1 pelaajan pelinappulan muodolle. 
        /// </summary>
        public String Player1Shape
        {
            get { return (String)GetValue(Player1ShapeProperty); }
            set { SetValue(Player1ShapeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Player1Shape.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Player1ShapeProperty =
            DependencyProperty.Register("Player1Shape", typeof(String), typeof(SettingsWindow), new PropertyMetadata("Ympyrä"));


        /// <summary>
        /// Dependeny property 2 pelaajan pelinappulan muodolle.
        /// </summary>
        public String Player2Shape
        {
            get { return (String)GetValue(Player2ShapeProperty); }
            set { SetValue(Player2ShapeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Player2Shape.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Player2ShapeProperty =
            DependencyProperty.Register("Player2Shape", typeof(String), typeof(SettingsWindow), new PropertyMetadata("Kolmio"));


        /// <summary>
        /// Dependency property pelialueen pohjaväriä varten.
        /// </summary>
        public Color GameBoardColor
        {
            get { return (Color)GetValue(GameBoardColorProperty); }
            set { SetValue(GameBoardColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for GameBoardColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty GameBoardColorProperty =
            DependencyProperty.Register("GameBoardColor", typeof(Color), typeof(SettingsWindow), new PropertyMetadata(Color.FromRgb(0,0,255)));

        /// <summary>
        /// Kun ok buttonia painetaan, niin tallennetaan asetukset.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Ok_Button_Click(object sender, RoutedEventArgs e)
        {
            // Jos on contolleilla virheitä, niin poistutaan heti.
            if (!IsValid(Ikkuna as DependencyObject)) return;
            Properties.Settings.Default.P1Name = Player1Name;
            Properties.Settings.Default.P2Name = Player2Name;
            Properties.Settings.Default.P1Color = System.Drawing.Color.FromArgb(Player1Color.A, Player1Color.R, Player1Color.G, Player1Color.B);
            Properties.Settings.Default.P2Color = System.Drawing.Color.FromArgb(Player2Color.A, Player2Color.R, Player2Color.G, Player2Color.B);
            Properties.Settings.Default.P1Shape = Player1Shape;
            Properties.Settings.Default.P2Shape = Player2Shape;
            Properties.Settings.Default.BoardColor = System.Drawing.Color.FromArgb(GameBoardColor.A, GameBoardColor.R, GameBoardColor.G, GameBoardColor.B);
            Properties.Settings.Default.MapType = CurrentMap;
            Properties.Settings.Default.Save();

            // Annetaan kutsujalle tieto siitä, että ok:ta on painettu.
            this.DialogResult = true;
            // Suljetaan koko höskä.
            this.Close();
        }

        /// <summary>
        /// Cancel nappulaa painettu. Ei tehdä mitään tallennuksia.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cancel_Button_Click(object sender, RoutedEventArgs e)
        {
            // Annetaan kutsujalle tieto siitä, että cancelia on painettu.
            this.DialogResult = false;
            // Suljetaan ikkuna.
            this.Close();
        }

        /// <summary>
        /// Enternappulan painallukselle executed metodi. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Ok_Button_Click(this, new RoutedEventArgs());
        }

        /// <summary>
        /// Enternappulan painalluksen canexecute metodi.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = IsValid(sender as DependencyObject);
        }

        /// <summary>
        /// Apumetodi netistä, joka tarkastaa onko jollakin controllilla virheitä. Palauttaa truen, jos ei ole virheitä, muutoin palauttaa falsen.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private bool IsValid(DependencyObject obj)
        {
            // The dependency object is valid if it has no errors and all
            // of its children (that are dependency objects) are error-free.
            return !Validation.GetHasError(obj) &&
            LogicalTreeHelper.GetChildren(obj)
            .OfType<DependencyObject>()
            .All(IsValid);
        }
    } // Settings window

    /// <summary>
    /// Validation rule pelaajan tekstiä varten. Ei hyväksy tyhjiä merkkijonoja.
    /// </summary>
    class PlayerNameRule : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            var s = value as String;
            if (s == null) throw new Exception("PlayerNameRule:Validate: value ei ole tyyppiä String.");

            if (s.Trim().Length == 0) return new ValidationResult(false, "Pelaajan nimi ei voi olla tyhjä merkkijono.");
            return ValidationResult.ValidResult;
        }
    }


} // namespace
