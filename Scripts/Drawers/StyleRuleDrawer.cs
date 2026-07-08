using System;
using System.Collections.Generic;

using UnityEditor;
using UnityEditorInternal;

using UnityEngine;

namespace Emp37.ET
{
        using static EditorGUIUtility;

        [CustomPropertyDrawer(typeof(StyleRule))]
        internal class StyleRuleDrawer : PropertyDrawer
        {
                private const string p_Selectors = nameof(StyleRule.Selectors);
                private const string p_PseudoClasses = nameof(StyleRule.PseudoClasses);
                private const string p_Mask = nameof(StyleRule.PropertyMask);

                private const float BaseHeight = 21F, EmptyStyleLineHeight = 2F;

                private static readonly StyleAttributes[] StyleAttributeEnumValues = (StyleAttributes[]) Enum.GetValues(typeof(StyleAttributes));

                private readonly Dictionary<string, ReorderableList> lists = new();

                private static readonly GUIStyle
                        propertyExpandToggle = new(EditorStyles.foldoutHeader)
                        {
                                fontStyle = FontStyle.Normal,
                                fixedHeight = BaseHeight
                        },
                        propertyMaskField = new(EditorStyles.layerMaskField)
                        {
                                alignment = TextAnchor.MiddleCenter,
                                fontStyle = FontStyle.Italic,
                                fixedHeight = 30F
                        };


                public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
                {
                        using EditorGUI.PropertyScope propertyScope = new(position, label, property);

                        position.height = propertyExpandToggle.fixedHeight;
                        property.isExpanded = GUI.Toggle(position, property.isExpanded, label, propertyExpandToggle);

                        if (!property.isExpanded) return;

                        position.y += position.height + standardVerticalSpacing;

                        SerializedProperty selectors = property.FindPropertyRelative(p_Selectors), pseudoClasses = property.FindPropertyRelative(p_PseudoClasses), mask = property.FindPropertyRelative(p_Mask);

                        DrawReorderableList(ref position, selectors);
                        position.y += standardVerticalSpacing;

                        DrawReorderableList(ref position, pseudoClasses);
                        position.y += standardVerticalSpacing;

                        DrawHeader(ref position, mask.displayName);

                        using (EditorGUI.ChangeCheckScope check = new())
                        {
                                position.height = propertyMaskField.fixedHeight;
                                Enum value = EditorGUI.EnumFlagsField(position, (StyleAttributes) mask.enumValueFlag, propertyMaskField);
                                if (check.changed) mask.enumValueFlag = (int) (StyleAttributes) value;

                                position.y += position.height + standardVerticalSpacing;
                        }
                        using (new EditorGUI.IndentLevelScope(1))
                        {
                                StyleAttributes flags = (StyleAttributes) mask.enumValueFlag;
                                if (flags == 0)
                                {
                                        position.height = EmptyStyleLineHeight;
                                        EditorGUI.DrawRect(ETHelpers.Inset(position, 40F), Color.red);

                                        position.y += position.height;
                                }
                                else foreach (StyleAttributes flag in StyleAttributeEnumValues)
                                {
                                        if ((flags & flag) != flag) continue;

                                        SerializedProperty attribute = property.FindPropertyRelative(flag.ToString());

                                        position.height = EditorGUI.GetPropertyHeight(attribute, true);
                                        EditorGUI.PropertyField(position, attribute, true);

                                        position.y += position.height + standardVerticalSpacing;
                                }
                        }
                }
                public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
                {
                        float height = propertyExpandToggle.fixedHeight;

                        if (!property.isExpanded) return height;

                        SerializedProperty selectors = property.FindPropertyRelative(p_Selectors), pseudoClasses = property.FindPropertyRelative(p_PseudoClasses), mask = property.FindPropertyRelative(p_Mask);

                        height += 4F * standardVerticalSpacing + GetReorderableListHeight(GetList(selectors)) + GetReorderableListHeight(GetList(pseudoClasses)) + BaseHeight + propertyMaskField.fixedHeight;

                        StyleAttributes flags = (StyleAttributes) mask.enumValueFlag;
                        if (flags == 0) return height + EmptyStyleLineHeight + standardVerticalSpacing;
                        else foreach (StyleAttributes flag in StyleAttributeEnumValues)
                        {
                                if ((flags & flag) != flag) continue;

                                SerializedProperty attribute = property.FindPropertyRelative(flag.ToString());
                                height += EditorGUI.GetPropertyHeight(attribute, true) + standardVerticalSpacing;
                        }
                        return height;
                }

                private static void DrawHeader(ref Rect position, string label)
                {
                        position.height = BaseHeight;

                        EditorGUI.DrawRect(position, ETStyles.accentTone);
                        EditorGUI.LabelField(position, label, ETStyles.boldCenteredText);

                        position.y += position.height;
                }
                private void DrawReorderableList(ref Rect position, SerializedProperty property)
                {
                        DrawHeader(ref position, property.displayName);

                        ReorderableList list = GetList(property);

                        position.height = list.GetHeight();
                        list.DoList(position);
                        position.y += position.height;
                }
                private ReorderableList GetList(SerializedProperty property)
                {
                        string key = $"{property.serializedObject.targetObject.GetInstanceID()}:{property.propertyPath}";

                        if (lists.TryGetValue(key, out ReorderableList list)) return list;

                        return lists[key] = list = new ReorderableList(property.serializedObject, property, true, false, false, false)
                        {
                                headerHeight = 0F,
                                footerHeight = EditorStyles.miniButton.fixedHeight,
                                drawFooterCallback = rect =>
                                {
                                        int size = property.arraySize;

                                        rect.y -= 1F;
                                        rect.width *= 0.5F;

                                        if (GUI.Button(rect, ETStyles.ToolbarPlus, EditorStyles.miniButtonLeft) || size == 0)
                                        {
                                                property.InsertArrayElementAtIndex(size);
                                        }
                                        rect.x += rect.width;

                                        if (GUI.Button(rect, ETStyles.ToolbarMinus, EditorStyles.miniButtonRight) && size > 0)
                                        {
                                                property.DeleteArrayElementAtIndex(size - 1);
                                        }
                                },
                                drawElementCallback = (rect, index, active, focused) =>
                                {
                                        SerializedProperty element = property.GetArrayElementAtIndex(index);

                                        rect.y += standardVerticalSpacing;
                                        rect.height = EditorGUI.GetPropertyHeight(element, GUIContent.none, false);

                                        _ = EditorGUI.PropertyField(rect, element, GUIContent.none, false);
                                },
                                elementHeightCallback = index =>
                                {
                                        SerializedProperty element = property.GetArrayElementAtIndex(index);
                                        return EditorGUI.GetPropertyHeight(element, false);
                                },
                        };
                }
                private float GetReorderableListHeight(ReorderableList list) => list.GetHeight() + list.footerHeight + standardVerticalSpacing;
        }
}