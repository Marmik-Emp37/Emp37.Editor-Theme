using System.Linq;

namespace Emp37.ET
{
      [System.Serializable]
      public struct StyleRuleGroup
      {
            public string Title;

            public StyleRule[] StyleRules;

            [field: UnityEngine.SerializeField] public bool Enabled { get; set; }

            public readonly override string ToString() => Enabled ? string.Join('\n', from rule in StyleRules let value = rule.ToString() where !string.IsNullOrEmpty(value) select value) : null;
      }
}