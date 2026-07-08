using System;
using System.Linq;

namespace Emp37.ET
{
	  [Serializable]
	  public struct StyleOffset : IEquatable<StyleOffset>
	  {
			public enum Unit : byte
			{
				  None,
				  Pixel,
				  Percent
			}

			public int Top, Right, Bottom, Left;
			public Unit UnitType;

			public readonly bool Equals(StyleOffset other) => Top == other.Top && Right == other.Right && Bottom == other.Bottom && Left == other.Left && UnitType == other.UnitType;
			public override readonly bool Equals(object obj) => obj is StyleOffset other && Equals(other);
			public override readonly int GetHashCode() => HashCode.Combine(Top, Right, Bottom, Left, UnitType);
			public override readonly string ToString()
			{
				  string unit = UnitType switch { Unit.Pixel => "px", Unit.Percent => "%", _ => string.Empty, };
				  return string.Join(' ', from value in new[] { Top, Right, Bottom, Left } select $"{value}{(value == 0 ? string.Empty : unit)}");
			}

			public static bool operator ==(StyleOffset left, StyleOffset right) => left.Equals(right);
			public static bool operator !=(StyleOffset left, StyleOffset right) => !left.Equals(right);
	  }
}