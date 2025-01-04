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
            private static readonly GUIStyle arrayHeaderLabelStyle = new(boldLabel) { alignment = TextAnchor.MiddleCenter };


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

                              using (new EditorGUI.IndentLevelScope(1))
                              {
                                    SerializedProperty propertyMask = property.FindPropertyRelative(p_PropertyMask);
                                    position.height = EditorGUI.GetPropertyHeight(propertyMask);
                                    _ = EditorGUI.PropertyField(position, propertyMask);
                                    position.y += position.height + standardVerticalSpacing;
                                    foreach (Properties _property in StyleRule.propertiesMap)
                                    {
                                          if (((Properties) propertyMask.enumValueFlag).HasFlag(_property))
                                          {
                                                SerializedProperty context = property.FindPropertyRelative(_property switch
                                                {
                                                      Properties.BackgroundImage => "BackgroundTexture",
                                                      Properties.BackgroundColor => "BackgroundColor",
                                                      Properties.BorderColor => "BorderColor",
                                                      Properties.BorderTopColor => "BorderTopColor",
                                                      Properties.BorderRightColor => "BorderRightColor",
                                                      Properties.BorderBottomColor => "BorderBottomColor",
                                                      Properties.BorderLeftColor => "BorderLeftColor",
                                                      Properties.Color => "TextColor",
                                                      Properties.BorderRadius => "BorderRadius",
                                                      Properties.BorderWidth => "BorderWidth",
                                                      _ => null
                                                });
                                                if (context != null)
                                                {
                                                      position.height = EditorGUI.GetPropertyHeight(context);
                                                      _ = EditorGUI.PropertyField(position, context, true);
                                                      position.y += position.height + standardVerticalSpacing;
                                                }
                                          }
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
                        height += EditorGUI.GetPropertyHeight(propertyMask);
                        foreach (Properties _property in StyleRule.propertiesMap)
                        {
                              if (((Properties) propertyMask.enumValueFlag).HasFlag(_property))
                              {
                                    SerializedProperty context = property.FindPropertyRelative(_property switch
                                    {
                                          Properties.BackgroundImage => "BackgroundTexture",
                                          Properties.BackgroundColor => "BackgroundColor",
                                          Properties.BorderColor => "BorderColor",
                                          Properties.BorderTopColor => "BorderTopColor",
                                          Properties.BorderRightColor => "BorderRightColor",
                                          Properties.BorderBottomColor => "BorderBottomColor",
                                          Properties.BorderLeftColor => "BorderLeftColor",
                                          Properties.Color => "TextColor",
                                          Properties.BorderRadius => "BorderRadius",
                                          Properties.BorderWidth => "BorderWidth",
                                          _ => null
                                    });
                                    if (context != null)
                                    {
                                          height += EditorGUI.GetPropertyHeight(context) + standardVerticalSpacing;
                                    }
                              }
                        }
                  }
                  return height;
            }

            private static void DrawArrayProperty(ref Rect position, SerializedProperty property)
            {
                  EditorGUI.LabelField(position, property.displayName, GUI.skin.box); // header
                  position.y += position.height + standardVerticalSpacing;

                  for (int size = property.arraySize, i = 0; i < size; i++) // elements
                  {
                        SerializedProperty context = property.GetArrayElementAtIndex(i);
                        Rect elementRect = new(position) { height = EditorGUI.GetPropertyHeight(context) };
                        _ = EditorGUI.PropertyField(elementRect, context, GUIContent.none, false);
                        position.y += elementRect.height + standardVerticalSpacing;
                  }

                  // options
                  Rect optionRect = new(position) { width = position.width * 0.5F, height = miniButton.fixedHeight };
                  if (GUI.Button(optionRect, "+", miniButtonLeft) || property.arraySize is 0) property.InsertArrayElementAtIndex(property.arraySize);
                  optionRect.x += optionRect.width;
                  if (GUI.Button(optionRect, "-", miniButtonRight) && property.arraySize > 1) property.DeleteArrayElementAtIndex(property.arraySize - 1);
                  position.y += optionRect.height + standardVerticalSpacing;

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