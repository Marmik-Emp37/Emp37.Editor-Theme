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

            private const float HeaderHeight = 21F;

            private static readonly GUIStyle expandToggleStyle = new(foldoutHeader) { fontStyle = FontStyle.Normal, fixedHeight = HeaderHeight };

            public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
            {
                  using (new EditorGUI.PropertyScope(position, label, property))
                  {
                        position.height = HeaderHeight;
                        if (property.isExpanded = GUI.Toggle(position, property.isExpanded, label, expandToggleStyle))
                        {
                              position.y += position.height + standardVerticalSpacing;

                              position.y = GUIHelpers.ArrayField(position, property.FindPropertyRelative(p_Selectors));
                              position.y = GUIHelpers.ArrayField(position, property.FindPropertyRelative(p_PseudoStates));

                              using (new EditorGUI.IndentLevelScope(2))
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
                  float height = HeaderHeight + standardVerticalSpacing;
                  if (property.isExpanded)
                  {
                        height += GUIHelpers.GetArrayHeight(property.FindPropertyRelative(p_Selectors), HeaderHeight) + GUIHelpers.GetArrayHeight(property.FindPropertyRelative(p_PseudoStates), HeaderHeight);
                        height += standardVerticalSpacing;

                        SerializedProperty propertyMask = property.FindPropertyRelative(p_PropertyMask);
                        Properties mask = (Properties) propertyMask.enumValueFlag;
                        height += EditorGUI.GetPropertyHeight(propertyMask)
                              + StyleRule.PropertiesMap.Where(entry => mask.HasFlag(entry.Key)).Select(item => property.FindPropertyRelative(item.Value)).Sum(item => EditorGUI.GetPropertyHeight(item) + standardVerticalSpacing);
                  }
                  return height;
            }
      }
}