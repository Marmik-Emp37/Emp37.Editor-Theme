using System;
using System.Linq;

using UnityEditor;
using static UnityEditor.EditorGUIUtility;

using UnityEngine;
using UnityEngine.UIElements;

namespace Emp37.ET
{
      // @r: == Redeem
      [CustomPropertyDrawer(typeof(StyleRule))]
      internal class EditorStyleDrawer : PropertyDrawer
      {
            private const string p_Selectors = "ClassSelectors";
            private const string p_PseudoClasses = "PseudoClasses";
            private const string p_Mask = "PropertyMask";

            private const float BaseHeight = 21F, SectionTitleHeight = 24F;

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
                  Rect headerRect = new(position) { height = SectionTitleHeight };
                  ETStyles.DrawHeaderRect(headerRect, property.displayName);

                  position.y += headerRect.height + standardVerticalSpacing; // - Header

                  int count = property.arraySize;
                  for (int i = 0; i < count; i++)
                  {
                        SerializedProperty element = property.GetArrayElementAtIndex(i);
                        Rect elementRect = new(position) { height = EditorGUI.GetPropertyHeight(element, false) };
                        EditorGUI.PropertyField(elementRect, element, GUIContent.none, false);
                        position.y += elementRect.height + ETHelpers.Spacing; // - Elements
                  }

                  Rect buttonRect = new(position) { size = new(x: position.width * 0.5F, y: EditorStyles.miniButton.fixedHeight) };
                  if (GUI.Button(buttonRect, IconContent("d_Toolbar Plus"), EditorStyles.miniButtonLeft) || count == 0)
                  {
                        property.InsertArrayElementAtIndex(count++);
                  }
                  buttonRect.x += buttonRect.width;
                  if (GUI.Button(buttonRect, IconContent("d_Toolbar Minus"), EditorStyles.miniButtonRight) && count > 0)
                  {
                        property.DeleteArrayElementAtIndex(--count);
                  }

                  position.y += buttonRect.height + ETHelpers.Spacing; // - Footer
                  return position;
            }
            private static Rect DrawAttributes(Rect position, SerializedProperty property)
            {
                  SerializedProperty mask = property.FindPropertyRelative(p_Mask);

                  using (EditorGUI.ChangeCheckScope scope = new())
                  {
                        Rect maskRect = new(position) { height = propertyMaskField.fixedHeight };
                        Enum value = EditorGUI.EnumFlagsField(maskRect, (StyleAttributes) mask.enumValueFlag, propertyMaskField);
                        if (scope.changed)
                        {
                              mask.enumValueFlag = (int) (StyleAttributes) value;
                        }
                        position.y += maskRect.height + standardVerticalSpacing;
                  }

                  using (new EditorGUI.IndentLevelScope(1))
                  {
                        foreach ((StyleAttributes flag, string name) in StyleRule.PropertyMap.Where(entry => ((StyleAttributes) mask.enumValueFlag).HasFlag(entry.Flag)))
                        {
                              SerializedProperty context = property.FindPropertyRelative(name);
                              Rect propertyRect = new(position) { height = EditorGUI.GetPropertyHeight(context) };
                              _ = EditorGUI.PropertyField(propertyRect, context, true);
                              position.y += propertyRect.height + standardVerticalSpacing;
                        }
                  }

                  return position;
            }
            public static float CalculateArrayPropertyHeight(SerializedProperty property)
            {
                  float value = SectionTitleHeight + EditorStyles.miniButton.fixedHeight + standardVerticalSpacing + ETHelpers.Spacing;
                  for (int i = property.arraySize - 1; i >= 0; i--)
                  {
                        value += EditorGUI.GetPropertyHeight(property.GetArrayElementAtIndex(i)) + standardVerticalSpacing;
                  }
                  return value;
            }

            public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
            {
                  using (new EditorGUI.PropertyScope(position, label, property))
                  {
                        position.height = BaseHeight;
                        if (property.isExpanded = GUI.Toggle(position, property.isExpanded, label, propertyExpandToggle))
                        {
                              position.y += position.height + standardVerticalSpacing; // - H : Header

                              position = DrawArrayProperty(position, property.FindPropertyRelative(p_Selectors)); // - H : Array-Selectors
                              position = DrawArrayProperty(position, property.FindPropertyRelative(p_PseudoClasses)); // - H : Array-PseudoClasses

                              position.height = SectionTitleHeight;
                              ETStyles.DrawHeaderRect(position, property.displayName); // - H : Attributes 

                              position.y += position.height + standardVerticalSpacing; // - H : Mask Header

                              _ = DrawAttributes(position, property); // - H : Attributes + H : Property Mask
                        }
                  }
            }
            public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
            {
                  if (property.isExpanded)
                  {
                        StyleAttributes activeFlags = (StyleAttributes) property.FindPropertyRelative(p_Mask).enumValueFlag;
                        return BaseHeight + standardVerticalSpacing /*- @r >> H : Header*/
                              + CalculateArrayPropertyHeight(property.FindPropertyRelative(p_Selectors)) + CalculateArrayPropertyHeight(property.FindPropertyRelative(p_PseudoClasses)) /*- @r >> H : Arrays*/
                              + SectionTitleHeight + standardVerticalSpacing /*- @r >> H : Mask Header*/
                              + StyleRule.PropertyMap.Where(entry => activeFlags.HasFlag(entry.Flag)).Sum(item => EditorGUI.GetPropertyHeight(property.FindPropertyRelative(item.Name)) + standardVerticalSpacing) /*- @r >> H : Attributes*/
                              + propertyMaskField.fixedHeight; /*- @r >> H : Property Mask*/
                  }
                  else
                  {
                        return BaseHeight;
                  }
            }
      }
}