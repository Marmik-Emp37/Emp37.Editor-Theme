using UnityEngine;

namespace Emp37.ET
{
      public static class ETHelpers
      {
            public const float Spacing = 2F, IndentWidth = 15F;

            public static Rect Indent(this Rect rect, float value) => new(rect) { x = rect.x + value, width = rect.width - value };
            public static Rect Inset(this Rect rect, float value) => new(rect) { x = rect.x + value, width = rect.width - 2F * value };
      }
}