using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Windows.Input;
using System.Runtime.CompilerServices;
using System.Windows;
using Mylly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Mylly.Tests
{

    /* TESTIT */

    /* Versionumero 0.1.0

    [TestClass()]
    public class MyllyViewModelTests
    {
        /// <summary>
        /// Testimetodi, joka testaa ainoastaan sen tilanteen, että 
        /// InsertPieceSelection_Executed(object sender, ExecutedRoutedEventArgs e) 
        /// e.Param on null, eli ei ole Block luokan olio. Eli ohjelman tilan ei tulisi muuttua mitenkään. 
        /// </summary>
        [TestMethod()]
        public void MyllyViewModelTest_1()
        {
            // Luodaan lähtötiladata.
            var initialData = new TestData();
            // Luodaan oletettavissa oleva lopputiladata.
            var shouldBeData = new TestData();

            // Laitetaan ohjelma alkutilan mukaiseen tilaan.
            var mvm = TestHelper.CreateMVM(initialData);
            // Laitetaan toinen ohjelma oletettavissa olevaan tilaan.
            var mvmShouldBe = TestHelper.CreateMVM(shouldBeData);

            // Suoritetaan testattava metodi.
            TestHelper.RunTest(mvm,null);
            // Konvertoidaan ohjelman tila takaisin testausData-olioksi.
            var afterTestData = TestHelper.ConvertMVMtoTestData(mvm);

            // Konvertoidaan myös halutunlaisessa lopputilassa olevan ohjelma takaisin testausdata-olioksi.
            var shouldBeTestData = TestHelper.ConvertMVMtoTestData(mvmShouldBe);

            StringBuilder errData = new StringBuilder(1000); errData.Append("\n");
            bool result = TestHelper.TestDataEquals(afterTestData,shouldBeTestData,errData);
            
            Assert.AreEqual(true, result, errData.ToString());
        }

        /// <summary>
        /// Testaus tilanne. Peliruudukko on tyhjä. Pelaajilla ei ole pelimerkkejä kädessä. Asetetaan vuoro 1.pelaajalle. 
        /// Asetetaan peli InsertState tilaan. Kutsutaan metodia siten, että ensimmäinen blocki lähetetään metodille, eli pelialueen 
        /// vasen yläreuna, jossa on vapaa pelinappula paikka. Metodin tulee heittää poikkeus tällaisessa tilanteessa, jossa ollan 
        /// insterModessa, mutta ei nappuloita ei ole kädessä.
        /// 
        /// Testi hyväksytään, jos saadaan 
        /// 'InsertPieceSelection_Executer:Player1Table.Count == 0. Looginen virhe. Ei voi olla lisaystilassa jos napit on jo kaytetty'
        /// poikkeus.
        /// </summary>
        [TestMethod()]
        public void MyllyViewModelTest_2()
        {
            // Luodaan lähtötiladata.
            var initialData = new TestData();
            // Annetaan vuoro 1 pelaajalle.
            initialData.Player1.HasTurn = true;
            // Alkutilanteeseen InsertState päälle.
            initialData.GameState = MyllyViewModel.GameState.InsertState;
            initialData.CreateBlocksFromArray(initialData.TestBlockData);

            // Laitetaan ohjelma alkutilan mukaiseen tilaan.
            var mvm = TestHelper.CreateMVM(initialData);

            // Nyt yritetään saada aikaiseksi poikkeus.
            bool saatiinPoikkeus = false;
            string msg = "";
            string exceptionMessage = "InsertPieceSelection_Executer:Player1Table.Count == 0. Looginen virhe. Ei voi olla lisaystilassa jos napit on jo kaytetty";
            try
            {
                TestHelper.RunTest(mvm, mvm.MVBlocks[0]);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                if (ex.Message == exceptionMessage)
                {
                    saatiinPoikkeus = true;
                }
            }

            Assert.AreEqual(true,
                saatiinPoikkeus,  "\nEi saatu aikaiseksi '" + exceptionMessage + "'poikkeusta.");
        }

        /// <summary>
        /// Vastaava tilanne kuin MyllyViewModelTest_2:sessa, mutta nyt vuorossa on 2. pelaaja. 
        /// Haetaan 'InsertPieceSelection_Executer:Player2Table.Count == 0. Looginen virhe. Ei voi olla lisäystilassa jos napit on jo käytetty'
        /// poikkeusta.
        /// </summary>
        [TestMethod()]
        public void MyllyViewModelTest_3()
        {
            // Luodaan lähtötiladata.
            var initialData = new TestData();
            // Annetaan vuoro 2 pelaajalle.
            initialData.Player2.HasTurn = true;
            // Alkutilanteeseen InsertState päälle.
            initialData.GameState = MyllyViewModel.GameState.InsertState;
            // Luodaan blockit.
            initialData.CreateBlocksFromArray(initialData.TestBlockData);

            // Laitetaan ohjelma alkutilan mukaiseen tilaan.
            var mvm = TestHelper.CreateMVM(initialData);

            // Nyt yritetään saada aikaiseksi poikkeus.
            bool saatiinPoikkeus = false;
            string exceptionMessage = "InsertPieceSelection_Executer:Player2Table.Count == 0. Looginen virhe. Ei voi olla lisäystilassa jos napit on jo käytetty";
            try
            {
                TestHelper.RunTest(mvm, mvm.MVBlocks[0]);
            }
            catch (Exception ex)
            {
                if (ex.Message.Equals(exceptionMessage))
                {
                    saatiinPoikkeus = true;
                }
            }

            Assert.AreEqual(true,
                saatiinPoikkeus,
                "\nEi saatu aikaiseksi '" + exceptionMessage + "' poikkeusta.");
        }

        /// <summary>
        /// Testiasetelma 4.
        /// 
        /// Pelaaja 1 (A).
        /// 
        /// ALKUTILANNE:
        /// 
        ///   A--O--O
        ///   |  |  |
        ///   O--O--O
        ///   |  |  |
        ///   O--O--O
        /// 
        /// 
        /// Peli on insertStatessa. 
        /// Vuoro on  1.pelaajalla. 
        /// 1.pelaajalla on 1 nappulaa kädessään.
        /// 
        /// *** Action: "klikataan" vasenta yläkulmaa. ***
        /// 
        /// HALUTTU LOPPU TILANNE: 
        /// 
        /// Peli on insertStatessa.
        /// Vuoro on 1.pelaajalla.
        /// 1.pelaajalla on 1 nappulaa kädessään.
        /// Kaikki muut paitsi indeksissä 0 olevan blockin IsSelectable = true.
        /// 
        ///   A--O--O
        ///   |  |  |
        ///   O--O--O
        ///   |  |  |
        ///   O--O--O
        /// 
        /// LOPPUHUOMIOT:
        /// 
        /// Mitä tapahtuu, jos peliruudukko on jo täyttä, ja pelimerkkejä on kädessä? Päädytäänkö ikuiseen silmukkaan?
        /// </summary>
        [TestMethod()]
        public void MyllyViewModelTest_4()
        {
            // Luodaan lähtötiladata.
            var initialData = new TestData();
            initialData.GameState = MyllyViewModel.GameState.InsertState;
            initialData.Player1.HasTurn = true;
            initialData.Player1Table.Add(new Dummy()); // Lisätään pelaajalle yksi nappula käteen.
            // Luodaan blockit.
            initialData.CreateBlocksFromArray(initialData.TestBlockData);
            initialData.Blocks[0].HasPiece = true;
            initialData.Blocks[0].BlockOwner = initialData.Player1;

            // "Kloonataan" alkutilanne, sillä sen pitäisi pysyä samana.
            var shouldBeData = TestHelper.CloneTestData(initialData);

            // Muutetaan kaiiki muut paitsi X:n blockien IsSelectable arvo trueksi.
            for (int i=1; i<shouldBeData.Blocks.Count; i++)
            {
                shouldBeData.Blocks[i].IsSelectable = true;
            }

            // Testataan.
            var result = TestHelper.DoWholeTest(initialData, shouldBeData, 0);

            Assert.AreEqual(true, result.Item1, result.Item2);
        }


        /// <summary>
        /// Testiasetelma 5.
        /// 
        /// Pelaaja 1 (A).
        /// Pelaaja 2 (B).
        /// 
        /// ALKUTILANNE:
        /// 
        ///   O--O--O
        ///   |  |  |
        ///   O--O--O
        ///   |  |  |
        ///   O--O--O
        /// 
        /// 
        /// Peli on insertStatessa. 
        /// Vuoro on  2.pelaajalla. 
        /// 1.pelaajalla on 4 nappulaa kädessään.
        /// 2.pelaajalla on 4 nappulaa kädessään.
        /// 
        /// *** Action: "klikataan" vasenta yläkulmaa. ***
        /// 
        /// HALUTTU LOPPU TILANNE: 
        /// 
        /// Peli on insertStatessa.
        /// Vuoro on 1.pelaajalla.
        /// 1.pelaajalla on 4 nappulaa kädessään.
        /// 2.pelaajalla on 3 nappulaa kädessään.
        /// 
        /// Kaikki paitsi 0-indeksissä olevien blockien IsSelectable arvot täytyy olla true.
        /// 
        ///   A--O--O
        ///   |  |  |
        ///   O--O--O
        ///   |  |  |
        ///   O--O--O
        /// 
        /// LOPPUHUOMIOT:
        /// 
        /// 
        /// </summary>
        [TestMethod()]
        public void MyllyViewModelTest_5()
        {
            // Luodaan lähtötiladata.
            var initialData = new TestData();
            initialData.GameState = MyllyViewModel.GameState.InsertState;
            initialData.Player2.HasTurn = true;
            initialData.AddPiecesToHand(TestData.PlayerTableId.Two, 4);
            initialData.AddPiecesToHand(TestData.PlayerTableId.One, 4);
            // Luodaan blockit.
            initialData.CreateBlocksFromArray(initialData.TestBlockData);

            // Kloonataan alkutilanne.
            var shouldBeData = TestHelper.CloneTestData(initialData);
            shouldBeData.Blocks[0].BlockOwner = shouldBeData.Player2;
            shouldBeData.Blocks[0].HasPiece = true;
            shouldBeData.Blocks[0].IsSelectable = false;

            // Muutetaan kaiiki muut paitsi X:n blockien IsSelectable arvo trueksi.
            for (int i = 1; i < shouldBeData.Blocks.Count; i++)
            {
                shouldBeData.Blocks[i].IsSelectable = true;
            }
            shouldBeData.Player1.HasTurn = true;
            shouldBeData.Player2.HasTurn = false;

            // Testataan.
            var result = TestHelper.DoWholeTest(initialData, shouldBeData, 0);

            Assert.AreEqual(true, result.Item1, result.Item2);
        }

        /// <summary>
        /// Testiasetelma 6.
        /// 
        /// Pelaaja 1 (A).
        /// Pelaaja 2 (B).
        /// 
        /// ALKUTILANNE:
        /// 
        ///   O--A--A
        ///   |  |  |
        ///   O--O--O
        ///   |  |  |
        ///   O--O--B
        /// 
        /// 
        /// Peli on insertStatessa. 
        /// Vuoro on  1.pelaajalla. 
        /// 1.pelaajalla on 4 nappulaa kädessään.
        /// 2.pelaajalla on 4 nappulaa kädessään.
        /// 
        /// *** Action: "klikataan" vasenta yläkulmaa. ***
        /// 
        /// HALUTTU LOPPU TILANNE: 
        /// 
        /// Peli on removeStatessa.
        /// Vuoro on 1.pelaajalla.
        /// 1.pelaajalla on 3 nappulaa kädessään.
        /// 2.pelaajalla on 4 nappulaa kädessään.
        /// Kaikkien blockien IsSelectable arvot pitää olla false.
        /// Indeksissä 8 olevan (B):n IsContentSelectable arvo täytyy olla true, muiden false.
        /// 
        ///   A--A--A
        ///   |  |  |
        ///   O--O--O
        ///   |  |  |
        ///   O--O--B
        /// 
        /// LOPPUHUOMIOT:
        /// 
        /// Miten käy, jos A saa myllyn, ja B:llä ei ole nappuloita pöydässä?
        /// 
        /// </summary>
        [TestMethod()]
        public void MyllyViewModelTest_6()
        {
            // Luodaan lähtötiladata.
            var initialData = new TestData();
            initialData.GameState = MyllyViewModel.GameState.InsertState;
            initialData.Player1.HasTurn = true;
            initialData.AddPiecesToHand(TestData.PlayerTableId.Two, 4);
            initialData.AddPiecesToHand(TestData.PlayerTableId.One, 4);
            // Luodaan blockit.
            initialData.CreateBlocksFromArray(initialData.TestBlockData);

            // 1. pelaajan paikat.
            
            initialData.Blocks[1].HasPiece = true;
            initialData.Blocks[1].BlockOwner = initialData.Player1;
            initialData.Blocks[2].HasPiece = true;
            initialData.Blocks[2].BlockOwner = initialData.Player1;

            // 2. pelaajan paikat.
            initialData.Blocks[8].HasPiece = true;
            initialData.Blocks[8].BlockOwner = initialData.Player2;

            // Kloonataan alkutilanne.
            var shouldBeData = TestHelper.CloneTestData(initialData);

            // Tehdään muutokset, jotta saadaan toivottu lopputilanne.
            shouldBeData.Blocks[0].BlockOwner = shouldBeData.Player1;
            shouldBeData.Blocks[0].HasPiece = true;
            shouldBeData.Blocks[8].IsContentSelectable = true;
            shouldBeData.GameState = MyllyViewModel.GameState.RemoveState;
            shouldBeData.Player1Table.Remove(shouldBeData.Player1Table.Last());
            foreach (var item in shouldBeData.Blocks)
            {
                item.IsSelectable = false;
            }

            // Testataan.
            var result = TestHelper.DoWholeTest(initialData, shouldBeData, 0);

            Assert.AreEqual(true, result.Item1, result.Item2);
        }

        /// <summary>
        /// Testiasetelma 7.
        /// 
        /// Pelaaja 1 (A).
        /// Pelaaja 2 (B).
        /// 
        /// ALKUTILANNE:
        /// 
        ///   A--O--O
        ///   |  |  |
        ///   O--O--O
        ///   |  |  |
        ///   O--O--O
        /// 
        /// 
        /// Peli on removeStatessa. 
        /// Vuoro on  1.pelaajalla. 
        /// 1.pelaajalla on 4 nappulaa kädessään.
        /// yläkulman block.IsContentSelectable = false
        /// 
        /// *** Action: "klikataan" vasenta yläkulmaa. ***
        /// 
        /// HALUTTU LOPPU TILANNE: 
        /// 
        /// Peli on removeStatessa.
        /// Vuoro on 1.pelaajalla.
        /// 1.pelaajalla on 4 nappulaa kädessään.
        /// 
        ///   A--O--O
        ///   |  |  |
        ///   O--O--O
        ///   |  |  |
        ///   O--O--O
        /// 
        /// LOPPUHUOMIOT:
        /// 
        /// Mitä tapahtuu, jos yritetään poistaa omaa nappulaa? Näyttäisi siltä, että tässä luotetaan ettei sellaista tilannetta tule. 
        /// Tällainen pitäisi varmaankin dokumentoida esiehdoksi, tai sitten tehdä joku ratkaisu koodiin, esim. metodista poistuminen tai
        /// virheen käsittely.
        ///
        /// 
        /// </summary>
        [TestMethod()]
        public void MyllyViewModelTest_7()
        {
            // Luodaan lähtötiladata.
            var initialData = new TestData();
            initialData.GameState = MyllyViewModel.GameState.RemoveState;
            initialData.Player1.HasTurn = true;
            initialData.AddPiecesToHand(TestData.PlayerTableId.One, 4);
            // Luodaan blockit.
            initialData.CreateBlocksFromArray(initialData.TestBlockData);

            // 1. pelaajan paikat.

            initialData.Blocks[0].HasPiece = true;
            initialData.Blocks[0].IsContentSelectable = false;
            initialData.Blocks[0].BlockOwner = initialData.Player1;

            // Kloonataan alkutilanne. Tilanne ei saisi tästä testittä muuttua.
            var shouldBeData = TestHelper.CloneTestData(initialData);

            // Testataan.
            var result = TestHelper.DoWholeTest(initialData, shouldBeData, 0);

            Assert.AreEqual(true, result.Item1, result.Item2);
        }

        /// <summary>
        /// Testiasetelma 8.
        /// 
        /// Pelaaja 1 (A).
        /// Pelaaja 2 (B).
        /// 
        /// ALKUTILANNE:
        /// 
        ///   O--O--O
        ///   |  |  |
        ///   O--O--O
        ///   |  |  |
        ///   O--O--B
        /// 
        /// 
        /// Peli on removeStatessa. 
        /// Vuoro on  1.pelaajalla. 
        /// 1.pelaajalla on 4 nappulaa kädessään.
        /// 2.pelaajalla on 2 nappulaa kädessään.
        /// 
        /// *** Action: "klikataan" oikeaa alakulmaa. ***
        /// 
        /// HALUTTU LOPPU TILANNE: 
        /// 
        /// Peli on gameoverStatessa.
        /// Kummallakaan pelaajalla ei ole vuoroa. (tämä selviää ainoastaan lukemalla lähdekoodia ChangeState metodista).
        /// 1.pelaajalla on 3 nappulaa kädessään.
        /// 2.pelaajalla on 2 nappulaa kädessään.
        /// TheWinner = 1.pelaaja.
        /// 
        ///   O--O--O
        ///   |  |  |
        ///   O--O--O
        ///   |  |  |
        ///   O--O--O
        /// 
        /// LOPPUHUOMIOT:
        /// 
        /// ChangeState metodilla, jolla vaihetaan pelin tiloja, ja jota kutsutaan myös tästä testattavasta metodista on myös paljon tomintalogiikkaa. 
        /// Toimintalogiikka on päälekkäistä, ja näyttäisi siltä, että metodit luottavat toistensa toimintaan, mutta tällainen jaettu toimintalogiikka 
        /// ilman kunnon dokumentointia aiheuttaa suurella todennäköisyydellä ongelmatilanteita. Tässä tapauksessa voisi olla parempi, että 
        /// toimintalogiikka olisi yhdessä paikassa, eikä hajautettuna. Tai sitten täytyisi tehdä metodeihin parempi virheentarkistus, ja/tai dokumentointi.
        /// 
        /// </summary>
        [TestMethod()]
        public void MyllyViewModelTest_8()
        {
            // Luodaan lähtötiladata.
            var initialData = new TestData();
            initialData.GameState = MyllyViewModel.GameState.RemoveState;
            initialData.Player1.HasTurn = true;
            initialData.AddPiecesToHand(TestData.PlayerTableId.One, 3);
            initialData.AddPiecesToHand(TestData.PlayerTableId.Two, 2);
            // Luodaan blockit.
            initialData.CreateBlocksFromArray(initialData.TestBlockData);

            // 2. pelaajan paikat.
            initialData.Blocks[8].HasPiece = true;
            initialData.Blocks[8].BlockOwner = initialData.Player2;
            initialData.Blocks[8].IsContentSelectable = true;

            // Kloonataan alkutilanne.
            var shouldBeData = TestHelper.CloneTestData(initialData);

            // Tehdään muutokset, jotta saadaan toivottu lopputilanne.
            shouldBeData.Blocks[8].BlockOwner = null;
            shouldBeData.Blocks[8].HasPiece = false;
            shouldBeData.Blocks[8].IsContentSelectable = false; // Tämäkin selviää vasta lukemalla ChangeState metodia.
            shouldBeData.GameState = MyllyViewModel.GameState.GameOver;
            shouldBeData.Player2Table.Remove(shouldBeData.Player1Table.Last());
            shouldBeData.TheWinner = shouldBeData.Player1;
            shouldBeData.Player1.HasTurn = false;
            shouldBeData.Player2.HasTurn = false;

            // Testataan.
            var result = TestHelper.DoWholeTest(initialData, shouldBeData, 8);

            Assert.AreEqual(true, result.Item1, result.Item2);
        }

        /// <summary>
        /// Testiasetelma 9.
        /// 
        /// Z.IsContentSelected = true
        /// Q.IsContentSelected = true
        /// 
        /// ALKUTILANNE:
        /// 
        ///   Z--Q--O
        ///   |  |  |
        ///   O--O--O
        ///   |  |  |
        ///   O--O--O
        /// 
        /// 
        /// Peli on moveStatessa. 
        /// Vuoro on  1.pelaajalla. 
        /// 1.pelaajalla on 4 nappulaa kädessään.
        /// 2.pelaajalla on 4 nappulaa kädessään.
        /// 
        /// *** Action: "klikataan" keskimmäistä blockia. ***
        /// 
        /// HALUTTU LOPPU TILANNE: 
        /// 
        /// Metodi heittää poikkeuksen:
        /// 
        /// "InsercPiece_Executed: Ollaan IsMove tilassa, mutta pelaaja on muka valinnut useita nappuloita. Peli bugittaa."
        /// 
        /// LOPPUHUOMIOT:
        /// 
        /// 
        /// </summary>
        [TestMethod()]
        public void MyllyViewModelTest_9()
        {
            // Luodaan lähtötiladata.
            var initialData = new TestData();
            initialData.GameState = MyllyViewModel.GameState.MoveState;
            initialData.Player1.HasTurn = true;
            initialData.AddPiecesToHand(TestData.PlayerTableId.One, 4);
            initialData.AddPiecesToHand(TestData.PlayerTableId.Two, 4);
            // Luodaan blockit.
            initialData.CreateBlocksFromArray(initialData.TestBlockData);

            // Muokataan Z ja Q.
            initialData.Blocks[0].IsContentSelected = true;
            initialData.Blocks[1].IsContentSelected = true;

            // Kloonataan alkutilanne.
            var shouldBeData = TestHelper.CloneTestData(initialData);

            // Testataan.
            string exceptionMessage = "InsercPiece_Executed: Ollaan IsMove tilassa, mutta pelaaja on muka valinnut useita nappuloita. Peli bugittaa.";
            string errMsg = "Ei saatu aikaiseksi poikkeusta '" + exceptionMessage + "'.";
            bool saatiinPoikkeus = false;
            try
            {
                var result = TestHelper.DoWholeTest(initialData, shouldBeData, 4);
            }
            catch (Exception ex)
            {
                if (ex.Message.Equals(exceptionMessage)) saatiinPoikkeus = true; 
                
            }
            Assert.AreEqual(true, saatiinPoikkeus, errMsg);
        }

        /// <summary>
        /// Testiasetelma 10.
        /// 
        /// Pelaaja 1 (A).
        /// Pelaaja 2 (B).
        /// 
        /// ALKUTILANNE: Yläkulma on valittu, ja S ovat siirtomahdolisuudet.
        /// 
        ///   A--S--O
        ///   |  |  |
        ///   S--O--O
        ///   |  |  |
        ///   O--O--A
        /// 
        /// Peli on moveStatessa. 
        /// Vuoro on  1.pelaajalla.
        /// 1.pelaajalla on 4 nappulaa kädessään.
        /// 2.pelaajalla on 4 nappulaa kädessään.
        /// (yläkulma)A.IsContentSelected = true
        /// Kaikkien muiden blockien IsContentSelected = false;
        /// S.Selectable = true
        /// 
        /// 
        /// *** Action: "klikataan" oikeaa alakulmaa. ***
        /// 
        /// HALUTTU LOPPU TILANNE: (On klikattu oikeaa alakulmaa, jolloin siitä tulee valittu ja T ovat uudet siirtomahdollisuudet.
        /// 
        /// Peli on moveTilassa.
        /// Vuoro on  1.pelaajalla. 
        /// 1.pelaajalla on 4 nappulaa kädessään.
        /// 2.pelaajalla on 4 nappulaa kädessään.
        /// (alakulma)A.IsContentSelected = true
        /// (yläkulma)A.IsContentSelected = false
        /// S.Selectable = false
        /// T.Selectable = true
        /// 
        ///   A--S--O
        ///   |  |  |
        ///   S--O--T
        ///   |  |  |
        ///   O--T--A
        /// 
        /// LOPPUHUOMIOT:
        /// 
        /// Logiikka pettää hieman. Jos on nappuloita kädessä, niin ei saisi olla liikutustilassa. Kannattaisi tehdä ehkä tarkistus tästä.
        /// Toki itse pelissä tämä on tällä hetkellä mahdoton tilanne, mutta tällainen tarkistus voisi olla ihan hyvä.
        /// 
        /// 
        /// </summary>
        [TestMethod()]
        public void MyllyViewModelTest_10()
        {
            // Luodaan lähtötiladata.
            var initialData = new TestData();
            initialData.GameState = MyllyViewModel.GameState.MoveState;
            initialData.Player1.HasTurn = true;
            initialData.AddPiecesToHand(TestData.PlayerTableId.One, 4);
            initialData.AddPiecesToHand(TestData.PlayerTableId.Two, 4);
            // Luodaan blockit.
            initialData.CreateBlocksFromArray(initialData.TestBlockData);

            initialData.Blocks[0].HasPiece = true;
            initialData.Blocks[0].BlockOwner = initialData.Player1;
            initialData.Blocks[0].IsContentSelected = true;
            initialData.Blocks[0].IsContentSelectable = true;

            initialData.Blocks[8].HasPiece = true;
            initialData.Blocks[8].IsContentSelectable = true;
            initialData.Blocks[8].BlockOwner = initialData.Player1;

            initialData.Blocks[1].IsSelectable = true;
            initialData.Blocks[3].IsSelectable = true;

            // Luodaan lopputilanne.
            var shouldBeData = TestHelper.CloneTestData(initialData);
            shouldBeData.Blocks[0].IsContentSelected = false;
            shouldBeData.Blocks[1].IsSelectable = false;
            shouldBeData.Blocks[3].IsSelectable = false;

            shouldBeData.Blocks[5].IsSelectable = true;
            shouldBeData.Blocks[7].IsSelectable = true;
            shouldBeData.Blocks[8].IsContentSelected = true;

            // Testataan.
            var result = TestHelper.DoWholeTest(initialData, shouldBeData, 8);

            Assert.AreEqual(true, result.Item1, result.Item2);
        }

        /// <summary>
        /// Testiasetelma 11.
        /// 
        /// Pelaaja 1 (A).
        /// Pelaaja 2 (B).
        /// 
        /// ALKUTILANNE:
        /// 
        ///   O--O--O
        ///   |  |  |
        ///   O--O--O
        ///   |  |  |
        ///   O--O--O
        /// 
        /// Peli on moveStatessa. 
        /// Vuoro on  1.pelaajalla.
        /// 1.pelaajalla on 4 nappulaa kädessään.
        /// 2.pelaajalla on 4 nappulaa kädessään.
        /// Kaikkien blockien IsContentSelected = false;
        /// Kaikkien blockien IsSelectable = false;
        /// 
        /// *** Action: "klikataan" oikeaa alakulmaa. ***
        /// 
        /// HALUTTU LOPPU TILANNE:
        /// 
        /// Sama kuin alkutilanne.
        /// 
        /// LOPPUHUOMIOT:
        /// 
        /// 
        /// 
        /// </summary>
        [TestMethod()]
        public void MyllyViewModelTest_11()
        {
            // Luodaan lähtötiladata.
            var initialData = new TestData();
            initialData.GameState = MyllyViewModel.GameState.MoveState;
            initialData.Player1.HasTurn = true;
            initialData.AddPiecesToHand(TestData.PlayerTableId.One, 4);
            initialData.AddPiecesToHand(TestData.PlayerTableId.Two, 4);
            // Luodaan blockit.
            initialData.CreateBlocksFromArray(initialData.TestBlockData);

            // Luodaan lopputilanne.
            var shouldBeData = TestHelper.CloneTestData(initialData);

            // Testataan.
            var result = TestHelper.DoWholeTest(initialData, shouldBeData, 8);

            Assert.AreEqual(true, result.Item1, result.Item2);
        }

        /// <summary>
        /// Testiasetelma 12.
        /// 
        /// Pelaaja 1 (A).
        /// Pelaaja 2 (B).
        /// 
        /// ALKUTILANNE: Yläkulma on valittu, ja S ovat siirtomahdolisuudet.
        /// 
        ///   A--S--O
        ///   |  |  |
        ///   S--A--A
        ///   |  |  |
        ///   B--B--B
        /// 
        /// Peli on moveStatessa. 
        /// Vuoro on  1.pelaajalla.
        /// 1.pelaajalla on 0 nappulaa kädessään.
        /// 2.pelaajalla on 0 nappulaa kädessään.
        /// (yläkulma)A.IsContentSelected = true
        /// Kaikkien muiden blockien IsContentSelected = false;
        /// S.Selectable = true
        /// Kaikkien muiden blockien Selectable = false
        /// 
        /// 
        /// *** Action: "klikataan" 1. indeksissä olevaa blockia, eli keskella/ylhäällä olevaa S:ää. ***
        /// 
        /// HALUTTU LOPPU TILANNE: Nyt yläkulmassa oleva A pitäisi siirtyä yhden verran oikealle. Vuoro siirtyy 2. pelaajalle. Nyt B:n nappulat ovat valittavina (IsContentSelectable = true). Muut blockien 
        ///                        valintaan liittyvät propertyt ovat false.
        /// 
        /// Peli on moveTilassa.
        /// Vuoro on  2.pelaajalla. 
        /// 1.pelaajalla on 0 nappulaa kädessään.
        /// 2.pelaajalla on 0 nappulaa kädessään.
        /// kaikkien blockien Selectable = false
        /// kaikkien blockien IsContentSelected = false
        /// B blockien IsContentSelected = true
        /// kaikkien muiden blockien IsContentSelected = false
        /// 
        /// 
        ///   O--A--O
        ///   |  |  |
        ///   O--A--A
        ///   |  |  |
        ///   B--B--B
        /// 
        /// LOPPUHUOMIOT:
        /// 
        /// 
        /// 
        /// </summary>
        [TestMethod()]
        public void MyllyViewModelTest_12()
        {
            // Luodaan lähtötiladata.
            var initialData = new TestData();
            initialData.GameState = MyllyViewModel.GameState.MoveState;
            initialData.Player1.HasTurn = true;
            initialData.AddPiecesToHand(TestData.PlayerTableId.One, 0);
            initialData.AddPiecesToHand(TestData.PlayerTableId.Two, 0);

            // Luodaan blockit.
            initialData.CreateBlocksFromArray(initialData.TestBlockData);

            // 1. pelaaja blockit.
            initialData.Blocks[0].HasPiece = true;
            initialData.Blocks[0].BlockOwner = initialData.Player1;
            initialData.Blocks[0].IsContentSelected = true;
            initialData.Blocks[0].IsContentSelectable = true;

            initialData.Blocks[4].HasPiece = true;
            initialData.Blocks[4].IsContentSelectable = true;
            initialData.Blocks[4].BlockOwner = initialData.Player1;

            initialData.Blocks[5].HasPiece = true;
            initialData.Blocks[5].IsContentSelectable = true;
            initialData.Blocks[5].BlockOwner = initialData.Player1;


            // 2. pelaaja blockit.
            initialData.Blocks[6].HasPiece = true;
            initialData.Blocks[6].BlockOwner = initialData.Player2;

            initialData.Blocks[7].HasPiece = true;
            initialData.Blocks[7].BlockOwner = initialData.Player2;

            initialData.Blocks[8].HasPiece = true;
            initialData.Blocks[8].BlockOwner = initialData.Player2;

            // S-blockit.
            initialData.Blocks[1].IsSelectable = true;
            initialData.Blocks[3].IsSelectable = true;

            // Luodaan lopputilanne.
            var shouldBeData = TestHelper.CloneTestData(initialData);

            // 1. pelaaja.
            shouldBeData.Blocks[0].HasPiece = false;
            shouldBeData.Blocks[0].BlockOwner = null;
            shouldBeData.Blocks[0].IsContentSelected = false;
            shouldBeData.Blocks[0].IsContentSelectable = false;

            shouldBeData.Blocks[1].IsSelectable = false;
            shouldBeData.Blocks[1].HasPiece = true;
            shouldBeData.Blocks[1].BlockOwner = shouldBeData.Player1;

            shouldBeData.Blocks[4].IsContentSelectable = false;
            shouldBeData.Blocks[5].IsContentSelectable = false;

            // indeksissä 3 oleva S.
            shouldBeData.Blocks[3].IsSelectable = false;

            // 2. pelaaja.

            shouldBeData.Blocks[6].IsContentSelectable = true;
            shouldBeData.Blocks[7].IsContentSelectable = true;
            shouldBeData.Blocks[8].IsContentSelectable = true;

            shouldBeData.Player1.HasTurn = false;
            shouldBeData.Player2.HasTurn = true;

            // Testataan.
            var result = TestHelper.DoWholeTest(initialData, shouldBeData, 1);

            Assert.AreEqual(true, result.Item1, result.Item2);
        }



    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    /// <summary>
    /// Testausdatan luokka. Testausdataan asetetaan lähtötilanteeseen liittyviä parametrejä sekä hyväkstyttävän lopputilan parametrit.
    /// Toisin sanoen, kutakin testiä varten luodaan käytännössä katsoen kaksi TestData oliota, alkutilanne ja odotettavissa oleva lopputilanne.
    /// </summary>
    public class TestData
    {
        public enum PlayerTableId { One, Two };

        /// <summary>
        /// Property myllypelin testikentän rakenteen luomista varten. Int-taulukko pitää numeerisesti sisällään sen, millainen on 
        /// kentän rakenne. Kenttä koostuu Block-olioista. Oletuksena luodaan, graafisesti tällainen kenttä:
        /// 
        /// O--O--O
        /// |     |
        /// O--O--O
        /// |     |
        /// O--O--O
        /// 
        /// TestBlockDatan oletuksena oleva int-taulukko sisältää siis yhdeksän blockin LeftWay, RightWay, UpWay, DownWay sekä 
        /// HasObject boolean arvot. Numerot konvertoidaan Block-olioiksi myöhemmin. Nämä arvot ovat pelin kannalta tavallaan staattisia arvota, 
        /// sillä ne kertovat vain sen mihinkä suuntaan blockista voi mennä ja sen, onko blockissa pelinappulapaikkaa. Block-olioilla on myös HasPiece, 
        /// BlockOwner, IsSelectable, IsContentSelectable ja IsContentSelected propertyt. Nämä propertyt ovat luonteeltaan 
        /// dynaamisia, eli muuttuvat pelin kulun aikana, eikä niitä aseteta tässä int-taulukossa olevien numeroiden avulla.
        /// 
        /// </summary>
        public int[] TestBlockData { get; set; } = new int[9] { 25, 27, 26, 29, 31, 30, 21, 23, 22 };

        /// <summary>
        /// Property, joka määrää testikentän leveyden. 
        /// </summary>
        public int XDimension { get; set; } = 3;

        /// <summary>
        /// Property, joka määrää testikentän korkeuden.
        /// </summary>
        public int YDimension { get; set; } = 3;

        public Player Player1 { get; set; } = CreateTestPlayer("A", 4, false);

        public Player Player2 { get; set; } = CreateTestPlayer("B", 4, false);

        public Player TheWinner { get; set; }

        /// <summary>
        /// Property, joka pitää sisällään 1-pelaajan kädessä olevien pelimerkkien lukumäärän.
        /// </summary>
        public ObservableCollection<Dummy> Player1Table { get; set; } = new ObservableCollection<Dummy>();

        /// <summary>
        /// Property, joka pitää sisällään 2-pelaajan kädessä olevien pelimerkkien lukumäärän.
        /// </summary>
        public ObservableCollection<Dummy> Player2Table { get; set; } = new ObservableCollection<Dummy>();

        /* Pelille asetettava tila. */
        public MyllyViewModel.GameState GameState { get; set; } = MyllyViewModel.GameState.InsertState;

        /// <summary>
        /// Property pelikentän blockeille.
        /// </summary>
        public ObservableCollection<Block> Blocks { get; set; } = new ObservableCollection<Block>();

        public TestData()
        {
            CreateBlocksFromArray(new int[9] { 25, 27, 26, 29, 31, 30, 21, 23, 22 });
        }

        /// <summary>
        /// Konvertoi int taulukon Block dataksi.
        /// </summary>
        /// <param name="array"></param>
        public void CreateBlocksFromArray(int[] array)
        {
            if (array == null) throw new NullReferenceException("TestData.CreateBlocksFromArray: array == null.");
        
            MyllyMap mMap = new MyllyMap();
            mMap.CreateDataFromArray(array);
            Blocks = mMap.GameBoardData;
        }

        /// <summary>
        /// Lisätään joko 1. tai 2. pelaajan käteen pelimerkkejä.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="pieceCount"></param>
        public void AddPiecesToHand(PlayerTableId id, int pieceCount)
        {
            for (int i=0; i<pieceCount; i++)
            {
                if (id == PlayerTableId.One) Player1Table.Add(new Dummy());
                else if (id == PlayerTableId.Two) Player2Table.Add(new Dummy());
            }
        }

        /// <summary>
        /// Luo testipelaajan. 
        /// </summary>
        /// <param name="name">Pelaajan nimi</param>
        /// <param name="pieceCount">Pelaajan pelinappuloiden kokonaislukumäärä.</param>
        /// <param name="hasTurn">Pelaajan vuoroa ilmaiseva parametri.</param>
        /// <returns>testipelaaja.</returns>
        public static Player CreateTestPlayer(string name, int pieceCount, bool hasTurn)
        {
            if (name == null || pieceCount < 0)
                throw new Exception(String.Format("CreateTestPlayer: name == null -> {0} || pieceCount < 0 -> {1}",
                                                   name == null, pieceCount < 0));
            var p = new Player();
            p.PlayerName = name;
            p.PieceCount = pieceCount;
            p.HasTurn = hasTurn;
            return p;
        }
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    /* Testauksen apu-metodit. */

    /// <summary>
    /// Luokka, joka pitää sisällään apumetodeja testausta varten.
    /// </summary>
    public class TestHelper
    {
        /// <summary>
        /// Eräänlainen tehdas-metodi, joka luo MyllyViewModelin instanssin, joka on ns. aloitustilassa. Parametrina annettu td 
        /// määrää osan ohjelman alkutiloista. Kaikkia tiloja ei voi tässä vaiheessa vielä alustaa.
        /// </summary>
        /// <param name="td">Aloitus tila</param>
        /// <returns></returns>
        public static MyllyViewModel CreateMVM(TestData td)
        {
            // Luodaan MyllyViewModel tavanomaiseen tapaan. Testauksen kannalta mvm on nyt todennäköisesti väärässä tilassa.
            var mvm = new MyllyViewModel();

            mvm.Player1 = td.Player1;
            mvm.Player2 = td.Player2;
            mvm.Player1Table = td.Player1Table;
            mvm.Player2Table = td.Player2Table;
            mvm.TheWinner = td.TheWinner;

            // Luodaan blockit int-taulukosta.
            // CreateGameBoardBlocks(mvm, td.TestBlockData, td.XDimension, td.XDimension);

   
            /*   
            foreach (var item in td.Blocks)
            {
                Block b = new Block();
                b.BlockOwner = item.BlockOwner;
                b.DownWay = item.DownWay;
                b.LeftWay = item.LeftWay;
                b.UpWay = item.UpWay;
                b.RightWay = item.RightWay;
                b.HasObject = item.HasObject;
                b.HasPiece = item.HasPiece;
                b.IsContentSelectable = item.IsContentSelectable;
                b.IsContentSelected = item.IsContentSelected;
                b.IsSelectable = item.IsSelectable;
                mvm.MVBlocks.Add(b);
            }
            */
        
            mvm.MVBlocks = td.Blocks;
            mvm.GameColumnCount = td.YDimension;
            mvm.GameRowCount = td.XDimension;

            // Tässä luodaan PrivateObject luokan instanssi. Tämän avulla päästään helposti käsiksi mvm:n privaatteihin atribuutteihin ja metodeihin.
            //PrivateObject obj = new PrivateObject((Object)mvm);

            // Asetetaan pelin tila halutunlaiseksi.
            //obj.Invoke("ChangeState", td.GameState);

            mvm.IsGameBegin = false;
            mvm.IsGameOver = false;
            mvm.IsInsertMode = false;
            mvm.IsMoveMode = false;
            mvm.IsRemoveMode = false;

            switch (td.GameState)
            {
                case MyllyViewModel.GameState.InsertState:
                    mvm.IsInsertMode = true;
                    break;
                case MyllyViewModel.GameState.MoveState:
                    mvm.IsMoveMode = true;
                    break;
                case MyllyViewModel.GameState.RemoveState:
                    mvm.IsRemoveMode = true;
                    break;
                case MyllyViewModel.GameState.GameOver:
                    mvm.IsGameOver = true;
                    break;
                case MyllyViewModel.GameState.GameStart:
                    mvm.IsGameBegin = true;
                    break;
            }

            return mvm;
        }

        /// <summary>
        /// Tässä metodissa luodaan MyllyViewModelille testi-kenttä (Block-oliot) ja dimensiot pelikentälle.
        /// Jotta voidaan käyttää MyllyMapia, niin täytyy Myllyn Properties -> AssemblyInfo.cs:n asettaa seuraava rivi:
        ///      [assembly: InternalsVisibleTo("MyllyTests")]
        /// Näin joudutaan tekemään, sillä MyllyMap on MyllyViewModel.cs:n internal luokka, eikä näin ollen ole 
        /// automaattisesti saavutettavissa muista käännösyksiköistä.
        /// </summary>
        /// <param name="mvm">testattava MyllyViewModel.</param>
        /// <param name="boardData">testattavan pelikentän data int-taulukkona.</param>
        /// <param name="xDim">Pelikentän leveys.</param>
        /// <param name="yDim">Pelikentän korkeus.</param>
        public static void CreateGameBoardBlocks(TestData td, int[] boardData, int xDim, int yDim)
        {
            // Luodaan testikenttä.
            MyllyMap testiMap = new MyllyMap();

            // Annetaan pelialueen dimensiot. 
            testiMap.RowCount = xDim;
            testiMap.ColumnCount = yDim;

            // Konvertoidaan ja asetetaan int taulukon data Block-olioiksi ja asetetaan konvertoidut Blockit testiMapin GameBoardData propertyyn.
            // Mylly pelissä pelilauta koostuu Block-olioista.
            testiMap.CreateDataFromArray(boardData);

            // Asetetaan nyt pelikentan Blockit MyllyViewModeliin.
            td.Blocks = testiMap.GameBoardData;
            td.XDimension = xDim;
            td.YDimension = yDim;
        }

        /// <summary>
        /// Suorittaa testattavan metodin.
        /// </summary>
        /// <param name="mvm">Testattavana oleva MyllyViewModel.</param>
        /// <param name="b">Testatttavlle metodille 'InsertPieceSelection_Executed' vietävä Block luokan olio.</param>
        public static void RunTest(MyllyViewModel mvm, Block b)
        {
            // Luodaan PrivateObject olio. Tämän avulla päästään suorittamaan mvm:n privaatteja metodeja.
            PrivateObject obj = new PrivateObject((Object)mvm);

            // Luodaan ExecutedRoutedEventArgs luodan olio reflektiolla, sillä sen rakentaa ei voi kutsua suoraan.
            // Rakentaja ottaa argumenttina ICommand olion sekä blockin, jota on loogisesti katsottuna "klikattu". 
            // Asetetaan commandiksi ApplicatinCommands.Paste. Tällä ei ole mitään merkitystä, sillä 
            // commandeja ei tarvita tässä testauksessa mihinkään. Nyt tehdään vain konstruktori tyytyväiseksi. 
            // Ainoa oleellinen argumentti on b, sillä testatta metodi käyttää vain sitä.
            // Parametri b wrapataan siis ExecutedRoutedEventArgs luokan ilmentymän Param propertyyn.
            // Näin voidaan simuloida sitä tilannetta, että klikalaan graafisen käyttöliittymän kautta 
            // block-oliota. 
            var e = (ExecutedRoutedEventArgs)typeof(ExecutedRoutedEventArgs).GetConstructor(
                  BindingFlags.NonPublic | BindingFlags.Instance,
                  null, new Type[2] {typeof(ICommand),typeof(object) }, null).Invoke(new object[2] { ApplicationCommands.Paste, b });

            // Kutsutaan testattavaa metodia. Asetetaan lähettäjä nulliksi, sillä testattava metodi ei käytä sitä lainkaan.
            obj.Invoke("InsertPieceSelection_Executed", new object[2] { null, e });
        }

        /// <summary>
        /// Varsinainen testausmetodi/oraakkeli.
        /// </summary>
        /// <param name="initialData">Lähtötilanne.</param>
        /// <param name="shouldBeData">Odotettavissa oleva tilanne.</param>
        /// <param name="blockIndex">Klikattavan blockin indeksi.</param>
        /// <returns>Testin lopputulos.</returns>
        public static Tuple<bool, string> DoWholeTest(TestData initialData, TestData shouldBeData, int blockIndex /*, [CallerMemberName]string memberName = "" */)
        {
            // VirheViesti.
            StringBuilder errMsg = new StringBuilder();

            if (initialData == null || shouldBeData == null)
            {
                errMsg.Append("DoWholeTest:\n");
                if (initialData == null) errMsg.Append("initialData == null\n");
                if (shouldBeData == null) errMsg.Append("shouldBeData == null\n");
                return new Tuple<bool, string>(false, errMsg.ToString());
            }

            // Laitetaan ohjelma alkutilan mukaiseen tilaan.
            var mvm = TestHelper.CreateMVM(initialData);

            // Laitetaan toinen ohjelma oletettavissa olevaan tilaan.
            var mvmShouldBe = TestHelper.CreateMVM(shouldBeData);

            // Suoritetaan testattava metodi.
            try
            {
                TestHelper.RunTest(mvm, mvm.MVBlocks[blockIndex]);
            }
            catch (IndexOutOfRangeException)
            {
                errMsg.Append(String.Format("DoWholeTest: blockIndex out of bounds."));
                return new Tuple<bool, string>(false, errMsg.ToString());
            }

            // Konvertoidaan ohjelman tila takaisin testausData-olioksi.
            var afterTestData = TestHelper.ConvertMVMtoTestData(mvm);

            // Konvertoidaan myös halutunlaisessa lopputilassa olevan ohjelma takaisin testausdata-olioksi.
            var shouldBeTestData = TestHelper.ConvertMVMtoTestData(mvmShouldBe);

            // Testataan alkutilanne ja haluttu tilanne.
            bool result = TestHelper.TestDataEquals(afterTestData, shouldBeTestData, errMsg);

            return new Tuple<bool, string>(result, errMsg.ToString());
        }

        public static TestData ConvertMVMtoTestData(MyllyViewModel mvm)
        {
            if (mvm == null) throw new NullReferenceException("ConvertMVMtoTestData: mvm == null.");

            var data = new TestData();

            data.Blocks = mvm.MVBlocks;
            // data.GameState
            var gamestates = new Dictionary<string, bool>();
            gamestates.Add("IsGameBegin", mvm.IsGameBegin);
            gamestates.Add("IsGameOver", mvm.IsGameOver);
            gamestates.Add("IsInsertMode", mvm.IsInsertMode);
            gamestates.Add("IsMoveMode", mvm.IsMoveMode);
            gamestates.Add("IsRemoveMode", mvm.IsRemoveMode);

            // Lasketaan gamestateseista true arvojen määrä.
            int y = gamestates.Values.Count(x => x == true);

            // Jos kaikki pelin tilat ovat false tai jos useampi kuin yksi pelin tiloista on true, niin tällöin ei voida määritellä pelin tilaa.
            // Näin sattuessa MyllyViewModel luokassa on todennäköisesti jokin looginen bugi.
            if (y != 1)
            {
                StringBuilder sb = new StringBuilder();

                foreach (var item in gamestates)
                {
                    sb.Append("mvm." + item.Key + " == " + item.Value + "\n");
                }

                throw new Exception(
                    String.Format("ConvertMVMtoTestData: Ei voida määrittää mvm:n Gamestatea. Täsmälleen yhden tilan tulee olla true.\n{0}", sb.ToString()));
            }

            // Määritellään GameState testDatalle.
            if (mvm.IsGameBegin) data.GameState = MyllyViewModel.GameState.GameStart;
            else if (mvm.IsGameOver) data.GameState = MyllyViewModel.GameState.GameOver;
            else if (mvm.IsInsertMode) data.GameState = MyllyViewModel.GameState.InsertState;
            else if (mvm.IsMoveMode) data.GameState = MyllyViewModel.GameState.MoveState;
            else if (mvm.IsRemoveMode) data.GameState = MyllyViewModel.GameState.RemoveState;

            data.Player1 = mvm.Player1;
            data.Player2 = mvm.Player2;
            data.Player1Table = mvm.Player1Table;
            data.Player2Table = mvm.Player2Table;
            data.TheWinner = mvm.TheWinner;

            // Muutetaan blockit int taulukoksi ja asetetaan sitten TestDatan TestBlockData propertyyn.
            List<int> blockitIntMuodossa = new List<int>();
            foreach (var item in mvm.MVBlocks)
            {
                blockitIntMuodossa.Add(ConvertBlockToInt(item));
            }

            data.TestBlockData = blockitIntMuodossa.ToArray();

            data.XDimension = mvm.GameRowCount;
            data.YDimension = mvm.GameColumnCount;

            data.Blocks = mvm.MVBlocks;

            return data;
        }

        public static bool CompareBlocks(ObservableCollection<Block> actualBlocks, ObservableCollection<Block> shouldBeBlocks, StringBuilder errMsg)
        {
            if (actualBlocks == null || shouldBeBlocks == null || errMsg == null)
                throw new NullReferenceException(String.Format("CompareBlocks: actualBlocks == null -> {0}, shouldBeBlocks == null -> {1}, errMsg == null -> {2}.",
                                                                actualBlocks == null, shouldBeBlocks == null, errMsg == null));
            bool testOk = true;

            if (actualBlocks.Count != shouldBeBlocks.Count)
            {
                testOk = false;
                errMsg.Append(String.Format("Blocks Count pitäisi olla {0}, mutta nyt se on {1}.\n",
                    shouldBeBlocks.Count,
                    actualBlocks.Count));
                // Nyt täytyy poistua. Muuten voi tulla index out of bounds for silmukassa.
                return testOk;
            }

            // Käytään kaikki blocksit läpi. Indeksoidaan, jotta voidaan sanoa missä indekseissä blockit poikkeavat.
            for (int i = 0; i < actualBlocks.Count; i++)
            {
                bool indexPrinted = false;

                if (actualBlocks[i].DownWay != shouldBeBlocks[i].DownWay)
                {
                    testOk = false;
                    PrintIndex(ref indexPrinted, i, errMsg);
                    errMsg.Append(String.Format("Block.DownWay pitäisi olla {0}, mutta nyt se on {1}.\n",
                        shouldBeBlocks[i].DownWay,
                        actualBlocks[i].DownWay));
                }

                if (actualBlocks[i].LeftWay != shouldBeBlocks[i].LeftWay)
                {
                    testOk = false;
                    PrintIndex(ref indexPrinted, i, errMsg);
                    errMsg.Append(String.Format("Block.LeftWay pitäisi olla {0}, mutta nyt se on {1}.\n",
                        shouldBeBlocks[i].LeftWay,
                        actualBlocks[i].LeftWay));
                }

                if (actualBlocks[i].UpWay != shouldBeBlocks[i].UpWay)
                {
                    testOk = false;
                    PrintIndex(ref indexPrinted, i, errMsg);
                    errMsg.Append(String.Format("Block.UpWay pitäisi olla {0}, mutta nyt se on {1}.\n",
                        shouldBeBlocks[i].UpWay,
                        actualBlocks[i].UpWay));
                }

                if (actualBlocks[i].RightWay != shouldBeBlocks[i].RightWay)
                {
                    testOk = false;
                    PrintIndex(ref indexPrinted, i, errMsg);
                    errMsg.Append(String.Format("Block.RightWay pitäisi olla {0}, mutta nyt se on {1}.\n",
                        shouldBeBlocks[i].RightWay,
                        actualBlocks[i].RightWay));
                }

                if (actualBlocks[i].HasObject != shouldBeBlocks[i].HasObject)
                {
                    testOk = false;
                    PrintIndex(ref indexPrinted, i, errMsg);
                    errMsg.Append(String.Format("Block.HasObject pitäisi olla {0}, mutta nyt se on {1}.\n",
                        shouldBeBlocks[i].HasObject,
                        actualBlocks[i].HasObject));
                }

                if (actualBlocks[i].HasPiece != shouldBeBlocks[i].HasPiece)
                {
                    testOk = false;
                    PrintIndex(ref indexPrinted, i, errMsg);
                    errMsg.Append(String.Format("Block.HasPiece pitäisi olla {0}, mutta nyt se on {1}.\n",
                        shouldBeBlocks[i].HasPiece,
                        actualBlocks[i].HasPiece));
                }

                if (actualBlocks[i].IsContentSelectable != shouldBeBlocks[i].IsContentSelectable)
                {
                    testOk = false;
                    PrintIndex(ref indexPrinted, i, errMsg);
                    errMsg.Append(String.Format("Block.IsContentSelectable pitäisi olla {0}, mutta nyt se on {1}.\n",
                        shouldBeBlocks[i].IsContentSelectable,
                        actualBlocks[i].IsContentSelectable));
                }

                if (actualBlocks[i].IsContentSelected != shouldBeBlocks[i].IsContentSelected)
                {
                    testOk = false;
                    PrintIndex(ref indexPrinted, i, errMsg);
                    errMsg.Append(String.Format("Block.IsContentSelected pitäisi olla {0}, mutta nyt se on {1}.\n",
                        shouldBeBlocks[i].IsContentSelected,
                        actualBlocks[i].IsContentSelected));
                }

                if (actualBlocks[i].IsSelectable != shouldBeBlocks[i].IsSelectable)
                {
                    testOk = false;
                    PrintIndex(ref indexPrinted, i, errMsg);
                    errMsg.Append(String.Format("Block.IsSelectable pitäisi olla {0}, mutta nyt se on {1}.\n",
                        shouldBeBlocks[i].IsSelectable,
                        actualBlocks[i].IsSelectable));
                }

                // kumpikin testattavan blockin BlockOwner == null, eli tilanne ok.
                if (actualBlocks[i].BlockOwner == null && shouldBeBlocks[i].BlockOwner == null)
                {
                    // Ei tehdä mitään.
                }

                // Jos vain toinen BlockOwner on null, niin tällöin ei voi olla sisällöltään samata blockit.
                else if ((actualBlocks[i].BlockOwner == null && shouldBeBlocks[i].BlockOwner != null) || (actualBlocks[i].BlockOwner != null && shouldBeBlocks[i].BlockOwner == null))
                {
                    testOk = false;
                    PrintIndex(ref indexPrinted, i, errMsg);
                    errMsg.Append(String.Format("Block.BlockOwner pitäisi olla {0}, mutta nyt se on {1}.\n",
                        shouldBeBlocks[i].BlockOwner == null ? "null" : shouldBeBlocks[i].BlockOwner.PlayerName,
                        actualBlocks[i].BlockOwner == null ? "null" : actualBlocks[i].BlockOwner.PlayerName));
                }
                // Pelaajien nimet eivät ole samat. HUOM: testeissä pelaajien nimien tulee olla erilaiset. Muuten testiin ei voi luottaa.
                else if (!actualBlocks[i].BlockOwner.PlayerName.Equals(shouldBeBlocks[i].BlockOwner.PlayerName))
                {
                    testOk = false;
                    PrintIndex(ref indexPrinted, i, errMsg);
                    errMsg.Append(String.Format("Block.BlockOwner.PlayerName pitäisi olla {0}, mutta nyt se on {1}.\n",
                        shouldBeBlocks[i].BlockOwner.PlayerName,
                        actualBlocks[i].BlockOwner.PlayerName));
                }
            } // for loop

            return testOk;
        }

        public static void PrintIndex(ref bool indexPrinted, int index, StringBuilder errMsg)
        {
            if (!indexPrinted) { errMsg.Append(String.Format("Virhe Blocks indeksissä {0}.\n", index)); indexPrinted = true; }
        }

        /// <summary>
        /// Apumetodi, joka muuttaa parametrina olevan blockin numeeriseen muotoon. 
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public static int ConvertBlockToInt(Block b)
        {
            if (b == null) throw new NullReferenceException("ConvertBlockToint: b == null.");

            // Huomio. Nämä copypastetettu MyllyMap-luokasta. Parannasehdotus, lisää Blockien int:ksi konvertointi metodi MyllyMap luokkaan, eikä tänne.
            const int pRight = 1;
            const int pLeft = 2;
            const int pUp = 4;
            const int pDown = 8;
            const int pHasObject = 16;

            int mask = 0;
            if (b.RightWay) mask = mask | pRight;
            if (b.LeftWay) mask = mask | pLeft;
            if (b.UpWay) mask = mask | pUp;
            if (b.DownWay) mask = mask | pDown;
            if (b.HasObject) mask = mask | pHasObject;

            return mask;
        }

        /// <summary>
        /// Metodi, joka konvertoi GameState enumin merkkijonomuotoon.
        /// </summary>
        /// <param name="gState"></param>
        /// <returns></returns>
        public static string GameStateToString(MyllyViewModel.GameState gState)
        {
            switch (gState)
            {
                case MyllyViewModel.GameState.GameOver: return "MyllyViewModel.GameState.GameOver";
                case MyllyViewModel.GameState.GameStart: return "MyllyViewModel.GameState.GameStart";
                case MyllyViewModel.GameState.InsertState: return "MyllyViewModel.GameState.InsertState";
                case MyllyViewModel.GameState.MoveState: return "MyllyViewModel.GameState.MoveState";
                case MyllyViewModel.GameState.RemoveState: return "MyllyViewModel.GameState.RemoveState";
                default: throw new Exception("GameStateToString: gStatea vastaavaa casea ei ole määritelty.");
            }
        }

        // Keskeneräinen. TODO: tee loppuun jos ehdit. Ei välttämätön testaamisen kannalta, mutta olisi kuitenkin ihan hyvä.
        public static String TestDataToString(TestData td)
        {
            StringBuilder sb = new StringBuilder();

            var state = GameStateToString(td.GameState);
            var na = "ei maaritelty";

            // Pelaaja 1 tiedot.
            var p1 = td.Player1;
            var player1Name = p1 == null ? "null" : p1.PlayerName;
            var player1PieceCount = p1 == null ? na : p1.PieceCount.ToString();
            var player1Table = td.Player1Table == null ? "null" : td.Player1Table.Count.ToString();

            // Pelaaja 2 tiedot.
            var p2 = td.Player2;
            var player2Name = p1 == null ? "null" : p2.PlayerName;
            var player2PieceCount = p1 == null ? na : p2.PieceCount.ToString();
            var player2Table = td.Player2Table == null ? "null" : td.Player1Table.Count.ToString();

            // Tulostetaan voittaneen pelaajan nimi.
            var winner = td.TheWinner == null ? "null" : td.TheWinner.PlayerName;

            // Tulostetaan tBlockData (int-taulukko).
            var tBlockData = "";
            if (td.TestBlockData == null) tBlockData = "null";
            else 
            {
                tBlockData = string.Join(", ", td.TestBlockData.Select(x => x.ToString()));
            }

            var xDim = td.XDimension.ToString();
            var yDim = td.YDimension.ToString();

            return "";
        }

        /// <summary>
        /// Testaa sen, ovatko kaksi parametrina olevaa TestDataa arvoiltaan samanlaiset. Jos eroavaisuuksia tulee, niin ne lisätään 
        /// parametriin errMsg.
        /// </summary>
        /// <param name="actualData">Todelinen testidata.</param>
        /// <param name="shouldBeData">halutunlaisessa tilassa oleva testidata.</param>
        /// <param name="errMsg">Testidatojen eroavaisuudet lisätään tähän.</param>
        /// <returns>true, jos TestDatat ovat arvoiltaan samat, muuten false.</returns>
        public static bool TestDataEquals(TestData actualData, TestData shouldBeData, StringBuilder errMsg)
        {
            // Kriittisiä null tarkistuskia.
            if (actualData == null || shouldBeData == null || errMsg == null)
                throw new NullReferenceException(String.Format("TestEquals: actualData == null -> {0}, shouldBeData == null -> {1}, errMsg == null -> {2}.",
                                                                actualData == null, shouldBeData == null, errMsg == null));

            // Lisää null tarkistuksia.
            if (actualData.Player1 == null || shouldBeData.Player1 == null ||
                actualData.Player2 == null || shouldBeData.Player2 == null ||
                actualData.Player1Table == null || shouldBeData.Player1Table == null ||
                actualData.Player2Table == null || shouldBeData.Player2Table == null)
            {
                string err = "TestEquals:\n";
                err += string.Format("actualData.Player1 == null -> {0}.\n", actualData.Player1 == null);
                err += string.Format("shouldBeData.Player1 == null -> {0}.\n", shouldBeData.Player1 == null);
                err += string.Format("actualData.Player2 == null -> {0}.\n", actualData.Player2 == null);
                err += string.Format("shouldBeData.Player2 == null -> {0}.\n", shouldBeData.Player2 == null);
                err += string.Format("actualData.Player1Table == null -> {0}.\n", actualData.Player1Table == null);
                err += string.Format("shouldBeData.Player1Table == null -> {0}.\n", shouldBeData.Player1Table == null);
                err += string.Format("actualData.Player2Table == null -> {0}.\n", actualData.Player2Table == null);
                err += string.Format("shouldBeData.Player2Table == null -> {0}.", shouldBeData.Player2Table == null);
                throw new NullReferenceException(err);
            }   

            bool testOk = true;

            // Vertaillaan GameStatea.
            if (actualData.GameState != shouldBeData.GameState)
            {
                testOk = false;
                errMsg.Append(String.Format("GameState pitäisi olla {0}, mutta GameState on {1}.\n",
                    GameStateToString(shouldBeData.GameState),
                    GameStateToString(actualData.GameState)));
            }

            // Vertaillaan kentän "staatinen" rakenne, eli TestBlockDatat.
            if (!actualData.TestBlockData.SequenceEqual(shouldBeData.TestBlockData))
            {
                testOk = false;
                errMsg.Append(String.Format("TestBlockData pitäisi olla {0}, mutta TestBlockData on {1}.\n",
                    "[{0}]", string.Join(", ", shouldBeData.TestBlockData),
                    "[{0}]", string.Join(", ", actualData.TestBlockData)));
            }

            // Vertaillaan kentän leveys.
            if (actualData.XDimension != shouldBeData.XDimension)
            {
                testOk = false;
                errMsg.Append(String.Format("XDimension pitäisi olla {0}, mutta XDimension on {1}.\n", shouldBeData.XDimension, actualData.XDimension));
            }

            // Vertaillaan kentän korkeus
            if (actualData.YDimension != shouldBeData.YDimension)
            {
                testOk = false;
                errMsg.Append(String.Format("YDimension pitäisi olla {0}, mutta YDimension on {1}.\n", shouldBeData.YDimension, actualData.YDimension));
            }

            // Testataan blockit.
            testOk = CompareBlocks(actualData.Blocks, shouldBeData.Blocks, errMsg);

            // jos pelaajien nimet ovat null, niin tällöin ollaan epäonnistuttu. Lähtöoletuksena on se, että nimet eivät saa olla null.
            // TODO: dokumentoi tämä oletus jonnekin.
            if (actualData.Player1.PlayerName == null || shouldBeData.Player1.PlayerName == null)
            {
                testOk = false;
                errMsg.Append(String.Format("Palyer1.PlayerName ei saa olla null. Nyt actualData.Player1.PlayerName == {0}, shouldBeData.Player1.PlayerName == {1}.\n", 
                    actualData.Player1 == null ? "null" : actualData.Player1.PlayerName,
                    shouldBeData.Player1 == null ? "null" : shouldBeData.Player1.PlayerName));
            }
            else if (!actualData.Player1.PlayerName.Equals(shouldBeData.Player1.PlayerName))
            {
                testOk = false;
                errMsg.Append(String.Format("Player1.PlayerName pitäisi olla {0}, mutta on nyt {1}.\n", shouldBeData.Player1.PlayerName, actualData.Player1.PlayerName));
            }
            else if (actualData.Player1.HasTurn != shouldBeData.Player1.HasTurn)
            {
                testOk = false;
                errMsg.Append(String.Format("Player1.HasTurn pitäisi olla {0}, mutta on nyt {1}.\n", shouldBeData.Player1.HasTurn, actualData.Player1.HasTurn));
            }

            if (actualData.Player2.PlayerName == null || shouldBeData.Player2.PlayerName == null)
            {
                testOk = false;
                errMsg.Append(String.Format("Palyer2.PlayerName ei saa olla null. Nyt actualData.Player2.PlayerName == {0}, shouldBeData.Player2.PlayerName == {1}.\n",
                    actualData.Player2 == null ? "null" : actualData.Player2.PlayerName,
                    shouldBeData.Player2 == null ? "null" : shouldBeData.Player2.PlayerName));
            }
            else if (!actualData.Player2.PlayerName.Equals(shouldBeData.Player2.PlayerName))
            {
                testOk = false;
                errMsg.Append(String.Format("Player2.PlayerName pitäisi olla {0}, mutta on nyt {1}.\n", shouldBeData.Player2.PlayerName, actualData.Player2.PlayerName));
            }
            else if (actualData.Player2.HasTurn != shouldBeData.Player2.HasTurn)
            {
                testOk = false;
                errMsg.Append(String.Format("Player2.HasTurn pitäisi olla {0}, mutta on nyt {1}.\n", shouldBeData.Player2.HasTurn, actualData.Player2.HasTurn));
            }

            if (actualData.TheWinner == null && shouldBeData.TheWinner == null)
            {
                // ok. Samat ovat.
            }
            // Toinen on null, eivät siis ole samat.
            else if (actualData.TheWinner == null || shouldBeData.TheWinner == null)
            {
                testOk = false;
                errMsg.Append(String.Format("TheWinner pitäisi olla {0}, mutta on nyt {1}.\n",
                    shouldBeData.TheWinner == null ? "null" : shouldBeData.TheWinner.PlayerName,
                    actualData.TheWinner == null ? "null" : actualData.TheWinner.PlayerName));
            }
            // Voittajan nimet eroavat toisistaan. Eivät ole samat.
            else if (!actualData.TheWinner.PlayerName.Equals(shouldBeData.TheWinner.PlayerName))
            {
                testOk = false;
                errMsg.Append(String.Format("TheWinner pitäisi olla {0}, mutta on nyt {1}.\n",
                    shouldBeData.TheWinner.PlayerName,
                    actualData.TheWinner.PlayerName));
            } 



            return testOk;
        }

        /// <summary>
        /// Nopea hätäratkaisu TestDatan osittaiseen kloonaamiseen. Tässä kloonataan vain testaamiseen tarvittava data. TODO: jos jää aikaa, niin kehitä jokin 
        /// fiksumpi tapa.
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public static TestData CloneTestData(TestData src)
        {
            var dst = new TestData();

            Player p1 = new Player();
            if (src.Player1 != null)
            {
                p1.PlayerName = src.Player1.PlayerName;
                p1.HasTurn = src.Player1.HasTurn;
            }

            dst.Player1 = p1;

            Player p2 = new Player();
            if (src.Player2 != null)
            {
                p2.PlayerName = src.Player2.PlayerName;
                p2.HasTurn = src.Player2.HasTurn;
            }

            dst.Player2 = p2;

            Player winner = null;

            if (src.TheWinner != null)
            {
                winner = new Player();
                if (src.TheWinner.PlayerName.Equals(p1.PlayerName)) winner = p1;
                else if (src.TheWinner.PlayerName.Equals(p2.PlayerName)) winner = p2;
                else throw new Exception("CloneTestData: jotain mataa winnerin maarittamisessa.");
            }

            dst.TheWinner = winner;

            dst.Player1Table = null;

            if (src.Player1Table != null)
            {
                dst.Player1Table = new ObservableCollection<Dummy>();
                for (int i = 0; i < src.Player1Table.Count; i++)
                {
                    dst.Player1Table.Add(new Dummy());
                }
            }

            dst.Player2Table = null;

            if (src.Player2Table != null)
            {
                dst.Player2Table = new ObservableCollection<Dummy>();
                for (int i = 0; i < src.Player2Table.Count; i++)
                {
                    dst.Player2Table.Add(new Dummy());
                }
            }

            dst.TestBlockData = null;

            if (src.TestBlockData != null)
            {
                dst.TestBlockData = (int[])src.TestBlockData.Clone();
            }

            dst.XDimension = src.XDimension;
            dst.YDimension = src.YDimension;

            dst.GameState = src.GameState;

            dst.Blocks = null;

            if (src.Blocks != null)
            {
                dst.Blocks = new ObservableCollection<Block>();

                
                foreach (var item in src.Blocks)
                {
                    Block b = new Block();
                    Player bOwner = null;

                    if (item.BlockOwner != null)
                    {
                        if (item.BlockOwner.PlayerName.Equals(p1.PlayerName)) bOwner = p1;
                        else if (item.BlockOwner.PlayerName.Equals(p2.PlayerName)) bOwner = p2;
                        else throw new Exception("CloneTestData: jotain mataa BlockOwnerin maarittamisessa.");
                    }

                    b.BlockOwner = bOwner;
                    b.DownWay = item.DownWay;
                    b.LeftWay = item.LeftWay;
                    b.UpWay = item.UpWay;
                    b.RightWay = item.RightWay;
                    b.HasObject = item.HasObject;
                    b.HasPiece = item.HasPiece;
                    b.IsContentSelectable = item.IsContentSelectable;
                    b.IsContentSelected = item.IsContentSelected;
                    b.IsSelectable = item.IsSelectable;

                    dst.Blocks.Add(b);
                }
            }
            return dst;
 
        }
        


    }
}