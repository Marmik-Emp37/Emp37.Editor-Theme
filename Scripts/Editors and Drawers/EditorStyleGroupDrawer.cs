using System.Linq;

using UnityEditor;
using static UnityEditor.EditorStyles;
using static UnityEditor.EditorGUIUtility;

using UnityEngine;

namespace Emp37.ET
{
      [CustomPropertyDrawer(typeof(StyleRuleGroup))]
      internal class EditorStyleGroupDrawer : PropertyDrawer
      {
            private const string p_Enabled = "<Enabled>k__BackingField", p_Title = "Title", p_StyleRules = "StyleRules";
            private const string control_TargetTitle = "Control.StyleRuleGroup.Title";

            private const float headerHeight = 32F, highlightWidth = 3F, backgroundAlpha = 0.15F;
            private const int textSize = 14;

            private static readonly GUIStyle titleLabelStyle = new(label) { fontSize = textSize };
            private static readonly GUIStyle titleTextFieldStyle = new(textField) { alignment = TextAnchor.MiddleLeft, fontSize = textSize };

            public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
            {
                  using SerializedProperty enabled = property.FindPropertyRelative(p_Enabled), description = property.FindPropertyRelative(p_Title);

                  using (new EditorGUI.PropertyScope(position, label, property))
                  {
                        #region H E A D E R
                        Rect headerRect = new(position) { height = headerHeight };

                        // background highlight rect
                        EditorGUI.DrawRect(new(headerRect) { width = highlightWidth }, GUIHelpers.BackgroundTint);
                        EditorGUI.DrawRect(headerRect, GUIHelpers.BackgroundTint.SetAlpha(backgroundAlpha));
                        headerRect.x += highlightWidth + 3F;

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
                        headerRect.width = position.width - (headerRect.x - position.x) - headerHeight;
                        GUI.SetNextControlName(control_TargetTitle); // - [ c:0 ]
                        if (description.isExpanded)
                        {
                              EditorGUI.BeginChangeCheck();
                              description.stringValue = EditorGUI.DelayedTextField(headerRect, description.stringValue, titleTextFieldStyle);
                              if (EditorGUI.EndChangeCheck()) description.isExpanded = false;
                        }
                        else
                        {
                              EditorGUI.LabelField(headerRect, description.stringValue, titleLabelStyle);
                        }
                        headerRect.x += headerRect.width;

                        // edit title button
                        headerRect.width = headerHeight;
                        if (GUI.Button(headerRect, IconContent("_Menu")) && (description.isExpanded = !description.isExpanded))
                        {
                              GUI.FocusControl(control_TargetTitle); // - [ c:0 ]
                        }
                        #endregion

                        if (property.isExpanded)
                        {
                              Rect contentRect = new(position) { y = position.y + (headerRect.height += standardVerticalSpacing), height = position.height - headerRect.height - standardVerticalSpacing };

                              EditorGUI.DrawRect(contentRect, GUIHelpers.BackgroundTint.SetAlpha(backgroundAlpha)); // background

                              #region A R R A Y   E L E M E N T S
                              SerializedProperty group = property.FindPropertyRelative(p_StyleRules);
                              int size = group.arraySize;
                              for (int i = 0; i < size; i++)
                              {
                                    SerializedProperty context = group.GetArrayElementAtIndex(i);
                                    contentRect.height = EditorGUI.GetPropertyHeight(context);
                                    _ = EditorGUI.PropertyField(contentRect, context, true);
                                    contentRect.y += contentRect.height + standardVerticalSpacing; // - [ h:1 ]
                              }
                              #endregion

                              #region C O N T R O L   O P T I O N S
                              contentRect.width /= 2F;
                              contentRect.height = miniButton.fixedHeight; // - [ h:2 ]
                              if (GUI.Button(contentRect, "Add", miniButtonLeft)) group.InsertArrayElementAtIndex(size++);
                              contentRect.x += contentRect.width;
                              if (GUI.Button(contentRect, "Remove", miniButtonRight) && size > 0) group.DeleteArrayElementAtIndex(--size);
                              #endregion
                        }
                  }
            }
            public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
            {
                  if (!property.isExpanded) return headerHeight;

                  SerializedProperty group = property.FindPropertyRelative(p_StyleRules);
                  float arrayHeight = 0F;
                  for (int size = group.arraySize, i = 0; i < size; i++) arrayHeight += EditorGUI.GetPropertyHeight(group.GetArrayElementAtIndex(i)) + standardVerticalSpacing; // - [ h:1 ]

                  return headerHeight + arrayHeight + (miniButton.fixedHeight /*- [ h:2 ]*/ + standardVerticalSpacing /*- [ h:1 ]*/);
            }
      }
}