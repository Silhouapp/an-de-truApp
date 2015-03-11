using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
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
            image.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);

            // Creates a texture from IO.Stream - our memory stream  
            Texture2D texture = Texture2D.FromStream(
                GraphicsDevice, memoryStream);

            //memoryStream.Dispose();

            return texture;
        }

        public static Texture2D BitmapToTexture2D2(GraphicsDevice GraphicsDevice,
                                                   Bitmap image)
        {
            Texture2D tx = null;
            using (MemoryStream s = new MemoryStream())
            {
                image.Save(s, System.Drawing.Imaging.ImageFormat.Png);
                s.Seek(0, SeekOrigin.Begin); //must do this, or error is thrown in next line
                tx = Texture2D.FromStream(GraphicsDevice, s);
            }

            return tx;
        }
    }
}
