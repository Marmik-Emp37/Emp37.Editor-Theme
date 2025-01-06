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

            private const float h_Header = 32F;
            private const float size_Button = 28F;
            private const int size_Highlight = 3;
            private const int size_Text = 14;

            private static readonly GUIStyle style_TitleLabel = new(label)
            {
                  fontSize = size_Text
            };
            private static readonly GUIStyle style_TitleTextField = new(textField)
            {
                  alignment = TextAnchor.MiddleLeft,
                  fontSize = size_Text
            };


            public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
            {
                  EditorGUI.BeginProperty(position, label, property);
                  position.height = h_Header;
                  Header(position, property);
                  if (property.isExpanded)
                  {
                        position.y += position.height + standardVerticalSpacing; // - [ height : 0 ]
                        SerializedProperty group = property.FindPropertyRelative(p_StyleRules);
                        int size = group.arraySize;
                        for (int i = 0; i < size; i++)
                        {
                              SerializedProperty context = group.GetArrayElementAtIndex(i);
                              position.height = EditorGUI.GetPropertyHeight(context);
                              _ = EditorGUI.PropertyField(position, context, true);
                              position.y += position.height + standardVerticalSpacing; // - [ height : 1 ]
                        }
                        Rect buttonRect = new(position) { width = position.width * 0.5F, height = size_Button };
                        if (GUI.Button(buttonRect, "Add", GUI.skin.button)) group.InsertArrayElementAtIndex(size++);
                        buttonRect.x += buttonRect.width;
                        if (GUI.Button(buttonRect, "Remove", GUI.skin.button) && size > 0) group.DeleteArrayElementAtIndex(--size);
                        position.y += buttonRect.height + standardVerticalSpacing; // - [ height : 2 ]
                        position.height = ETHelpers.Spacing; // - [ height : 3 ]
                        EditorGUI.DrawRect(position, ETHelpers.ThemeAccent);
                  }
                  EditorGUI.EndProperty();
            }
            public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
            {
                  if (!property.isExpanded) return h_Header;
                  float height = h_Header + standardVerticalSpacing;  // - [ @r: height : 0 ]
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
                  SerializedProperty enabled = property.FindPropertyRelative(p_Enabled), description = property.FindPropertyRelative(p_Title);
                  Rect original = position;
                  EditorGUI.DrawRect(position, ETHelpers.ThemeAccent);
                  position.width = size_Highlight;
                  EditorGUI.DrawRect(position, ETHelpers.ThemeTint);
                  position.x += size_Highlight;
                  position.width = 20F;
                  {
                        EditorGUI.BeginChangeCheck();
                        property.isExpanded = EditorGUI.Toggle(position, property.isExpanded, foldout);
                        if (EditorGUI.EndChangeCheck()) description.isExpanded = false;
                        position.x += position.width;
                        enabled.boolValue = EditorGUI.Toggle(position, enabled.boolValue);
                  }
                  position.x += position.width;
                  position.width = original.width - (position.x - original.x) - ETHelpers.Spacing - size_Button;
                  GUI.SetNextControlName(control_TargetTitle); // - [ control : 0 ]
                  if (description.isExpanded)
                  {
                        EditorGUI.BeginChangeCheck();
                        description.stringValue = EditorGUI.DelayedTextField(position, description.stringValue, style_TitleTextField);
                        if (EditorGUI.EndChangeCheck()) description.isExpanded = false;
                  }
                  else
                  {
                        EditorGUI.LabelField(position, description.stringValue, style_TitleLabel);
                  }
                  position.x += position.width + ETHelpers.Spacing;
                  position.width = size_Button;
                  if (GUI.Button(position, IconContent("_Menu")) && (description.isExpanded = !description.isExpanded))
                  {
                        GUI.FocusControl(control_TargetTitle); // - [ @r: control : 0 ]
                  }
            }
      }
}