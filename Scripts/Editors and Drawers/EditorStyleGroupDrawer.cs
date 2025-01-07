using UnityEditor;

using UnityEngine;

namespace Emp37.ET
{
      [CustomPropertyDrawer(typeof(StyleRuleGroup))]
      internal class EditorStyleGroupDrawer : PropertyDrawer
      {
            private const string p_Enabled = "<Enabled>k__BackingField";
            private const string p_Title = "Title";
            private const string p_StyleRules = "StyleRules";
            private const string control_TargetTitle = "Control.StyleRuleGroup.Title";

            private const int HeaderSize = 32, HighlightWidth = 3, ElementWidth = 20, FooterHeight = 24;


            public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
            {
                  using (new EditorGUI.PropertyScope(position, label, property))
                  {
                        position.height = HeaderSize;
                        DrawHeader(position, property);

                        if (property.isExpanded)
                        {
                              position.y += position.height + EditorGUIUtility.standardVerticalSpacing; // - H : Header

                              SerializedProperty group = property.FindPropertyRelative(p_StyleRules);

                              for (int i = 0; i < group.arraySize; i++)
                              {
                                    SerializedProperty element = group.GetArrayElementAtIndex(i);
                                    position.height = EditorGUI.GetPropertyHeight(element);
                                    EditorGUI.PropertyField(position, element, true);
                                    position.y += position.height + EditorGUIUtility.standardVerticalSpacing; // - H : Array Elements
                              }

                              position.height = FooterHeight;
                              DrawFooter(position, group);

                              position.y += position.height + EditorGUIUtility.standardVerticalSpacing;  // - H : Array Controls
                              position.height = ETHelpers.Spacing; // - H : Separator
                              EditorGUI.DrawRect(position, ETHelpers.ThemeAccent);
                        }
                  }
            }
            public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
            {
                  if (!property.isExpanded) return HeaderSize;

                  float height = HeaderSize + EditorGUIUtility.standardVerticalSpacing; // - @r >> H : Header

                  SerializedProperty group = property.FindPropertyRelative(p_StyleRules);
                  for (int i = 0; i < group.arraySize; i++)
                  {
                        height += EditorGUI.GetPropertyHeight(group.GetArrayElementAtIndex(i)) + EditorGUIUtility.standardVerticalSpacing; // - @r >> H : Array Elements
                  }

                  height += FooterHeight + EditorGUIUtility.standardVerticalSpacing; // - @r >> H : Array Controls
                  height += ETHelpers.Spacing; // - @r >> H : Separator

                  return height;
            }

            /// <summary>
            /// Draws the header section of the property.
            /// </summary>
            private static void DrawHeader(Rect position, SerializedProperty property)
            {
                  SerializedProperty enabled = property.FindPropertyRelative(p_Enabled), description = property.FindPropertyRelative(p_Title);

                  Rect current = position;

                  // Draw Background
                  EditorGUI.DrawRect(current, ETHelpers.ThemeAccent);

                  current.width = HighlightWidth;
                  EditorGUI.DrawRect(current, ETHelpers.ThemeTint);

                  // Draw Expand Toggle
                  current.x += current.width;
                  current.width = ElementWidth;
                  property.isExpanded = EditorGUI.Toggle(current, property.isExpanded, EditorStyles.foldout);

                  // Draw Group.Enabled Toggle
                  current.x += current.width;
                  enabled.boolValue = EditorGUI.Toggle(current, enabled.boolValue);

                  // Draw Title
                  current.x += current.width;
                  current.width = position.width - (current.x - position.x) - ETHelpers.Spacing - HeaderSize; // - [ W : Spacing - W : Menu Button ]
                  GUI.SetNextControlName(control_TargetTitle);

                  if (description.isExpanded)
                  {
                        using var check = new EditorGUI.ChangeCheckScope();
                        string text = EditorGUI.DelayedTextField(current, description.stringValue, ETStyles.largeTextField);
                        if (check.changed)
                        {
                              description.stringValue = text;
                              description.isExpanded = false;
                        }
                  }
                  else
                  {
                        EditorGUI.LabelField(current, description.stringValue, ETStyles.largeText);
                  }

                  // Draw Rename Title Button
                  current.x += current.width + ETHelpers.Spacing; // - W : Spacing
                  current.width = HeaderSize; // - W : Menu Button
                  if (GUI.Button(current, EditorGUIUtility.IconContent("_Menu")))
                  {
                        description.isExpanded = !description.isExpanded;
                        GUI.FocusControl(control_TargetTitle);
                  }
            }
            /// <summary>
            /// Draws the Add and Remove buttons for the array.
            /// </summary>
            private static void DrawFooter(Rect position, SerializedProperty arrayProperty)
            {
                  int size = arrayProperty.arraySize;

                  position.width *= 0.5F;
                  if (GUI.Button(position, "Add", GUI.skin.button))
                  {
                        arrayProperty.InsertArrayElementAtIndex(size++);
                  }

                  position.x += position.width;
                  if (GUI.Button(position, "Remove", GUI.skin.button) && size > 0)
                  {
                        arrayProperty.DeleteArrayElementAtIndex(--size);
                  }
            }
      }
}