using System.Collections.Generic;

namespace Cyotek.Data.Ini
{
  public abstract class IniTokenComparer : IComparer<IniToken>
  {
    #region Public Fields

    public static IniTokenComparer Ordinal = new OrdinalIniTokenComparer();

    #endregion Public Fields

    #region Public Methods

    public int Compare(IniToken x, IniToken y)
    {
      int result;

      if (object.ReferenceEquals(x, y))
      {
        result = 0;
      }
      else if (object.ReferenceEquals(null, y))
      {
        result = 1;
      }
      else if (object.ReferenceEquals(null, x))
      {
        result = -1;
      }
      else
      {
        result = this.CompareCore(x, y);
      }

      return result;
    }

    #endregion Public Methods

    #region Protected Methods

    protected abstract int CompareCore(IniToken x, IniToken y);

    #endregion Protected Methods
  }
}