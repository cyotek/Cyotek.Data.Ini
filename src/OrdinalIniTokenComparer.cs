using System;

// Cyotek Ini Reader / Writer Library
// https://github.com/cyotek/Cyotek.Data.Ini

// Copyright © 2014-2022 Cyotek Ltd.

// This work is licensed under the MIT License.
// See LICENSE.TXT for the full text

// Found this code useful?
// https://www.cyotek.com/contribute

namespace Cyotek.Data.Ini
{
  internal sealed class OrdinalIniTokenComparer : IniTokenComparer
  {
    #region Protected Methods

    protected override int CompareCore(IniToken x, IniToken y)
    {
      int result;

      result = string.CompareOrdinal(x.Name, y.Name);

      if (result == 0)
      {
        result = string.CompareOrdinal(x.Value, y.Value);

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