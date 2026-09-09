using System;
using System.Linq;

using UnityEditor;
using UnityEngine;

namespace Emp37.ET
{
	  public abstract class Theme : ScriptableObject
	  {
			public enum Type { Light, Dark }


			public StyleRuleGroup[] StyleRuleGroups;
			public Color SelectionColor;

			[Tooltip("Disable to preview changes immediately as they are applied.\n\n<b>Note:</b> Some changes may require a domain reload to take full effect.")]
			public bool RecompileOnApply;

			public abstract Type ThemeType { get; }


			[ContextMenu("Sort/Title")]
			private void Sort()
			{
				  Undo.RecordObject(this, $"Sort {nameof(StyleRuleGroups)} by Title");
				  Array.Sort(StyleRuleGroups, (a, b) => a.Title.CompareTo(b.Title));
			}

			[ContextMenu("Log Test Messages")]
			private void GenerateSampleLogs()
			{
				  Debug.Log("This is a normal log.");
				  Debug.LogWarning("This is a warning log.");
				  Debug.LogError("This is an error log.");
			}

			/// <summary>
			/// Generates a finalised USS code representation for this theme.
			/// </summary>
			public sealed override string ToString()
			{
				  return $"/*----------\n[ Theme: {ThemeType}-{name} ]\n----------*/\n\n{string.Join("\n\n", from @group in StyleRuleGroups let value = @group.ToString() where !string.IsNullOrEmpty(value) select value)}";
			}
	  }
}