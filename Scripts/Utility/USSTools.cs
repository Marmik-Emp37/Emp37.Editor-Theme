using UnityEditor;

using UnityEngine;

namespace Emp37.ET
{
      public static class USSTools
      {
            public static string Format(Texture2D texture) => texture == null ? "none" : $"resource(\"{AssetDatabase.GetAssetPath(texture)}\")";
            public static string Format(Color32 color) => $"rgba({color.r:000}, {color.g:000}, {color.b:000}, {color.a / 255F:0.00})";
            public static string Format(RectOffset offset, string unit) => $"{Format(offset.top, unit)} {Format(offset.right, unit)} {Format(offset.bottom, unit)} {Format(offset.left, unit)}";
            public static string Format(int value, string unit) => value is 0 ? "0" : value + unit;
      }
}