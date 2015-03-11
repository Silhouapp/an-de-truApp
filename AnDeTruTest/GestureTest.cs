using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AnDeTruSprites;

namespace AnDeTruTest
{
    [TestClass]
    public class GestureTest
    {
        [TestMethod]
        public void CompareTest()
        {
            Gesture paper = new Paper();
            Gesture scissors = new Scissors();
            Gesture rock = new Rock();

            Assert.IsTrue(rock > scissors);
            Assert.IsTrue(scissors > paper);
            Assert.IsTrue(paper > rock);

            Assert.IsTrue(scissors < rock);
            Assert.IsTrue(paper < scissors);
            Assert.IsTrue(rock < paper);

            Assert.IsFalse(rock < scissors);
            Assert.IsFalse(scissors < paper);
            Assert.IsFalse(paper < rock);

            Assert.IsFalse(scissors > rock);
            Assert.IsFalse(paper > scissors);
            Assert.IsFalse(rock > paper);
        }
        
        [TestMethod]
        public void EqualsTest() {
            Gesture paper = new Paper();
            Gesture scissors = new Scissors();
            Gesture rock = new Rock();
            Gesture paper2 = new Paper();
            Gesture scissors2 = new Scissors();
            Gesture rock2 = new Rock();

            Assert.IsTrue(rock == rock2);
            Assert.IsTrue(paper == paper2);
            Assert.IsTrue(scissors == scissors2);

            Assert.IsFalse(paper == rock2);
            Assert.IsFalse(rock == paper2);
            Assert.IsFalse(scissors == paper);
        }
    }
}
