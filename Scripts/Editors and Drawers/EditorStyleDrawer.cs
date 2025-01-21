using System;
using System.Linq;

using UnityEditor;
using static UnityEditor.EditorGUIUtility;

using UnityEngine;

namespace Emp37.ET
{
      // @r: == Redeem
      [CustomPropertyDrawer(typeof(StyleRule))]
      internal class EditorStyleDrawer : PropertyDrawer
      {
            private const string p_Selectors = nameof(StyleRule.ClassSelectors);
            private const string p_PseudoClasses = nameof(StyleRule.PseudoClasses);
            private const string p_Mask = nameof(StyleRule.PropertyMask);

            private const float BaseHeight = 21F;
            private const float TitleSectionHeight = 24F;
            private const float RedLineHeight = 2F;

            private static readonly GUIStyle propertyExpandToggle = new(EditorStyles.foldoutHeader)
            {
                  fontStyle = FontStyle.Normal,
                  fixedHeight = BaseHeight
            };
            private static readonly GUIStyle propertyMaskField = new(EditorStyles.layerMaskField)
            {
                  alignment = TextAnchor.MiddleCenter,
                  fontStyle = FontStyle.Italic,
                  fixedHeight = 30F
            };


            private static Rect DrawArrayProperty(Rect position, SerializedProperty property)
            {
                  Rect original = position;

                  position.height = TitleSectionHeight;
                  ETStyles.DrawAccentTitle(position, property.displayName);

                  position.y += position.height + standardVerticalSpacing;

                  int count = property.arraySize;
                  for (int i = 0; i < count; i++)
                  {
                        SerializedProperty element = property.GetArrayElementAtIndex(i);

                        position.height = singleLineHeight;
                        EditorGUI.PropertyField(position, element, GUIContent.none, false);

                        position.y += position.height + standardVerticalSpacing;
                  }

                  Rect buttonRect = new(position) { size = new(x: position.width * 0.5F, y: EditorStyles.miniButton.fixedHeight) };
                  if (GUI.Button(buttonRect, ETStyles.BoldPlus, EditorStyles.miniButtonLeft) || count == 0)
                  {
                        property.InsertArrayElementAtIndex(count++);
                  }
                  buttonRect.x += buttonRect.width;
                  if (GUI.Button(buttonRect, ETStyles.BoldMinus, EditorStyles.miniButtonRight) && count > 0)
                  {
                        property.DeleteArrayElementAtIndex(--count);
                  }

                  position.y += buttonRect.height + standardVerticalSpacing;

                  return new(original) { y = position.y };
            }
            private static Rect DrawAttributes(Rect position, SerializedProperty property, StyleAttributes flags)
            {
                  Rect original = position;
                  using (new EditorGUI.IndentLevelScope(1))
                  {
                        if (flags is 0)
                        {
                              position.height = RedLineHeight;
                              EditorGUI.DrawRect(ETHelpers.Inset(position, 40F), Color.red);
                              position.y += position.height; // - [ H ] : Red Line
                        }
                        else
                        {
                              foreach ((_, string name) in StyleRule.PropertyMap.Where(entry => flags.HasFlag(entry.Flag)))
                              {
                                    SerializedProperty attribute = property.FindPropertyRelative(name);
                                    position.height = EditorGUI.GetPropertyHeight(attribute);
                                    _ = EditorGUI.PropertyField(position, attribute, true);
                                    position.y += position.height + standardVerticalSpacing; // - [ H ] : Attributes
                              }
                        }
                  }
                  return new(original) { y = position.y };
            }

            public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
            {
                  using EditorGUI.PropertyScope propertyScope = new(position, label, property);

                  position.height = BaseHeight;
                  if (property.isExpanded = GUI.Toggle(position, property.isExpanded, label, propertyExpandToggle))
                  {
                        position.y += position.height + standardVerticalSpacing; // - H : Header

                        position = DrawArrayProperty(position, property.FindPropertyRelative(p_Selectors));
                        position = DrawArrayProperty(position, property.FindPropertyRelative(p_PseudoClasses));


                        SerializedProperty mask = property.FindPropertyRelative(p_Mask);

                        position.height = TitleSectionHeight;
                        ETStyles.DrawAccentTitle(position, mask.displayName);

                        position.y += position.height + standardVerticalSpacing; // - H : Mask Title

                        using (EditorGUI.ChangeCheckScope check = new())
                        {
                              position.height = propertyMaskField.fixedHeight;
                              Enum value = EditorGUI.EnumFlagsField(position, (StyleAttributes) mask.enumValueFlag, propertyMaskField);
                              if (check.changed)
                              {
                                    mask.enumValueFlag = (int) (StyleAttributes) value;
                              }
                              position.y += position.height + standardVerticalSpacing; // - H : Property Mask
                        }

                        _ = DrawAttributes(position, property, (StyleAttributes) mask.enumValueFlag); // - H : Attributes
                  }
            }
            public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
            {
                  if (property.isExpanded)
                  {
                        float value = BaseHeight + standardVerticalSpacing;   // @r >> H : Header

                        value += CalculateArrayPropertyHeight(property.FindPropertyRelative(p_Selectors));
                        value += CalculateArrayPropertyHeight(property.FindPropertyRelative(p_PseudoClasses));

                        value += TitleSectionHeight + propertyMaskField.fixedHeight + 2F * standardVerticalSpacing; // @r >> H : Mask Title + H : Property Mask

                        // @r >> H : Attributes
                        StyleAttributes flags = (StyleAttributes) property.FindPropertyRelative(p_Mask).enumValueFlag;
                        value += flags is 0 ? RedLineHeight :
                              StyleRule.PropertyMap.Where(entry => flags.HasFlag(entry.Flag)).Sum(entry => EditorGUI.GetPropertyHeight(property.FindPropertyRelative(entry.Name)) + standardVerticalSpacing);

                        return value;
                  }
                  else
                  {
                        return BaseHeight;
                  }
            }

            private static float CalculateArrayPropertyHeight(SerializedProperty property)
            {
                  float height = TitleSectionHeight + 2F * standardVerticalSpacing + EditorStyles.miniButton.fixedHeight;
                  height += property.arraySize * (singleLineHeight + standardVerticalSpacing);
                  //for (int i = 0; i < property.arraySize; i++)
                  //{
                  //      height += EditorGUI.GetPropertyHeight(property.GetArrayElementAtIndex(i)) + standardVerticalSpacing;
                  //}
                  return height;
            }
      }
}