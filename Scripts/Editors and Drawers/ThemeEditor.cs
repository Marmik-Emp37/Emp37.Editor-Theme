using System.Collections.Generic;
using System.Linq;

using UnityEditor;

using UnityEngine;

namespace Emp37.ET
{
      [CustomEditor(typeof(Theme), true)]
      internal class ThemeEditor : Editor
      {
            private const float ButtonSize = 42F, SearchBarSize = 32F;

            private string searchQuery = string.Empty;

            private IEnumerable<(string Name, string Path)> selectors;


            private void OnEnable()
            {
                  selectors = (target as Theme).StyleRuleGroups.SelectMany((group, index) => group.StyleRules.SelectMany((rule, index) => rule.ClassSelectors.Select(item => (item, $"{group.Title} > Rule Element[{index}])"))));
            }

            public override void OnInspectorGUI()
            {
                  #region S E A R C H   B A R
                  EditorGUILayout.LabelField("Search Bar", EditorStyles.largeLabel);
                  GUILayout.BeginHorizontal(GUILayout.Height(SearchBarSize));
                  searchQuery = EditorGUILayout.TextField(searchQuery, ETStyles.largeTextField, GUILayout.Height(SearchBarSize));
                  using (new EditorGUI.DisabledGroupScope(searchQuery.Length is 0))
                  using (new ETHelpers.BackgroundColorScope(Color.red))
                  {
                        if (GUILayout.Button(EditorGUIUtility.IconContent("d_clear"), GUILayout.Width(SearchBarSize), GUILayout.ExpandHeight(true)))
                        {
                              searchQuery = string.Empty;
                        }
                  }
                  GUILayout.EndHorizontal();
                  EditorGUI.DrawRect(GUILayoutUtility.GetRect(default, 2F), ETStyles.ThemeTint);
                  #endregion

                  GUILayout.Space(EditorGUIUtility.standardVerticalSpacing);

                  if (searchQuery.Length == 0)
                  {
                        serializedObject.Update();
                        DrawPropertiesExcluding(serializedObject, "m_Script");
                        serializedObject.ApplyModifiedProperties();

                        GUILayout.Space(10F);
                        if (GUILayout.Button("Apply Theme", GUILayout.Height(ButtonSize)))
                        {
                              Theme target = base.target as Theme;
                              target.WriteTheme();
                        }

                        #region E X P A N D / C O L L A P S E   B U T T O N S
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
                        #endregion
                  }
                  else
                  {
                        var results = string.IsNullOrWhiteSpace(searchQuery) ? selectors : selectors.Where(entry => entry.Name.Contains(searchQuery, System.StringComparison.OrdinalIgnoreCase));
                        if (!results.Any())
                        {
                              EditorGUILayout.HelpBox("No matching properties found.", MessageType.Info);
                        }
                        else
                        {
                              foreach ((string name, string path) in results)
                              {
                                    GUILayout.BeginHorizontal();
                                    EditorGUILayout.LabelField(name);
                                    EditorGUILayout.LabelField(path, ETStyles.miniTextRight);
                                    GUILayout.EndHorizontal();
                              }
                        }
                  }
            }

            private void ExpandProperties(bool expand)
            {
                  SerializedProperty iterator = serializedObject.GetIterator();
                  while (iterator.NextVisible(true)) if (iterator.depth < 4) iterator.isExpanded = expand;
            }
      }
}