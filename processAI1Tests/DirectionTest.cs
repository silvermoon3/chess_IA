using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using processAI1.Board.Bitboard;

namespace processAI1Tests
{
    [TestClass]
    public class DirectionTest
    {
        [TestMethod]
        public void testInverse()
        {
            Assert.AreEqual(Direction.N,Direction.S.InverseOf());
            Assert.AreEqual(Direction.S, Direction.N.InverseOf());
            Assert.AreEqual(Direction.NE, Direction.SW.InverseOf());
            Assert.AreEqual(Direction.E, Direction.W.InverseOf());
            Assert.AreEqual(Direction.NW, Direction.SE.InverseOf());
        }
    }
}
