using UnityEditor;

using UnityEngine;

namespace Emp37.ET
{
      public static class StyleHelpers
      {
            public static readonly Color BackgroundTint = EditorGUIUtility.isProSkin ? Color.black : Color.white;

            public static Color ChangeOpacity(this Color color, float value) => new(color.r, color.g, color.b, value);
            public static Rect Inset(this Rect rect, float value) => new(rect.x + value, rect.y, rect.width - 2F * value, rect.height);
      }
}