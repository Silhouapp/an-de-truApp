using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnDeTruSprites
{
    public static class Gestures
    {
        private static Gesture[] availableGestures;
        
        public static Gesture randomGesture()
        {
            return AvailableGestures[new Random().Next(0, AvailableGestures.Length - 1)];
        }

        private static Gesture[] AvailableGestures
        {
            get
            {
                if (availableGestures == null)
                {
                    LinkedList<Gesture> available = new LinkedList<Gesture>();
                    available.AddLast(new Rock());
                    available.AddLast(new Scissors());
                    available.AddLast(new Paper());
                    availableGestures = available.ToArray<Gesture>();
                }

                return availableGestures;
            }
        }
    }
}
