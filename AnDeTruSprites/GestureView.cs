using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnDeTruSprites
{
    public class GestureView
    {
        public Gesture Gesture { get; set; }
        public Point Point { get; set; }
        public bool IsDying { get; set; }
        public AnimatedSprite Sprite { get; set; }
    }
}
