using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using UnityEngine;

namespace Emp37.ET
{
      [Serializable]
      public struct StyleRule
      {
            public string[] Selectors;
            public PseudoStates[] PseudoStates;
            public Properties PropertyMask;

            public Texture2D BackgroundTexture;
            public Color32 BackgroundColor, BorderColor, BorderTopColor, BorderRightColor, BorderBottomColor, BorderLeftColor, TextColor;
            public RectOffset BorderRadius, BorderWidth;

            public static Dictionary<Properties, string> propertiesMap = new()
            {
                  { Properties.BackgroundImage, nameof(BackgroundTexture) + "XD" },
                  { Properties.BackgroundColor, nameof(BackgroundColor) },
                  { Properties.BorderColor, nameof(BorderColor) },
                  { Properties.BorderTopColor, nameof(BorderTopColor) },
                  { Properties.BorderRightColor, nameof(BorderRightColor) },
                  { Properties.BorderBottomColor, nameof(BorderBottomColor) },
                  { Properties.BorderLeftColor, nameof(BorderLeftColor) },
                  { Properties.Color, nameof(TextColor) },
                  { Properties.BorderRadius, nameof(BorderRadius) },
                  { Properties.BorderWidth, nameof(BorderWidth) }
            };


            public override readonly string ToString()
            {
                  StyleRule rule = this;

                  IEnumerable<string> classes =
                        from selector in rule.Selectors
                        where !string.IsNullOrWhiteSpace(selector)
                        from stateMask in rule.PseudoStates
                        let pseudoChain = stateMask is 0 ? null : $":{string.Join(':', stateMask.ToString().Split(',').Select(value => value.Trim().ToLower()))}"
                        select $".{selector}{pseudoChain}";

                  if (!classes.Any()) return null;

                  IEnumerable<string> properties =
                        from property in propertiesMap.Keys
                        where rule.PropertyMask.HasFlag(property)
                        let propertyName = Regex.Replace(property.ToString(), "(?<!^)([A-Z])", "-$1").ToLower()
                        let expression = property switch
                        {
                              Properties.BackgroundImage => USSTools.Format(rule.BackgroundTexture),
                              Properties.BackgroundColor => USSTools.Format(rule.BackgroundColor),
                              Properties.BorderColor => USSTools.Format(rule.BorderColor),
                              Properties.BorderTopColor => USSTools.Format(rule.BorderTopColor),
                              Properties.BorderRightColor => USSTools.Format(rule.BorderRightColor),
                              Properties.BorderBottomColor => USSTools.Format(rule.BorderBottomColor),
                              Properties.BorderLeftColor => USSTools.Format(rule.BorderLeftColor),
                              Properties.BorderRadius => USSTools.Format(rule.BorderRadius),
                              Properties.BorderWidth => USSTools.Format(rule.BorderWidth),
                              Properties.Color => USSTools.Format(rule.TextColor),
                              _ => null,
                        }
                        where expression != null
                        select $"\t{propertyName}: {expression};";

                  if (!properties.Any()) return null;

                  return $"{string.Join(",\n", classes)} {{\n{string.Join('\n', properties)}\n}}";
            }
      }
}