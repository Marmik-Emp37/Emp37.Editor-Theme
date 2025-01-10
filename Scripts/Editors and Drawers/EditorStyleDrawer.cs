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

                              ArrayField(position, selectors);
                              position.y += CalculateArrayHeight(selectors);
                              ArrayField(position, pseudoStates);
                              position.y +=CalculateArrayHeight(pseudoStates);

                              SerializedProperty propertyMask = property.FindPropertyRelative(p_PropertyMask);

                              position.height = 24;
                              EditorGUI.DrawRect(position, ETHelpers.ThemeAccent);
                              EditorGUI.LabelField(position, propertyMask.displayName, ETStyles.centeredLabel);
                              position.y += position.height + standardVerticalSpacing;

                              position.height = maskFieldStyle.fixedHeight;
                              propertyMask.intValue = (int) (USSProperties) EditorGUI.EnumFlagsField(position, (USSProperties) propertyMask.intValue, maskFieldStyle);

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
                        height += CalculateArrayHeight(property.FindPropertyRelative(p_ClassSelectors)) + CalculateArrayHeight(property.FindPropertyRelative(p_PseudoClasses));
                        height += spacing;
                        height += 24 + spacing;

                        SerializedProperty propertyMask = property.FindPropertyRelative(p_PropertyMask);
                        USSProperties mask = (USSProperties) propertyMask.enumValueFlag;
                        height += maskFieldStyle.fixedHeight
                              + StyleRule.PropertyMap.Where(entry => mask.HasFlag(entry.Property)).Select(item => property.FindPropertyRelative(item.Name)).Sum(item => EditorGUI.GetPropertyHeight(item) + spacing);
                  }
                  return height;
            }


            public static void ArrayField(Rect position, SerializedProperty property)
            {
                  position.height = 24F;
                  EditorGUI.DrawRect(position, ETHelpers. ThemeAccent);
                  EditorGUI.LabelField(position, property.displayName, ETStyles.centeredLabel);
                  position.y += position.height + EditorGUIUtility.standardVerticalSpacing; // - [ h:0 ]

                  for (int i = 0; i < property.arraySize; i++)
                  {
                        SerializedProperty element = property.GetArrayElementAtIndex(i);
                        position.height = EditorGUI.GetPropertyHeight(element);
                        EditorGUI.PropertyField(position, element, GUIContent.none, false);
                        position.y += position.height + EditorGUIUtility.standardVerticalSpacing; // - [ h:1 ]
                  }

                  position = EditorGUI.IndentedRect(position);
                  position.height = EditorStyles.miniButton.fixedHeight;
                  position.width /= 2F; // splitting buttons
                  int size = property.arraySize;
                  if (GUI.Button(position, EditorGUIUtility.IconContent("d_Toolbar Plus"), EditorStyles.miniButtonLeft) || size is 0) property.InsertArrayElementAtIndex(size++);
                  position.x += position.width; // append to right
                  if (GUI.Button(position, EditorGUIUtility.IconContent("d_Toolbar Minus"), EditorStyles.miniButtonRight) && size > 0) property.DeleteArrayElementAtIndex(--size);
            }
            public static float CalculateArrayHeight(SerializedProperty property)
            {
                  float height = 24F + EditorGUIUtility.standardVerticalSpacing; // - [ h:0 ]
                  for (int i = 0; i < property.arraySize; i++) height += EditorGUI.GetPropertyHeight(property.GetArrayElementAtIndex(i)) + EditorGUIUtility.standardVerticalSpacing; // - [ h:1 ]
                  height += EditorStyles.miniButton.fixedHeight + EditorGUIUtility.standardVerticalSpacing; // options button height + extra spacing
                  return height;
            }
      }
}