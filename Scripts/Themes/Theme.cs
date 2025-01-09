using System.Collections.Generic;
using System.IO;

using UnityEditor;
using UnityEditor.Compilation;

using UnityEditorInternal;

using UnityEngine;

namespace Emp37.ET
{
      public abstract class Theme : ScriptableObject
      {
            public const string DIRECTORY = "Assets/Editor/StyleSheets/Extensions", FILE_EXTENSION = ".uss";

            public enum Type { Light, Dark }

            public StyleRuleGroup[] StyleRuleGroups;
            [field: Tooltip("Enable to preview changes immediately as they are applied.\n\n<b>Note:</b> Some changes may require a domain reload to take full effect.")]
            [SerializeField] private bool instantApply;

            public abstract Type ThemeType { get; }

            private IEnumerable<string> WriteStyleGroups
            {
                  get
                  {
                        foreach (StyleRuleGroup group in StyleRuleGroups)
                        {
                              string value = group.ToString();
                              if (string.IsNullOrEmpty(value)) continue;
                              yield return $"/*---[{group.Title}]---*/\n{value}";
                        }
                  }
            }


            /// <summary>
            /// Generates a finalised USS code representation for this theme.
            /// </summary>
            public sealed override string ToString() => $"/*-----\n[ Theme: {ThemeType}-{name} ]\n-----*/\n\n{string.Join("\n\n", WriteStyleGroups)}";

            public void WriteTheme()
            {
                  if (!Directory.Exists(DIRECTORY)) Directory.CreateDirectory(DIRECTORY);

                  string path = Path.Combine(DIRECTORY, ThemeType + FILE_EXTENSION);
                  File.WriteAllText(path, ToString());
                  AssetDatabase.Refresh();

                  if (ShouldSwitchSkin(ThemeType))
                  {
                        InternalEditorUtility.SwitchSkinAndRepaintAllViews();
                  }
                  else
                  {
                        InternalEditorUtility.RepaintAllViews();
                        if (!instantApply)
                        {
                              CompilationPipeline.RequestScriptCompilation();
                        }
                  }
            }
            private bool ShouldSwitchSkin(Type themeType) => (themeType == Type.Dark) ^ EditorGUIUtility.isProSkin;
      }
}