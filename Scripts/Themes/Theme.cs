using System.Linq;

using UnityEngine;

namespace Emp37.ET
{
      public abstract class Theme : ScriptableObject
      {
            public enum Type { Light, Dark }

            public StyleRuleGroup[] StyleRuleGroups;
            [field: Tooltip("Enable to preview changes immediately as they are applied.\n\n<b>Note:</b> Some changes may require a domain reload to take full effect.")]
            [field: SerializeField] public bool InstantApply { get; private set; }

            public abstract Type ThemeType { get; }


            /// <summary>
            /// Generates a finalised USS code representation for this theme.
            /// </summary>
            public sealed override string ToString() => $"/*-----\n[ Theme: {ThemeType}-{name} ]\n-----*/\n\n" +
                  string.Join("\n\n", from ruleGroup in StyleRuleGroups let value = ruleGroup.ToString() where !string.IsNullOrEmpty(value) select $"/*-[{ruleGroup.Title}]-*/\n{value}");
      }
}