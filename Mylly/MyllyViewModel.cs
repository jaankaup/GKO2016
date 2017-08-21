using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
// using System.Drawing;
using System.Windows.Media;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Printing;
using System.Windows.Data;
using System.Windows.Shapes;
using Mylly;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*****************************************************
                           **********************    *
                           * TranslateConverter *    *
                           **********************    *
                                       |             *                    
    *******************         ******************   *
    * MainWindow(XAML)*<------->* GameNappula    *   *
    *******************         ******************   *
             |                                       *
             |                                       *
             V                                       *
    *******************         ******************   *
    * MyllyViewModel  *-------->* SettingsWindow *   *
    *******************         ******************   *
             |          \                  |         *
             |           \                 |         *
             V            \-------\        |         *
+----------------------------+     \       V         *
|  Data Modelit:             |      -> Asetukset     *                                                
|                            |                       *                                    
|  Player                    |                       *                              
|  Block                     |                       *                                   
|  Dummy                     |                       *                                    
|                            |                       *                               
+----------------------------+                       *                                                                
                                                     *
****************************************************** 

Ylhäällä on pienehkö kaavio, joka selittää hieman eri luokkien suhteesta.
Data modelit ei tiedä kenestäkään mitään. Ne vain sisältävät dataa, ja tarjoavat lähinnä dependencypropertyjä ulospäin.
Data modeleissa ei ole oikeastaan toimintalogiikkaa.

MyllyViewModel on mammutti luokka, joka sisältää käytännössä katsoen koko pelin tomintalogiikan. MyllyViewModel ei tiedä mitään maindWindowista, vaan tarjoaa 
rajapinnan peliin dependencypropertyjen ja commandien muodossa. MyllyViewModel polkaisee käyntiin SettingsWindowin, mutta ei tiedä tuosta luokasta juuri mitään. 
MyllyViewModel lataa asetuksista pelin asetukset. ViewModel rekisteröi RoutedCommandit itseensä alustusvaiheessa. MyllyViewModel tuntee ja manipuloi data modelleja, ja tarjoaa 
MainWindowille pääsyn datamodellien DependencyPropertyihin, mutta vain itsensä kautta. MainWindow ei pääse koskaan suoraan käsiksi datamodelleihin.

SettingsWindow toimii ainoastaan graafisena käyttöliittymänä pelin asetusten muuttamista varten. Se ei tiedä mitään muista luokista. SettingsWindow lataa ja tallentaa 
asetuksia.

GameNappula on pelinappulakomponentti. Se elää yhdessä MainWindowsin kanssa. GameNappula tarvitsee pelaajien väriä ja muotoId:n, mutta ne bindataan XAML:ssa MainWindowin kautta 
MyllyViewModelin rajapintaan, joka tarjoaa näkymää esimerkiksi pelaajaan. GameNappula ei siis ole suoraan yhteydessä MyllyViewModeliin vaan ainoastaan MainWindowin kautta.

MainWindow on hoitaa kaiken visuaalisen puolen ja käyttäjän kanssa keskustelun. MainWindow käyttää MyllyViewModelin rajapintaa, mutta ei sisällä lainkaan älyä. Se vain reagoi 
MyllyViewModelin dependencypropertyjen muutoksiin, ja interaktio pelaajan ja myllyviewmodelin välityksellä menee RoutedCommandien muodossa. Commandit vievät siis käyttäjän valikoiden sekä 
pelialustaan kohdistuvien hiiren- sekä pikanäppäinten painallukset MyllyViewModelin tietoon, joka sitten pyörittää peliä saamansa tiedon perusteella. MyllyViewModel asetetaan 
ohjelman App.xaml resourceen, josta se sitten asetetaan MainWindowin Datacontextiin. Samalla MyllyViewModelin RoutedCommandin bindataan xamlissa Attached propertyn avulla. Näin ei tarvitse tehdä 
erillisiä CommandBindauksien määrityksiä MainWindowin resourcessa. Tosin, jos sitten koodissa halutaan viitata ViewModelin commandeihin, täytyy siellä viitata suoraan MyllyViewModelin 
Commandiin.

TranslateConverteria käytetään apuna, kun keskitetään piirretäviä komponettejä.

