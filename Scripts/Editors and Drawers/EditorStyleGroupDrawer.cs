using System.Linq;

using UnityEditor;
using static UnityEditor.EditorStyles;

using UnityEngine;

namespace Emp37.ET
{
      [CustomPropertyDrawer(typeof(StyleRuleGroup))]
      internal class EditorStyleGroupDrawer : PropertyDrawer
      {
            private const string p_Enabled = "Enabled", p_Title = "Title", p_StyleRules = "StyleRules";
            private const string groupTitleControl = "TitleControl";

            private const float HeaderSize = 32F, HighlightWidth = 3F, BackgroundAlpha = 0.25F;

            private static readonly GUIStyle descriptionLabelStyle = new(label) { fontSize = 14 };
            private static readonly GUIStyle descriptionTextFieldStyle = new(textField) { alignment = TextAnchor.MiddleLeft, fontSize = 14 };


            public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
            {
                  using SerializedProperty enabled = property.FindPropertyRelative(p_Enabled), description = property.FindPropertyRelative(p_Title);

                  using (new EditorGUI.PropertyScope(position, label, property))
                  {
                        #region H E A D E R
                        Rect headerRect = new(position) { height = HeaderSize };

                        // background highlight rect
                        EditorGUI.DrawRect(new(headerRect) { width = HighlightWidth }, StyleHelpers.BackgroundTint);
                        EditorGUI.DrawRect(headerRect, StyleHelpers.BackgroundTint.ChangeOpacity(BackgroundAlpha));
                        headerRect.x += HighlightWidth + 3F;

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
                        headerRect.width = position.width - (headerRect.x - position.x) - HeaderSize;
                        GUI.SetNextControlName(groupTitleControl); // - [ c:0 ]
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
                        headerRect.width = HeaderSize;
                        if (GUI.Button(headerRect, EditorGUIUtility.IconContent("_Menu")) && (description.isExpanded = !description.isExpanded))
                        {
                              GUI.FocusControl(groupTitleControl); // - [ c:0 ]
                        }
                        #endregion

                        if (property.isExpanded)
                        {
                              headerRect.height += EditorGUIUtility.standardVerticalSpacing; // - [ h:0 ]

                              Rect contentRect = new(position) { y = position.y + headerRect.height, height = position.height - headerRect.height - EditorGUIUtility.standardVerticalSpacing /*extra*/ };

                              // background highlight rect
                              EditorGUI.DrawRect(contentRect, StyleHelpers.BackgroundTint.ChangeOpacity(BackgroundAlpha));

                              #region A R R A Y   E L E M E N T S
                              SerializedProperty group = property.FindPropertyRelative(p_StyleRules);
                              int size = group.arraySize;
                              for (byte i = 0; i < size; i++)
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
                              if (GUI.Button(contentRect, EditorGUIUtility.IconContent("d_Toolbar Plus"), miniButtonLeft)) group.InsertArrayElementAtIndex(size++);
                              contentRect.x += contentRect.width;
                              if (GUI.Button(contentRect, EditorGUIUtility.IconContent("d_Toolbar Minus"), miniButtonRight) && size > 0) group.DeleteArrayElementAtIndex(--size);
                              #endregion
                        }
                  }
            }
            public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
            {
                  float height = HeaderSize;
                  if (property.isExpanded)
                  {
                        SerializedProperty group = property.FindPropertyRelative(p_StyleRules);
                        height += Enumerable.Range(0, group.arraySize).Sum(index => EditorGUI.GetPropertyHeight(group.GetArrayElementAtIndex(index)))
                              + miniButton.fixedHeight + EditorGUIUtility.standardVerticalSpacing; // - [ h:0 + h:1 ]
                  }
                  return height;
            }
      }
}