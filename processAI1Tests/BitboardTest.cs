using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using processAI1.Board.Bitboard;

namespace processAI1Tests
{
    /// <summary>
    /// Description résumée pour BitboardTest
    /// </summary>
    [TestClass]
    public class BitboardTest
    {
        public BitboardTest()
        {
            //
            // TODO: ajoutez ici la logique du constructeur
            //
        }

        private TestContext _testContextInstance;

        /// <summary>
        ///Obtient ou définit le contexte de test qui fournit
        ///des informations sur la série de tests active, ainsi que ses fonctionnalités.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return _testContextInstance;
            }
            set
            {
                _testContextInstance = value;
            }
        }

        #region Attributs de tests supplémentaires
        //
        // Vous pouvez utiliser les attributs supplémentaires suivants lorsque vous écrivez vos tests :
        //
        // Utilisez ClassInitialize pour exécuter du code avant d'exécuter le premier test de la classe
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Utilisez ClassCleanup pour exécuter du code une fois que tous les tests d'une classe ont été exécutés
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Utilisez TestInitialize pour exécuter du code avant d'exécuter chaque test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Utilisez TestCleanup pour exécuter du code après que chaque test a été exécuté
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void TestValidBitBoardStartPos()
        {
            BitBoard b = new BitBoard();
            b.InitStartingBoard();
            //WHITE
            Assert.AreEqual(0xFF00UL, b.WhitePawns);
            Assert.AreEqual(0x81UL, b.WhiteRooks);
            Assert.AreEqual(0x24UL, b.WhiteBishops);
            Assert.AreEqual(0x8UL, b.WhiteQueens);
            Assert.AreEqual(0x10UL, b.WhiteKing);
            Assert.AreEqual(0x42UL, b.WhiteKnights);

            //black
            Assert.AreEqual(0xFF000000000000UL, b.BlackPawns);
            Assert.AreEqual(0x8100000000000000UL, b.BlackRooks);
            Assert.AreEqual(0x2400000000000000UL, b.BlackBishops);
            Assert.AreEqual(0x800000000000000UL, b.BlackQueens);
            Assert.AreEqual(0x1000000000000000UL, b.BlackKing);
            Assert.AreEqual(0x4200000000000000UL, b.BlackKnights);

        }
    }
}
