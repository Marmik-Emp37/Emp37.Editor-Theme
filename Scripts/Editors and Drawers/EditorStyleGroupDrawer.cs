using UnityEditor;
using static UnityEditor.EditorStyles;
using static UnityEditor.EditorGUIUtility;

using UnityEngine;

namespace Emp37.ET
{
      // @r: == redeem label
      [CustomPropertyDrawer(typeof(StyleRuleGroup))]
      internal class EditorStyleGroupDrawer : PropertyDrawer
      {
            private const string p_Enabled = "<Enabled>k__BackingField";
            private const string p_Title = "Title";
            private const string p_StyleRules = "StyleRules";
            private const string control_TargetTitle = "Control.StyleRuleGroup.Title";

            private const float height_Header = 32F;
            private const float size_Button = 24F;


            public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
            {
                  using (new EditorGUI.PropertyScope(position, label, property))
                  {
                        position.height = height_Header;

                        Header(position, property);

                        if (property.isExpanded)
                        {
                              position.y += position.height + standardVerticalSpacing; // - [ height : 0 ]

                              SerializedProperty group = property.FindPropertyRelative(p_StyleRules);
                              for (int size = group.arraySize, i = 0; i < size; i++)
                              {
                                    SerializedProperty context = group.GetArrayElementAtIndex(i);
                                    position.height = EditorGUI.GetPropertyHeight(context);
                                    _ = EditorGUI.PropertyField(position, context, true);

                                    position.y += position.height + standardVerticalSpacing; // - [ height : 1 ]
                              }
                              position.height = size_Button;

                              DrawArrayControls(position, group);

                              position.y += position.height + standardVerticalSpacing; // - [ height : 2 ]
                              position.height = ETHelpers.Spacing; // - [ height : 3 ]

                              EditorGUI.DrawRect(position, ETHelpers.ThemeAccent);
                        }
                  }
            }
            public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
            {
                  if (!property.isExpanded) return height_Header;

                  float height = height_Header + standardVerticalSpacing;  // - [ @r: height : 0 ]

                  SerializedProperty group = property.FindPropertyRelative(p_StyleRules);
                  for (int size = group.arraySize, i = 0; i < size; i++)
                  {
                        height += EditorGUI.GetPropertyHeight(group.GetArrayElementAtIndex(i)) + standardVerticalSpacing; // - [ @r: height : 1 ]
                  }

                  height += size_Button + standardVerticalSpacing; // - [ @r: height : 2 ]

                  height += ETHelpers.Spacing;  // - [ @r: height : 3 ]

                  return height;
            }

            private static void Header(Rect position, SerializedProperty property)
            {
                  const int size_Highlight = 3;


                  SerializedProperty enabled = property.FindPropertyRelative(p_Enabled), description = property.FindPropertyRelative(p_Title);

                  Rect current = position;

                  EditorGUI.DrawRect(current, ETHelpers.ThemeAccent);

                  current.width = size_Highlight;

                  EditorGUI.DrawRect(current, ETHelpers.ThemeTint);

                  current.x += current.width;
                  current.width = 20F; // assumed width

                  property.isExpanded = EditorGUI.Toggle(current, property.isExpanded, foldout);

                  current.x += current.width;

                  enabled.boolValue = EditorGUI.Toggle(current, enabled.boolValue);

                  current.x += current.width;
                  current.width = position.width - (current.x - position.x) - ETHelpers.Spacing - size_Button; // - [ @r: width : 0 ]

                  GUI.SetNextControlName(control_TargetTitle); // - [ control : 0 ]

                  if (description.isExpanded)
                  {
                        using EditorGUI.ChangeCheckScope scope = new();
                        string text = EditorGUI.DelayedTextField(current, description.stringValue, ETStyles.largeTextField);
                        if (scope.changed)
                        {
                              description.stringValue = text;
                              description.isExpanded = false;
                        }
                  }
                  else
                  {
                        EditorGUI.LabelField(current, description.stringValue, ETStyles.largeText);
                  }

                  current.x += current.width + ETHelpers.Spacing; // - [ width : 0 ]
                  current.width = size_Button;

                  if (GUI.Button(current, IconContent("_Menu")) && (description.isExpanded = !description.isExpanded))
                  {
                        GUI.FocusControl(control_TargetTitle); // - [ @r: control : 0 ]
                  }
            }
            private void DrawArrayControls(Rect position, SerializedProperty property)
            {
                  int size = property.arraySize;

                  position.width *= 0.5F;

                  if (GUI.Button(position, "Add", GUI.skin.button)) property.InsertArrayElementAtIndex(size++);

                  position.x += position.width;

                  if (GUI.Button(position, "Remove", GUI.skin.button) && size > 0) property.DeleteArrayElementAtIndex(--size);
            }
      }
}