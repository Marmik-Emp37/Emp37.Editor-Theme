using System;

namespace Emp37.ET
{
      [Flags]
      public enum PseudoClasses : int
      {
            Active = 1 << 0, Hover = 1 << 1, Focus = 1 << 2, Selected = 1 << 3, Checked = 1 << 4, Enabled = 1 << 5, Disabled = 1 << 6
      }

      [Flags]
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