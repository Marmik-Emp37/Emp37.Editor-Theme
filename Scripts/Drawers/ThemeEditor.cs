using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEditorInternal;
using UnityEngine;

namespace Emp37.ET
{
      [CustomEditor(typeof(Theme), true)]
      internal class ThemeEditor : Editor
      {
            private const string DIRECTORY = "Assets/Editor/StyleSheets/Extensions", FILE_EXTENSION = ".uss";

            private const float ButtonSize = 42F;
            private const float SearchFieldSize = 32F;
            private const int MaxSearchResults = 60;

            private string queryText = string.Empty;
            private IEnumerable<(string Name, string Path, Action Action)> selectorHierarchy;

            private Theme Target => target as Theme;


            private void OnEnable()
            {
                  selectorHierarchy = Target.StyleRuleGroups
                        .SelectMany((styleGroup, groupIdx) => styleGroup.StyleRules
                              .SelectMany((style, styleIdx) => style.Selectors
                                    .Select(selector => (Name: selector, Path: $"Style Rule Group [{groupIdx}]: \"{styleGroup.Title}\" > Element [{styleIdx}]", Action: new Action(() =>
                                    {
                                          SerializedProperty
                                                groups = serializedObject.FindProperty(nameof(Target.StyleRuleGroups)),
                                                groupsElement = groups.GetArrayElementAtIndex(groupIdx),
                                                rules = groupsElement.FindPropertyRelative(nameof(styleGroup.StyleRules)).GetArrayElementAtIndex(styleIdx);

                                          ExpandProperties(false);
                                          groups.isExpanded = groupsElement.isExpanded = rules.isExpanded = true;

                                          queryText = string.Empty;
                                          GUIUtility.keyboardControl = default;
                                    })))));
            }
            public override void OnInspectorGUI()
            {
                  DrawSearchField();

                  GUILayout.Space(EditorGUIUtility.standardVerticalSpacing);

                  if (string.IsNullOrWhiteSpace(queryText))
                  {
                        serializedObject.Update();
                        DrawPropertiesExcluding(serializedObject, "m_Script");
                        serializedObject.ApplyModifiedProperties();
                        DrawOptions();
                  }
                  else
                  {
                        DrawSearchResults();
                  }
            }

            private void DrawSearchField()
            {
                  EditorGUILayout.LabelField("Search Selector", EditorStyles.largeLabel);
                  using (new GUILayout.HorizontalScope(GUILayout.Height(SearchFieldSize)))
                  {
                        queryText = EditorGUILayout.TextField(queryText, ETStyles.largeTextField, GUILayout.Height(SearchFieldSize)).Trim();

                        using (new EditorGUI.DisabledGroupScope(string.IsNullOrEmpty(queryText)))
                        using (new ETHelpers.BackgroundColorScope(Color.red))
                        {
                              if (GUILayout.Button(ETStyles.Clear, GUILayout.Width(SearchFieldSize), GUILayout.ExpandHeight(true)))
                              {
                                    queryText = string.Empty;
                                    GUIUtility.keyboardControl = default;
                              }
                        }
                  }
                  EditorGUI.DrawRect(GUILayoutUtility.GetRect(default, 2F), ETStyles.BaseTone);
            }
            private void DrawOptions()
            {
                  GUILayout.Space(10F);
                  if (GUILayout.Button("Apply Theme", GUILayout.Height(ButtonSize)))
                  {
                        Directory.CreateDirectory(DIRECTORY);

                        string path = Path.Combine(DIRECTORY, Target.ThemeType + FILE_EXTENSION);
                        File.WriteAllText(path, Target.ToString());
                        AssetDatabase.Refresh();

                        if ((Target.ThemeType == Theme.Type.Dark) ^ EditorGUIUtility.isProSkin)
                        {
                              InternalEditorUtility.SwitchSkinAndRepaintAllViews();
                        }
                        else
                        {
                              InternalEditorUtility.RepaintAllViews();
                              if (Target.RecompileOnApply)
                              {
                                    CompilationPipeline.RequestScriptCompilation();
                              }
                        }

                        GUI.skin.settings.selectionColor = Target.SelectionColor;
                  }
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
            private void DrawSearchResults()
            {
                  IEnumerable<(string, string, Action)> results = selectorHierarchy.Where(entry => entry.Name.Contains(queryText, StringComparison.OrdinalIgnoreCase)).Take(MaxSearchResults);
                  int count = results.Count();
                  if (count > 0)
                  {
                        using (ETHelpers.BackgroundColorScope scope = new())
                        {
                              int i = 0;
                              foreach ((string name, string path, Action action) in results)
                              {
                                    scope.BackgroundColor = ((i++ & 1) == 0) ? ETStyles.BaseTone : ETStyles.AccentTone;

                                    using (new EditorGUILayout.HorizontalScope())
                                    {
                                          using (new EditorGUILayout.VerticalScope())
                                          {
                                                EditorGUILayout.LabelField(name, ETStyles.largeHelpBox);
                                                EditorGUILayout.LabelField(path, EditorStyles.helpBox);
                                          }
                                          if (GUILayout.Button(ETStyles.INFoldout, GUILayout.Width(25F), GUILayout.ExpandHeight(true)))
                                          {
                                                action?.Invoke();
                                          }
                                    }
                              }
                        }
                        if (count == MaxSearchResults)
                        {
                              EditorGUILayout.HelpBox($"Showing the first {MaxSearchResults} results out of {selectorHierarchy.Count()}. More matches may exist, refine your search for precise results.", MessageType.Info);
                        }
                  }
                  else
                  {
                        EditorGUILayout.HelpBox("No matching properties found.", MessageType.Info);
                  }
            }
            private void ExpandProperties(bool expand)
            {
                  SerializedProperty iterator = serializedObject.GetIterator();
                  while (iterator.NextVisible(true)) if (iterator.depth < 4) iterator.isExpanded = expand;
            }
      }
}