namespace Emp37.ET
{
      [System.Serializable]
      public struct StyleOffset
      {
            public enum Unit
            {
                  Pixels,
                  Percentage,
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
      }
}