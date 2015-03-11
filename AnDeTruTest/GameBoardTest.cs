﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AnDeTruSprites;
using System.Collections.Generic;

namespace AnDeTruTest
{
    [TestClass]
    public class GameBoardTest
    {
        [TestMethod]
        public void AddGestureTest()
        {
            GameBoard gameboard = new GameBoard();
            Assert.AreEqual(9, nullsInArray(gameboard.CurrentGestureViews));
            gameboard.addGesture();
            Assert.AreEqual(8, nullsInArray(gameboard.CurrentGestureViews));
            gameboard.addGesture();
            Assert.AreEqual(7, nullsInArray(gameboard.CurrentGestureViews));
            gameboard.addGesture();
            Assert.AreEqual(6, nullsInArray(gameboard.CurrentGestureViews));
            gameboard.addGesture();
            Assert.AreEqual(5, nullsInArray(gameboard.CurrentGestureViews));
            gameboard.addGesture();
            Assert.AreEqual(4, nullsInArray(gameboard.CurrentGestureViews));
            gameboard.addGesture();
            Assert.AreEqual(3, nullsInArray(gameboard.CurrentGestureViews));
            gameboard.addGesture();
            Assert.AreEqual(2, nullsInArray(gameboard.CurrentGestureViews));
            gameboard.addGesture();
            Assert.AreEqual(1, nullsInArray(gameboard.CurrentGestureViews));
            gameboard.addGesture();
            Assert.AreEqual(0, nullsInArray(gameboard.CurrentGestureViews));
            gameboard.addGesture();
            Assert.AreEqual(0, nullsInArray(gameboard.CurrentGestureViews));
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

        private int nullsInArray(List<GestureView> objects)
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
