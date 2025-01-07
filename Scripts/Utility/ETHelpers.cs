using UnityEditor;

using UnityEngine;

namespace Emp37.ET
{
      public static class ETHelpers
      {
            public const float Spacing = 2F;

            public static readonly Color ThemeTint = EditorGUIUtility.isProSkin ? Color.black : Color.white;
            public static readonly Color ThemeAccent = new(ThemeTint.r, ThemeTint.g, ThemeTint.b, 0.2F);

            public static Rect Inset(this Rect rect, float value) => new(rect) { x = rect.x + value, width = rect.width - 2F * value };

            public static void ArrayField(Rect position, SerializedProperty property)
            {
                  position.height = 24F;
                  EditorGUI.DrawRect(position, ThemeAccent);
                  EditorGUI.LabelField(position, property.displayName, ETStyles.centeredLabel);
                  position.y += position.height + EditorGUIUtility.standardVerticalSpacing; // - [ h:0 ]

                  for (int i = 0; i < property.arraySize; i++)
                  {
                        SerializedProperty element = property.GetArrayElementAtIndex(i);
                        position.height = EditorGUI.GetPropertyHeight(element);
                        EditorGUI.PropertyField(position, element, GUIContent.none, false);
                        position.y += position.height + EditorGUIUtility.standardVerticalSpacing; // - [ h:1 ]
                  }

                  position = EditorGUI.IndentedRect(position);
                  position.height = EditorStyles.miniButton.fixedHeight;
                  position.width /= 2F; // splitting buttons
                  int size = property.arraySize;
                  if (GUI.Button(position, EditorGUIUtility.IconContent("d_Toolbar Plus"), EditorStyles.miniButtonLeft) || size is 0) property.InsertArrayElementAtIndex(size++);
                  position.x += position.width; // append to right
                  if (GUI.Button(position, EditorGUIUtility.IconContent("d_Toolbar Minus"), EditorStyles.miniButtonRight) && size > 0) property.DeleteArrayElementAtIndex(--size);
            }
            public static float CalculateArrayHeight(SerializedProperty property)
            {
                  float height = 24F + EditorGUIUtility.standardVerticalSpacing; // - [ h:0 ]
                  for (int i = 0; i < property.arraySize; i++) height += EditorGUI.GetPropertyHeight(property.GetArrayElementAtIndex(i)) + EditorGUIUtility.standardVerticalSpacing; // - [ h:1 ]
                  height += EditorStyles.miniButton.fixedHeight + EditorGUIUtility.standardVerticalSpacing; // options button height + extra spacing
                  return height;
            }
      }
}