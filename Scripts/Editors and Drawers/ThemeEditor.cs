using System.Collections.Generic;
using System.Linq;

using UnityEditor;

using UnityEngine;

namespace Emp37.ET
{
      [CustomEditor(typeof(Theme), true)]
      internal class ThemeEditor : Editor
      {
            private const float ButtonSize = 42F;
            private const float SearchSectionSize = 32F;

            private const int MaxVisibleResults = 60;
            private string queryText = string.Empty;
            private IEnumerable<(string Name, string Path)> selectorHierarchy;

            private Theme Target => target as Theme;


            private void OnEnable()
            {
                  selectorHierarchy = Target.StyleRuleGroups
                        .SelectMany((styleGroup, groupIdx) => styleGroup.StyleRules.SelectMany((style, styleIdx) => style.ClassSelectors.Select(selector => (selector, $"Group[{groupIdx}]: \"{styleGroup.Title}\" > Rule[{styleIdx}]"))));
            }

            private void DrawSearchField()
            {
                  EditorGUILayout.LabelField("Search Selector", EditorStyles.largeLabel);
                  using (new GUILayout.HorizontalScope(GUILayout.Height(SearchSectionSize)))
                  {
                        queryText = EditorGUILayout.TextField(queryText, ETStyles.largeTextField, GUILayout.Height(SearchSectionSize)).Trim();

                        using (new EditorGUI.DisabledGroupScope(string.IsNullOrEmpty(queryText)))
                        using (new ETHelpers.BackgroundColorScope(Color.red))
                        {
                              if (GUILayout.Button(EditorGUIUtility.IconContent("d_clear"), GUILayout.Width(SearchSectionSize), GUILayout.ExpandHeight(true)))
                              {
                                    queryText = string.Empty;
                              }
                        }
                  }
                  EditorGUI.DrawRect(GUILayoutUtility.GetRect(default, 2F), ETStyles.ThemeTint);
            }
            private void DrawInspectorPanel()
            {
                  serializedObject.Update();
                  DrawPropertiesExcluding(serializedObject, "m_Script");
                  serializedObject.ApplyModifiedProperties();

                  GUILayout.Space(10F);
                  if (GUILayout.Button("Apply Theme", GUILayout.Height(ButtonSize)))
                  {
                        Target.WriteTheme();
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
            private void ShowSearchResults()
            {
                  List<(string Name, string Path)> results = selectorHierarchy.Where(entry => entry.Name.Contains(queryText, System.StringComparison.OrdinalIgnoreCase)).ToList();

                  if (results.Any())
                  {
                        int totalResults = results.Count;
                        using (ETHelpers.BackgroundColorScope scope = new())
                        {
                              for (int count = Mathf.Min(totalResults, MaxVisibleResults), i = 0; i < count; i++)
                              {
                                    (string name, string path) = results[i];
                                    scope.BackgroundColor = ((i & 1) == 0) ? ETStyles.ThemeTint : ETStyles.ThemeAccent;

                                    EditorGUILayout.LabelField(name, ETStyles.largeHelpBox);
                                    EditorGUI.LabelField(GUILayoutUtility.GetLastRect(), path, ETStyles.miniLabelRight);
                              }
                        }
                        if (totalResults > MaxVisibleResults)
                        {
                              EditorGUILayout.HelpBox($"Showing {MaxVisibleResults} of {totalResults} results. Refine your search for more specific results.", MessageType.Info);
                        }
                  }
                  else
                  {
                        EditorGUILayout.HelpBox("No matching properties found.", MessageType.Info);
                  }
            }

            public override void OnInspectorGUI()
            {
                  DrawSearchField();

                  GUILayout.Space(EditorGUIUtility.standardVerticalSpacing);

                  if (string.IsNullOrWhiteSpace(queryText))
                  {
                        DrawInspectorPanel();
                  }
                  else
                  {
                        ShowSearchResults();
                  }
            }

            private void ExpandProperties(bool expand)
            {
                  SerializedProperty iterator = serializedObject.GetIterator();
                  while (iterator.NextVisible(true)) if (iterator.depth < 4) iterator.isExpanded = expand;
            }
      }
}