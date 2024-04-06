﻿using System.Linq;

namespace Emp37.ET
{
      [System.Serializable]
      public struct StyleRuleGroup
      {
            public bool Enabled;
            public string Title;

            public StyleRule[] StyleRules;


            public readonly override string ToString() => Enabled ? string.Join('\n', from rule in StyleRules let value = rule.ToString() where !string.IsNullOrEmpty(value) select value) : null;
      }
}