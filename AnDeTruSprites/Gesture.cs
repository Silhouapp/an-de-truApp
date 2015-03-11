using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnDeTruSprites
{
    public abstract class Gesture : IComparable
    {
        /// <summary>
        /// Returns the gesture type that gonna lose to the current gesture.
        /// i.e in the Paper class, it will return the Rock class.
        /// </summary>
        /// <returns>Type</returns>
        abstract public Type WillWinGesture();

        public bool willWin(Gesture other)
        {
            return this.WillWinGesture().Equals(other.GetType());
        }

        #region Operators

        public static bool operator <(Gesture left, Gesture right)
        {
            return (left.CompareTo(right) < 0);
        }

        public static bool operator >(Gesture left, Gesture right)
        {
            return (left.CompareTo(right) > 0);
        }

        public static bool operator ==(Gesture left, Gesture right)
        {
            return (left.Equals(right));
        }

        public static bool operator !=(Gesture left, Gesture right)
        {
            return (!left.Equals(right));
        }
        
        #endregion

        // override object.Equals
        public override bool Equals(object obj)
        {
            if (obj == null || !this.GetType().IsInstanceOfType(obj))
            {
                return false;
            }

            return this.GetType() == obj.GetType();
        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public int CompareTo(object obj)
        {
            Gesture other = obj as Gesture;

            if (other == null)
            {
                return -1;
            }
            else if (this.GetType().Equals(other.GetType()))
            {
                return 0;
            }
            else if (this.willWin(other))
            {
                return 1;
            }
            else
            {
                return -1;
            }
        }
    }
}
