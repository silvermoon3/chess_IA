using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using processAI1.Board;

namespace processAI1Tests
{
    [TestClass]
    public class SquareTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            for(int i = 0; i < Square.SIZE;i++)
                Assert.AreEqual(i, Square.From88(Square.To88(i)));
        }
    }
}
