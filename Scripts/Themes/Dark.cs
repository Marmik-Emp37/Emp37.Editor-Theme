namespace Emp37.ET
{
      [UnityEngine.CreateAssetMenu(menuName = "Editor-Theme/New Dark Theme", fileName = "New Dark Theme", order = 10)]
      internal class Dark : Theme
      {
            public override string FileName => "Dark.uss";
            public override bool IsSkinInvalid => !UnityEditor.EditorGUIUtility.isProSkin;
      }
}