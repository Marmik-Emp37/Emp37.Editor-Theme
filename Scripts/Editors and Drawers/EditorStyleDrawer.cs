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
            internal const float HeaderSize = 24F;

            private const string p_Selectors = "Selectors", p_PseudoStates = "PseudoStates", p_PropertyMask = "PropertyMask";

            private static readonly GUIStyle expandToggleStyle = new(foldoutHeader) { fontStyle = FontStyle.Normal, fixedHeight = HeaderSize }, maskFieldStyle = new(layerMaskField) { fixedHeight = HeaderSize };


            public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
            {
                  SerializedProperty selectors = property.FindPropertyRelative(p_Selectors), pseudoStates = property.FindPropertyRelative(p_PseudoStates);

                  using (new EditorGUI.PropertyScope(position, label, property))
                  {
                        position.height = HeaderSize;
                        if (property.isExpanded = GUI.Toggle(position, property.isExpanded, label, expandToggleStyle))
                        {
                              position.y += position.height + standardVerticalSpacing; // - [ height : 0 ]

                              GUIHelpers.ArrayField(position, selectors);
                              position.y += GUIHelpers.CalculateArrayHeight(selectors);
                              GUIHelpers.ArrayField(position, pseudoStates);
                              position.y += GUIHelpers.CalculateArrayHeight(pseudoStates);

                              SerializedProperty propertyMask = property.FindPropertyRelative(p_PropertyMask);

                              position.height = 24;
                              EditorGUI.DrawRect(position, GUIHelpers.EditorThemeTint.SetAlpha(0.15F));
                              EditorGUI.LabelField(position, propertyMask.displayName, CustomGUIStyles.centeredLabel);
                              position.y += position.height + standardVerticalSpacing;

                              position.height = maskFieldStyle.fixedHeight;
                              propertyMask.enumValueFlag = (int) (Properties) EditorGUI.EnumFlagsField(position, (Properties) propertyMask.enumValueFlag, maskFieldStyle);
                              position.y += position.height + standardVerticalSpacing;

                              using (new EditorGUI.IndentLevelScope(1))
                              {
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
                  if (!property.isExpanded) return HeaderSize;

                  float spacing = standardVerticalSpacing, height = HeaderSize;
                  if (property.isExpanded)
                  {
                        height += GUIHelpers.CalculateArrayHeight(property.FindPropertyRelative(p_Selectors)) + GUIHelpers.CalculateArrayHeight(property.FindPropertyRelative(p_PseudoStates));
                        height += spacing;
                        height += 24 + spacing;

                        SerializedProperty propertyMask = property.FindPropertyRelative(p_PropertyMask);
                        Properties mask = (Properties) propertyMask.enumValueFlag;
                        height += maskFieldStyle.fixedHeight
                              + StyleRule.PropertiesMap.Where(entry => mask.HasFlag(entry.Key)).Select(item => property.FindPropertyRelative(item.Value)).Sum(item => EditorGUI.GetPropertyHeight(item) + spacing);
                  }
                  return height;
            }
      }
}