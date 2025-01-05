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

            private const float HeaderHeight = 21F, LineHeight = 2F;

            private static readonly GUIStyle expandToggleStyle = new(foldoutHeader) { fontStyle = FontStyle.Normal, fixedHeight = HeaderHeight };


            public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
            {
                  using (new EditorGUI.PropertyScope(position, label, property))
                  {
                        position.height = HeaderHeight;
                        if (property.isExpanded = GUI.Toggle(position, property.isExpanded, label, expandToggleStyle))
                        {
                              position.y += position.height + standardVerticalSpacing;

                              DrawArrayProperty(ref position, property.FindPropertyRelative(p_Selectors));
                              DrawArrayProperty(ref position, property.FindPropertyRelative(p_PseudoStates));

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
                        height += GetArrayPropertyHeight(property.FindPropertyRelative(p_Selectors)) + GetArrayPropertyHeight(property.FindPropertyRelative(p_PseudoStates));
                        height += standardVerticalSpacing;

                        SerializedProperty propertyMask = property.FindPropertyRelative(p_PropertyMask);
                        Properties mask = (Properties) propertyMask.enumValueFlag;
                        height += EditorGUI.GetPropertyHeight(propertyMask)
                              + StyleRule.PropertiesMap.Where(entry => mask.HasFlag(entry.Key)).Select(item => property.FindPropertyRelative(item.Value)).Sum(item => EditorGUI.GetPropertyHeight(item) + standardVerticalSpacing);
                  }
                  return height;
            }

            private static void DrawArrayProperty(ref Rect position, SerializedProperty property)
            {
                  EditorGUI.LabelField(position, property.displayName, GUI.skin.box); // header
                  position.y += position.height + standardVerticalSpacing;

                  Rect contentRect = position;
                  for (int size = property.arraySize, i = 0; i < size; i++) // elements
                  {
                        SerializedProperty context = property.GetArrayElementAtIndex(i);
                        contentRect.height = EditorGUI.GetPropertyHeight(context);
                        _ = EditorGUI.PropertyField(contentRect, context, GUIContent.none, false);
                        position.y = contentRect.y += contentRect.height + standardVerticalSpacing;
                  }

                  // options
                  contentRect.width *= 0.5F;
                  contentRect.height = miniButton.fixedHeight;
                  if (GUI.Button(contentRect, IconContent("d_Toolbar Plus"), miniButtonLeft) || property.arraySize is 0) property.InsertArrayElementAtIndex(property.arraySize);
                  contentRect.x += contentRect.width;
                  if (GUI.Button(contentRect, IconContent("d_Toolbar Minus"), miniButtonRight) && property.arraySize > 1) property.DeleteArrayElementAtIndex(property.arraySize - 1);
                  position.y += contentRect.height + standardVerticalSpacing;

                  EditorGUI.DrawRect(new(position) { height = LineHeight }, StyleHelpers.BackgroundTint.ChangeOpacity(0.25F)); // separator line
                  position.y += LineHeight + standardVerticalSpacing;
            }
            private static float GetArrayPropertyHeight(SerializedProperty property)
            {
                  return HeaderHeight + standardVerticalSpacing // header
                  + Enumerable.Range(0, property.arraySize).Sum(index => EditorGUI.GetPropertyHeight(property.GetArrayElementAtIndex(index)) + standardVerticalSpacing)
                  + miniButton.fixedHeight + standardVerticalSpacing // option buttons
                  + LineHeight + standardVerticalSpacing; // additional line
            }
      }
}