Pelilauta koostuu grideistä, joissa jokaisen gridin sisällä on Block-luokan olio. Block olio pitää kirjaa dependencypropertyjen muodossa siitä, mihin suuntiin siitä voi kulkea, sisältääkö se 
objectin tai nappulan. Lisäksi se tietää kuka sen omistaa (Player) ja sen, miten ko. block on valittu (onko se pelimerkkipaikan suhteen valittu, onko "nappula" valittu, tai onko joko pelipaikka/nappula 
valittavissa. Block olio ei tiedä mikä sen sisällä oleva pelimerkki on, vaan ko. property on ainoastaan boolean arvo. Block on siis helposti bindattavissa, ja käyttöliittymäluokat pääsevät sen propertyihin 
käsiksi, mutta ei suoraan, vaan ainoastaan MyllyViewModelin kautta. 

Player luokka on vain data luokka, joka sisältää dependencypropertyjä kuten pelaajan väri, pelimerkin muodo Id jne.

Dummy luokka ei tee mitään eikä omaa mitään dataa. Sitä käytetään vain lähinnä pelaajan "kädessä" olevien pelimerkkien lukumääränä, jonka MainWindow lataa XAML:ssa Viewmodelin kautta.

Tässä on siis pyritty käyttämään MVVM mallia. Kyseessä on 3-taso arkkitehtuuri, jossa ylin taso on käyttöliittymä, keskitaso on toiminta logiikka ja alin taso koostuu data malleista. 
Todellisuudessa käyttöliittymiä ja viewmodeleita voi olla useita, viewmodel voi omistaa tosia viewmodeleita ja erilaisia tietokantoja voi olla siellä sun täällä. Mutta tässä on hyvin 
pelkistetty versio MVVM mallista. MVVM mallissa käyttöliittymä näkee ainoastaan viewmodellin, ja käyttää sen tarjoamaa rajapintaa. ViewModel näkee ainastaan datamallit, ja niitä hyväksi käyttäen tarjoaa 
rajapinna ylöspäin (käyttöliittymälle). Datamallit eivät tiedä mitään ViewModelista eikä käyttäliittymistä. Eli 3-taso arkkitehtuurissa näkyvyys suunta on ainoastaan alaspäin, eikä minkään kerroksen yli voi 
loikata. Tässä ohjelmassa ja muutoinkin MVVM:ssä ViewModel asetetaan MainWindowin datacontekstiin. DataConteksti periytyy ikkunalta alemmille componeteille, joten näin alempien käyttöliittymäkomponettien 
pääsy ViewModeliin helpottuu (ainakin teoriassa). 

Tässä ohjelmassa käyttöliitymää manipuloiva koodi on pidetty minimissään. Itseasiassa ohjelmassa ei pitäisi olla yhtään riviä c# koodia, joka manipuloi käyttöliitymä koodia. Pelaajien ja pelikentän väri 
on data malleissa, ja viewmodel ainoastaan tarjoaa rajapinnan noihin propertyihin. Tai c# koodia on kyllä hieman, mutta se on converterin muodossa. Oikeasti on järkevää myös hyödyntää c# koodia, vaikka 
tämän ohjelman tekijä ei sitä apua ottanutkaan käyttöönsä. Tämän seurauksena esimerkiksi animaatio (heiluva voittajan nimi) ei skaalaudu, kun ikkunan kokoa muutetaan. Ehkä jotain animaatio-luokkia tai 
ylipäätään graafisen puolen tapahtuman käsittelyä varten kannattaisi luoda omia luokkia, joita MainWindow sitten käyttää. MVVM malli on ihan kätevän oloinen keksintö, kunhan sitä käyttää järkevästi, 
eli erottaa käyttöliitymän toimintalogiikasta ja datamalleista.

*******************************************************************************************************/

namespace Mylly
{
    /// <summary>
    /// Janne Kauppinen 2017.
    /// Copyrights: None.
    /// 
    /// View-model luokka myllysovellukselle. Sisältää kaiken myllypelin toimintalogiikan, suurimman osan 
    /// commandeista, käyttää/kapseloi data modelleja ja tarjoaa xaml:lle rajapinnan. Ei ota kantaa miten piirretään, vaikka säilyttää propertyissä pelaajien pelinappulan värit, nappulan muodon id:n ja  
    /// pelikentän värin. ViewModel määrittää 
    /// ainoastaan pelikentän muodon ja miten data on yhteyksissä toisiinsa, mutta ei ota lainkaan kantaa siihen, kuinka itse peli piirretään tai miten käyttäjän kanssa kommunikoidaan. 
    /// ViewModel tarjoaa vain rajapinnan peliin, ja pyörittää peliä ilman mitään visuaalisuutta. 
    /// Rajapinnan kautta xaml-koodi päivittää tietoja ViewModellin ja pelin muutokset xamlista bindataan view-modeliin. 
    /// View Model ei ole tietoinen xaml:sta, eivätä datamallit ole tietoisia viewmodellista.
    /// Teetätti ihan liikaa työtä tehdä tämä tällä tavalla, mutta tulipahan nyt tehtyä näin. Jäipähän edes jonkinlainen 
    /// käsitys siitä, mikä on mvvm. Tästä luokasta tuli pitkä kuin nälkävuosi.
    /// </summary>
    public class MyllyViewModel : DependencyObject
    {

        /// <summary>
        /// Enum tyyppi pelin sen hetkiselle tilalle. GameStart tilaa ei käytetä tässä pelissä. mutta se on nyt jätetty siihen siltä varalta, että sille on jotain hyödyllistä käyttöä, 
        /// kuten esim. kun peli aukaistaan niin jokin tervetuloa animaatio, esim. pelin tekijät tmv.
        /// </summary>
        private enum GameState { InsertState = 0, RemoveState = 1, MoveState = 2, GameOver = 3, GameStart = 4 }

        /// <summary>
        /// Enum erilaisia blokkien selectioita varten.
        /// </summary>
        private enum SelectionEnum { /*IsSelected = 1,*/ IsSelectable=2, IsContentSelected=4, IsContentSelectable=8}

        /// <summary>
        /// Enum tyyppi pelaajan tilalle. Tässä on nyt CurrentPlayer, joka kertoo sen, että pelaajalla on vuoro. Opponent taas kertoo sen, että ei ole pelaajan vuoro.
        /// </summary>
        private enum PlayerState { CurrentPlayer, Opponent}

        /// <summary>
        /// Enum pelikenttää varten. Hieman jäykkä ratkaisu, mutta olkoon tässä pelissä nyt näin.
        /// Original on tavallinen pelikenttä ja Exotic on hieman erilainen pelikenttä.
        /// </summary>
        private enum GameMap { Original = 0, Exotic = 1}

        /// <summary>
        /// Property, joka pitää kirjaa siitä, mikä myllypelipohja on käytössä. 
        /// Muuttamalla suoraan tätä propertyä ei saada muutoksia vastaaviin dependency propertyihin.
        /// Muutos tulee voimaan vasta CreateNewGame metodia kutsumalla. Olisihan tästäkin voinnut tehdä dependency 
        /// propertyn ja sitten bindata kooditasolla vastaaviin dependency propertyihin, mutta olkoon nyt näin.
        /// </summary>
        private GameMap CurrentGameMapEnum { get; set; }
        
        /// <summary>
        /// Rakentaja.
        /// </summary>
        public MyllyViewModel()
        {
            // Luodaan kooditasolla command bindingit (ainakin ne, jotka liittyvät pelin toimintaan). Näin pidetään 
            // toimintalogiikka poissa xaml:sta ja windows-luokasta.
            CommandBindings = CreateCommmandBindings();
            // Asetetaan aluksi perinteinen myllypelipohja.
            CurrentGameMapEnum = GameMap.Exotic;

            // Luodaan ensimmäinen pelaaja.
            Player1 = new Player(); ;

            // Luodaan toinen pelaaja.
            Player2 = new Player();

            // Ladataan asetukset.
            UpdateSettings();

            // Luodaan uusi peli, eli asetetaan peli sellaiseen tilaan että voidaan aloittaa pelaaminen.
            CreateNewGame();
        }

        /// <summary>
        /// Dependency property pelikentän palasille.
        /// </summary>
        public ObservableCollection<Block> MVBlocks
        {
            get { return (ObservableCollection<Block>)GetValue(MVBlocksProperty); }
            set { SetValue(MVBlocksProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MVBlocks.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MVBlocksProperty =
            DependencyProperty.Register("MVBlocks", typeof(ObservableCollection<Block>), typeof(MyllyViewModel), new PropertyMetadata(null));


        /// <summary>
        /// Dependencyproperty, joka kertoo sen kuinka monta pelinappulaa on pelin aloituksessa.
        /// </summary>
        public int StartingPieceCount
        {
            get { return (int)GetValue(StartingPieceCountProperty); }
            set { SetValue(StartingPieceCountProperty, value); }
        }

        // Using a DependencyProperty as the backing store for StartingPieceCount.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StartingPieceCountProperty =
            DependencyProperty.Register("StartingPieceCount", typeof(int), typeof(MyllyViewModel), new PropertyMetadata(9));

        /// <summary>
        /// Dependency property pelikentän rivien määrää varten.
        /// </summary>
        public int GameRowCount
        {
            get { return (int)GetValue(GameRowCountProperty); }
            set { SetValue(GameRowCountProperty, value); }
        }

        // Using a DependencyProperty as the backing store for GameRowCount.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty GameRowCountProperty =
            DependencyProperty.Register("GameRowCount", typeof(int), typeof(MyllyViewModel), new PropertyMetadata(0));

        /// <summary>
        /// Dependency property pelikentän sarakkeiden määrää varten.
        /// </summary>
        public int GameColumnCount
        {
            get { return (int)GetValue(GameColumnCountProperty); }
            set { SetValue(GameColumnCountProperty, value); }
        }


        /// <summary>
        /// Dependencyproperty, joka kertoo pelin tilanteen. Voi käyttää esim. tulosteena.
        /// </summary>
        public string GameMessage
        {
            get { return (string)GetValue(GameMessageProperty); }
            set { SetValue(GameMessageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for GameMessage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty GameMessageProperty =
            DependencyProperty.Register("GameMessage", typeof(string), typeof(MyllyViewModel), new PropertyMetadata(""));

        /// <summary>
        /// Dependencyproperty pelilaudan taustavärille. Eli ottaa tämä viewmodel hieman kantaa siihen, miten piirretään. 
        /// </summary>
        public Brush GameBoardColor
        {
            get { return (Brush)GetValue(GameBoardColorProperty); }
            set { SetValue(GameBoardColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for GameBoardColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty GameBoardColorProperty =
            DependencyProperty.Register("GameBoardColor", typeof(Brush), typeof(MyllyViewModel), new PropertyMetadata(null));



        // Using a DependencyProperty as the backing store for GameColumnCount.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty GameColumnCountProperty =
            DependencyProperty.Register("GameColumnCount", typeof(int), typeof(MyllyViewModel), new PropertyMetadata(0));

        /// <summary>
        /// Dependency property pelaajan "kädessä" oleville nappuloille. Tämä dependency property pitää käytännössä sisällään Dummy luokan olioita, joissa ei 
        /// tällä hetkellä ole mitään dataa. Tämä on siitä syystä, että nämä "pelimerkit" ladataa xamliin, ja xaml saa päättää miten nämä Dummy-oliot piirretään.
        /// </summary>
        public ObservableCollection<Dummy> Player1Table
        {
            get { return (ObservableCollection<Dummy>)GetValue(Player1TableProperty); }
            set { SetValue(Player1TableProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Player1Table.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Player1TableProperty =
            DependencyProperty.Register("Player1Table", typeof(ObservableCollection<Dummy>), typeof(MyllyViewModel), new PropertyMetadata(null));

        /// <summary>
        /// Vastaavanlainen dependency property "kädessä" oleville nappuloille kuin 1 pelaajalla. Katso ykköspelaajan dependencypropertyn dokumentointi.
        /// </summary>
        public ObservableCollection<Dummy> Player2Table
        {
            get { return (ObservableCollection<Dummy>)GetValue(Player2TableProperty); }
            set { SetValue(Player2TableProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Player2TableProperty =
            DependencyProperty.Register("Player2Table", typeof(ObservableCollection<Dummy>), typeof(MyllyViewModel), new PropertyMetadata(null));



        // DEPENCENCY PROPERTYT PELAAJIA VARTEN.

        /// <summary>
        /// Dependency property 1. pelaajaa varten. Kaipa tämä on ihan käypä ratkaisu tässä, kun ei ole 
        /// enempää kuin kaksi pelaajaa.
        /// </summary>
        public Player Player1
        {
            get { return (Player)GetValue(Player1Property); }
            set { SetValue(Player1Property, value); }
        }

        // Using a DependencyProperty as the backing store for Player1.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Player1Property =
            DependencyProperty.Register("Player1", typeof(Player), typeof(MyllyViewModel), new PropertyMetadata(null));

        /// <summary>
        /// Dependency property 2. pelaajaa varten.
        /// </summary>
        public Player Player2
        {
            get { return (Player)GetValue(Player2Property); }
            set { SetValue(Player2Property, value); }
        }

        // Using a DependencyProperty as the backing store for Player2.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Player2Property =
            DependencyProperty.Register("Player2", typeof(Player), typeof(MyllyViewModel), new PropertyMetadata(null));

        // TILAT OVAT NYT ERITELTYNÄ DEPENDENCY PROPERTYINÄ. Tämä helmpottaa huomattavasti xaml-koodia.
        // TILOJA EI SAA MUUTTAA SUORAAN ASETAMALLA PROPERTYJEN ARVOJA. TILAT TÄYTYY MUUTTAA KÄYTTÄMÄLLÄ CHANGESTATE-METODIA. 
        // MUUSSA TAPAUKSESSA OHJELMAN LOGIIKKA MENEE RIKKI. TODO: jos jää aikaa, niin määrittele paremman säännöt pelin tiloja koskeville 
        // propertyille. Esim, että muutokset eivät tule voimaan, jos tiloja yrittää muuttaa kukaan muu kuin ViewModel, tai jotain muuta vastaavaa.

        /// <summary>
        /// Dependency property, joka kertoo sen onko peli tila insert modessa.
        /// </summary>
        public bool IsInsertMode
        {
            get { return (bool)GetValue(IsInsertModeProperty); }
            set { SetValue(IsInsertModeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsInsertMode.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsInsertModeProperty =
            DependencyProperty.Register("IsInsertMode", typeof(bool), typeof(MyllyViewModel), new PropertyMetadata(false));

        /// <summary>
        /// Dependency property, joka kertoo sen onko peli remove piece tilassa.
        /// </summary>
        public bool IsRemoveMode
        {
            get { return (bool)GetValue(IsRemoveModeProperty); }
            set { SetValue(IsRemoveModeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsRemoveMode.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsRemoveModeProperty =
            DependencyProperty.Register("IsRemoveMode", typeof(bool), typeof(MyllyViewModel), new PropertyMetadata(false));


        /// <summary>
        /// Dependency property, joka kertoo sen onko peli move tilassa.
        /// </summary>
        public bool IsMoveMode
        {
            get { return (bool)GetValue(IsMoveModeProperty); }
            set { SetValue(IsMoveModeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsMoveMode.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsMoveModeProperty =
            DependencyProperty.Register("IsMoveMode", typeof(bool), typeof(MyllyViewModel), new PropertyMetadata(false));


        /// <summary>
        /// Dependency property, joka kertoo sen onko peli game over tilassa.
        /// </summary>
        public bool IsGameOver
        {
            get { return (bool)GetValue(IsGameOverProperty); }
            set { SetValue(IsGameOverProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsGameOver.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsGameOverProperty =
            DependencyProperty.Register("IsGameOver", typeof(bool), typeof(MyllyViewModel), new PropertyMetadata(false));

        /// <summary>
        /// Dependency property, joka kertoo sen onko peli begin tilassa.
        /// </summary>
        public bool IsGameBegin
        {
            get { return (bool)GetValue(IsGameBeginProperty); }
            set { SetValue(IsGameBeginProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsGameBeginProperty =
            DependencyProperty.Register("IsGameBegin", typeof(bool), typeof(MyllyViewModel), new PropertyMetadata(false));

        /// <summary>
        /// Dependency property, joka kertoo kuka on voittaja.
        /// </summary>
        public Player TheWinner
        {
            get { return (Player)GetValue(TheWinnerProperty); }
            set { SetValue(TheWinnerProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TheWinner.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TheWinnerProperty =
            DependencyProperty.Register("TheWinner", typeof(Player), typeof(MyllyViewModel), new PropertyMetadata(null));


        // ROUTED COMMANDIT.

        /// <summary>
        /// Routed command uuden pelin aloittamista varten.
        /// </summary>
        public static readonly RoutedUICommand NewGame
            = new RoutedUICommand("New Game", "New Game", typeof(MyllyViewModel), new InputGestureCollection()
                    { new KeyGesture(Key.N,ModifierKeys.Alt)});

        /// <summary>
        /// Routed command yhden pelitilanteen tulostamista varten. Jos on gameover tilassa, niin tulostaa myös pomppivan tekstin. TODO: Jos jää liikaa häiritsemään, niin korjaa.
        /// </summary>
        public static readonly RoutedUICommand Print
            = new RoutedUICommand("Print", "Print", typeof(MyllyViewModel), new InputGestureCollection()
                    { new KeyGesture(Key.P,ModifierKeys.Control)});

        /// <summary>
        /// Routed command kaikkien pelitilanteiden tulostamista varten.
        /// </summary>
        public static readonly RoutedUICommand PrintFull
            = new RoutedUICommand("Print Full", "Print Full", typeof(MyllyViewModel), new InputGestureCollection()
                    { new KeyGesture(Key.F,ModifierKeys.Control)});

        /// <summary>
        /// Routed command nappulan lisäämistä varten. TODO: poista. 
        /// </summary>
        public static readonly RoutedUICommand InsertPiece
            = new RoutedUICommand("Insert Piece", "Insert Piece", typeof(MyllyViewModel), new InputGestureCollection()
                    { new KeyGesture(Key.I,ModifierKeys.Alt)});

        /// <summary>
        /// Routed command nappulan poistamista varten. TODO: poista.
        /// </summary>
        public static readonly RoutedUICommand RemovePiece
            = new RoutedUICommand("Remove Piece", "Remove Piece", typeof(MyllyViewModel), new InputGestureCollection()
                    { new KeyGesture(Key.R,ModifierKeys.Alt)});

        /// <summary>
        /// Routed command nappulan liikuttamista varten. TODO: poista.
        /// </summary>
        public static readonly RoutedUICommand MovePiece
            = new RoutedUICommand("Move Piece", "Move Piece", typeof(MyllyViewModel), new InputGestureCollection()
                    { new KeyGesture(Key.M,ModifierKeys.Alt)});

        /// <summary>
        /// Routed command sille, kun valitaan ohjelmassa valitaan block. Nimi on hieman harhaanjohtava, sillä sen alkuperäinen tarkoitus oli toinen, mutta yritetään elää tämän nimen kanssa.
        /// </summary>
        public static readonly RoutedCommand InsertPieceSelection
            = new RoutedCommand("InsertPieceSelection",typeof(MyllyViewModel));

        /// <summary>
        /// Routed command settings painikkeelle. Täällä pitäisi siis aueta Settings dialogi.
        /// </summary>
        public static readonly RoutedUICommand SettingsCommand
            = new RoutedUICommand("Settings", "Settings", typeof(MyllyViewModel), new InputGestureCollection()
                    { new KeyGesture(Key.S,ModifierKeys.Alt)});

        /// <summary>
        /// Routed command aboutille.
        /// </summary>
        public static readonly RoutedUICommand AboutCommand
            = new RoutedUICommand("About", "About", typeof(MyllyViewModel), new InputGestureCollection()
                    { new KeyGesture(Key.U,ModifierKeys.Control)});

        /// <summary>
        /// Routed command helpille.
        /// </summary>
        public static readonly RoutedUICommand HelpCommand
            = new RoutedUICommand("Help", "Help", typeof(MyllyViewModel), new InputGestureCollection()
                    { new KeyGesture(Key.F1)});

        /// <summary>
        /// Apu metodi, jossa luodaan mylly-sovelluksen command bindaukset. Huomaa, että command bindingit 
        /// rekisteröidään nimenomaan viewmodel luokan otuksiin, ei window luokkaan! Tomintalogiikka vm:ään.
        /// </summary>
        /// <returns>Kaikki command bindaukset.</returns>
        private CommandBindingCollection CreateCommmandBindings()
        {
            // Vähän on copypasten kaltaista toimintaa, mutta ei uuden binding-register metodin luominenkaan 
            // oikein tuntunut järkevältä ratkaisulta.
            var commandCollection = new CommandBindingCollection();

            // Uuden pelin aloittamis command bindaus.
            CommandBinding newGameBinding = new CommandBinding(NewGame);
            newGameBinding.CanExecute += NewGameCommand_CanExecute;
            newGameBinding.Executed += NewGameCommand_Executed;
            CommandManager.RegisterClassCommandBinding(typeof(MyllyViewModel), newGameBinding);
            commandCollection.Add(newGameBinding);

            // Printtaus commanding bindaus.
            CommandBinding printBinding = new CommandBinding(Print);
            printBinding.CanExecute += PrintCommand_CanExecute;
            printBinding.Executed += PrintCommand_Executed;
            CommandManager.RegisterClassCommandBinding(typeof(MyllyViewModel), printBinding);
            commandCollection.Add(printBinding);

            // Printtaa kaikki pelitilanteet bindaus.
            CommandBinding printFullBinding = new CommandBinding(PrintFull);
            printFullBinding.CanExecute += PrintFullCommand_CanExecute;
            printFullBinding.Executed += PrintFullCommand_Executed;
            CommandManager.RegisterClassCommandBinding(typeof(MyllyViewModel), printFullBinding);
            commandCollection.Add(printFullBinding);

            // Nappulan lisäys command bindaus.
            CommandBinding insertPieceBinding = new CommandBinding(InsertPiece);
            insertPieceBinding.CanExecute += InsertPieceCommand_CanExecute;
            insertPieceBinding.Executed += InsertPieceCommand_Executed;
            CommandManager.RegisterClassCommandBinding(typeof(MyllyViewModel), insertPieceBinding);
            commandCollection.Add(insertPieceBinding);

            // Nappulan poisto command bindaus.
            CommandBinding removePieceBinding = new CommandBinding(RemovePiece);
            removePieceBinding.CanExecute += RemovePieceCommand_CanExecute;
            removePieceBinding.Executed += RemovePieceCommand_Executed;
            CommandManager.RegisterClassCommandBinding(typeof(MyllyViewModel), removePieceBinding);
            commandCollection.Add(removePieceBinding);

            // Nappulan siirto command bindaus.
            CommandBinding movePieceBinding = new CommandBinding(MovePiece);
            movePieceBinding.CanExecute += MovePieceCommand_CanExecute;
            movePieceBinding.Executed += MovePieceCommand_Executed;
            CommandManager.RegisterClassCommandBinding(typeof(MyllyViewModel), movePieceBinding);
            commandCollection.Add(movePieceBinding);

            // InsertModessa tapahtuvan nappulapaikan valinnan commandin bindaus.           
            CommandBinding insertPieceSelectionBinding = new CommandBinding(InsertPieceSelection);
            insertPieceSelectionBinding.CanExecute += InsertPieceSelection_CanExecute;
            insertPieceSelectionBinding.Executed += InsertPieceSelection_Executed;
            CommandManager.RegisterClassCommandBinding(typeof(MyllyViewModel), insertPieceSelectionBinding);
            commandCollection.Add(insertPieceSelectionBinding);

            // Settings dialogiin liittyä commandi.
            CommandBinding settingsBinding = new CommandBinding(SettingsCommand);
            settingsBinding.CanExecute += Settings_CanExecute;
            settingsBinding.Executed += Settings_Executed;
            CommandManager.RegisterClassCommandBinding(typeof(MyllyViewModel), settingsBinding);
            commandCollection.Add(settingsBinding);

            // About dialogiin liittyä commandi.
            CommandBinding aboutBinding = new CommandBinding(AboutCommand);
            aboutBinding.CanExecute += About_CanExecute;
            aboutBinding.Executed += About_Executed;
            CommandManager.RegisterClassCommandBinding(typeof(MyllyViewModel), aboutBinding);
            commandCollection.Add(aboutBinding);

            // Help dialogiin liittyä commandi.
            CommandBinding helpBinding = new CommandBinding(HelpCommand);
            helpBinding.CanExecute += Help_CanExecute;
            helpBinding.Executed += Help_Executed;
            CommandManager.RegisterClassCommandBinding(typeof(MyllyViewModel), helpBinding);
            commandCollection.Add(helpBinding);

            return commandCollection;
        }

        /// <summary>
        /// Seuraava attached property ja sen korjaus on apinoitu osoitteista:
        /// https://codingcontext.wordpress.com/2008/12/10/commandbindings-in-mvvm/ ja 
        /// matthamilton.net/commandbindings-with-mvvm. Nyt view-modellin command bindsejä voi bindailla ja 
        /// puliveivailla puolin ja toisin xamlissa huolettomasti.
        /// </summary>
        public static DependencyProperty RegisterCommandBindingsProperty 
            = DependencyProperty.RegisterAttached("RegisterCommandBindings", 
                typeof(CommandBindingCollection), typeof(MyllyViewModel), 
                new PropertyMetadata(null, OnRegisterCommandBindingChanged));

        /// <summary>
        /// Toisen koodia. Katso ylös.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="value"></param>
        public static void SetRegisterCommandBindings(UIElement element, CommandBindingCollection value)
        {
            if (element != null)
                element.SetValue(RegisterCommandBindingsProperty, value);
        }

        /// <summary>
        /// Toisen koodia. Katso ylös.
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static CommandBindingCollection GetRegisterCommandBindings(UIElement element)
        {
            return (element != null ? (CommandBindingCollection)element.GetValue(RegisterCommandBindingsProperty) : null);
        }

        /// <summary>
        /// Toisen koodia. Katso ylös.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnRegisterCommandBindingChanged
        (DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            UIElement element = sender as UIElement;
            if (element != null)
            {
                CommandBindingCollection bindings = e.NewValue as CommandBindingCollection;
                if (bindings != null)
                {
                    element.CommandBindings.Clear();
                    element.CommandBindings.AddRange(bindings);
                }
            }
        }

        /// <summary>
        /// Dependency property Command bindingseihin. 
        /// </summary>
        public CommandBindingCollection CommandBindings
        {
            get { return (CommandBindingCollection)GetValue(CommandBindingsProperty); }
            set { SetValue(CommandBindingsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommandBindingsProperty =
            DependencyProperty.Register("CommandBindingsProperty", typeof(CommandBindingCollection), typeof(MyllyViewModel), new PropertyMetadata(null));

        /// <summary>
        /// Uuden pelin aloituscommandin canexecute metodi.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewGameCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        /// <summary>
        /// Uuden pelin aloituscommandin executed metodi.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewGameCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            CreateNewGame();
        }

        /// <summary>
        /// CanExecute print commandille.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PrintCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;

            // Jos pelilaudalla ei ole kenenkään pelaajan nappuloita, niin tällöin print-toiminto ei ole käytössä (kuten tason 2 vaatimuksessa on).
            if (GetGameBoardPieceCount(GetPlayer(PlayerState.CurrentPlayer)) == 0 && GetGameBoardPieceCount(GetPlayer(PlayerState.Opponent)) == 0) e.CanExecute = false;
        }

        /// <summary>
        /// Tulostaa nyt tällä hetkellä olevan pelitilanteen. Hieman tökerö toteutus, eli skaalataan ikkuna kokoo 3500/3000 tulostuksen ajaksi jonka jälkeen palautetaan ikkunan koko alkuperäiseksi.
        /// Homma toimii kuitenkin.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PrintCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Window w = sender as Window;
            if (w == null) return;

            PrintDialog pd = new PrintDialog();

            if (pd.ShowDialog() == true)
            {
                // Otetaan ikkunan alkuperäinen koko talteen.
                //var originalHeight = w.ActualHeight;
                //var originalWidth = w.ActualWidth;

                // Määritellään kuvan konteksti.
                RenderTargetBitmap targetBitmap = new RenderTargetBitmap((int)w.ActualWidth, 
                                                                         (int)w.ActualHeight,
                                                                         96d, 96d,
                                                                         PixelFormats.Default);
                // "Piirretään" ikkuna kuvakontekstiin.
                targetBitmap.Render(w);

                // Luodaan stackpanel, jonka sisältö sitten tulostetaan.
                StackPanel st = new StackPanel();
                st.Margin = new Thickness(15);

                // Mylly teksti.
                TextBlock myllyText = new TextBlock();
                myllyText.FontSize = 24;
                myllyText.Text = "MYLLY";
                myllyText.TextAlignment = TextAlignment.Center;
                st.Children.Add(myllyText);

                // Luodaan image, Joka otetaan mukaan tulostukseen.
                Image image = new Image();
                image.Width = w.ActualWidth;
                image.Height = w.ActualHeight;
                image.Source = targetBitmap;

                // Laitetaan image mahtumaan! Toimii nyt ainakin tässä.
                Viewbox bx = new Viewbox();
                bx.Child = image;

                st.Children.Add(bx);

                // Skaalataan stackpanel vastaamaan tulostuksen kokoa.
                st.Measure(new Size(pd.PrintableAreaWidth, pd.PrintableAreaHeight));
                st.Arrange(new Rect(new Point(0, 0), st.DesiredSize));       
                
                // Tulostetaan.
                pd.PrintVisual(st, "My Print");

                // Palautetaan ikkuna takaisin oikeaan kokoon.
                //w.Height = originalHeight;
                //w.Width = originalWidth;          
            }
        }

        /// <summary>
        /// Kaikkien tilanteiden tulostus canexecute metodi. Ei toteutettu.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PrintFullCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;
        }

        /// <summary>
        /// Ei toteutettu.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PrintFullCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            // MessageBox.Show("Älä printaa kaikkia sivuja. Säästä luontoa!");
        }

        /// <summary>
        /// Pelinappulan lisäämiscommandin canexecute metodi. On turha tässä ohjelmassa, sillä nappuloiden lisäämistä ei hoideta valikon kautta.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InsertPieceCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;
        }

        /// <summary>
        /// Pelinappulan lisäämiscommandin exetuted metodi. Ei tee mitään! Tämä ominaisuus on kokonaan korvattu insertpieceselection commandilla.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InsertPieceCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }

        /// <summary>
        /// Ei tee mitään järkevää. Nappuloiden poistoa ei hoideta tämän kommandin kautta.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RemovePieceCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;
        }

        /// <summary>
        /// Nappuloita ei poisteta remove menun kautta. Ei siis toteutettu.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RemovePieceCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            
        }

        /// <summary>
        /// Nappuloita ei siirretä menun kautta. Ei käytössä.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MovePieceCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;
        }

        /// <summary>
        /// Nappuloita ei siirretä menun kautta. Ei toteutettu.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MovePieceCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            
        }

        /// <summary>
        /// Metodi InsertPieceSelection commandille. Tässä oletetaan, että CanExecuteRoutetEventArgsissa
        /// e.Param on klikattu block. Tyhmä nimi tälle metodille, sillä tämä metodi ottaa vastaan kaikki blokkeihin kohdistuvat klikkaukset. TODO: jos jää aikaa, niin 
        /// anna metodille parempi nimi.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InsertPieceSelection_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;

            // Kaikissa muissa paitsi GameOver tilassa voidaan valita blokki klikattavaksi.
            if (!IsGameOver)
            {
                e.CanExecute = true;
            }
        }

        /// <summary>
        /// Sisältää kaikki pelikentän blokkien klikkaukseen liittyvät toimenpiteet, eli varsin paljon logiikkaa tässä metodissa.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InsertPieceSelection_Executed(object sender, ExecutedRoutedEventArgs e)
        {

            Block block = e.Parameter as Block;
            if (block == null) return;

            // Selvitetään kuka on pelivuorossa.
            Player currentPlayer = GetPlayer(PlayerState.CurrentPlayer);

            // Jos ollaan insert-modessa, niin...
            if (IsInsertMode)
            {
                // Asetetaan vapaat nappulapaikat vapaaksi riistaksi.
                ShowAllFreeBlocks();

                // Tässä oletetaan, että vain toisella on pelivuoro käynnissä.
                switch (Player1.HasTurn)
                {
                    case true:
                        if (Player1Table.Count == 0) throw new Exception("InsertPieceSelection_Executer:Player1Table.Count == 0. Looginen virhe. Ei voi olla lisäystilassa jos napit on jo käytetty");
                        break;
                    case false:
                        if (Player2Table.Count == 0) throw new Exception("InsertPieceSelection_Executer:Player2Table.Count == 0. Looginen virhe. Ei voi olla lisäystilassa jos napit on jo käytetty");
                        break;
                }

                // Yritetään lisätä merkkiä toisen merkin päälle. Ei käy.
                if (block.HasPiece) return;

                // Vähennetään pelaajan "kädessä" olevaa nappulamäärää yhdellä.
                if (Player1.HasTurn && Player1Table.Count > 0) Player1Table.Remove(Player1Table.Last());
                else if (Player2Table.Count > 0) Player2Table.Remove(Player2Table.Last());

                // Asetetaan blocking omistajaksi current player. Tässä siis blockista tulee pelaajan omistama.
                block.BlockOwner = currentPlayer;
                // Asetetaan kyseinen block ei-valittavaan tilaan.
                block.IsSelectable = false;
                // Nyt on blokissa nappula.
                block.HasPiece = true;
            }
            // Jos ollaan remove tilassa, niin...
            else if (IsRemoveMode)
            {
                // Jos block on IsContentSelectable == true tilassa, niin kyseessä on poistettavissa oleva nappula. Tässä luotetaan siihen, että vain poistettavissa
                // olevilla blockeilla on kyseinen property asetettuna trueksi.
                if (block.IsContentSelectable) {
                    // Tehdään tarvittavat toimenpiteet "poistetulle" nappulalle. 
                    block.BlockOwner = null;
                    block.HasPiece = false;
                    // Poistetaan kaikki nappuloiden valinnat ym.
                    ClearAllSelectables();
                    // Jos jää aikaa, niin paranna kontrollia. Tosin eihän gotossa ole mitään vikaa, kun sitä osaa käyttää...
                    goto loppu;
                }
                // Yritettiin valita jotain, mikä ei ollut valittavissa. Poistutaan.
                return;
            }

            // Jos ollaan move tilassa.
            else if (IsMoveMode)
            {
                // Tutkitaan onko siirrettävää nappula jo valittu.
                int selectionCount = 0;
                Block moveablePiece = new Block();
                foreach (var item in MVBlocks)
                {
                    // Tässä olisi voinnut break:ta ensimmäiseen nappulaan, joka on valittuna, mutta debuggaus syistä tarkistetaan kaikki nappulat.
                    if (item.IsContentSelected) { selectionCount++; moveablePiece = item; }
                }
                if (selectionCount > 1) throw new Exception("InsercPiece_Executed: Ollaan IsMove tilassa, mutta pelaaja on muka valinnut useita nappuloita. Peli bugittaa.");

                // Tsekataan, onko pelaaja jo valinnut jonkin siirretävän nappulan. Oletetaan nyt, että ohjelma toimii, ja että sellaiset blockit, jonne nappulan voi siirtää on jo asetettu trueksi.
                if (selectionCount == 1)
                {
                    // Löytyi uusi paikka nappulalle!
                    if (block.IsSelectable)
                    {
                        // Swapataan vanhan blockin ja uuden blockin arvot. Nappula on tämän jälkeen "siirretty" uuteen paikkaan.
                        block.BlockOwner = moveablePiece.BlockOwner; moveablePiece.BlockOwner = null;
                        block.HasPiece = true; moveablePiece.HasPiece = false;
                        // Poistetaan kaikki valinnat.
                        ClearAllSelectables();
                        goto myllyTarkistus;
                    }
                }

                if (!block.IsContentSelectable) return;
                // Hävitetään edelliset blokkeihin liittyvät tarpeettomat selection asetukset.
                ClearSelections((int)SelectionEnum.IsContentSelected |(int)SelectionEnum.IsSelectable);
                // Nyt tämä nappula on valittu.
                block.IsContentSelected = true;

                // Etsitään kaikki mahdolliset siirrettävät kohteet ja asetetaan ne mahdollisiksi kohteiksi.
                CheckPossipleMoves(block,true, currentPlayer);
                   
                return;
            }

            // Eli tänne hypätään, kun halutaan tarkistaa mylly.
            myllyTarkistus:

            // Tarkastetaan tuliko mylly. Jos tulee mylly, niin siirrytään remove stateen.
            if (CheckMill(block))
            {
                ChangeState(GameState.RemoveState);
                return;
            }

            // Tänne saavutaan silloin, kun halutaan tarkistaa voittoehto ja vaihtaa pelaajien vuoroa.
            loppu:

            // Tarkistetaan voittiko kukaan.
            if (CheckWinningState())
            {
                // Peli on ohi. Laitetaan peli GameOver tilaan. Enää ei tarvitse vaihtaa pelaajien vuoroja.
                ChangeState(GameState.GameOver);
                return;
            }

            // Vaihdetaan pelaajien vuorot.
            TogglePlayerTurns();

            // Jos "kädessä" olevien nappuloiden lukumäärät ovat 0, niin siirrytään movestateen. Muutoin mennään insert tilaan.
            if (Player1Table.Count == 0 && Player2Table.Count == 0) ChangeState(GameState.MoveState);
            else ChangeState(GameState.InsertState);
        }

        /// <summary>
        /// Settings commandille canexecute metodi.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Settings_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        /// <summary>
        /// Settings commandille exetuted metodi.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Settings_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var settings = new SettingsWindow();
            var dialogResult = settings.ShowDialog();
     
            // Dialog resultilaa ei ole arvoa, niin poistutaan.
            if (!dialogResult.HasValue) return;
            // Dialog result on false. Poistutaan.
            if (!dialogResult.Value) return;
            // Tänne päädytään, kun dialog result on tallentanut jotain muutoksia. Päivitetään muutokset peliin.
            UpdateSettings();
        }

        /// <summary>
        /// Abouting canexectue metodi.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void About_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        /// <summary>
        /// Aboutin executed metodi.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void About_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var about = new AboutWindow();
            about.ShowDialog();
        }

        /// <summary>
        /// Helpin canexectue metodi.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Help_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        /// <summary>
        /// Helpin executed metodi.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Help_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var help = new HelpWindow();
            help.ShowDialog();
        }

        /// <summary>
        /// Privaatti metodi pelin tilan muuttamiselle. Uusi tila korvaa vahnan tilan.
        /// Metodi muuttaa kaikki tilat falseksi ja asettaa tämän jälkeen uuden tilan trueksi.
        /// Jos nykyinen tila ja uusi tila ovat samat, niin nykyinen tila käy hetkellisesti falsessa.
        /// </summary>
        /// <param name="gState"></param>
        private void ChangeState(GameState gState)
        {
            // Muutetaan tämän hetinen pelin tila falseksi. Ei muuteta mitään turhaan, joten ehkä vältytään turhilta xamlissa turhilta trigger testailuilta.
            if (IsGameBegin) IsGameBegin = false;
            if (IsGameOver) IsGameOver = false;
            if (IsInsertMode) IsInsertMode = false;
            if (IsMoveMode) IsMoveMode = false;
            if (IsRemoveMode) IsRemoveMode = false;

            switch (gState)
            {
                case GameState.InsertState:
                    IsInsertMode = true;
                    // Vapaat paikan "näkyviin".
                    ShowAllFreeBlocks();
                    break;
                case GameState.MoveState:
                    IsMoveMode = true;
                    ClearAllSelectables();
                    // Poistetaan vapaiden paikkojen mahdollisuus. Siirrettävien nappuloiden siirtomahdollisuuden tutkitaan muualla.
                    // Korostetaan kaikki sellaiset pelimerkit, jotka ovat siirrettävissä. Tässä näytetään siis vastustajan liikutettavat merkit, sillä 
                    // tässä jossain välissä vaihtuu vuoro. Tilakoneen tominta on nyt hajautettu ja hieman epäselvä, mutta se toimii tässä ohjelmassa.
                    ShowAllMovablePieces(GetPlayer(PlayerState.CurrentPlayer));
                    break;
                case GameState.RemoveState:
                    ShowRemovablePieces(true); // Laitetaan poistettavien nappuloiden flagit päälle.
                    IsRemoveMode = true;
                    ClearSelections((int)SelectionEnum.IsSelectable);
                    break;
                case GameState.GameOver:
                    // Muutetaan pelin tila gameoveriksi.
                    IsGameOver = true;
                    // Pelaavien vuorot falseksi.
                    Player1.HasTurn = false;
                    Player2.HasTurn = false;
                    // Jos jotain oli nyt valittuna, niin poistetaan se.
                    ClearAllSelectables();
                    break;
                case GameState.GameStart:
                    IsGameBegin = true;
                    break;
            }
            // Päivitetään lopuksi pelin teksti.
            UpdateGameMessage();
        }

        /// <summary>
        /// Metodi, joka päivittää pelin tilannetekstin.
        /// </summary>
        private void UpdateGameMessage()
        {
            if (IsInsertMode)
            {
                GameMessage = String.Format("{0}: On sinun vuorosi laittaa pelimerkki pelipöydälle.", GetPlayer(PlayerState.CurrentPlayer).PlayerName);
            }
            else if (IsMoveMode)
            {
                GameMessage = String.Format("{0}: On sinun vuorosi siirtää pelimerkkiä.", GetPlayer(PlayerState.CurrentPlayer).PlayerName);
            }
            else if (IsRemoveMode)
            {
                GameMessage = String.Format("{0}: On sinun vuorosi poistaa vastustajan pelimerkki.", GetPlayer(PlayerState.CurrentPlayer).PlayerName);
            }
            else if (IsGameOver)
            {
                GameMessage = String.Format("{0}: Onneksi olkoon. VOITIT PELIN!!!", TheWinner.PlayerName);
            }
        }

        /// <summary>
        /// Metodi, joka kääntää pelaajien vuorot päikseeen.
        /// </summary>
        private void TogglePlayerTurns()
        {
            Player1.HasTurn = !Player1.HasTurn;
            Player2.HasTurn = !Player2.HasTurn;
        }

        /// <summary>
        /// Metodi, joka palauttaa joko sen pelaajan, jonka vuoro on tai sen pelaajan vuoron, kenellä ei ole nyt vuoroa. PlayerState parametri määrä kumman pelaajan metodi palauttaa.
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        private Player GetPlayer(PlayerState state)
        {
            /*
            if (Player1 == null || Player2 == null)
                throw new NullReferenceException("MyllyModelView:GetCurrentPlayer: Player1 tai Player2 on null!!!");
            if (Player1.HasTurn == Player2.HasTurn)
                throw new Exception("MyllyModelView:GetCurrentPlayer: Kummallakin pelaajalla sama HasTurn arvo!!!");
                */
            Player current =  (Player1.HasTurn) ? Player1 : Player2;
            Player opponent = (Player1.HasTurn) ? Player2 : Player1;
            return (state == PlayerState.CurrentPlayer) ? current : opponent;

        }

        /// <summary>
        /// Metodi, joka alustaa ja polkaisee uuden pelin käyntiin.
        /// </summary>
        private void CreateNewGame()
        {
            StartingPieceCount = 9;
            if (Player1 == null || Player2 == null)
                throw new NullReferenceException("MyllyModelView:CreateNewGame: Player1 tai Player2 on null!!!");

            // Alustetaan voittaja nulliksi.
            TheWinner = null;

            // Luodaan blockit kenttää varten. Ehkä hölmöä laittaa property argumentiksi. TODO: korjaa jos jää häiritsemään liikaa.
            CreateMap(CurrentGameMapEnum);

            // Arvotaan aloittavan pelaajan vuoro.
            Random rnd = new Random();
            int playerTurn = rnd.Next(0, 2);
            Player1.HasTurn = false;
            Player2.HasTurn = false;
            if (playerTurn == 0) Player1.HasTurn = true;
            else Player2.HasTurn = true;

            // Annetaan pelaajille nappulat, eli dummy-olioita.
            Player1Table = new ObservableCollection<Dummy>();
            Player2Table = new ObservableCollection<Dummy>();

            for (int i=0; i<StartingPieceCount; i++)
            {
                GameNappula p1Nappula = new GameNappula();
                GameNappula p2Nappula = new GameNappula();
                Player1Table.Add(new Dummy());
                Player2Table.Add(new Dummy());
            }
            
            // Asetetaan pelin tilaksi nappunlan lisäystila.
            ChangeState(GameState.InsertState);
        }

        /// <summary>
        /// Rakennetaan myllyn pohjarakenteet. Tässä luodaan perinteinen mylly kenttä sekä hieman 
        /// eksoottisempi, tajunnan räjäyttävä, mylly-kenttä.
        /// </summary>
        /// <param name="mapType">pelikentän enum.</param>
        private void CreateMap(GameMap mapType)
        {
            switch(mapType)
            {
                // Tavallinen myllykenttä.
                case GameMap.Original:
                    MyllyMap perinteinen = new MyllyMap();

                    // Annetaan pelialueen dimensiot. 
                    perinteinen.RowCount = 7;
                    perinteinen.ColumnCount = 7;

                    // Tavallisen myllykentän blockien data numeerisessa muodossa. Katso MyllyMap luokan CreateDataFromArray metodia, niin selviää miten bitit tulkitaan.
                    int[] original = { 25 , 3, 3, 27, 3, 3, 26,   //  o--o--o
                                       12 ,25, 3, 31, 3,26, 12,   //  |o-o-o| 
                                       12 ,12,25, 23,26,12, 12,   //  ||ooo||
                                       29 ,31,30,  0,29,31, 30,   //  ooo ooo
                                       12 ,12,21, 27,22,12, 12,   //  ||ooo||
                                       12 ,21, 3, 31, 3,22, 12,   //  |o-o-o|
                                       21 , 3, 3, 23, 3, 3, 22 }; //  o--o--o Kaikki viivat eivät näy tässä ascii kuviossa.

                    // Muuttaa taulukon blockeiksi MyllyMap luokan olion GameBoardData propertyyn.
                    perinteinen.CreateDataFromArray(original);

                    // Tämän olisi voinnut laittaa suoraan 7:ksi, mutta olkoon nyt tällä kertaa näin. 
                    // Olkoon tämä mallina siitä, miten propertyt tulee asettaa, jos MyllyMap tulisi jostain muualta käsin.
                    GameRowCount = perinteinen.RowCount;
                    GameColumnCount = perinteinen.ColumnCount;

                    // Asetetaan MyllyMap oliosta blockit modelview luokan MVBlocks propertyyn.
                    MVBlocks = perinteinen.GameBoardData;
                    break;
                
                // Custom myllykenttä.
                case GameMap.Exotic:
                    MyllyMap exotic = new MyllyMap();
                    exotic.RowCount = 10;
                    exotic.ColumnCount = 10;

                    int[] customMap = { 25, 3, 3, 27, 26, 0, 25, 26, 0, 24,
                                        12, 0,25, 31, 31, 18, 12, 12, 0, 12,
                                        29,26,29, 30, 21,26,  12, 29, 3, 22,
                                        29,23,22, 28,  0,21, 30,  20, 0,  0,
                                        29, 3, 3, 31,  3, 3, 31,  3, 3, 26,
                                        29,27,26, 12, 25, 3, 31,  3,26, 12,
                                        29,31,22, 12, 12,25, 23, 26,12, 12,
                                        21,30, 0, 29, 23,30,  0, 29,31, 30,
                                        0,12, 0, 12, 0,21, 27, 22,20, 12,
                                        0,21, 3, 23 , 3, 3, 23, 3, 3, 22, 
                    };
             
                    exotic.CreateDataFromArray(customMap);
                    GameRowCount = exotic.RowCount;
                    GameColumnCount = exotic.ColumnCount;
                    MVBlocks = exotic.GameBoardData;
                    break;
            }
        }

        /// <summary>
        /// Privaatti metodi, joka tarkistaa onko parametrina annettu Block osa myllyä.
        /// </summary>
        /// <param name="currentBlock"></param>
        private bool CheckMill(Block currentBlock)
        {
            Player owner = currentBlock.BlockOwner;
            if (owner == null) throw new ArgumentNullException("CheckMill metodin blokilla ei ole omistajaa.");

            int startIndex = MVBlocks.IndexOf(currentBlock);

            // Jos parametrina tuodulla blokilla ei ole nappulaa, niin ei se myöskään voi olla osa mitään myllyä. Palautetaan false.
            if (!currentBlock.HasPiece) return false;

            // Tarkistetaan ensin vaakasuora mylly.
            int verticalCounter = 1 + IterateRight(currentBlock, owner) + IterateLeft(currentBlock, owner);
            if (verticalCounter > 2) return true;

            // Tarkistetaan pystysuora mylly.
            int horizontalCounter = 1 + IterateUp(currentBlock, owner) + IterateDown(currentBlock, owner);
            if (horizontalCounter > 2) return true;

            return false;
        }

        /// <summary>
        /// Metodi, joka ottaa parametrina blockin, ja käy katsomassa lähitienoilta, että löytyisikö sen vierestä vapaita paikkoja johon siirtää nappula.
        /// Jos pelaajalla on vain kolme nappulaa käytössään, niin tällöin kaikki vapaat paikat ovat potentiaalisia siirtopaikkoja. Jos p == null, niin 
        /// vapaat paikat ilmoitetaan ottamatta huomioon kuka omistaa blockin.
        /// Jos metodi löytää sellaisen, niin se asettaa ko. blockkien IsSelectable arvot trueksi. Ei mikään kaunis toteutus, ehkä vähän kopioi/limaa tyyliä. 
        /// Jos jää aikaa, niin keksin jotain hieman elegantimpaa.
        /// </summary>
        /// <param name="b">Tarkastelun kohteena oleva block.</param>
        /// <param name="sideeffects">Sallitaanko muutokset, vai ainoastaanko lasketaan.</param>
        /// <returns>Mahdollisten liikkumapaikkojen määrä.</returns>
        private int CheckPossipleMoves(Block b, bool sideeffects, Player p)
        {
            // if (p == null) throw new NullReferenceException("CheckPossibleMoves: p == null.");
            int movePossibilities = 0;
            int startIndex = MVBlocks.IndexOf(b);

            // Jos valitussa blokissa ei ole pelaajan nappulaa, niin palautetaan oitis 0. Poikkeuksen tekee jos parametri p == null, eli ei tarkastella vapaita paikkoja 
            // kenenkään tietyn pelaajan näkökulmasta.
            if (p != null && b.BlockOwner != p)
            {
                Console.WriteLine("Tähän kosahti."); return 0;
            }
            
            // Jos pelaajalla on pöydässä tasan kolme nappulaa, eikä muita nappuloita enää ole, niin asetetaan kaikki vapaat nappulapaikat valittaviksi.
            // Tällaista sääntöä ei ole tilanteelle, jossa parametri p == null.
            if (p != null && GetOverallPieceCount(p) == 3 && GetGameBoardPieceCount(p) == 3)
            {
                foreach (var item in MVBlocks)
                {
                    // Jos blockissa on vapaa nappulapaikka niin asetetaan nappula valittavaksi (jos sideeffects on true).
                    if (item.HasObject == true && item.HasPiece == false)
                    {
                        if (sideeffects )item.IsSelectable = true;
                        movePossibilities++;
                    }
                }
                // Console.WriteLine("CheckPossipleMobes(): movePossibilities == {0}", movePossibilities);
                return movePossibilities;
            }

            // Etsitään tyhjää nappulapaikkaa oikealta.
            int r = startIndex;
            while (true)
            {
                // Siirretään indeksejä niin, että mennään yksi askel eteenpäin.
                r++;
                if (r >= MVBlocks.Count) break;

                Block right = MVBlocks[r];

                // Jos ei ole reittiä vasemmalle, niin ei voitu tulla tänne asti ilman ilmaloikkaa. Lopetetaan.
                if (!right.LeftWay) break;

                // Ei nappulapaikkaa. Jatketaan etsimistä.
                if (right.HasObject == false) continue;

                // On nappulapaikka.
                if (right.HasObject == true)
                {
                    // löytyi vapaa nappulapaikka. Jos sivuvaikutuksen ovat päällä, niin asetetaan paikka valittavaksi.
                    if (right.HasPiece == false) { if (sideeffects) right.IsSelectable = true; movePossibilities++; }
                    break;
                }
            }

            // Etsitään tyhjää nappulapaikkaa vasemmalta.
            int l = startIndex;
            while (true)
            {
                l--;
                if (l < 0) break;
                Block left = MVBlocks[l];
                if (!left.RightWay) break;
                if (left.HasObject == false) continue;
                if (left.HasObject == true)
                {
                    if (left.HasPiece == false) { if (sideeffects) left.IsSelectable = true; movePossibilities++; }
                break;
                }
            }

            // Etsitään tyhjää nappulapaikkaa ylhäältä.
            int u = startIndex;
            while (true)
            {
                u -= GameColumnCount;
                if (u < 0) break;

                Block up = MVBlocks[u];

                if (!up.DownWay) break;
                if (up.HasObject == false) continue;
                if (up.HasObject == true)
                {
                    if (up.HasPiece == false) { if (sideeffects) up.IsSelectable = true; movePossibilities++; }
                    break;
                }
            }

            // Etsitään tyhjää nappulapaikkaa alhaalta.
            int d = startIndex;
            while (true)
            {
                d += GameColumnCount;
                if (d >= MVBlocks.Count) break;

                Block down = MVBlocks[d];
                if (!down.UpWay) break;
                if (down.HasObject == false) continue;
                if (down.HasObject == true)
                {
                    if (down.HasPiece == false) { if (sideeffects) down.IsSelectable = true; movePossibilities++; }
                    break;
                }
            }
            // Console.WriteLine("CheckPossipleMobes(): movePossibilities == {0}", movePossibilities);
            return movePossibilities;
        }

        /// <summary>
        /// Rekurssiivinen metodi, joka käy läpi annetun blockin oikeanpuoleisia blockeja, ja palauttaa kokonaisluvus, joka kertoo pelaajan oikeanpuoleisten vierekkäisten nappuloiden lukumäärän.
        /// </summary>
        /// <param name="block"></param>
        /// <param name="player on pelaaja, jonka merkeistä ollaan kiinnostunut."></param>
        /// <returns></returns>
        private int IterateRight(Block block, Player player)
        {
            int startIndex = MVBlocks.IndexOf(block);
            if (startIndex + 1 >= MVBlocks.Count) return 0;
            if (!block.RightWay) return 0;

            Block nextBlock = MVBlocks[startIndex + 1];
            if (nextBlock.HasObject)
            {
                Player p = nextBlock.BlockOwner;
                if (p == null || p != player) return 0;
                else return 1 + IterateRight(nextBlock, player);
            }
            return IterateRight(nextBlock, player);
        }

        /// <summary>
        /// Vastaanalainen metodi kuin yläpuolella, mutta tarkastetaan vasemmanpuoleiset nappulat.
        /// </summary>
        /// <param name="block"></param>
        /// <param name="player"></param>
        /// <returns></returns>
        private int IterateLeft(Block block, Player player)
        {
            int startIndex = MVBlocks.IndexOf(block);
            if (startIndex - 1 < 0) return 0;
            if (!block.LeftWay) return 0;

            Block previousBlock = MVBlocks[startIndex - 1];
            if (previousBlock.HasObject)
            {
                Player p = previousBlock.BlockOwner;
                if (p == null || p != player) return 0;
                else return 1 + IterateLeft(previousBlock, player);
            }
            return IterateLeft(previousBlock, player);
        }

        /// <summary>
        /// Tarkastetaan blockin alapuoliset merkit.
        /// </summary>
        /// <param name="block"></param>
        /// <param name="player"></param>
        /// <returns></returns>
        private int IterateDown(Block block, Player player)
        {
            int startIndex = MVBlocks.IndexOf(block);
            if (startIndex + GameColumnCount >= MVBlocks.Count) return 0;
            if (!block.DownWay) return 0;

            Block nextBlock = MVBlocks[startIndex + GameColumnCount];
            if (nextBlock.HasObject)
            {
                Player p = nextBlock.BlockOwner;
                if (p == null || p != player) return 0;
                else return 1 + IterateDown(nextBlock, player);
            }
            return IterateDown(nextBlock, player);
        }

        /// <summary>
        /// Tarkastetaan blockin yläpuoliset merkit.
        /// </summary>
        /// <param name="block"></param>
        /// <param name="player"></param>
        /// <returns></returns>
        private int IterateUp(Block block, Player player)
        {
            int startIndex = MVBlocks.IndexOf(block);
            if (startIndex - GameColumnCount < 0) return 0;
            if (!block.UpWay) return 0;

            Block previousBlock = MVBlocks[startIndex - GameColumnCount];
            if (previousBlock.HasObject)
            {
                Player p = previousBlock.BlockOwner;
                if (p == null || p != player) return 0;
                else return 1 + IterateUp(previousBlock, player);
            }
            return IterateUp(previousBlock, player);
        }

        /// <summary>
        /// Metodi, joka tarkistaa voittoehdot, ja jos joku pelaaja voittaa pelin, asettaa pelin gameover tilaan ja asettaa voittanan TheWinner propertyyn.
        /// </summary>
        /// <returns>true, jos peli saatiin päätökseen, false muutoin.</returns>
        private bool CheckWinningState()
        {
            Player currentPlayer = GetPlayer(PlayerState.CurrentPlayer);
            Player opponent = GetPlayer(PlayerState.Opponent);

            // Vastustajan kaikkien nappuloiden lukumäärä.
            int opponentBlockCount = GetOverallPieceCount(opponent);
            // Vuorossa olevan pelaajan kaikkien nappuloiden lukumäärä.
            int currentBlockCount = GetOverallPieceCount(currentPlayer);

            // Jos vastustajalla on vähemmän pelimerkkejä kuin 3, niin tämä pelaaja on voittanut.
            if (opponentBlockCount < 3)
            {
                TheWinner = currentPlayer;
                return true;
            }

            // Jos pelaajalla on vähemmän pelimerkkejä kuin 3, niin vastustaja on voittanut.
            if (currentBlockCount < 3)
            {
                TheWinner = opponent;
                return true;
            }


            // Katsotaan vuorossa olevan pelaajan taskussa olevaa nappulamäärää.
            int piecesAtTaskuCurrent = GetPiecesAtHand(currentPlayer);

            // Katsotaan vastustajan taskussa olevaa nappulamäärää.
            int piecesAtTaskuOpponent = GetPiecesAtHand(opponent);

            // Pelaajan taskussa oli vielä nappuloita, ei tarvitse käydä tarkistamassa voiko pelaaja liikuttaa nappuloita, sillä hän ei ole vielä siinä vaiheessa.
            // Tai jos pelaajalla on vain kolme nappulaa pöydässä, niin hän voi hyppyyttää nappuloitaan. Tässä oletetaan nyt, että nappuloita ei ole koskaan niin paljon 
            // pelilaudalla, että tässä vaiheessa ei olisi tilaa hyppiä.
            //Console.WriteLine("piecesAtTaskuCurrent = {0}, currentBlockCount = {1}, piecesAtTaskuOpponent = {2}, opponentBlockCount = {3}", piecesAtTaskuCurrent, currentBlockCount, piecesAtTaskuOpponent, opponentBlockCount);
            if (piecesAtTaskuCurrent > 0 || currentBlockCount == 3) return false;
            else
            {
                // Nappuloita ei voi enää lisätä. Pelaajalla on pöydässä enemmän kuin 3 nappulaa. Tarkistetaan voiko pelaaja siirtää nappuloitaan. Jos ei, niin vastustaja voitti.
                if (!CanMove(currentPlayer))
                {
                    TheWinner = opponent;
                    return true;
                }
            }

            // Tehdään vastaava tarkistus vastustajalle. Hieman copy/pastemaista toimintaa, mutta koska pelaajia on vain kaksi, niin olkoon nyt näin tämän kerran.
            if (piecesAtTaskuOpponent > 0 || opponentBlockCount == 3) return false;
            else
            {
                // Nappuloita ei voi enää lisätä. Pelaajalla on pöydässä enemmän kuin 3 nappulaa. Tarkistetaan voiko pelaaja siirtää nappuloitaan. Jos ei, niin vastustaja voitti.
                if (!CanMove(opponent))
                {
                    TheWinner = currentPlayer;
                    return true;
                }
            }

            // Ei voitu ratkaista voittajaa.
            return false;
        }

        /// <summary>
        /// Metodi, joka tarkistaa voiko pelaaja liikuttaa enää nappuloitaan. Jos voi, niin palauttaa truen, jos ei voi, niin palauttaa falsen.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private bool CanMove(Player p)
        {
            Console.WriteLine("Can move..");

            foreach (var item in MVBlocks)
            {
                if (item.BlockOwner == p)
                {
                    if (CheckPossipleMoves(item, false, p) > 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Metodi, joka tarkistaa parametrina annetun blockin "vapaat" naapurit. Oletuksena on että, annettu block sisältää nappulapaikan, ja "vapaa" naapuri on sellainen naapuri 
        /// johon on pääsy annetusta blockista ja että naapuri sisältää tyhjän nappulapaikan. Heittää poikkeuksen, jos annettu nappula ei sisällä nappulapaikkaa.
        /// </summary>
        /// <param name="b">boolean arvo, joka kertoo sen löytyikö sellainen tyhjä nappulapaikka, johon tästä blockista pääsee kulkemaan "tietä" pitkin.</param>
        /// <returns></returns>
        private bool HasEmptyNeighbours(Block b)
        {
            if (!b.HasObject) throw new Exception("MyllyViewModel:HasEmptyNeighbours: Parametrina annetulla blockilla ei ole nappulapaikkaa.");

            // Tarkastetaan annetun b:n vierekkäisten nappulapaikkojen lukumäärää, ja palautetaan false, tai true riippuen siitä löytyykö niitä.
            Console.WriteLine("HasEmptyNeighbours: CheckPossipleMoves(b, false, null) == {0}", CheckPossipleMoves(b, false, null));
            return CheckPossipleMoves(b, false, null) == 0 ? false : true; 
        }

        /// <summary>
        /// Metodi, joka palauttaa parametrina annetun pelaajn pelipöydässä olevien nappuloiden lukumäärän. (ei "kädessä" olevien nappuloiden lukumäärää).
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private int GetGameBoardPieceCount(Player p)
        {
            int count = 0;

            foreach (var x in MVBlocks)
            {
                if (x.BlockOwner == p) count++;
            }
            return count;
        }

        /// <summary>
        /// Metodi, joka palauttaa parametrina annetun pelaajan kädessä olevien merkkien lukumäärän.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private int GetPiecesAtHand(Player p)
        {
            return (p == Player1) ? Player1Table.Count : Player2Table.Count;
        }

        /// <summary>
        /// Metodi, joka laskee annetun pelaajan kaikkien nappuloiden lukumäärän.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private int GetOverallPieceCount(Player p)
        {
            int count = GetGameBoardPieceCount(p);

            if (p == Player1) return count + Player1Table.Count;

            return count + Player2Table.Count;
        }

        /// <summary>
        /// Metodi, joka asettaa kaikki sellaisten blockien IsSelectable arvot trueksi, jotka sisältävät nappulapaikan, mutta ei ole kenenkään omistuksessa.
        /// Asettaa kaikki edelliset IsSelect arvot falseksi ennen kuin suorittaa edellä mainitun operaation.
        /// </summary>
        private void ShowAllFreeBlocks()
        {
            foreach (var item in MVBlocks)
            {
                item.IsSelectable = false;
                if (item.BlockOwner == null && item.HasObject == true) item.IsSelectable = true;
            }
        }

        /// <summary>
        /// Metodi, joka asettaa "syötävien" blockien IsContentSelectable arvon trueksi ja asettaa muiden blockien vastaavan propertyn falseksi.
        /// Etusijalla on ne blockit, jotka eivät ole osana mitään myllyä, mutta jos kaikki vastustajan blockit ovat osa myllyä, niin asetetaan tällöin kaikki vastustajan 
        /// blockit ovat valittavissa poistettavaksi.
        /// </summary>
        /// <param name="show">Boolean arvo, joka kertoo sen, että valitaanko vastustajan nappulat (true), vai asetetaanko kaikkien blockien IsContentSelectable falseksi (false).</param>
        private void ShowRemovablePieces(bool show)
        {
            Player opponent = GetPlayer(PlayerState.Opponent);

            // Lista blokeista, jotka ovat osana jotain myllyä.
            List<Block> inMyllyBlocks = new List<Block>();

            // Lista blokeista, jotka eivät ole osana mitään myllyä.
            List<Block> notInMyllyBlocks = new List<Block>();

            // Lajitellaan blokit edellämainittuihin listoihin sen mukaan, ovatko ne osana myllyä vai ei.
            foreach (var item in MVBlocks)
            {
                if (item.BlockOwner == opponent)
                {
                    item.IsContentSelectable = false;
                    if (CheckMill(item)) inMyllyBlocks.Add(item);
                    else notInMyllyBlocks.Add(item);
                }
            }

            // Löytyi ainakin yksi myllyn ulkopuolinen blocki. Asetetaan kaikki myllyn ulkopuolisten blockien IsContentSelectable arvo trueksi ja poistutaan.
            if (notInMyllyBlocks.Count != 0)
            {
                foreach (var item in notInMyllyBlocks)
                {
                    if (item.BlockOwner == opponent && show) item.IsContentSelectable = true;
                }
                return;
            }

            // koskan myllyn ulkopuolisia blockeja ei ole, asetetaan myllyissä olevien blockien IsContentSelectable arvo trueksi.
            else
            {
                foreach (var item in inMyllyBlocks)
                {
                    if (item.BlockOwner == opponent && show) item.IsContentSelectable = true;
                }
            }
        }

        /// <summary>
        /// Privaatti metodi, joka etsii kaikki parametrina annetun pelaajan nappulat, jotka ovat siirrettävissä ja asettaa sellaiste blockien IsContentSelectable arvot trueksi. 
        /// Metodi ei muuta vanhoja selectioneita.
        /// </summary>
        /// <param name="p">pelaaja.</param>
        private void ShowAllMovablePieces(Player p)
        {
            if (p == null) throw new NullReferenceException("ShowAllMovablePieces: p == null.");

            // Jos pelaajalla on vain 3 pelimerkkiä, niin tällöin kaikki nappulat ovat valittavissa.
            bool threePieces = (GetOverallPieceCount(p) == 3) ? true : false;           

            foreach (var item in MVBlocks)
            {
                // Muutetaan sellaisten blockien IsContentSelectable arvot trueksi, jotka ovat pelaajan p omistamia ja joilla on vapaita siirtomahdollisuuksia.
                if (item.BlockOwner == p)
                {
                    if (HasEmptyNeighbours(item) || threePieces) item.IsContentSelectable = true;
                }
            }
        }

        /// <summary>
        /// Metodi, joka asettaa kaikien pelikentän blockien Selectable arvot falseksi.
        /// </summary>
        private void ClearAllSelectables()
        {
            ClearSelections((int)SelectionEnum.IsContentSelected | (int)SelectionEnum.IsContentSelectable | (int)SelectionEnum.IsSelectable);
        }

        /// <summary>
        /// Clear metodi, joka asettaa kaikkien pelin blockien selection arvot falseksi, riippuen annetusta flagista (SelectionEnum).
        /// </summary>
        /// <param name="flag">on or operaattorilla yhdistetty SelectionEnum arvo</param>
        private void ClearSelections(int flag)
        {
            foreach (var item in MVBlocks)
            {
                if ((flag & (int)SelectionEnum.IsContentSelectable) != 0) item.IsContentSelectable = false;
                if ((flag & (int)SelectionEnum.IsContentSelected) != 0) item.IsContentSelected = false;
                if ((flag & (int)SelectionEnum.IsSelectable) != 0) item.IsSelectable = false;
            }
        }

        /// <summary>
        /// Päivittää asetukset peliin. Ei ole tarkistusta tyhjiiin merkkijonoihin tai mahdollisesti olemattomiin coloreihin. Tässä oletetaan nyt, että asetuksissa on jotain järkevää tavaraa.
        /// Tässä olisi tietenkin voinnut tehdä niin, että jos jotain poikkeuksia tulee, tai pelaajien nimet ovat tyhjiä, niin olisi jotkut hard koodatut asetukset, jotka otettaisin käyttöön jos
        /// jotain menee pieleen.
        /// </summary>
        private void UpdateSettings()
        {
            // Yritetään ladata olemassa olevat asetukset.
            try {
                var P1Col = Properties.Settings.Default.P1Color;
                Player1.PlayerColor = new SolidColorBrush(Color.FromArgb(P1Col.A, P1Col.R, P1Col.G, P1Col.B));
                var P2Col = Properties.Settings.Default.P2Color;
                Player2.PlayerColor = new SolidColorBrush(Color.FromArgb(P2Col.A, P2Col.R, P2Col.G, P2Col.B));
                Player1.PlayerName = Properties.Settings.Default.P1Name;
                Player2.PlayerName = Properties.Settings.Default.P2Name;

                // Tässä tehdään nyt muutos "Ympyrä -> 0, Kolmio -> 1 tai Neliö -> 2". Ehkä hölmöläisen touhua. Olisi voinnut tehdään suoraan depencendy property, joka ottaa muodon merkkijonona. 
                // Tässä on nyt laiskan ohjelmoijan pika muuunnosfunktio merkkijonosta maagiseksi numeroksi. Maaginen numero ei ole kovinkaan hyvä ratkaisu.
                Func<String,int> MuunnosFunktio = s => { if (s.Equals("Ympyrä")) return 0; if (s.Equals("Kolmio")) return 1; if (s.Equals("Neliö")) return 2; throw new Exception("Update Settings: Väärä muoto."); };
                Console.WriteLine(Properties.Settings.Default.P1Shape);
                Console.WriteLine(Properties.Settings.Default.P2Shape);
                Player1.PieceShapeId = MuunnosFunktio(Properties.Settings.Default.P1Shape);
                Player2.PieceShapeId = MuunnosFunktio(Properties.Settings.Default.P2Shape);
                var BCol = Properties.Settings.Default.BoardColor;
                GameBoardColor = new SolidColorBrush(Color.FromArgb(BCol.A, BCol.R, BCol.G, BCol.B));

                // Hardkoodattu nyt tähän. Jos kenttiä olisi enemmän, niin pitäisi keksiä jotain luovempaa.
                if (Properties.Settings.Default.MapType == "Original") CurrentGameMapEnum = GameMap.Original;
                else CurrentGameMapEnum = GameMap.Exotic;


                // Päivitetään pelin teksti.
                UpdateGameMessage();
            }
            // Tänne päädytään, jos asetuksissa oli jotain häikkää (jos joku esim. peukaloi setuksia muualta kuin ohjelmasta käsin, kuten ohjelmantekijä itse teki muutamaan otteeseen). 
            // Palautetaan ns. tehdas asetukset.
            catch
            {
                Properties.Settings.Default.P1Name = "Player1";
                Properties.Settings.Default.P2Name = "Player2";
                Properties.Settings.Default.P1Color = System.Drawing.Color.FromArgb(255, 255, 0, 0);
                Properties.Settings.Default.P2Color = System.Drawing.Color.FromArgb(255, 0, 0, 255);
                Properties.Settings.Default.P1Shape = "Ympyrä";
                Properties.Settings.Default.P2Shape = "Kolmio";
                Properties.Settings.Default.BoardColor = System.Drawing.Color.FromArgb(255, 66, 133, 244);
                Properties.Settings.Default.Save();

                // Uusi yritys tehdasasetuksille!
                UpdateSettings();
            }
        }

    } // MyllyViewModel

    /// <summary>
    /// Tyhmä luokka, jota käytetään, kun xamlista "ladataan" pelaajien merkit. Yksi Dummy siis vastaa yhtä pelimerkkiä "kädessä".
    /// </summary>
    public class Dummy
    {
        public Dummy()
        {

        }
    }

    /// <summary>
    /// Luokka, datamalli, myllykentän rakenteelle.
    /// </summary>
    internal class MyllyMap
    {
        /// <summary>
        /// Property rivien lukumäärälle.
        /// </summary>
        public int RowCount { get; set; }

        /// <summary>
        /// Property sarakkeiden lukumäärälle.
        /// </summary>
        public int ColumnCount { get; set; }

        /// <summary>
        /// Property varsinaiselle pelikentän datalle.
        /// </summary>
        public ObservableCollection<Block> GameBoardData { get; set; }

        /// <summary>
        /// Bitti maskeja, joiden arvot määrittävät left, right, up, down ja hasObject arvot.
        /// </summary>
        private static int pRight = 1;
        private static int pLeft = 2;
        private static int pUp = 4;
        private static int pDown = 8;
        private static int pHasObject = 16;

        /// <summary>
        /// Rakentaja.
        /// </summary>
        public MyllyMap()
        {
            GameBoardData = new ObservableCollection<Block>();
        }

        /// <summary>
        /// Rakennetaan GameBoardData annetusta int-arraysta.
        /// </summary>
        /// <param name="data"></param>
        public void CreateDataFromArray(int[] data)
        {
            foreach(var x in data)
            {
                Block temp = CreateBlock(x);
                GameBoardData.Add(temp);
            }       
        }

        /// <summary>
        /// Luodaan bitti maskista Block olio, joka edustaa yhtä pelikentän palasta.
        /// </summary>
        /// <param name="valueMask"></param>
        /// <returns>Uusi block data-olio.</returns>
        private Block CreateBlock(int valueMask)
        {
            Block b = new Block();
            b.LeftWay = (valueMask & pLeft) != 0;
            b.RightWay = (valueMask & pRight) != 0;
            b.DownWay = (valueMask & pDown) != 0;
            b.UpWay = (valueMask & pUp) != 0;
            b.HasObject = (valueMask & pHasObject) != 0;
            return b;
        }
    }
} // Namespace
