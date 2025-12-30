using System;
using System.Linq;
using UnityEngine;

namespace Emp37.ET
{
      [Serializable]
      public struct StyleRuleGroup
      {
            public string Title;
            public StyleRule[] StyleRules;
            [field: SerializeField] public bool Enabled { get; set; }

            public readonly override string ToString() => Enabled ? $"/* {Title} */\n{string.Join('\n', from style in StyleRules let value = style.ToString() where !string.IsNullOrEmpty(value) select value)}" : null;
      }
}