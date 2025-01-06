using System.Linq;

using UnityEditor;
using static UnityEditor.EditorStyles;
using static UnityEditor.EditorGUIUtility;

using UnityEngine;

namespace Emp37.ET
{
      [CustomPropertyDrawer(typeof(StyleRule))]
      internal class EditorStyleDrawer : PropertyDrawer
      {
            private const string p_Selectors = "Selectors", p_PseudoStates = "PseudoStates", p_PropertyMask = "PropertyMask";

            private const float headerHeight = 21F;

            private static readonly GUIStyle expandToggleStyle = new(foldoutHeader) { fontStyle = FontStyle.Normal, fixedHeight = headerHeight };

            public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
            {
                  SerializedProperty selectors = property.FindPropertyRelative(p_Selectors), pseudoStates = property.FindPropertyRelative(p_PseudoStates);

                  using (new EditorGUI.PropertyScope(position, label, property))
                  {
                        position.height = headerHeight;
                        if (property.isExpanded = GUI.Toggle(position, property.isExpanded, label, expandToggleStyle))
                        {
                              position.y += position.height + standardVerticalSpacing;

                              GUIHelpers.ArrayField(position, selectors);
                              position.y += GUIHelpers.CalculateArrayHeight(selectors);
                              GUIHelpers.ArrayField(position, pseudoStates);
                              position.y += GUIHelpers.CalculateArrayHeight(pseudoStates);

                              position.height = GUIHelpers.Spacing;
                              EditorGUI.DrawRect(position, GUIHelpers.BackgroundTint.SetAlpha(0.25F));
                              position.y += position.height + standardVerticalSpacing;

                              using (new EditorGUI.IndentLevelScope(1))
                              {
                                    SerializedProperty propertyMask = property.FindPropertyRelative(p_PropertyMask);
                                    position.height = EditorGUI.GetPropertyHeight(propertyMask);
                                    _ = EditorGUI.PropertyField(position, propertyMask);
                                    position.y += position.height + standardVerticalSpacing;

                                    Properties mask = (Properties) propertyMask.enumValueFlag;
                                    foreach (SerializedProperty context in from item in StyleRule.PropertiesMap where mask.HasFlag(item.Key) select property.FindPropertyRelative(item.Value))
                                    {
                                          position.height = EditorGUI.GetPropertyHeight(context);
                                          _ = EditorGUI.PropertyField(position, context, true);
                                          position.y += position.height + standardVerticalSpacing;
                                    }
                              }
                        }
                  }
            }
            public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
            {
                  if (!property.isExpanded) return headerHeight;

                  float spacing = standardVerticalSpacing, height = headerHeight;
                  if (property.isExpanded)
                  {
                        height += GUIHelpers.CalculateArrayHeight(property.FindPropertyRelative(p_Selectors)) + GUIHelpers.CalculateArrayHeight(property.FindPropertyRelative(p_PseudoStates));
                        height += spacing;
                        height += GUIHelpers.Spacing + spacing;

                        SerializedProperty propertyMask = property.FindPropertyRelative(p_PropertyMask);
                        Properties mask = (Properties) propertyMask.enumValueFlag;
                        height += EditorGUI.GetPropertyHeight(propertyMask)
                              + StyleRule.PropertiesMap.Where(entry => mask.HasFlag(entry.Key)).Select(item => property.FindPropertyRelative(item.Value)).Sum(item => EditorGUI.GetPropertyHeight(item) + spacing);
                  }
                  return height;
            }
      }
}