namespace Emp37.ET
{
      [System.Flags]
      public enum Properties : int
      {
            BackgroundImage = 1,
            BackgroundColor = 2,
            BorderColor = 4,
            BorderTopColor = 8,
            BorderRightColor = 16,
            BorderBottomColor = 32,
            BorderLeftColor = 64,
            BorderRadius = 128,
            BorderWidth = 256,
            Color = 512
      }
}