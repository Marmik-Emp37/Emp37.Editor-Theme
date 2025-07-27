using System.Collections.Generic;
using System.Linq;

namespace Emp37.ET
{
      [System.Serializable]
      public struct StyleRuleGroup
      {
            public string Title;

            public StyleRule[] StyleRules;

            [field: UnityEngine.SerializeField] public bool Enabled { get; set; }

            private readonly IEnumerable<string> WriteRules => from style in StyleRules let value = style.ToString() where !string.IsNullOrEmpty(value) select value;


            public readonly override string ToString() => Enabled ? $"/* {Title} */\n{string.Join('\n', WriteRules)}" : null;
      }
}