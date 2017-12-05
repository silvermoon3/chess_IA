using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using processAI1.Board;
using processAI1.Board.Bitboard;

namespace processAI1Tests
{
    [TestClass]
    public class BitTest
    {
        [TestMethod]
        public void IsSetTest()
        {
            Bit.Init();
            UInt64 b =  1UL;
            Assert.IsTrue(Bit.IsSet(b,0));
            
            b = Bit.MakeBit(square.A3);
            Assert.IsTrue(Bit.IsSet(b, square.A3));
            b = 0x4242424242424242UL;
            Assert.AreEqual(16, Bit.Count(b));
            b = 1UL;
            Assert.AreEqual(0, Bit.First(b));
            b = 3UL;
            Assert.AreEqual(0, Bit.First(b));
            
        }
    }
}
