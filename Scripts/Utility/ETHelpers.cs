using UnityEngine;

namespace Emp37.ET
{
      public static class ETHelpers
      {
            public const float Spacing = 2F, IndentWidth = 15F;

            public static Rect Indent(Rect rect, Vector2 value) => new(x: rect.x + value.x, y: rect.y + value.y, width: rect.width - value.x, height: rect.height - value.y);
            public static Rect Inset(Rect rect, Vector2 value) => new(x: rect.x + value.x, y: rect.y + value.y, width: rect.width - 2F * value.x, height: rect.height - 2F * value.y);


            public class BackgroundColorScope : GUI.Scope
            {
                  private readonly Color original = GUI.backgroundColor;
                  public Color BackgroundColor { get => GUI.backgroundColor; set => GUI.backgroundColor = value; }

                  public BackgroundColorScope()
                  {
                  }
                  public BackgroundColorScope(Color color)
                  {
                        GUI.backgroundColor = color;
                  }
                  protected override void CloseScope()
                  {
                        GUI.backgroundColor = original;
                  }
            }
      }
}