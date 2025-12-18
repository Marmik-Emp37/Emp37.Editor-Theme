using System;

using UnityEngine;

using UnityEditor;

namespace Emp37.ET
{
      public class GUIStyleExplorerWindow : EditorWindow
      {
            private Vector2 scrollPosition;
            private GUIStyle active;
            private string searchQuery = string.Empty;

            private const float PreviewHeight = 70F;
            private static readonly Vector2 Padding = new(15F, 5F);

            private GUIStyle[] cachedStyles;

            private readonly GUIContent previewContent = new("Sample Text");
            private bool simulateFocus, simulateHover, simulateActive;


            [MenuItem("Tools/Emp37/ET.GUI Style Explorer")]
            private static void ShowWindow()
            {
                  GUIStyleExplorerWindow window = GetWindow<GUIStyleExplorerWindow>();
                  window.titleContent = EditorGUIUtility.IconContent("scrollright");
                  window.titleContent.text = "GUI Style Explorer";
                  window.minSize = new(400F + 2F * Padding.x, 300F + PreviewHeight);
                  window.Show();
            }

            private void OnGUI()
            {
                  cachedStyles ??= GUI.skin.customStyles;

                  EditorGUI.BeginChangeCheck();
                  searchQuery = EditorGUILayout.TextField(searchQuery);
                  if (EditorGUI.EndChangeCheck())
                  {
                        cachedStyles = Array.FindAll(GUI.skin.customStyles, style => style.name.Contains(searchQuery, StringComparison.OrdinalIgnoreCase));
                  }

                  using (GUILayout.ScrollViewScope view = new(scrollPosition))
                  {
                        scrollPosition = view.scrollPosition;

                        foreach (GUIStyle style in cachedStyles)
                              if (GUILayout.Button(style.name, EditorStyles.label))
                              {
                                    if (active != style) active = style;
                                    else CopyToClipboard(style);
                              }
                  }

                  DrawSeparator(1F);

                  if (active == null)
                  {
                        GUILayout.Label("No style selected.", EditorStyles.centeredGreyMiniLabel);
                        return;
                  }
                  GUILayout.Label("Preview: " + active.name, EditorStyles.boldLabel);
                  GUILayout.Space(2F);

                  using (new GUILayout.HorizontalScope())
                  {
                        simulateFocus = GUILayout.Toggle(simulateFocus, "Focused"); simulateHover = GUILayout.Toggle(simulateHover, "Hover"); simulateActive = GUILayout.Toggle(simulateActive, "Active");
                        GUILayout.FlexibleSpace();
                  }
                  previewContent.text = EditorGUILayout.TextField(string.Empty, previewContent.text);
                  GUILayout.Space(2F);

                  DrawSeparator(1F);

                  GUILayout.Space(Padding.y);
                  Rect previewRect = GUILayoutUtility.GetRect(18F, PreviewHeight, GUILayout.ExpandWidth(true));
                  previewRect.xMin += Padding.x; previewRect.xMax -= Padding.x;
                  if (Event.current.type == EventType.Repaint) active.Draw(previewRect, previewContent, simulateHover, simulateActive, false, simulateFocus);
                  GUILayout.Space(Padding.y);
            }

            private static void DrawSeparator(float height) => EditorGUI.DrawRect(GUILayoutUtility.GetRect(default, height), EditorGUIUtility.isProSkin ? Color.white : Color.black);
            private static void CopyToClipboard(GUIStyle style)
            {
                  string content = style.name.Replace(' ', '-');
                  EditorGUIUtility.systemCopyBuffer = content;

                  Debug.Log($"'{content}' copied to clipboard.");
            }
      }
}