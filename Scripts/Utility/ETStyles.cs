using UnityEditor;

using UnityEngine;

namespace Emp37.ET
{
      public static class ETStyles
      {
            /// <summary>
            /// A label style with centered alignment.
            /// Useful for displaying text that needs to be horizontally and vertically centered.
            /// </summary>
            public static readonly GUIStyle centeredLabel = new(GUI.skin.label) { alignment = TextAnchor.MiddleCenter };
            /// <summary>
            /// A label style with larger text size.
            /// Ideal for headers, titles or prominent text elements in the editor.
            /// </summary>
            public static readonly GUIStyle largeText = new(EditorStyles.label) { fontSize = 14 };
            /// <summary>
            /// A text field style with left-aligned text and a larger font size.
            /// Suitable for input fields that need to match the font size of large labels.
            /// </summary>
            public static readonly GUIStyle largeTextField = new(EditorStyles.textField) { alignment = TextAnchor.MiddleLeft, fontSize = largeText.fontSize };
      }
}