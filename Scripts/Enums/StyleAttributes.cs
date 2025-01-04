namespace Emp37.ET
{
      [System.Flags]
      public enum StyleAttributes : int
      {
            BackgroundImage = 1 << 0,
            BackgroundColor = 1 << 1,
            BorderColor = 1 << 2,
            BorderTopColor = 1 << 3,
            BorderRightColor = 1 << 4,
            BorderBottomColor = 1 << 5,
            BorderLeftColor = 1 << 6,
            BorderRadius = 1 << 7,
            BorderWidth = 1 << 8,
            Color = 1 << 9
      }
}