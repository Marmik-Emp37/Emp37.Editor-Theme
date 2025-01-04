namespace Emp37.ET
{
      [System.Flags]
      public enum PseudoStates : int
      {
            Active = 1, Hover = 2, Focus = 4, Selected = 8, Checked = 16, Enabled = 32, Disabled = 64
      }
}