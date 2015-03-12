using Microsoft.Xna.Framework.Content;
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
        private static Dictionary<Gesture, SpriteViewDetail> gestures = new Dictionary<Gesture, SpriteViewDetail>();

        public static Gesture randomGesture()
        {
            return AvailableGestures[new Random().Next(0, AvailableGestures.Count)];
        }

        public static Dictionary<Gesture, SpriteViewDetail> loadGestures(params KeyValuePair<Gesture, SpriteViewDetail>[] gestureTextureKeyValuePairs)
        {
            foreach (KeyValuePair<Gesture, SpriteViewDetail> gestureTextureKeyValuePair in gestureTextureKeyValuePairs)
            {
                Gesture gesture = gestureTextureKeyValuePair.Key;
                SpriteViewDetail svd = gestureTextureKeyValuePair.Value;
                if (gesture == null || svd == null)
                {
                    throw new Exception("Make sure the format is Gesture, Texture pair. without fucking nulls.");
                }
                gestures.Add(gesture, svd);
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

        public static void InitGestures(ContentManager cm)
        {
            Gestures.loadGestures(
                new KeyValuePair<Gesture, SpriteViewDetail> (
                    new Paper(), 
                    new SpriteViewDetail {
                        LiveTexture = cm.Load<Texture2D>("Paper2048"),
                        DeadTexture = cm.Load<Texture2D>("Explosion"),
                        Rows = 2,
                        Cols = 2
                    }
                ), new KeyValuePair<Gesture, SpriteViewDetail>(
                    new Rock(),
                    new SpriteViewDetail
                    {
                        LiveTexture = cm.Load<Texture2D>("Rock2048"),
                        DeadTexture = cm.Load<Texture2D>("Explosion"),
                        Rows = 2,
                        Cols = 2
                    }
                ), new KeyValuePair<Gesture, SpriteViewDetail>(
                    new Scissors(),
                    new SpriteViewDetail
                    {
                        LiveTexture = cm.Load<Texture2D>("Scissors2048"),
                        DeadTexture = cm.Load<Texture2D>("Explosion"),
                        Rows = 2,
                        Cols = 2
                    }
                )
            );
        }

        internal static SpriteViewDetail DetailsFor(Gesture gesture)
        {
            return gestures[gesture];
        }
    }
}
