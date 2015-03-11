using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace AnDeTruSprites
{
    public class AnimatedSprite
    {
        public Texture2D Texture { get; set; }
        public Rectangle sourceRect{ get; set; }
        public Rectangle destRect { get; set; }
        private int Rows { get; set; }
        private int Columns { get; set; }
        private int currentFrame;
        private int currentDraw;
        private int totalFrames;
        private int fpd;
        private double scale;

        public AnimatedSprite(Texture2D texture, int rows, int columns, int framePerDraw)
        {
            Texture = texture;
            Rows = rows;
            Columns = columns;
            currentFrame = 0;
            currentDraw = 0;
            totalFrames = Rows * Columns;
            fpd = framePerDraw;
            scale = 0;
        }

        public void Update()
        {
            currentDraw++;
            scale = Math.Min(0.2, scale + 0.0005);

            currentFrame = currentDraw / fpd;

            if (currentFrame > totalFrames - 1)
            {
                currentFrame = (2 * (totalFrames - 1)) - currentFrame;
            }

            if (currentDraw == ((totalFrames * 2) - 2) * fpd)
            {
                currentDraw = 0;
            }

            int width = Texture.Width / Columns;
            int height = Texture.Height / Rows;
            int row = (int)((float)currentFrame / (float)Columns);
            int column = currentFrame % Columns;

            sourceRect = new Rectangle(width * column, height * row, width, height);
            destRect = new Rectangle(0, 0, (int)(width * scale), (int)(height * scale));
        }
    }
}