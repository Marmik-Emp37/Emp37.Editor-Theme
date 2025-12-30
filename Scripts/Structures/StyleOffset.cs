using System;

namespace Emp37.ET
{
      [Serializable]
      public struct StyleOffset
      {
            public enum Unit
            {
                  None,
                  Pixel,
                  Percent,
            }

            public int Top, Right, Bottom, Left;
            public Unit UnitType;

            public override readonly bool Equals(object obj) => obj is StyleOffset offset && Top == offset.Top && Right == offset.Right && Bottom == offset.Bottom && Left == offset.Left && UnitType == offset.UnitType;
            public override readonly int GetHashCode() => HashCode.Combine(Top, Right, Bottom, Left, UnitType);
      }
}