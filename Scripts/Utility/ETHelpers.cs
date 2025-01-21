using UnityEngine;

namespace Emp37.ET
{
      public static class ETHelpers
      {
            public const float Spacing = 2F, IndentWidth = 15F;


            public static Rect Indent(Rect rect, float value) => new(rect) { x = rect.x + value, width = rect.width - value };
            public static Rect Inset(Rect rect, float value) => new(rect) { x = rect.x + value, width = rect.width - 2F * value };

            public static string Truncate(string text, int length) => text.Length > length ? $"{text[..length]}..." : text;


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