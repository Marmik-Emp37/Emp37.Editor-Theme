using System;
using System.IO;

using UnityEditor;
using UnityEditor.Compilation;
using UnityEditorInternal;

using UnityEngine;

namespace Emp37.ET
{
        using static EditorGUIUtility;

        [CustomEditor(typeof(Theme), true)]
        internal class ThemeEditor : Editor
        {
                private const string ExportDirectory = "Assets/Editor/StyleSheets/Extensions", FileExtension = ".uss";

                private const float SearchFieldHeight = 32F, ActionButtonSize = 42F;
                private static readonly GUIContent ApplyThemeLabel = new("Apply Theme"), ExpandLabel = new("Expand"), CollapseLabel = new("Collapse");

                private string searchQuery = string.Empty;

                private SerializedProperty property_StyleRuleGroups;

                private Theme Target => target as Theme;


                private void OnEnable()
                {
                        property_StyleRuleGroups = serializedObject.FindProperty(nameof(Theme.StyleRuleGroups));
                }

                public override void OnInspectorGUI()
                {
                        #region Search Field
                        EditorGUILayout.LabelField("Search Selectors", EditorStyles.largeLabel);
                        using (new EditorGUILayout.HorizontalScope(GUILayout.Height(SearchFieldHeight)))
                        {
                                searchQuery = EditorGUILayout.TextField(searchQuery, ETStyles.largeTextField, GUILayout.Height(SearchFieldHeight)).Trim();

                                using (new EditorGUI.DisabledGroupScope(string.IsNullOrEmpty(searchQuery)))
                                using (new ETHelpers.BackgroundColorScope(Color.red))
                                {
                                        if (GUILayout.Button(ETStyles.Clear, GUILayout.Width(SearchFieldHeight), GUILayout.ExpandHeight(true)))
                                        {
                                                ClearSearch();
                                        }
                                }
                        }
                        #endregion

                        if (string.IsNullOrWhiteSpace(searchQuery))
                        {
                                serializedObject.Update();
                                DrawPropertiesExcluding(serializedObject, "m_Script");
                                serializedObject.ApplyModifiedProperties();

                                EditorGUILayout.Space(10F);
                                if (GUILayout.Button(ApplyThemeLabel, GUILayout.Height(ActionButtonSize)))
                                {
                                        ExportTheme();
                                }
                                using (new EditorGUILayout.HorizontalScope())
                                {
                                        if (GUILayout.Button(ExpandLabel, EditorStyles.miniButtonLeft)) ExpandProperties(true);
                                        if (GUILayout.Button(CollapseLabel, EditorStyles.miniButtonRight)) ExpandProperties(false);
                                }
                                return;
                        }
                        #region Search Results
                        StyleRuleGroup[] groups = Target.StyleRuleGroups;
                        for (int g = 0; g < groups.Length; g++)
                        {
                                ref StyleRuleGroup group = ref groups[g];

                                StyleRule[] rules = group.StyleRules;
                                for (int r = 0; r < rules.Length; r++)
                                {
                                        ref StyleRule rule = ref rules[r];

                                        string[] selectors = rule.Selectors;
                                        for (int s = 0; s < selectors.Length; s++)
                                        {
                                                string selector = selectors[s];
                                                if (!selector.Contains(searchQuery, StringComparison.OrdinalIgnoreCase)) continue;

                                                using (new EditorGUILayout.HorizontalScope())
                                                {
                                                        using (new EditorGUILayout.VerticalScope())
                                                        {
                                                                string displayPath = $"{group.Title} > [{r}] > {selector}";

                                                                EditorGUILayout.LabelField(selector, ETStyles.largeHelpBox);
                                                                EditorGUILayout.LabelField(displayPath, EditorStyles.helpBox);
                                                        }
                                                        if (GUILayout.Button(ETStyles.INFoldout, GUILayout.Width(ActionButtonSize), GUILayout.ExpandHeight(true)))
                                                        {
                                                                ExpandProperties(false);
                                                                SerializedProperty groupProperty = property_StyleRuleGroups.GetArrayElementAtIndex(g), ruleProperty = groupProperty.FindPropertyRelative(nameof(group.StyleRules)).GetArrayElementAtIndex(r);

                                                                property_StyleRuleGroups.isExpanded = groupProperty.isExpanded = ruleProperty.isExpanded = true;
                                                                ClearSearch();
                                                                GUIUtility.ExitGUI();
                                                        }
                                                }
                                        }
                                }
                        }
                        #endregion
                }

                private void ExportTheme()
                {
                        Directory.CreateDirectory(ExportDirectory);

                        string path = Path.Combine(ExportDirectory, Target.ThemeType + FileExtension);
                        File.WriteAllText(path, Target.ToString());

                        AssetDatabase.Refresh();

                        if ((Target.ThemeType == Theme.Type.Dark) ^ isProSkin)
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
                private void ExpandProperties(bool expand)
                {
                        SerializedProperty iterator = serializedObject.GetIterator();
                        while (iterator.NextVisible(true)) if (iterator.depth < 4) iterator.isExpanded = expand;
                }
                private void ClearSearch()
                {
                        searchQuery = string.Empty;
                        GUIUtility.keyboardControl = default;
                }
        }
}