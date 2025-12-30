using System;
using System.Linq;
using System.Collections.Generic;

using UnityEditor;
using UnityEngine;

namespace Emp37.ET
{
      public abstract class Theme : ScriptableObject
      {
            public enum Type { Light, Dark }

            public abstract Type ThemeType { get; }

            public StyleRuleGroup[] StyleRuleGroups;
            public Color SelectionColor;

            [Tooltip("Disable to preview changes immediately as they are applied.\n\n<b>Note:</b> Some changes may require a domain reload to take full effect.")]
            public bool RecompileOnApply;

            [ContextMenu("Sort/Title")]
            private void Sort()
            {
                  Undo.RecordObject(this, $"Sort {nameof(StyleRuleGroups)} by Title");
                  Array.Sort(StyleRuleGroups, (a, b) => a.Title.CompareTo(b.Title));
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