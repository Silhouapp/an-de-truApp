using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Microsoft.Xna.Framework;
using System.IO;
using Microsoft.Xna.Framework.Graphics;

namespace AnDeTruApp
{
    public static class ConversionServices
    {
        public static Texture2D BitmapToTexture2D(GraphicsDevice GraphicsDevice,
                                                  Bitmap image)
        {
            // Buffer size is size of color array multiplied by 4 because   
            // each pixel has four color bytes  
            int bufferSize = image.Height * image.Width * 4;

            // Create new memory stream and save image to stream so   
            // we don't have to save and read file  
            System.IO.MemoryStream memoryStream =
                new System.IO.MemoryStream(bufferSize);
            image.RotateFlip(RotateFlipType.RotateNoneFlipX);
            image.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);

            // Creates a texture from IO.Stream - our memory stream  
            Texture2D texture = Texture2D.FromStream(
                GraphicsDevice, memoryStream);

            memoryStream.Dispose();

            return texture;
        }

        //public static Microsoft.Xna.Framework.Point FromIndexToLocation(int i, int j, int colWidth, int rowHeight)
        //{
        //    var x = i * colWidth;
        //    x += colWidth / 2;

        //    var y = j * rowHeight;
        //    y += rowHeight / 2;

        //    return new Microsoft.Xna.Framework.Point(x, y);
        //}

        public static AnDeTruSprites.Point FromLocationToIndex(int x, int y, int colWidth, int rowHeight)
        {
            int i = (848-x - 70) / colWidth;
            int j = y / rowHeight;

            return new AnDeTruSprites.Point(i, j);
        }
    }
}
