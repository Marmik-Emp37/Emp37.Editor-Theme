using UnityEditor;
using UnityEngine;

namespace Emp37.ET
{
        using static ETHelpers;
        using static EditorGUIUtility;

        [CustomPropertyDrawer(typeof(StyleRuleGroup))]
        internal class StyleRuleGroupDrawer : PropertyDrawer
        {
                private const string p_Enabled = "<" + nameof(StyleRuleGroup.Enabled) + ">k__BackingField";
                private const string p_Title = nameof(StyleRuleGroup.Title);
                private const string p_StyleRules = nameof(StyleRuleGroup.StyleRules);
                private const string control_TargetTitle = nameof(StyleRuleGroup) + ":Control.Title";

                private const float HeaderSize = 32F, FooterHeight = 24F, EndLineHeight = 3F;


                public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
                {
                        _ = EditorGUI.BeginProperty(position, label, property);

                        position.height = HeaderSize;
                        DrawHeader(position, property);

                        if (property.isExpanded)
                        {
                                position.y += position.height + standardVerticalSpacing;

                                SerializedProperty group = property.FindPropertyRelative(p_StyleRules);
                                for (int size = group.arraySize, i = 0; i < size; i++)
                                {
                                        SerializedProperty element = group.GetArrayElementAtIndex(i);

                                        float elementHeight = EditorGUI.GetPropertyHeight(element, true);
                                        EditorGUI.PropertyField(new(position) { height = elementHeight }, element, true);

                                        position.y += elementHeight + standardVerticalSpacing;
                                }
                                DrawFooter(ref position, group);

                                position.y += standardVerticalSpacing;
                                position.height = EndLineHeight;
                                EditorGUI.DrawRect(Inset(position, 20F), ETStyles.accentTone);
                        }
                        EditorGUI.EndProperty();
                }
                public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
                {
                        float height = HeaderSize;
                        if (!property.isExpanded) return height;

                        height += 2F * standardVerticalSpacing + FooterHeight + EndLineHeight;
                        SerializedProperty group = property.FindPropertyRelative(p_StyleRules);
                        for (int i = group.arraySize - 1; i >= 0; i--)
                        {
                                SerializedProperty element = group.GetArrayElementAtIndex(i);
                                height += EditorGUI.GetPropertyHeight(element) + standardVerticalSpacing;
                        }
                        return height;
                }

                private static void DrawHeader(Rect position, SerializedProperty property)
                {
                        Rect original = position;

                        // background
                        EditorGUI.DrawRect(position, ETStyles.accentTone);

                        position.width = 3F;
                        EditorGUI.DrawRect(position, ETStyles.baseTone);
                        position.x += position.width + HorizontalSpacing;

                        // expand toggle
                        position.width = 20F;
                        property.isExpanded = EditorGUI.Toggle(position, property.isExpanded, EditorStyles.foldout);
                        position.x += position.width;

                        // enabled property
                        SerializedProperty enabled = property.FindPropertyRelative(p_Enabled);
                        enabled.boolValue = EditorGUI.Toggle(position, enabled.boolValue);
                        position.x += position.width;

                        // title
                        position.width = original.width - (position.x - original.x) - HorizontalSpacing - HeaderSize; // - [ w : spacing & w : menu button ]
                        GUI.SetNextControlName(control_TargetTitle);
                        SerializedProperty title = property.FindPropertyRelative(p_Title);
                        if (title.isExpanded)
                        {
                                using EditorGUI.ChangeCheckScope check = new();
                                string text = EditorGUI.DelayedTextField(position, title.stringValue, ETStyles.largeTextField);
                                if (check.changed) { title.stringValue = text; title.isExpanded = false; }
                        }
                        else
                        {
                                EditorGUI.LabelField(position, title.stringValue, ETStyles.largeText);
                        }
                        position.x += position.width + HorizontalSpacing; // - w : spacing

                        // rename title button
                        position.width = HeaderSize; // - w : menu button
                        if (GUI.Button(position, ETStyles.Menu) && (title.isExpanded = !title.isExpanded))
                        {
                                GUI.FocusControl(control_TargetTitle);
                        }
                }
                private static void DrawFooter(ref Rect position, SerializedProperty property)
                {
                        int size = property.arraySize;
                        position.height = FooterHeight;

                        Rect rect = position;
                        rect.width *= 0.5F;
                        if (GUI.Button(rect, "Add", GUI.skin.button))
                        {
                                property.InsertArrayElementAtIndex(size);
                        }
                        rect.x += rect.width;
                        if (GUI.Button(rect, "Remove", GUI.skin.button) && size > 0)
                        {
                                property.DeleteArrayElementAtIndex(size - 1);
                        }
                        position.y += position.height;
                }
        }
}