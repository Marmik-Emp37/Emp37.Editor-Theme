using UnityEditor;
using static UnityEditor.EditorGUIUtility;
using UnityEngine;

namespace Emp37.ET
{
      using static ETHelpers;

      [CustomPropertyDrawer(typeof(StyleOffset))]
      internal class StyleOffsetDrawer : PropertyDrawer
      {
            private static readonly string[] p_OffsetFields = { nameof(StyleOffset.Left), nameof(StyleOffset.Bottom), nameof(StyleOffset.Right), nameof(StyleOffset.Top) };
            private const string p_UnitType = nameof(StyleOffset.UnitType);

            private const float DropdownWidth = 20F;


            public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
            {
                  label = EditorGUI.BeginProperty(position, label, property);

                  Rect labelPosition = new(position) { width = labelWidth };
                  EditorGUI.LabelField(labelPosition, label);

                  int indent = EditorGUI.indentLevel;
                  EditorGUI.indentLevel = 0;

                  position = Indent(position, labelPosition.width + Spacing);
                  DrawFields(position, property);

                  EditorGUI.indentLevel = indent;

                  EditorGUI.EndProperty();
            }

            private void DrawFields(Rect position, SerializedProperty property)
            {
                  int count = p_OffsetFields.Length;
                  position.width = (position.width - DropdownWidth - Spacing) / count;
                  for (int i = count - 1; i >= 0; i--)
                  {
                        DrawInteger(position, property.FindPropertyRelative(p_OffsetFields[i]));
                        position.x += position.width;
                  }
                  position.x += Spacing;
                  position.width = DropdownWidth;

                  EditorGUI.PropertyField(position, property.FindPropertyRelative(p_UnitType), GUIContent.none);
            }
            private void DrawInteger(Rect position, SerializedProperty property)
            {
                  float width = position.width;
                  position.width = width * 0.25F;
                  EditorGUI.LabelField(position, $" {property.displayName[0]} ---");
                  position.x += position.width + Spacing;
                  position.width = width - position.width - Spacing;

                  property.intValue = Mathf.Clamp(EditorGUI.IntField(position, property.intValue), 0, int.MaxValue);
            }
      }
}