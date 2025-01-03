using System.IO;

using UnityEditor;
using UnityEditor.Compilation;

using UnityEditorInternal;

using UnityEngine;

namespace Emp37.ET
{
      [CustomEditor(typeof(Theme), true)]
      internal class BaseThemeEditor : Editor
      {
            private SerializedProperty arr_editorStyleGroup, bool_quickApply;

            private void OnEnable()
            {
                  arr_editorStyleGroup = serializedObject.FindProperty("StyleRules");
                  bool_quickApply = serializedObject.FindProperty("refresh");
            }
            public override void OnInspectorGUI()
            {
                  serializedObject.Update();
                  {
                        _ = EditorGUILayout.PropertyField(arr_editorStyleGroup);
                        _ = EditorGUILayout.PropertyField(bool_quickApply);
                  }
                  serializedObject.ApplyModifiedProperties();

                  #region W R I T E   O R   C R E A T E   C O M M O N   T H E M E
                  if (GUILayout.Button("Apply", GUILayout.Height(30F)))
                  {
                        if (!Directory.Exists(Theme.DIRECTORY)) /*>>*/ Directory.CreateDirectory(Theme.DIRECTORY);

                        var target = base.target as Theme;
                        var path = Path.Combine(Theme.DIRECTORY, target.FileName);
                        File.WriteAllText(path, target.ToString());
                        AssetDatabase.Refresh();

                        if (target.IsSkinInvalid)
                        {
                              InternalEditorUtility.SwitchSkinAndRepaintAllViews();
                              return;
                        }
                        InternalEditorUtility.RepaintAllViews();
                        if (target.RefreshOnApply) CompilationPipeline.RequestScriptCompilation();
                  }
                  #endregion

                  #region O P T I O N S
                  using (new GUILayout.HorizontalScope())
                  {
                        if (GUILayout.Button("Expand", EditorStyles.miniButtonLeft)) ExpandAll(true);
                        else
                        if (GUILayout.Button("Collapse", EditorStyles.miniButtonRight)) ExpandAll(false);
                  }
                  #endregion
            }
            private void ExpandAll(bool value)
            {
                  var iterator = serializedObject.GetIterator();

                  while (iterator.NextVisible(true)) iterator.isExpanded = value;
            }
      }
}