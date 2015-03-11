using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnDeTruSprites
{
    public class Scissors : Gesture
    {
        public override Type WillWinGesture()
        {
            return new Paper().GetType();
        }
    }
}
