using UnityEditor;

using UnityEngine;

namespace Emp37.ET
{
      public static class ETHelpers
      {
            public const float Spacing = 2F;

            public static readonly Color ThemeTint = EditorGUIUtility.isProSkin ? Color.black : Color.white;
            public static readonly Color ThemeAccent = new(ThemeTint.r, ThemeTint.g, ThemeTint.b, 0.2F);

            public static Rect Inset(this Rect rect, float value) => new(rect) { x = rect.x + value, width = rect.width - 2F * value };
      }
}