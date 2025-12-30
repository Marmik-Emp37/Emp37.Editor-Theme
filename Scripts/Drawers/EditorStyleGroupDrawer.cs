using UnityEditor;
using UnityEngine;

namespace Emp37.ET
{
      using static ETHelpers;

      // @r: == Redeem
      [CustomPropertyDrawer(typeof(StyleRuleGroup))]
      internal class EditorStyleGroupDrawer : PropertyDrawer
      {
            private const string p_Enabled = "<" + nameof(StyleRuleGroup.Enabled) + ">k__BackingField";
            private const string p_Title = nameof(StyleRuleGroup.Title);
            private const string p_StyleRules = nameof(StyleRuleGroup.StyleRules);
            private const string control_TargetTitle = nameof(StyleRuleGroup) + ":Control.Title";

            private const float HeaderSize = 32F;
            private const float FooterHeight = 24F;


            public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
            {
                  _ = EditorGUI.BeginProperty(position, label, property);

                  position.height = HeaderSize;
                  DrawHeader(position, property);

                  if (property.isExpanded)
                  {
                        position.y += position.height + Spacing; // - H : Header

                        SerializedProperty group = property.FindPropertyRelative(p_StyleRules);
                        position = DrawArrayElements(position, group); // - H : Elements

                        position.height = FooterHeight;
                        DrawFooter(position, group);

                        position.y += position.height + Spacing;  // - H : Footer

                        position.height = Spacing; // - H : Separator
                        EditorGUI.DrawRect(position, ETStyles.AccentTone);
                  }

                  EditorGUI.EndProperty();
            }
            public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
            {
                  if (property.isExpanded)
                  {
                        float value = HeaderSize + Spacing + /*- @r >> H : Header*/ FooterHeight + Spacing + /*- @r >> H : Footer*/ Spacing /*- @r >> H : Separator*/;

                        SerializedProperty group = property.FindPropertyRelative(p_StyleRules);
                        for (int i = group.arraySize - 1; i >= 0; i--)
                        {
                              value += EditorGUI.GetPropertyHeight(group.GetArrayElementAtIndex(i)) + Spacing; // - @r >> H : Elements
                        }

                        return value;
                  }
                  return HeaderSize;
            }

            private static void DrawHeader(Rect position, SerializedProperty property)
            {
                  Rect original = position;

                  // Background
                  EditorGUI.DrawRect(position, ETStyles.AccentTone);
                  position.width = 3F;
                  EditorGUI.DrawRect(position, ETStyles.BaseTone);

                  position.x += position.width + Spacing;

                  // Expand Toggle
                  position.width = 20F;
                  property.isExpanded = EditorGUI.Toggle(position, property.isExpanded, EditorStyles.foldout);

                  position.x += position.width;

                  // Enabled Property
                  SerializedProperty enabled = property.FindPropertyRelative(p_Enabled);
                  enabled.boolValue = EditorGUI.Toggle(position, enabled.boolValue);

                  position.x += position.width;

                  // Title
                  position.width = original.width - (position.x - original.x) - Spacing - HeaderSize; // - [ W : Spacing & W : Menu Button ]
                  GUI.SetNextControlName(control_TargetTitle);
                  SerializedProperty title = property.FindPropertyRelative(p_Title);
                  if (title.isExpanded)
                  {
                        using EditorGUI.ChangeCheckScope check = new();
                        string text = EditorGUI.DelayedTextField(position, title.stringValue, ETStyles.largeTextField);
                        if (check.changed) { title.stringValue = text; title.isExpanded = false; }
                  }
                  else
                  {
                        EditorGUI.LabelField(position, title.stringValue, ETStyles.largeText);
                  }

                  position.x += position.width + Spacing; // - W : Spacing

                  // Rename Title Button
                  position.width = HeaderSize; // - W : Menu Button
                  if (GUI.Button(position, ETStyles.Menu) && (title.isExpanded = !title.isExpanded))
                  {
                        GUI.FocusControl(control_TargetTitle);
                  }
            }
            private static Rect DrawArrayElements(Rect position, SerializedProperty property)
            {
                  for (int size = property.arraySize, i = 0; i < size; i++)
                  {
                        SerializedProperty element = property.GetArrayElementAtIndex(i);

                        Rect elementPosition = new(position) { height = EditorGUI.GetPropertyHeight(element, true) };
                        EditorGUI.PropertyField(elementPosition, element, true);

                        position.y += elementPosition.height + Spacing;
                  }
                  return position;
            }
            private static void DrawFooter(Rect position, SerializedProperty property)
            {
                  position.width *= 0.5F;
                  if (GUI.Button(position, "Add", GUI.skin.button))
                  {
                        property.InsertArrayElementAtIndex(property.arraySize);
                  }

                  position.x += position.width;

                  if (GUI.Button(position, "Remove", GUI.skin.button) && property.arraySize > 0)
                  {
                        property.DeleteArrayElementAtIndex(property.arraySize - 1);
                  }
            }
      }
}