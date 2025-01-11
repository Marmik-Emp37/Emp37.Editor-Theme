using UnityEditor;
using static UnityEditor.EditorGUIUtility;

using UnityEngine;

namespace Emp37.ET
{
      using static ETHelpers;

      // @r: == Redeem
      [CustomPropertyDrawer(typeof(StyleRuleGroup))]
      internal class EditorStyleGroupDrawer : PropertyDrawer
      {
            private const string p_Enabled = "<Enabled>k__BackingField";
            private const string p_Title = "Title";
            private const string p_StyleRules = "StyleRules";
            private const string control_TargetTitle = "Control.StyleRuleGroup.Title";

            private const float HeaderSize = 32F, FooterHeight = 24F;
            private const float ElementControlWidth = 20F;


            private void DrawHeader(Rect position, SerializedProperty property)
            {
                  Rect rect = position;

                  // Background
                  EditorGUI.DrawRect(rect, ThemeAccent);
                  rect.width = 3F;
                  EditorGUI.DrawRect(rect, ThemeTint);

                  rect.x += rect.width + Spacing;

                  // Expand Toggle
                  rect.width = 20F;
                  property.isExpanded = EditorGUI.Toggle(rect, property.isExpanded, EditorStyles.foldout);

                  rect.x += rect.width;

                  // Enabled Property
                  SerializedProperty enabled = property.FindPropertyRelative(p_Enabled);
                  enabled.boolValue = EditorGUI.Toggle(rect, enabled.boolValue);

                  rect.x += rect.width;

                  // Title
                  rect.width = position.width - (rect.x - position.x) - Spacing - HeaderSize; // - [ W : Spacing & W : Menu Button ]
                  GUI.SetNextControlName(control_TargetTitle);
                  SerializedProperty title = property.FindPropertyRelative(p_Title);
                  if (title.isExpanded)
                  {
                        using EditorGUI.ChangeCheckScope check = new();
                        string text = EditorGUI.DelayedTextField(rect, title.stringValue, ETStyles.largeTextField);
                        if (check.changed) { title.stringValue = text; title.isExpanded = false; }
                  }
                  else
                  {
                        EditorGUI.LabelField(rect, title.stringValue, ETStyles.largeText);
                  }

                  rect.x += rect.width + Spacing; // - W : Spacing

                  // Rename Title Button
                  rect.width = HeaderSize; // - W : Menu Button
                  if (GUI.Button(rect, IconContent("_Menu")) && (title.isExpanded = !title.isExpanded))
                  {
                        GUI.FocusControl(control_TargetTitle);
                  }
            }
            private Rect DrawElements(Rect position, SerializedProperty property)
            {
                  for (int size = property.arraySize, i = 0; i < size; i++)
                  {
                        SerializedProperty element = property.GetArrayElementAtIndex(i);

                        position.height = EditorGUI.GetPropertyHeight(element);
                        EditorGUI.PropertyField(position, element, true);

                        position.y += position.height + Spacing;
                  }
                  return position;
            }
            private static void DrawFooter(Rect position, SerializedProperty property)
            {
                  position.width *= 0.5F;
                  if (GUI.Button(position, "Add", GUI.skin.button))
                  {
                        property.InsertArrayElementAtIndex(property.arraySize);
                  }

                  position.x += position.width;

                  if (GUI.Button(position, "Remove", GUI.skin.button) && property.arraySize > 0)
                  {
                        property.DeleteArrayElementAtIndex(property.arraySize - 1);
                  }
            }

            public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
            {
                  _ = EditorGUI.BeginProperty(position, label, property);

                  position.height = HeaderSize;
                  DrawHeader(position, property);

                  if (property.isExpanded)
                  {
                        position.y += position.height + Spacing; // - H : Header

                        SerializedProperty group = property.FindPropertyRelative(p_StyleRules);
                        position = DrawElements(position, group); // - H : Elements

                        position.height = FooterHeight;
                        DrawFooter(position, group);

                        position.y += position.height + Spacing;  // - H : Footer

                        position.height = Spacing; // - H : Separator
                        EditorGUI.DrawRect(position, ThemeAccent);
                  }

                  EditorGUI.EndProperty();
            }
            public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
            {
                  if (property.isExpanded)
                  {
                        float value = HeaderSize + Spacing; // - @r >> H : Header
                        SerializedProperty group = property.FindPropertyRelative(p_StyleRules);
                        for (int i = group.arraySize - 1; i >= 0; i--)
                        {
                              value += EditorGUI.GetPropertyHeight(group.GetArrayElementAtIndex(i)) + Spacing; // - @r >> H : Elements
                        }
                        value += FooterHeight + Spacing; // - @r >> H : Footer
                        value += Spacing; // - @r >> H : Separator
                        return value;
                  }
                  else
                  {
                        return HeaderSize;
                  }
            }
      }
}