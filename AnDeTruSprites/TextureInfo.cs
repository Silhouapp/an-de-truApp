using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;


namespace AnDeTruSprites
{
    public class TextureInfo
    {
        public int Rows { get; set; }
        public int Cols { get; set; }

        public Texture2D Texture { get; set; }

        public bool withReversed { get; set; }
        public int FPD { get; set; }
    }
}
