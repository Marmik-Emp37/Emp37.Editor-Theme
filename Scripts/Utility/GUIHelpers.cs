using System.Linq;

using UnityEditor;

using UnityEngine;

namespace Emp37.ET
{
      public static class GUIHelpers
      {
            public const float Spacing = 2F;
            public static readonly Color BackgroundTint = EditorGUIUtility.isProSkin ? Color.black : Color.white;

            public static Color ChangeOpacity(this Color color, float value) => new(color.r, color.g, color.b, value);
            public static Rect Inset(this Rect rect, float value) => new(rect.x + value, rect.y, rect.width - 2F * value, rect.height);
            public static float ArrayField(Rect position, SerializedProperty property)
            {
                  EditorGUI.LabelField(position, property.displayName, GUI.skin.box); // header
                  position.y += position.height + EditorGUIUtility.standardVerticalSpacing;

                  Rect contentRect = position;
                  for (int size = property.arraySize, i = 0; i < size; i++) // elements
                  {
                        SerializedProperty context = property.GetArrayElementAtIndex(i);
                        contentRect.height = EditorGUI.GetPropertyHeight(context);
                        _ = EditorGUI.PropertyField(contentRect, context, GUIContent.none, false);
                        position.y = contentRect.y += contentRect.height + EditorGUIUtility.standardVerticalSpacing;
                  }

                  // options
                  contentRect.width *= 0.5F;
                  contentRect.height = EditorStyles.miniButton.fixedHeight;
                  if (GUI.Button(contentRect, EditorGUIUtility.IconContent("d_Toolbar Plus"), EditorStyles.miniButtonLeft) || property.arraySize is 0) property.InsertArrayElementAtIndex(property.arraySize);
                  contentRect.x += contentRect.width;
                  if (GUI.Button(contentRect, EditorGUIUtility.IconContent("d_Toolbar Minus"), EditorStyles.miniButtonRight) && property.arraySize > 1) property.DeleteArrayElementAtIndex(property.arraySize - 1);
                  position.y += contentRect.height + EditorGUIUtility.standardVerticalSpacing;

                  EditorGUI.DrawRect(new(position) { height = Spacing }, BackgroundTint.ChangeOpacity(0.25F)); // separator line
                  return position.y += Spacing + EditorGUIUtility.standardVerticalSpacing;
            }
            public static float GetArrayHeight(SerializedProperty property, float initialHeight)
            {
                  return initialHeight + EditorGUIUtility.standardVerticalSpacing // header
                  + Enumerable.Range(0, property.arraySize).Sum(index => EditorGUI.GetPropertyHeight(property.GetArrayElementAtIndex(index)) + EditorGUIUtility.standardVerticalSpacing)
                  + EditorStyles.miniButton.fixedHeight + EditorGUIUtility.standardVerticalSpacing // option buttons
                  + Spacing + EditorGUIUtility.standardVerticalSpacing; // additional line
            }
      }
}