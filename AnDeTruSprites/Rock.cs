using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnDeTruSprites
{
    public class Rock : Gesture
    {
        public override Type WillWinGesture()
        {
            return new Scissors().GetType();
        }
    }
}
