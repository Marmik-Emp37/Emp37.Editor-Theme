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
            public StyleAttributes PropertyMask;

            public Texture2D BackgroundTexture;
            public Color32 BackgroundColor, BorderColor, BorderTopColor, BorderRightColor, BorderBottomColor, BorderLeftColor, TextColor;
            public StyleOffset BorderRadius, BorderWidth;

            public static (StyleAttributes Flag, string Name)[] PropertyMap =
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

            private readonly IEnumerable<string> WriteSelectorList
            {
                  get
                  {
                        foreach (string classType in ClassSelectors.Where(item => !string.IsNullOrWhiteSpace(item)))
                        {
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
                        foreach ((StyleAttributes property, _) in PropertyMap)
                        {
                              if (!PropertyMask.HasFlag(property)) continue;

                              string expression = property switch
                              {
                                    StyleAttributes.BackgroundImage => USSTools.Format(BackgroundTexture),
                                    StyleAttributes.BackgroundColor => USSTools.Format(BackgroundColor),
                                    StyleAttributes.BorderColor => USSTools.Format(BorderColor),
                                    StyleAttributes.BorderTopColor => USSTools.Format(BorderTopColor),
                                    StyleAttributes.BorderRightColor => USSTools.Format(BorderRightColor),
                                    StyleAttributes.BorderBottomColor => USSTools.Format(BorderBottomColor),
                                    StyleAttributes.BorderLeftColor => USSTools.Format(BorderLeftColor),
                                    StyleAttributes.BorderRadius => USSTools.Format(BorderRadius),
                                    StyleAttributes.BorderWidth => USSTools.Format(BorderWidth),
                                    StyleAttributes.Color => USSTools.Format(TextColor),
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