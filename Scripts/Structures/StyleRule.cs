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

			public Texture2D BackgroundImage;
			public Color32 BackgroundColor, BorderColor, BorderTopColor, BorderRightColor, BorderBottomColor, BorderLeftColor, Color;
			public StyleOffset BorderRadius, BorderWidth;

			public readonly override string ToString()
			{
				  List<string> classes = new();
				  foreach (string selector in Selectors)
				  {
						if (string.IsNullOrWhiteSpace(selector)) continue;
						foreach (PseudoClasses value in PseudoClasses)
						{
							  string chain = value == 0 ? string.Empty : ":" + value.ToString().Replace(", ", ":").ToLowerInvariant();
							  classes.Add($".{selector}{chain}");
						}
				  }
				  if (classes.Count == 0) return null;

				  List<string> properties = new();
				  foreach (StyleAttributes attribute in Enum.GetValues(typeof(StyleAttributes)))
				  {
						if (!PropertyMask.HasFlag(attribute)) continue;
						string expression = attribute switch
						{
							  StyleAttributes.BackgroundImage => USSWriter.Format(BackgroundImage),
							  StyleAttributes.BackgroundColor => USSWriter.Format(BackgroundColor),
							  StyleAttributes.BorderColor => USSWriter.Format(BorderColor),
							  StyleAttributes.BorderTopColor => USSWriter.Format(BorderTopColor),
							  StyleAttributes.BorderRightColor => USSWriter.Format(BorderRightColor),
							  StyleAttributes.BorderBottomColor => USSWriter.Format(BorderBottomColor),
							  StyleAttributes.BorderLeftColor => USSWriter.Format(BorderLeftColor),
							  StyleAttributes.BorderRadius => USSWriter.Format(BorderRadius),
							  StyleAttributes.BorderWidth => USSWriter.Format(BorderWidth),
							  StyleAttributes.Color => USSWriter.Format(Color),
							  _ => null
						};
						if (string.IsNullOrEmpty(expression)) continue;

						string name = Regex.Replace(attribute.ToString(), "(?<!^)([A-Z])", "-$1").ToLowerInvariant();
						properties.Add($"\t{name}: {expression};");
				  }
				  if (properties.Count == 0) return null;

				  return $"{string.Join(",\n", classes)} {{\n{string.Join("\n", properties)}\n}}";
			}
	  }
}