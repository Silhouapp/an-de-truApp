using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnDeTruSprites
{
    public class GameBoard
    {
        private List<GestureView> gestures;
        int score = 0;

        public GameBoard()
        {
            this.gestures = new List<GestureView>(9);
            for (int i = 0; i < 9; i++)
            {
                this.gestures.Add(null);
            }
        }

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
            gestures[position.OneDimensional] = new GestureView { Gesture = gesture };
        }

        public bool throwGesture(Gesture gesture, Point position)
        {
            bool result = false;
            GestureView gestureView = this.gestures[position.OneDimensional];
            Gesture gestureStored = this.gestures[position.OneDimensional].Gesture;
            if (gestureView == null)
            {
            }
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

        /// <summary>
        /// Its fucking immutable, bitch
        /// </summary>
        public List<GestureView> CurrentGestureViews
        {
            get
            {
                // Fucking immutablity
                List<GestureView> val = new List<GestureView>(gestures.Count);
                val.AddRange(gestures);
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
                List<int> arr = new List<int>(gestures.Count);
                for (int i = 0; i < gestures.Count; i++)
                {
                    if (gestures[i] == null) arr.Add(i);
                }
                return arr;
            }
        }
    }
}
