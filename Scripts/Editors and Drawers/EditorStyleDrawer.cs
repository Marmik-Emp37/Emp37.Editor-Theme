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
            internal const float HeaderSize = 21F;

            private const string p_ClassSelectors = "ClassSelectors", p_PseudoClasses = "PseudoClasses", p_PropertyMask = "PropertyMask";

            private static readonly GUIStyle expandToggleStyle = new(foldoutHeader) { fontStyle = FontStyle.Normal, fixedHeight = HeaderSize }, maskFieldStyle = new(layerMaskField) { fixedHeight = HeaderSize };


            public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
            {
                  SerializedProperty selectors = property.FindPropertyRelative(p_ClassSelectors), pseudoStates = property.FindPropertyRelative(p_PseudoClasses);
                  using (new EditorGUI.PropertyScope(position, label, property))
                  {
                        position.height = HeaderSize;
                        if (property.isExpanded = GUI.Toggle(position, property.isExpanded, label, expandToggleStyle))
                        {
                              position.y += position.height + standardVerticalSpacing; // - [ height : 0 ]

                              ETHelpers.ArrayField(position, selectors);
                              position.y += ETHelpers.CalculateArrayHeight(selectors);
                              ETHelpers.ArrayField(position, pseudoStates);
                              position.y += ETHelpers.CalculateArrayHeight(pseudoStates);

                              SerializedProperty propertyMask = property.FindPropertyRelative(p_PropertyMask);

                              position.height = 24;
                              EditorGUI.DrawRect(position, ETHelpers.ThemeTint);
                              EditorGUI.LabelField(position, propertyMask.displayName, ETStyles.centeredLabel);
                              position.y += position.height + standardVerticalSpacing;

                              position.height = maskFieldStyle.fixedHeight;
                              EditorGUI.BeginChangeCheck();
                              var valueFlag = EditorGUI.EnumFlagsField(position, (USSProperties) propertyMask.enumValueFlag, maskFieldStyle);
                              if (EditorGUI.EndChangeCheck())
                              {
                                    property.enumValueFlag = (int) (USSProperties) valueFlag;
                              }
                              position.y += position.height + standardVerticalSpacing;

                              using (new EditorGUI.IndentLevelScope(1))
                              {
                                    USSProperties mask = (USSProperties) propertyMask.enumValueFlag;
                                    foreach (SerializedProperty context in from item in StyleRule.PropertyMap where mask.HasFlag(item.Property) select property.FindPropertyRelative(item.Name))
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
                        height += ETHelpers.CalculateArrayHeight(property.FindPropertyRelative(p_ClassSelectors)) + ETHelpers.CalculateArrayHeight(property.FindPropertyRelative(p_PseudoClasses));
                        height += spacing;
                        height += 24 + spacing;

                        SerializedProperty propertyMask = property.FindPropertyRelative(p_PropertyMask);
                        USSProperties mask = (USSProperties) propertyMask.enumValueFlag;
                        height += maskFieldStyle.fixedHeight
                              + StyleRule.PropertyMap.Where(entry => mask.HasFlag(entry.Property)).Select(item => property.FindPropertyRelative(item.Name)).Sum(item => EditorGUI.GetPropertyHeight(item) + spacing);
                  }
                  return height;
            }
      }
}