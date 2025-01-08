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
            public string[] ClassSelectors;
            public PseudoClasses[] PseudoClasses;
            public USSProperties PropertyMask;

            public Texture2D BackgroundTexture;
            public Color32 BackgroundColor, BorderColor, BorderTopColor, BorderRightColor, BorderBottomColor, BorderLeftColor, TextColor;
            public RectOffset BorderRadius, BorderWidth;

            public static (USSProperties Property, string Name)[] PropertyMap =
            {
                  (USSProperties.BackgroundImage, nameof(BackgroundTexture)),
                  (USSProperties.BackgroundColor, nameof(BackgroundColor)),
                  (USSProperties.BorderColor, nameof(BorderColor)),
                  (USSProperties.BorderTopColor, nameof(BorderTopColor)),
                  (USSProperties.BorderRightColor, nameof(BorderRightColor)),
                  (USSProperties.BorderBottomColor, nameof(BorderBottomColor)),
                  (USSProperties.BorderLeftColor, nameof(BorderLeftColor)),
                  (USSProperties.Color, nameof(TextColor)),
                  (USSProperties.BorderRadius, nameof(BorderRadius)),
                  (USSProperties.BorderWidth, nameof(BorderWidth))
            };

            public readonly override string ToString()
            {
                  IEnumerable<string> selectors = ConstructSelectors();
                  if (!selectors.Any())
                  {
                        return null;
                  }
                  IEnumerable<string> properties = ConstructPropertyBlock();
                  if (!properties.Any())
                  {
                        return null;
                  }
                  return $"{string.Join(",\n", selectors)} {{\n{string.Join('\n', properties)}\n}}";
            }

            private readonly IEnumerable<string> ConstructSelectors()
            {
                  foreach (string classType in ClassSelectors)
                  {
                        if (string.IsNullOrWhiteSpace(classType)) continue;
                        foreach (PseudoClasses pseudoClass in PseudoClasses)
                        {
                              string chain = pseudoClass == 0 ? string.Empty : ':' + pseudoClass.ToString().Replace(", ", ":").ToLower();
                              yield return $".{classType}{chain}";
                        }
                  }
            }
            private readonly IEnumerable<string> ConstructPropertyBlock()
            {
                  foreach ((USSProperties property, _) in PropertyMap)
                  {
                        if (!PropertyMask.HasFlag(property)) continue;
                        string expression = property switch
                        {
                              USSProperties.BackgroundImage => USSTools.Format(BackgroundTexture),
                              USSProperties.BackgroundColor => USSTools.Format(BackgroundColor),
                              USSProperties.BorderColor => USSTools.Format(BorderColor),
                              USSProperties.BorderTopColor => USSTools.Format(BorderTopColor),
                              USSProperties.BorderRightColor => USSTools.Format(BorderRightColor),
                              USSProperties.BorderBottomColor => USSTools.Format(BorderBottomColor),
                              USSProperties.BorderLeftColor => USSTools.Format(BorderLeftColor),
                              USSProperties.BorderRadius => USSTools.Format(BorderRadius),
                              USSProperties.BorderWidth => USSTools.Format(BorderWidth),
                              USSProperties.Color => USSTools.Format(TextColor),
                              _ => null
                        };
                        if (string.IsNullOrEmpty(expression)) continue;
                        string name = Regex.Replace(property.ToString(), "(?<!^)([A-Z])", "-$1").ToLower();
                        yield return $"\t{name}: {expression};";
                  }
            }
      }
}