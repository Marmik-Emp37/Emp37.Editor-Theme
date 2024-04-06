namespace Emp37.ET
{
      [System.Flags]
      public enum PseudoClasses : int
      {
            Active = 1 << 0, 
            Hover = 1 << 1, 
            Focus = 1 << 2,
            Selected = 1 << 3, 
            Checked = 1 << 4, 
            Enabled = 1 << 5,
            Disabled = 1 << 6
      }
}