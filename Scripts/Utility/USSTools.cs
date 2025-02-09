﻿using System.Linq;

using UnityEditor;

using UnityEngine;

namespace Emp37.ET
{
      public static class USSTools
      {
            public static string Format(Texture2D texture)
            {
                  return texture ? $"resource(\"{AssetDatabase.GetAssetPath(texture)}\")" : "none";
            }
            public static string Format(Color32 color)
            {
                  return color.a == byte.MaxValue ? $"rgb({color.r:D3}, {color.g:D3}, {color.b:D3})" : $"rgba({color.r:D3}, {color.g:D3}, {color.b:D3}, {color.a / 255f:0.##})";
            }
            public static string Format(StyleOffset offset)
            {
                  return string.Join(' ', from value in new[] { offset.Top, offset.Right, offset.Bottom, offset.Left } select $"{value}{(value == 0 ? string.Empty : GetUnitSuffix(offset.UnitType))}");

                  static string GetUnitSuffix(StyleOffset.Unit type) => type switch
                  {
                        StyleOffset.Unit.Pixels => "px",
                        StyleOffset.Unit.Percentage => "%",
                        _ => string.Empty
                  };
            }
      }
}