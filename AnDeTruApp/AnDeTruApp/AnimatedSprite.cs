using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace AnDeTruApp
{
    public class AnimatedSprite
    {
        public Texture2D Texture { get; set; }
        public int Rows { get; set; }
        public int Columns { get; set; }
        private int currentFrame;
        private int currentDraw;
        private int totalFrames;
        private int fpd;

        public AnimatedSprite(Texture2D texture, int rows, int columns, int framePerDraw)
        {
            Texture = texture;
            Rows = rows;
            Columns = columns;
            currentFrame = 0;
            currentDraw = 0;
            totalFrames = Rows * Columns;
            fpd = framePerDraw;
        }

        public void Update()
        {
            currentDraw++;

            currentFrame = currentDraw / fpd;

            if (currentFrame > totalFrames - 1)
            {
                currentFrame = (2 * (totalFrames - 1)) - currentFrame;
            }
            
            if (currentDraw == ((totalFrames * 2) - 2 ) * fpd)
            {
                currentDraw = 0;
            }

            

        }

        public void Draw(SpriteBatch spriteBatch, Vector2 location)
        {
            int width = Texture.Width / Columns;
            int height = Texture.Height / Rows;
            int row = (int)((float)currentFrame / (float)Columns);
            int column = currentFrame % Columns;

            Rectangle sourceRectangle = new Rectangle(width * column, height * row, width, height);
            Rectangle destinationRectangle = new Rectangle((int)location.X, (int)location.Y, width, height);

            spriteBatch.Draw(Texture, destinationRectangle, sourceRectangle, Color.White);
        }
    }
}