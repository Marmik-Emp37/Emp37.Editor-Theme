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

            private const float headerSize = 32F, highlightWidth = 3F, backgroundAlpha = 0.25F;
            private const int textSize = 14;

            private static readonly GUIStyle descriptionLabelStyle = new(label) { fontSize = textSize };
            private static readonly GUIStyle descriptionTextFieldStyle = new(textField) { alignment = TextAnchor.MiddleLeft, fontSize = textSize };

            public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
            {
                  using SerializedProperty enabled = property.FindPropertyRelative(p_Enabled), description = property.FindPropertyRelative(p_Title);

                  using (new EditorGUI.PropertyScope(position, label, property))
                  {
                        #region H E A D E R
                        Rect headerRect = new(position) { height = headerSize };

                        // background highlight rect
                        EditorGUI.DrawRect(new(headerRect) { width = highlightWidth }, GUIHelpers.BackgroundTint);
                        EditorGUI.DrawRect(headerRect, GUIHelpers.BackgroundTint.ChangeOpacity(backgroundAlpha));
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
                        headerRect.width = position.width - (headerRect.x - position.x) - headerSize;
                        GUI.SetNextControlName(control_TargetTitle); // - [ c:0 ]
                        if (description.isExpanded)
                        {
                              EditorGUI.BeginChangeCheck();
                              description.stringValue = EditorGUI.DelayedTextField(headerRect, description.stringValue, descriptionTextFieldStyle);
                              if (EditorGUI.EndChangeCheck()) description.isExpanded = false;
                        }
                        else
                        {
                              EditorGUI.LabelField(headerRect, description.stringValue, descriptionLabelStyle);
                        }
                        headerRect.x += headerRect.width;

                        // edit title button
                        headerRect.width = headerSize;
                        if (GUI.Button(headerRect, IconContent("_Menu")) && (description.isExpanded = !description.isExpanded))
                        {
                              GUI.FocusControl(control_TargetTitle); // - [ c:0 ]
                        }
                        #endregion

                        if (property.isExpanded)
                        {
                              Rect contentRect = new(position) { y = position.y + (headerRect.height += standardVerticalSpacing), height = position.height - headerRect.height - standardVerticalSpacing };

                              EditorGUI.DrawRect(contentRect, GUIHelpers.BackgroundTint.ChangeOpacity(backgroundAlpha)); // background

                              #region A R R A Y   E L E M E N T S
                              SerializedProperty group = property.FindPropertyRelative(p_StyleRules);
                              int size = group.arraySize;
                              for (int i = 0; i < size; i++)
                              {
                                    SerializedProperty context = group.GetArrayElementAtIndex(i);
                                    contentRect.height = EditorGUI.GetPropertyHeight(context);
                                    _ = EditorGUI.PropertyField(contentRect, context, true);
                                    contentRect.y += contentRect.height;
                              }
                              #endregion

                              #region C O N T R O L   O P T I O N S
                              contentRect.width /= 2F;
                              contentRect.height = miniButton.fixedHeight; // - [ h:1 ]
                              if (GUI.Button(contentRect, "Add", miniButtonLeft)) group.InsertArrayElementAtIndex(size++);
                              contentRect.x += contentRect.width;
                              if (GUI.Button(contentRect, "Remove", miniButtonRight) && size > 0) group.DeleteArrayElementAtIndex(--size);
                              #endregion
                        }
                  }
            }
            public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
            {
                  if (!property.isExpanded) return headerSize;

                  SerializedProperty group = property.FindPropertyRelative(p_StyleRules);
                  return headerSize + Enumerable.Range(0, group.arraySize).Sum(index => EditorGUI.GetPropertyHeight(group.GetArrayElementAtIndex(index))) + (miniButton.fixedHeight /*- [ h:0 ]*/ + standardVerticalSpacing /*- [ h:1 ]*/);
            }
      }
}