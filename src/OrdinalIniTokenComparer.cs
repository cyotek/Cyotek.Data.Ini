using System;

namespace Cyotek.Data.Ini
{
  internal sealed class OrdinalIniTokenComparer : IniTokenComparer
  {
    #region Protected Methods

    protected override int CompareCore(IniToken x, IniToken y)
    {
      int result;

      result = string.Compare(x.Name, y.Name, StringComparison.Ordinal);

      if (result == 0)
      {
        result = string.Compare(x.Value, y.Value, StringComparison.Ordinal);

        if (result == 0)
        {
          result = x.Type.CompareTo(y.Type);
        }
      }

      return result;
    }

    #endregion Protected Methods
  }
}