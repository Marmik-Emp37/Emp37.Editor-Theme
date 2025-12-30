using System;

using UnityEditor;
using UnityEngine;

namespace Emp37.ET
{
      public class GUIStyleExplorerWindow : EditorWindow
      {
            private const float PreviewHeight = 80F;
            private static readonly Vector2 Padding = new(15F, 5F);
            private readonly GUIContent previewContent = new("Sample Text");

            private GUIStyle selectedStyle;
            private GUIStyle[] cachedStyles;

            private string searchQuery = string.Empty;
            private bool focus, hover, active;
            private Vector2 scrollPosition;


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

                  #region S E A R C H   B A R
                  EditorGUI.BeginChangeCheck();
                  searchQuery = EditorGUILayout.TextField(searchQuery);
                  if (EditorGUI.EndChangeCheck())
                  {
                        cachedStyles = Array.FindAll(GUI.skin.customStyles, style => style.name.Contains(searchQuery, StringComparison.OrdinalIgnoreCase));
                  }
                  #endregion

                  #region L I S T   O F   S T Y L E S
                  using (GUILayout.ScrollViewScope view = new(scrollPosition))
                  {
                        scrollPosition = view.scrollPosition;

                        foreach (GUIStyle style in cachedStyles)
                              if (GUILayout.Button(style.name, EditorStyles.label))
                              {
                                    if (selectedStyle != style) selectedStyle = style;
                                    else CopyToClipboard(style);
                              }
                  }
                  #endregion

                  DrawSeparator(1F);

                  #region P R E V I E W   P A N E L
                  if (selectedStyle == null)
                  {
                        GUILayout.Label("No style selected.", EditorStyles.centeredGreyMiniLabel);
                        return;
                  }

                  GUILayout.Label("Preview: " + selectedStyle.name, EditorStyles.boldLabel);

                  GUILayout.Space(2F);
                  using (new GUILayout.HorizontalScope())
                  {
                        focus = GUILayout.Toggle(focus, "Focus"); hover = GUILayout.Toggle(hover, "Hover"); active = GUILayout.Toggle(active, "Active");
                        GUILayout.FlexibleSpace();
                  }
                  GUILayout.Space(2F);
                  previewContent.text = EditorGUILayout.TextField(string.Empty, previewContent.text);
                  GUILayout.Space(2F);

                  DrawSeparator(1F);

                  GUILayout.Space(Padding.y);
                  Rect previewRect = GUILayoutUtility.GetRect(18F, PreviewHeight, GUILayout.ExpandWidth(true));
                  previewRect.xMin += Padding.x; previewRect.xMax -= Padding.x;
                  if (Event.current.type == EventType.Repaint) selectedStyle.Draw(previewRect, previewContent, hover, active, false, focus);
                  GUILayout.Space(Padding.y);
                  #endregion
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