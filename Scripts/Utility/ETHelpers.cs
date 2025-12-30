using UnityEngine;

namespace Emp37.ET
{
      public static class ETHelpers
      {
            public const float Spacing = 2F, IndentWidth = 15F;

            public static Rect Indent(Rect rect, float value) => new(x: rect.x + value, y: rect.y, width: rect.width - value, height: rect.height);
            public static Rect Inset(Rect rect, float value) => new(x: rect.x + value, y: rect.y, width: rect.width - 2F * value, height: rect.height);


            public class BackgroundColorScope : GUI.Scope
            {
                  private readonly Color original;

                  public Color BackgroundColor { get => GUI.backgroundColor; set => GUI.backgroundColor = value; }

                  public BackgroundColorScope() => original = GUI.backgroundColor;
                  public BackgroundColorScope(Color color) : this() => GUI.backgroundColor = color;

                  protected override void CloseScope() => GUI.backgroundColor = original;
            }
      }
}