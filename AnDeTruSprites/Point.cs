using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnDeTruSprites
{
    public class Point
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int OneDimensional
        {
            get
            {
                return (3 * this.Y) + this.X;
            }
            set
            {
                int x = value % 3;
                int y = (value - x) / 3;
                this.X = x;
                this.Y = y;
            }
        }

        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }

        public override string ToString()
        {
            return String.Format("{0}@{1}", this.X, this.Y);
        }
    }
}
