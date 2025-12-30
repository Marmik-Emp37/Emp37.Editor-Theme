using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Emp37.ET
{
      [Serializable]
      public struct StyleRule
      {
            public string[] Selectors;
            public PseudoClasses[] PseudoClasses;
            public StyleAttributes PropertyMask;

            public Texture2D BackgroundTexture;
            public Color32 BackgroundColor, BorderColor, BorderTopColor, BorderRightColor, BorderBottomColor, BorderLeftColor, TextColor;
            public StyleOffset BorderRadius, BorderWidth;

            public static readonly (StyleAttributes Flag, string Name)[] AttributeMap =
            {
                  (StyleAttributes.BackgroundImage, nameof(BackgroundTexture)),
                  (StyleAttributes.BackgroundColor, nameof(BackgroundColor)),
                  (StyleAttributes.BorderColor, nameof(BorderColor)),
                  (StyleAttributes.BorderTopColor, nameof(BorderTopColor)),
                  (StyleAttributes.BorderRightColor, nameof(BorderRightColor)),
                  (StyleAttributes.BorderBottomColor, nameof(BorderBottomColor)),
                  (StyleAttributes.BorderLeftColor, nameof(BorderLeftColor)),
                  (StyleAttributes.BorderRadius, nameof(BorderRadius)),
                  (StyleAttributes.BorderWidth, nameof(BorderWidth)),
                  (StyleAttributes.Color, nameof(TextColor))
            };

            public readonly override string ToString()
            {
                  #region S E L E C T O R S
                  List<string> classes = new();
                  foreach (var selector in Selectors)
                  {
                        if (string.IsNullOrWhiteSpace(selector)) continue;
                        foreach (var value in PseudoClasses)
                        {
                              string chain = value == 0 ? string.Empty : ":" + value.ToString().Replace(", ", ":").ToLowerInvariant();
                              classes.Add($".{selector}{chain}");
                        }
                  }
                  if (classes.Count == 0) return null;
                  #endregion

                  #region P R O P E R T I E S
                  List<string> properties = new();
                  foreach ((var property, _) in AttributeMap)
                  {
                        if (!PropertyMask.HasFlag(property)) continue;
                        string expression = property switch
                        {
                              StyleAttributes.BackgroundImage => USSWriter.Format(BackgroundTexture),
                              StyleAttributes.BackgroundColor => USSWriter.Format(BackgroundColor),
                              StyleAttributes.BorderColor => USSWriter.Format(BorderColor),
                              StyleAttributes.BorderTopColor => USSWriter.Format(BorderTopColor),
                              StyleAttributes.BorderRightColor => USSWriter.Format(BorderRightColor),
                              StyleAttributes.BorderBottomColor => USSWriter.Format(BorderBottomColor),
                              StyleAttributes.BorderLeftColor => USSWriter.Format(BorderLeftColor),
                              StyleAttributes.BorderRadius => USSWriter.Format(BorderRadius),
                              StyleAttributes.BorderWidth => USSWriter.Format(BorderWidth),
                              StyleAttributes.Color => USSWriter.Format(TextColor),
                              _ => null
                        };
                        if (string.IsNullOrEmpty(expression)) continue;
                        string name = Regex.Replace(property.ToString(), "(?<!^)([A-Z])", "-$1").ToLowerInvariant();
                        properties.Add($"\t{name}: {expression};");
                  }
                  if (properties.Count == 0) return null;
                  #endregion

                  return $"{string.Join(",\n", classes)} {{\n{string.Join('\n', properties)}\n}}";
            }
      }
}