using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnDeTruSprites
{
    public class Paper : Gesture
    {
        public override Type WillWinGesture()
        {
            return new Rock().GetType();
        }
    }
}
