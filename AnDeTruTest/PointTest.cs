using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AnDeTruSprites;

namespace AnDeTruTest
{
    [TestClass]
    public class PointTest
    {
        [TestMethod]
        public void TestOneDimensional()
        {
            Assert.AreEqual(new Point { X = 0, Y = 0 }.ToString(), new Point { OneDimensional = 0 }.ToString());
            Assert.AreEqual(new Point { X = 1, Y = 0 }.ToString(), new Point { OneDimensional = 1 }.ToString());
            Assert.AreEqual(new Point { X = 2, Y = 0 }.ToString(), new Point { OneDimensional = 2 }.ToString());
            Assert.AreEqual(new Point { X = 0, Y = 1 }.ToString(), new Point { OneDimensional = 3 }.ToString());
            Assert.AreEqual(new Point { X = 1, Y = 1 }.ToString(), new Point { OneDimensional = 4 }.ToString());
            Assert.AreEqual(new Point { X = 2, Y = 1 }.ToString(), new Point { OneDimensional = 5 }.ToString());
            Assert.AreEqual(new Point { X = 0, Y = 2 }.ToString(), new Point { OneDimensional = 6 }.ToString());
            Assert.AreEqual(new Point { X = 1, Y = 2 }.ToString(), new Point { OneDimensional = 7 }.ToString());
            Assert.AreEqual(new Point { X = 2, Y = 2 }.ToString(), new Point { OneDimensional = 8 }.ToString());
        }
    }
}
