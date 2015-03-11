using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AnDeTruSprites;

namespace AnDeTruTest
{
    [TestClass]
    public class GameBoardTest
    {
        [TestMethod]
        public void AddGestureTest()
        {
            GameBoard gameboard = new GameBoard();
            Assert.AreEqual(9, nullsInArray(gameboard.CurrentGestures));
            gameboard.addGesture();
            Assert.AreEqual(8, nullsInArray(gameboard.CurrentGestures));
            gameboard.addGesture();
            Assert.AreEqual(7, nullsInArray(gameboard.CurrentGestures));
            gameboard.addGesture();
            Assert.AreEqual(6, nullsInArray(gameboard.CurrentGestures));
            gameboard.addGesture();
            Assert.AreEqual(5, nullsInArray(gameboard.CurrentGestures));
            gameboard.addGesture();
            Assert.AreEqual(4, nullsInArray(gameboard.CurrentGestures));
            gameboard.addGesture();
            Assert.AreEqual(3, nullsInArray(gameboard.CurrentGestures));
            gameboard.addGesture();
            Assert.AreEqual(2, nullsInArray(gameboard.CurrentGestures));
            gameboard.addGesture();
            Assert.AreEqual(1, nullsInArray(gameboard.CurrentGestures));
            gameboard.addGesture();
            Assert.AreEqual(0, nullsInArray(gameboard.CurrentGestures));
            gameboard.addGesture();
            Assert.AreEqual(0, nullsInArray(gameboard.CurrentGestures));
        }

        [TestMethod]
        public void ThrowGestureTest()
        {
            GameBoard gameboard = new GameBoard();
            Point where = new Point { X = 1, Y = 1 };
            gameboard.addGestureIn(new Rock(), where);
            Assert.IsFalse(gameboard.throwGesture(new Scissors(), where));
            Assert.IsTrue(gameboard.throwGesture(new Rock(), where));
            Assert.AreEqual(1, gameboard.Score);
            Assert.IsFalse(gameboard.throwGesture(new Scissors(), where));
            Assert.AreEqual(0, gameboard.Score);

        }

        private int nullsInArray(Object[] objects)
        {
            int i = 0;
            foreach (var obj in objects)
            {
                if (obj == null) i++;
            }
            return i;
        }
    }
}
