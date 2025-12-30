using UnityEditor;
using static UnityEditor.EditorGUIUtility;
using UnityEngine;

namespace Emp37.ET
{
      public static class ETStyles
      {
            /// <summary>
            /// The primary tint color of the theme, depending on the current editor skin.
            /// For dark theme Black otherwise, White.
            /// </summary>
            public static readonly Color BaseTone = isProSkin ? Color.black : Color.white;
            /// <summary>
            /// A semi-transparent accent color derived from the theme tint.
            /// Can be used for highlighting elements or creating subtle backgrounds.
            /// </summary>
            public static readonly Color AccentTone = new(BaseTone.r, BaseTone.g, BaseTone.b, 0.25F);

            /// <summary>
            /// A label style with centered alignment.
            /// Useful for displaying text that needs to be horizontally and vertically centered.
            /// </summary>
            public static readonly GUIStyle centeredText = new(GUI.skin.label)
            {
                  alignment = TextAnchor.MiddleCenter
            };
            /// <summary>
            /// A GUI style for displaying small text aligned to the right.
            /// Ideal for supplementary information or compact labels in the Unity Editor.
            /// </summary>
            public static readonly GUIStyle miniLabelRight = new(EditorStyles.miniLabel)
            {
                  alignment = TextAnchor.MiddleRight
            };
            /// <summary>
            /// A label style with larger text size.
            /// Ideal for headers, titles or prominent text elements in the editor.
            /// </summary>
            public static readonly GUIStyle largeText = new(EditorStyles.label)
            {
                  fontSize = 14
            };
            public static readonly GUIStyle largeHelpBox = new(EditorStyles.helpBox)
            {
                  fontSize = largeText.fontSize
            };
            /// <summary>
            /// A text field style with left-aligned text and a larger font size.
            /// Suitable for input fields that need to match the font size of large labels.
            /// </summary>
            public static readonly GUIStyle largeTextField = new(EditorStyles.textField)
            {
                  alignment = TextAnchor.MiddleLeft,
                  fontSize = largeText.fontSize
            };

            public static readonly GUIContent
                  ToolbarPlus = IconContent(isProSkin ? "d_Toolbar Plus" : "Toolbar Plus"),
                  ToolbarMinus = IconContent(isProSkin ? "d_Toolbar Minus" : "Toolbar Minus"),
                  Menu = IconContent("_Menu"),
                  Clear = IconContent("d_clear"),
                  INFoldout = IconContent("d_IN_foldout"),
                  CustomSorting = IconContent("CustomSorting");
      }
}