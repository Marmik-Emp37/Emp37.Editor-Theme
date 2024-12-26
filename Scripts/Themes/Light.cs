namespace Emp37.ET
{
      [UnityEngine.CreateAssetMenu(menuName = "Editor-Theme/New Light Theme", fileName = "New Light Theme", order = 9)]
      internal class Light : Theme
      {
            public override string FileName => "Light.uss";
            public override bool IsSkinInvalid => UnityEditor.EditorGUIUtility.isProSkin;
      }
}