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
        
        /// <summary>
        /// Will this gesture wins the other gesture?
        /// </summary>
        /// <param name="other">boolean. returns true if this gesture wins the other; otherwise, returns false.</param>
        /// <returns></returns>
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
            return (System.Object.ReferenceEquals(left, right) || left.Equals(right));
        }

        public static bool operator !=(Gesture left, Gesture right)
        {
            return (!System.Object.ReferenceEquals(left, right) || !left.Equals(right));
        }
        
        #endregion

        /// <summary>
        /// Tells if two gestures are the same
        /// </summary>
        /// <param name="obj">other gesture.</param>
        /// <returns>boolean. returns true if the gesture is the same as the other; otherwise, returns false.</returns>
        public override bool Equals(object obj)
        {
            return ((obj != null) && (this.GetType() == obj.GetType()));
        }

        public override int GetHashCode()
        {
            return this.GetType().GetHashCode();
        }

        public int CompareTo(object obj)
        {
            Gesture other = obj as Gesture;

            if (other == null)
            {
                return -1;
            }
            else if (this.Equals(other))
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
