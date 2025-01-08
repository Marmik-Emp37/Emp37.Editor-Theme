using System.Collections.Generic;

namespace Emp37.ET
{
      [System.Serializable]
      public struct StyleRuleGroup
      {
            public string Title;

            public StyleRule[] StyleRules;

            [field: UnityEngine.SerializeField] public bool Enabled { get; set; }

            private readonly IEnumerable<string> WriteRules
            {
                  get
                  {
                        foreach (StyleRule style in StyleRules)
                        {
                              string value = style.ToString();
                              if (string.IsNullOrEmpty(value)) continue;
                              yield return value;
                        }
                  }
            }


            public readonly override string ToString() => Enabled ? string.Join('\n', WriteRules) : null;
      }
}