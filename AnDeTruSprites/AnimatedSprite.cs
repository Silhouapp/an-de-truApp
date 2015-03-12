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
        public Rectangle sourceRect { get; set; }
        public Rectangle destRect { get; set; }
        private int Rows { get; set; }
        private int Columns { get; set; }
        private int currentDraw;
        private int totalFrames;
        private int fpd;
        private double scale;
        private bool withReversed;
        private bool fixedScale;
        private bool runOnce;

        public event EventHandler<EventArgs> FinishAnimationHandler;

        public AnimatedSprite(Texture2D texture, int rows, int columns, int framePerDraw, bool withReversed, bool runOnce = false, double scale = 0)
        {
            Texture = texture;
            Rows = rows;
            Columns = columns;
            currentDraw = 1;
            totalFrames = Rows * Columns;
            fpd = framePerDraw;
            fixedScale = scale != 0;
            this.scale = scale;
            this.withReversed = withReversed;
            this.runOnce = runOnce;
        }

        public void Update()
        {

            if (runOnce && currentDraw == 0)
            {
                var handler = FinishAnimationHandler;
                if (handler != null)
                {
                    handler(this, new EventArgs());
                }
            }

            currentDraw++;

            if (!fixedScale)
            {
                scale = Math.Min(0.2, scale + 0.0005);
            }

            int currentFrame;

            if (withReversed)
            {
                currentFrame = calcCurrentFrameWithReversed();
            }
            else
            {
                currentFrame = calcCurrentFrame();
            }

            int width = Texture.Width / Columns;
            int height = Texture.Height / Rows;
            int row = (int)((float)currentFrame / (float)Columns);
            int column = currentFrame % Columns;

            sourceRect = new Rectangle(width * column, height * row, width, height);
            destRect = new Rectangle(0, 0, (int)(width * scale), (int)(height * scale));
            
        }

        private int calcCurrentFrameWithReversed()
        {
            int currentFrame = currentDraw / fpd;

            if (currentFrame > totalFrames - 1)
            {
                currentFrame = (2 * (totalFrames - 1)) - currentFrame;
            }

            if (currentDraw == ((totalFrames * 2) - 2) * fpd)
            {
                currentDraw = 0;
            }

            return currentFrame;
        }

        private int calcCurrentFrame()
        {
            int currentFrame = currentDraw / fpd;

            if (currentDraw == totalFrames * fpd)
            {
                currentDraw = 0;
            }

            return currentFrame;
        }

    }
}