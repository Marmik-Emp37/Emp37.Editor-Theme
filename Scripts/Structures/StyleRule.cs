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

            private readonly IEnumerable<string> WriteSelectorList
            {
                  get
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
            }
            private readonly IEnumerable<string> WriteStyleBlock
            {
                  get
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
                                    USSProperties.BorderRadius => USSTools.Format(BorderRadius, "px"),
                                    USSProperties.BorderWidth => USSTools.Format(BorderWidth, "px"),
                                    USSProperties.Color => USSTools.Format(TextColor),
                                    _ => null
                              };
                              if (string.IsNullOrEmpty(expression)) continue;
                              string name = Regex.Replace(property.ToString(), "(?<!^)([A-Z])", "-$1").ToLower();
                              yield return $"\t{name}: {expression};";
                        }
                  }
            }


            public readonly override string ToString()
            {
                  IEnumerable<string> selectorList = WriteSelectorList;
                  if (!selectorList.Any())
                  {
                        return null;
                  }
                  IEnumerable<string> propertiesReference = WriteStyleBlock;
                  if (!propertiesReference.Any())
                  {
                        return null;
                  }
                  return $"{string.Join(",\n", selectorList)} {{\n{string.Join('\n', propertiesReference)}\n}}";
            }
      }
}