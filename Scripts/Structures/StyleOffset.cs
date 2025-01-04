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


            public StyleOffset(int top, int right, int bottom, int left, Unit unit)
            {
                  Top = top;
                  Right = right;
                  Bottom = bottom;
                  Left = left;
                  UnitType = unit;
            }

            public readonly override bool Equals(object obj)
            {
                  return obj is StyleOffset offset && Top == offset.Top && Right == offset.Right && Bottom == offset.Bottom && Left == offset.Left && UnitType == offset.UnitType;
            }
            public readonly override int GetHashCode()
            {
                  return HashCode.Combine(Top, Right, Bottom, Left, UnitType);
            }
      }
}