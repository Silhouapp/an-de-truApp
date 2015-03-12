using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace AnDeTruSprites
{
    public class GameBoard
    {
        private List<GestureView> gestures;
        int score = 0;
        private Timer timer;

        public GameBoard()
        {
            this.gestures = new List<GestureView>(9);
            for (int i = 0; i < 9; i++)
            {
                this.gestures.Add(null);
            }

            this.timer = new Timer
            {
                Interval = 2000,
                Enabled = true
            };
            timer.Elapsed += timer_Elapsed;
        }

        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            timer.Interval = new Random().Next(1000, 5000);
            this.addGestureView();
        }

        public void Update()
        {
            foreach (GestureView gestureView in this.CurrentGestureViews)
            {
                gestureView.Sprite.Update();
            }
        }

        public void addGestureView()
        {
            int gesturePlace = this.getRandomGesturePlace();
            if (gesturePlace == -1) return;
            Point position = new Point { OneDimensional = gesturePlace };
            Gesture randomGesture = Gestures.randomGesture();
            addGestureViewIn(randomGesture, position);
        }

        public void addGestureViewIn(Gesture gesture, Point position)
        {
            SpriteViewDetail svd = Gestures.DetailsFor(gesture);
            gestures[position.OneDimensional] = new GestureView {
                Gesture = gesture,
                Point = position,
                Sprite = new AnimatedSprite(svd.Texture, svd.Rows, svd.Cols, 5, true)
            };
        }

        public bool throwGesture(Gesture gesture, Point position)
        {
            bool result = false;
            GestureView gestureView = this.gestures[position.OneDimensional];
            if (gestureView != null)
            {
                Gesture gestureStored = this.gestures[position.OneDimensional].Gesture;

                if (gestureStored == null || gesture < gestureStored)
                {
                    result = false;
                    this.Score--;
                }
                else
                {
                    result = true;
                    this.Score++;
                    this.gestures[position.OneDimensional] = null;
                }
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
                lock (this)
                {
                    // Fucking immutablity
                    List<GestureView> val = new List<GestureView>(gestures.Count);
                    return gestures.Where(x => x != null).ToList<GestureView>();
                }
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
