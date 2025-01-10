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
            private const float ElementWidth = 20F;


            private void DrawHeader(Rect position, SerializedProperty property)
            {
                  SerializedProperty enabled = property.FindPropertyRelative(p_Enabled), title = property.FindPropertyRelative(p_Title);

                  Rect rect = new(position) { height = HeaderSize };

                  // Background
                  EditorGUI.DrawRect(rect, ThemeAccent);
                  rect.width = 3F;
                  EditorGUI.DrawRect(rect, ThemeTint);

                  rect.x += rect.width;

                  // Expand Toggle
                  rect.width = ElementWidth;
                  property.isExpanded = EditorGUI.Toggle(rect, property.isExpanded, EditorStyles.foldout);

                  rect.x += rect.width;

                  // Enabled Property
                  enabled.boolValue = EditorGUI.Toggle(rect, enabled.boolValue);

                  rect.x += rect.width;

                  // Title
                  rect.width = position.width - (rect.x - position.x) - Spacing - HeaderSize; // - [ W : Spacing & W : Menu Button ]
                  GUI.SetNextControlName(control_TargetTitle);
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
            private void DrawFooter(Rect rect, SerializedProperty group)
            {
                  int size = group.arraySize;

                  rect.height = FooterHeight;
                  rect.width *= 0.5F;

                  if (GUI.Button(rect, "Add", GUI.skin.button))
                  {
                        group.InsertArrayElementAtIndex(size++);
                  }

                  rect.x += rect.width;

                  if (GUI.Button(rect, "Remove", GUI.skin.button) && size > 0)
                  {
                        group.DeleteArrayElementAtIndex(--size);
                  }
            }

            public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
            {
                  using (new EditorGUI.PropertyScope(position, label, property))
                  {
                        DrawHeader(position, property);
                        if (property.isExpanded)
                        {
                              position.y += HeaderSize + Spacing; // - H : Header

                              #region E L E M E N TS
                              SerializedProperty group = property.FindPropertyRelative(p_StyleRules);

                              for (int size = group.arraySize, i = 0; i < size; i++)
                              {
                                    SerializedProperty element = group.GetArrayElementAtIndex(i);
                                    position.height = EditorGUI.GetPropertyHeight(element);
                                    EditorGUI.PropertyField(position, element, true);
                                    position.y += position.height + Spacing; // - H : Elements
                              }
                              #endregion

                              DrawFooter(position, group);

                              position.y += FooterHeight + Spacing; // - H : Footer

                              position.height = Spacing; // - H : Separator
                              EditorGUI.DrawRect(position, ThemeAccent);
                        }
                  }
            }
            public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
            {
                  if (property.isExpanded)
                  {
                        float value = HeaderSize + Spacing; // - @r >> H : Header
                        for (int i = property.FindPropertyRelative(p_StyleRules).arraySize - 1; i >= 0; i--)
                        {
                              value += EditorGUI.GetPropertyHeight(property.FindPropertyRelative(p_StyleRules).GetArrayElementAtIndex(i)) + Spacing; // - @r >> H : Elements
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