using UnityEditor;

using UnityEngine;

namespace Emp37.ET
{
      public static class USSTools
      {
            public static string Format(Texture2D texture) => texture == null ? "none" : $"resource(\"{AssetDatabase.GetAssetPath(texture)}\")";
            public static string Format(Color32 color) => $"rgba({color.r:000}, {color.g:000}, {color.b:000}, {color.a / 255F:0.00})";
            public static string Format(StyleOffset offset) => $"{Format(offset.Top, offset.UnitType)} {Format(offset.Right, offset.UnitType)} {Format(offset.Bottom, offset.UnitType)} {Format(offset.Left, offset.UnitType)}";
            public static string Format(int value, StyleOffset.Unit unit) => value is 0 ? "0" : value + unit switch { StyleOffset.Unit.Pixels => "px", _ => "%"};
      }
}