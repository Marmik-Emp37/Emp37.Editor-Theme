using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

using UnityEditor;
using UnityEditor.Compilation;

using UnityEditorInternal;

using UnityEngine;

namespace Emp37.ET
{
      public abstract class Theme : ScriptableObject
      {
            public enum Type { Light, Dark }

            public const string DIRECTORY = "Assets/Editor/StyleSheets/Extensions", FILE_EXTENSION = ".uss";
            private const string Key = "Emp37.ET.Theme.appliedValue";

            public StyleRuleGroup[] StyleRuleGroups;
            [SerializeField] private Color selectionColor;
            [field: Tooltip("Disable to preview changes immediately as they are applied.\n\n<b>Note: </b>Some changes may require a domain reload to take full effect.")]
            [SerializeField] private bool recompileOnApply;

            protected abstract Type ThemeType { get; }

            private IEnumerable<string> WriteStyleGroups => from @group in StyleRuleGroups let value = @group.ToString() where !string.IsNullOrEmpty(value) select value;


            private void Awake()
            {
                  if (name == EditorPrefs.GetString(Key))
                  {
                        ApplySettings();
                  }
            }

            [ContextMenu("Sort/Title")]
            private void Sort()
            {
                  Undo.RecordObject(this, $"Sort {nameof(StyleRuleGroups)} by Title");
                  Array.Sort(StyleRuleGroups, (a, b) => a.Title.CompareTo(b.Title));
            }

            public void WriteTheme()
            {
                  Directory.CreateDirectory(DIRECTORY);

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
                        if (recompileOnApply)
                        {
                              CompilationPipeline.RequestScriptCompilation();
                        }
                  }
                  ApplySettings();
            }
            private bool ShouldSwitchSkin(Type themeType) => (themeType == Type.Dark) ^ EditorGUIUtility.isProSkin;
            private void ApplySettings()
            {
                  GUI.skin.settings.selectionColor = selectionColor;
                  EditorPrefs.SetString(Key, name);
            }

            /// <summary>
            /// Generates a finalised USS code representation for this theme.
            /// </summary>
            public sealed override string ToString() => $"/*----------\n[ Theme: {ThemeType}-{name} ]\n----------*/\n\n{string.Join("\n\n", WriteStyleGroups)}";
      }
}