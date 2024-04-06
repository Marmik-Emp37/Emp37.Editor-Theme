using System.IO;

using UnityEditor;
using UnityEditor.Compilation;

using UnityEditorInternal;

using UnityEngine;

namespace Emp37.ET
{
      [CustomEditor(typeof(Theme), true)]
      internal class ThemeEditor : Editor
      {
            public const string DIRECTORY = "Assets/Editor/StyleSheets/Extensions", FILE_EXTENSION = ".uss";

            private const float ApplyButtonHeight = 42F;


            public override void OnInspectorGUI()
            {
                  base.OnInspectorGUI();

                  GUILayout.Space(10F);
                  if (GUILayout.Button("Apply Theme", GUILayout.Height(ApplyButtonHeight)))
                  {
                        if (!Directory.Exists(DIRECTORY)) Directory.CreateDirectory(DIRECTORY);

                        Theme target = base.target as Theme;
                        string path = Path.Combine(DIRECTORY, target.ThemeType + FILE_EXTENSION);
                        File.WriteAllText(path, target.ToString());
                        AssetDatabase.Refresh();
                        if (ValidateSkinSwitch(target.ThemeType))
                        {
                              InternalEditorUtility.SwitchSkinAndRepaintAllViews();
                        }
                        else
                        {
                              InternalEditorUtility.RepaintAllViews();
                              if (!target.InstantApply) CompilationPipeline.RequestScriptCompilation();
                        }
                  }

                  using (new EditorGUILayout.HorizontalScope())
                  {
                        if (GUILayout.Button("Expand", EditorStyles.miniButtonLeft)) ExpandProperties(true);
                        if (GUILayout.Button("Collapse", EditorStyles.miniButtonRight)) ExpandProperties(false);
                  }
            }

            private void ExpandProperties(bool value)
            {
                  using SerializedProperty iterator = serializedObject.GetIterator();
                  while (iterator.NextVisible(true))
                  {
                        if (iterator.depth < 4) iterator.isExpanded = value;
                  }
            }
            private bool ValidateSkinSwitch(Theme.Type themeType) => (themeType == Theme.Type.Dark) ^ EditorGUIUtility.isProSkin;
      }
}