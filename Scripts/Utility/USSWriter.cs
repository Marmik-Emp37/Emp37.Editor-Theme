using UnityEditor;
using UnityEngine;

namespace Emp37.ET
{
      public static class USSWriter
      {
            public static string Format(Texture2D texture) => texture ? $"resource(\"{AssetDatabase.GetAssetPath(texture)}\")" : "none";
            public static string Format(Color32 color) => color.a == byte.MaxValue ? $"rgb({color.r:D3}, {color.g:D3}, {color.b:D3})" : $"rgba({color.r:D3}, {color.g:D3}, {color.b:D3}, {color.a / 255f:0.##})";
            public static string Format(StyleOffset offset) => offset.ToString();
      }
}