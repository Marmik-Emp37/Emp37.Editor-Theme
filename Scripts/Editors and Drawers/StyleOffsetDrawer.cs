using UnityEngine;

using UnityEditor;
using static UnityEditor.EditorGUIUtility;

namespace Emp37.ET
{
      using static ETHelpers;


      [CustomPropertyDrawer(typeof(StyleOffset))]
      internal class StyleOffsetDrawer : PropertyDrawer
      {
            private static readonly string[] p_OffsetFields = { "Left", "Bottom", "Right", "Top" };
            private const string p_UnitType = "UnitType";

            private const float UnitPopupWidth = 20F;


            private void DrawInteger(Rect position, SerializedProperty property)
            {
                  float width = position.width;

                  position.width = width * 0.25F;
                  EditorGUI.LabelField(position, $" {property.displayName[0]} ---");

                  position.x += position.width + Spacing;

                  position.width = width - position.width - Spacing;
                  property.intValue = EditorGUI.IntField(position, property.intValue);
            }
            private void DrawField(Rect position, SerializedProperty property)
            {
                  int count = p_OffsetFields.Length;

                  position.width = (position.width - UnitPopupWidth - Spacing) / count;
                  for (int i = count - 1; i >= 0; i--)
                  {
                        DrawInteger(position, property.FindPropertyRelative(p_OffsetFields[i]));
                        position.x += position.width;
                  }

                  position.x += Spacing;

                  position.width = UnitPopupWidth;
                  EditorGUI.PropertyField(position, property.FindPropertyRelative(p_UnitType), GUIContent.none);
            }

            public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
            {
                  label = EditorGUI.BeginProperty(position, label, property);

                  Rect labelRect = new(position) { width = labelWidth };
                  EditorGUI.LabelField(labelRect, label);

                  int indent = EditorGUI.indentLevel;
                  EditorGUI.indentLevel = 0;

                  DrawField(position.Indent(labelRect.width + Spacing), property);

                  EditorGUI.indentLevel = indent;

                  EditorGUI.EndProperty();
            }
      }
}