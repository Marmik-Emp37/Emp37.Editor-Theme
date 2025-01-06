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
            private const string p_Enabled = "<Enabled>k__BackingField", p_Title = "Title", p_StyleRules = "StyleRules";
            private const string control_TargetTitle = "Control.StyleRuleGroup.Title";

            private const float headerLength = 32F, highlightWidth = 3F, backgroundAlpha = 0.15F;
            private const int textSize = 14;

            private static readonly GUIStyle labelTitleStyle = new(label) { fontSize = textSize }, editableTitleStyle = new(textField) { alignment = TextAnchor.MiddleLeft, fontSize = textSize };

            public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
            {
                  using SerializedProperty enabled = property.FindPropertyRelative(p_Enabled), description = property.FindPropertyRelative(p_Title);
                  using (new EditorGUI.PropertyScope(position, label, property))
                  {
                        #region H E A D E R
                        Rect headerRect = new(position) { height = headerLength }; // - [ height : 0 ]

                        // background highlight rect
                        EditorGUI.DrawRect(new(headerRect) { width = highlightWidth }, GUIHelpers.EditorThemeTint);
                        EditorGUI.DrawRect(headerRect, GUIHelpers.EditorThemeTint.SetAlpha(backgroundAlpha));
                        headerRect.x += highlightWidth + 4F;

                        // expandable foldout toggle
                        headerRect.width = 18F;
                        EditorGUI.BeginChangeCheck();
                        property.isExpanded = EditorGUI.Toggle(headerRect, property.isExpanded, foldout);
                        if (EditorGUI.EndChangeCheck()) description.isExpanded = false;
                        headerRect.x += headerRect.width;

                        // enabled state toggle
                        enabled.boolValue = EditorGUI.Toggle(headerRect, enabled.boolValue);
                        headerRect.x += headerRect.width;

                        // style group title
                        headerRect.width = position.width - (headerRect.x - position.x) - headerLength;
                        GUI.SetNextControlName(control_TargetTitle); // - [ control : 0 ]
                        if (description.isExpanded)
                        {
                              EditorGUI.BeginChangeCheck();
                              description.stringValue = EditorGUI.DelayedTextField(headerRect, description.stringValue, editableTitleStyle);
                              if (EditorGUI.EndChangeCheck()) description.isExpanded = false;
                        }
                        else
                        {
                              EditorGUI.LabelField(headerRect, description.stringValue, labelTitleStyle);
                        }
                        headerRect.x += headerRect.width;

                        // edit title button
                        headerRect.width = headerLength;
                        if (GUI.Button(headerRect, IconContent("_Menu")) && (description.isExpanded = !description.isExpanded))
                        {
                              GUI.FocusControl(control_TargetTitle); // - [ @r: control : 0 ]
                        }
                        #endregion

                        if (property.isExpanded)
                        {
                              Rect contentRect = new(position) { y = position.y + (headerRect.height += standardVerticalSpacing), height = position.height - headerRect.height - standardVerticalSpacing };

                              #region A R R A Y   E L E M E N T S
                              SerializedProperty group = property.FindPropertyRelative(p_StyleRules);
                              int size = group.arraySize;
                              for (int i = 0; i < size; i++)
                              {
                                    SerializedProperty context = group.GetArrayElementAtIndex(i);
                                    contentRect.height = EditorGUI.GetPropertyHeight(context);
                                    _ = EditorGUI.PropertyField(contentRect, context, true);
                                    contentRect.y += contentRect.height + standardVerticalSpacing; // - [ height : 1 ]
                              }
                              #endregion

                              #region C O N T R O L   O P T I O N S
                              contentRect.width /= 2F;
                              contentRect.height = EditorStyleDrawer.HeaderSize; // - [ height : 2 ]
                              if (GUI.Button(contentRect, "Add", GUI.skin.button)) group.InsertArrayElementAtIndex(size++);
                              contentRect.x += contentRect.width;
                              if (GUI.Button(contentRect, "Remove", GUI.skin.button) && size > 0) group.DeleteArrayElementAtIndex(--size);
                              #endregion

                              contentRect = new(position) { y = contentRect.y + contentRect.height + standardVerticalSpacing, height = GUIHelpers.Spacing }; 
                              EditorGUI.DrawRect(contentRect, GUIHelpers.EditorThemeTint.SetAlpha(backgroundAlpha));
                        }
                  }
            }
            public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
            {
                  if (!property.isExpanded) return headerLength;

                  SerializedProperty group = property.FindPropertyRelative(p_StyleRules);
                  float height = headerLength; // - [ @r: height : 0 ]
                  for (int size = group.arraySize, i = 0; i < size; i++) height += EditorGUI.GetPropertyHeight(group.GetArrayElementAtIndex(i)) + standardVerticalSpacing; // - [ @r: height : 1 ]
                  height += EditorStyleDrawer.HeaderSize + standardVerticalSpacing; // - [ @r: height : 2 ]
                  height += standardVerticalSpacing + GUIHelpers.Spacing;
                  return height;
            }
      }
}