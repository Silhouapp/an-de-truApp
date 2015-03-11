using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnDeTruSprites
{
    public class GameBoard
    {
        private Gesture[] gestures = new Gesture[9];
        int score = 0;

        public void addGesture()
        {
            int gesturePlace = this.getRandomGesturePlace();
            if (gesturePlace == -1) return;
            Point position = new Point { OneDimensional = gesturePlace };
            Gesture randomGesture = Gestures.randomGesture();
            addGestureIn(randomGesture, position);
        }

        public void addGestureIn(Gesture gesture, Point position)
        {
            gestures[position.OneDimensional] = gesture;
        }

        public bool throwGesture(Gesture gesture, Point position)
        {
            bool result = false;
            Gesture gestureStored = this.gestures[position.OneDimensional];
            if (gestureStored == null || gesture < gestureStored)
            {
                result = false;
                this.Score--;
            }
            else
            {
                result = true;
                this.Score++;
            }

            return result;
        }

        public int Score
        {
            set
            {
                this.score = Math.Max(0, value);
            }
            get
            {
                return this.score;
            }
        }

        public Gesture[] CurrentGestures
        {
            get
            {
                Gesture[] val = new Gesture[gestures.Length];
                for (int i = 0; i < gestures.Length; i++)
                {
                    val[i] = gestures[i];
                }
                return val;
            }
        }

        private int getRandomGesturePlace()
        {
            IList<int> emptyCells = this.EmptyCellsIndexes;
            if (emptyCells.Count < 1) return -1;
            int where = new Random().Next(0, emptyCells.Count);
            return emptyCells[where];
        }

        private IList<int> EmptyCellsIndexes
        {
            get
            {
                List<int> arr = new List<int>(gestures.Length);
                for (int i = 0; i < gestures.Length; i++)
                {
                    if (gestures[i] == null) arr.Add(i);
                }
                return arr;
            }
        }
    }
}
