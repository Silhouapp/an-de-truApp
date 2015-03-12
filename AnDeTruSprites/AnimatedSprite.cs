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
        public Rectangle sourceRect { get; set; }
        public Rectangle destRect { get; set; }
        private int currentDraw;
        private int totalFrames;
        private double scale;
        private bool fixedScale;
        private bool runOnce;

        public TextureInfo Info { get; set; }

        public event EventHandler<EventArgs> FinishAnimationHandler;

        public AnimatedSprite(TextureInfo ti, double scale = 0, bool runOnce = false)
        {
            this.Info = ti;
            currentDraw = 1;
            totalFrames = Info.Rows * Info.Cols;
            fixedScale = scale != 0;
            this.scale = scale;
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

            if (Info.withReversed)
            {
                currentFrame = calcCurrentFrameWithReversed();
            }
            else
            {
                currentFrame = calcCurrentFrame();
            }

            int width = Info.Texture.Width / Info.Cols;
            int height = Info.Texture.Height / Info.Rows;
            int row = (int)((float)currentFrame / (float)Info.Cols);
            int column = currentFrame % Info.Cols;

            sourceRect = new Rectangle(width * column, height * row, width, height);
            destRect = new Rectangle(0, 0, (int)(width * scale), (int)(height * scale));
            
        }

        private int calcCurrentFrameWithReversed()
        {
            int currentFrame = currentDraw / Info.FPD;

            if (currentFrame > totalFrames - 1)
            {
                currentFrame = (2 * (totalFrames - 1)) - currentFrame;
            }

            if (currentDraw == ((totalFrames * 2) - 2) * Info.FPD)
            {
                currentDraw = 0;
            }

            return currentFrame;
        }

        private int calcCurrentFrame()
        {
            int currentFrame = currentDraw / Info.FPD;

            if (currentDraw == totalFrames * Info.FPD)
            {
                currentDraw = 0;
            }

            return currentFrame;
        }

    }
}