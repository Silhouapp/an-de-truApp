using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnDeTruSprites
{
    public static class Gestures
    {
        private static Gesture[] availableGestures;
        private static Dictionary<Gesture, Texture2D> gestures = new Dictionary<Gesture, Texture2D>();

        public static Gesture randomGesture()
        {
            return AvailableGestures[new Random().Next(0, AvailableGestures.Count - 1)];
        }

        public static Dictionary<Gesture, Texture2D> loadGestures(params KeyValuePair<Gesture, Texture2D>[] gestureTextureKeyValuePairs)
        {
            foreach (KeyValuePair<Gesture, Texture2D> gestureTextureKeyValuePair in gestureTextureKeyValuePairs)
            {
                Gesture gesture = gestureTextureKeyValuePair.Key;
                Texture2D texture = gestureTextureKeyValuePair.Value;
                if (gesture == null || texture == null)
                {
                    throw new Exception("Make sure the format is Gesture, Texture pair. without fucking nulls.");
                }
                gestures.Add(gesture, texture);
            }
            return gestures;
        }

        private static List<Gesture> AvailableGestures
        {
            get
            {
                List<Gesture> g = new List<Gesture>();
                g.Add(new Rock());
                g.Add(new Scissors());
                g.Add(new Paper());
                return g;
            }
        }
    }
}
