using UnityEditor;
using UnityEngine;

namespace Emp37.ET
{
        using static ETHelpers;
        using static EditorGUIUtility;

        [CustomPropertyDrawer(typeof(StyleOffset))]
        internal class StyleOffsetDrawer : PropertyDrawer
        {
                private static readonly string[] p_OffsetFields = { nameof(StyleOffset.Left), nameof(StyleOffset.Bottom), nameof(StyleOffset.Right), nameof(StyleOffset.Top) };
                private const string p_UnitType = nameof(StyleOffset.UnitType);

                private const float DropdownWidth = 20F;


                public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
                {
                        label = EditorGUI.BeginProperty(position, label, property);

                        Rect rect = new(position)
                        {
                                size = new(labelWidth, singleLineHeight),
                        };
                        EditorGUI.LabelField(rect, label);

                        int indent = EditorGUI.indentLevel;
                        EditorGUI.indentLevel = 0;

                        rect = Indent(position, rect.width + HorizontalSpacing);
                        DrawFields(rect, property);

                        EditorGUI.indentLevel = indent;

                        EditorGUI.EndProperty();
                }

                private void DrawFields(Rect position, SerializedProperty property)
                {
                        const float labelRatio = 0.25F;

                        Rect dropdownRect = position;
                        dropdownRect.xMin = dropdownRect.xMax - DropdownWidth;
                        EditorGUI.PropertyField(dropdownRect, property.FindPropertyRelative(p_UnitType), GUIContent.none);

                        int count = p_OffsetFields.Length;
                        float elementWidth = (position.width - dropdownRect.width - HorizontalSpacing) / count;
                        float labelWidth = elementWidth * labelRatio, valueWidth = elementWidth - labelWidth;

                        for (int i = count - 1; i >= 0; i--)
                        {
                                SerializedProperty offset = property.FindPropertyRelative(p_OffsetFields[i]);

                                position.width = labelWidth;
                                EditorGUI.LabelField(position, $" {offset.displayName[0]} ---");
                                position.x += position.width + HorizontalSpacing;

                                position.width = valueWidth - HorizontalSpacing;
                                offset.intValue = Mathf.Max(0, EditorGUI.IntField(position, offset.intValue));
                                position.x += position.width;
                        }
                }
        }
}