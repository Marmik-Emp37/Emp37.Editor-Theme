using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace Emp37.ET
{
      [Serializable]
      internal struct StyleRule
      {
            private static readonly string[] properties =
            {
                  nameof(background_image),
                  nameof(background_color),
                  nameof(border_color),
                  nameof(border_top_color),
                  nameof(border_right_color),
                  nameof(border_bottom_color),
                  nameof(border_left_color),
                  nameof(border_radius),
                  nameof(border_width),
                  nameof(color)
            };
            public static readonly Dictionary<int, string> PropertyMap = properties.Select((name, index) => (Key: 1 << index, Value: name)).ToDictionary(entry => entry.Key, entry => entry.Value);

            public string[] ClassTypes;
            public PseudoStates[] PseudoStates;
            public int PropertyBitmask;


            public Texture2D
                  background_image;
            public Color32
                  background_color,
                  border_color,
                  border_top_color,
                  border_right_color,
                  border_bottom_color,
                  border_left_color,
                  color;
            public RectOffset
                  border_radius,
                  border_width;
      }
}