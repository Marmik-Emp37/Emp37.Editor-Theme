using System;
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
            private const float SearchFieldHeight = 32F;
            private const float SearchFieldButtonWidth = SearchFieldHeight;

            private const int MaxSearchResults = 60;
            private string queryText = string.Empty;
            private IEnumerable<(string Name, string Path, Action Action)> selectorHierarchy;

            private Theme Target => target as Theme;

            private void OnEnable()
            {
                  selectorHierarchy = Target.StyleRuleGroups.SelectMany((styleGroup, groupIdx) => styleGroup.StyleRules.SelectMany((style, styleIdx) => style.ClassSelectors.Select(selector =>
                  (Name: selector, Path: $"Group[{groupIdx}]: {styleGroup.Title.Truncate(20)} > Element[{styleIdx}]", Action: new Action(() =>
                  {
                        ExpandProperties(false);

                        SerializedProperty styleRuleGroups = serializedObject.FindProperty(nameof(Theme.StyleRuleGroups));
                        if (styleRuleGroups == null) return;

                        SerializedProperty group = styleRuleGroups.GetArrayElementAtIndex(groupIdx);
                        if (group == null) return;

                        SerializedProperty style = group.FindPropertyRelative(nameof(StyleRuleGroup.StyleRules))?.GetArrayElementAtIndex(styleIdx);
                        if (style == null) return;

                        styleRuleGroups.isExpanded = group.isExpanded = style.isExpanded = true;
                        GUIUtility.keyboardControl = 0;
                        queryText = string.Empty;
                  })))));
            }

            private void DrawSearchField()
            {
                  EditorGUILayout.LabelField("Search Selector", EditorStyles.largeLabel);
                  using (new GUILayout.HorizontalScope(GUILayout.Height(SearchFieldHeight)))
                  {
                        queryText = EditorGUILayout.TextField(queryText, ETStyles.largeTextField, GUILayout.Height(SearchFieldHeight)).Trim();

                        using (new EditorGUI.DisabledGroupScope(string.IsNullOrEmpty(queryText)))
                        using (new ETHelpers.BackgroundColorScope(Color.red))
                        {
                              if (GUILayout.Button(ETStyles.Clear, GUILayout.Width(SearchFieldButtonWidth), GUILayout.ExpandHeight(true)))
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
                                    scope.BackgroundColor = ((i++ & 1) == 0) ? ETStyles.ThemeTint : ETStyles.ThemeAccent;

                                    using (new GUILayout.HorizontalScope())
                                    {
                                          EditorGUILayout.LabelField(name, ETStyles.largeHelpBox);
                                          EditorGUI.LabelField(GUILayoutUtility.GetLastRect(), path + ' ', ETStyles.miniLabelRight);
                                          if (GUILayout.Button(ETStyles.INFoldout, GUILayout.Width(25F), GUILayout.ExpandHeight(true)))
                                          {
                                                action?.Invoke();
                                          }
                                    }
                              }
                        }
                        if (count == MaxSearchResults)
                        {
                              EditorGUILayout.HelpBox($"Displaying the first {MaxSearchResults} results out of {selectorHierarchy.Count()}.\nYour search might match more items. Try refining your query to see specific results.", MessageType.Info);
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
                        DrawSearchResults();
                  }
            }

            private void ExpandProperties(bool expand)
            {
                  SerializedProperty iterator = serializedObject.GetIterator();
                  while (iterator.NextVisible(true)) if (iterator.depth < 4) iterator.isExpanded = expand;
            }
      }
}