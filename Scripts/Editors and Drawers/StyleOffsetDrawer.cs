using UnityEngine;

using UnityEditor;

namespace Emp37.ET
{
      internal class StyleOffsetDrawer : PropertyDrawer
      {
            private static readonly string[] p_Offsets = { "Top", "Right", "Bottom", "Left" };
            private const string p_UnitType = "UnitType";


            private Rect DrawIntegerField(Rect position, SerializedProperty property)
            {
                  const float labelRatio = 0.2F;

                  Rect current = position;
                  current.width = position.width * labelRatio;
                  EditorGUI.LabelField(current, property.displayName[0].ToString());

                  current.x += current.width;

                  current.width = position.width * (1F - labelRatio);
                  property.intValue = EditorGUI.IntField(current, property.intValue);

                  current.x += current.width;
                  return current;
            }

            public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
            {
                  label = EditorGUI.BeginProperty(position, label, property);



                  EditorGUI.EndProperty();
            }

            public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
            {
                  return EditorGUIUtility.singleLineHeight;
            }
      }
}