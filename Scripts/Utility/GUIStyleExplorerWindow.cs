using UnityEditor;

using UnityEngine;

namespace Emp37.ET
{
      public class GUIStyleExplorerWindow : EditorWindow
      {
            private Vector2 scrollPos;
            private GUIStyle activeStyle;
            private string searchText = string.Empty;

            private const float PreviewHeight = 50F;
            private static readonly Vector2 Padding = new(x: 15F, y: 5F);


            private void DrawStyleList()
            {
                  using GUILayout.ScrollViewScope scrollView = new(scrollPos);
                  scrollPos = scrollView.scrollPosition;

                  foreach (GUIStyle style in GUI.skin.customStyles)
                  {
                        if (style.name.Contains(searchText, System.StringComparison.OrdinalIgnoreCase) && GUILayout.Button(style.name, EditorStyles.label))
                        {
                              if (activeStyle == style)
                              {
                                    CopyStyleName(style.name);
                              }
                              else
                              {
                                    activeStyle = style;
                              }
                        }
                  }
            }
            private void DrawStylePreview()
            {
                  GUILayout.Label("Preview:", EditorStyles.boldLabel);
                  if (activeStyle == null)
                  {
                        GUILayout.Label("No style selected.", EditorStyles.centeredGreyMiniLabel);
                        return;
                  }
                  GUILayout.Space(-2F);
                  EditorGUILayout.LabelField(activeStyle.name);
                  using (new GUILayout.VerticalScope())
                  {
                        GUILayout.Space(Padding.y);
                        using (new GUILayout.HorizontalScope(GUILayout.Height(PreviewHeight)))
                        {
                              GUILayout.Space(Padding.x);
                              Vector2 size = new(activeStyle.fixedWidth, activeStyle.fixedHeight);
                              GUILayoutOption[] options = { size.x > 0F ? GUILayout.Width(size.x) : GUILayout.ExpandWidth(true), size.y > 0F ? GUILayout.Height(size.y) : GUILayout.ExpandHeight(true) };
                              GUILayout.Button("Sample Text", activeStyle, options);
                              GUILayout.Space(Padding.x);
                        }
                        GUILayout.Space(Padding.y);
                  }
            }

            private void OnGUI()
            {
                  searchText = GUILayout.TextField(searchText);
                  DrawStyleList();
                  EditorGUI.DrawRect(GUILayoutUtility.GetRect(default, 2F), EditorGUIUtility.isProSkin ? Color.white : Color.black);
                  DrawStylePreview();
            }

            private static void CopyStyleName(string styleName)
            {
                  string formattedName = styleName.Replace(' ', '-');
                  EditorGUIUtility.systemCopyBuffer = formattedName;
                  Debug.Log($"'{formattedName}' copied to clipboard.");
            }

            [MenuItem("Tools/Emp37/ET.GUI Style Explorer")]
            private static void ShowWindow()
            {
                  GUIStyleExplorerWindow window = GetWindow<GUIStyleExplorerWindow>();
                  window.titleContent = EditorGUIUtility.IconContent("scrollright");
                  window.titleContent.text = "GUI Style Explorer";
                  window.minSize = new(400F + 2F * Padding.x, 300F + PreviewHeight);
                  window.Show();
            }
      }
}