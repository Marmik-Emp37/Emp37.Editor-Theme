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
            public const string DIRECTORY = "Assets/Editor/StyleSheets/Extensions";
            public const string FILE_EXTENSION = ".uss";

            private const float h_ApplyButton = 42F;

            public override void OnInspectorGUI()
            {
                  base.OnInspectorGUI();

                  GUILayout.Space(10F);
                  if (GUILayout.Button("Apply Theme", GUILayout.Height(h_ApplyButton)))
                  {
                        ApplyTheme();
                  }
                  DrawExpandCollapseButtons();
            }

            private void ApplyTheme()
            {
                  Theme target = base.target as Theme;

                  if (!Directory.Exists(DIRECTORY)) Directory.CreateDirectory(DIRECTORY);

                  string path = Path.Combine(DIRECTORY, target.ThemeType + FILE_EXTENSION);
                  File.WriteAllText(path, target.ToString());
                  AssetDatabase.Refresh();

                  if (ShouldSwitchSkin(target.ThemeType))
                  {
                        InternalEditorUtility.SwitchSkinAndRepaintAllViews();
                  }
                  else
                  {
                        InternalEditorUtility.RepaintAllViews();
                        if (!target.InstantApply)
                        {
                              CompilationPipeline.RequestScriptCompilation();
                        }
                  }
            }
            private void DrawExpandCollapseButtons()
            {
                  using (new EditorGUILayout.HorizontalScope())
                  {
                        if (GUILayout.Button("Expand", EditorStyles.miniButtonLeft))
                        {
                              ExpandProperties(true);
                        }
                        if (GUILayout.Button("Collapse", EditorStyles.miniButtonRight))
                        {
                              ExpandProperties(false);
                        }
                  }
            }
            private void ExpandProperties(bool expand)
            {
                  SerializedProperty iterator = serializedObject.GetIterator();
                  while (iterator.NextVisible(true) && iterator.depth < 4) iterator.isExpanded = expand;
            }
            private bool ShouldSwitchSkin(Theme.Type themeType) => (themeType == Theme.Type.Dark) ^ EditorGUIUtility.isProSkin;
      }
}