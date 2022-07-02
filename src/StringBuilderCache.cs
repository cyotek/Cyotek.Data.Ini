using System;
using System.Text;

// Cyotek Ini Reader / Writer Library
// https://github.com/cyotek/Cyotek.Data.Ini

// Copyright © 2014-2022 Cyotek Ltd.

// This work is licensed under the MIT License.
// See LICENSE.TXT for the full text

// Found this code useful?
// https://www.cyotek.com/contribute

namespace Cyotek
{
  internal static class StringBuilderCache
  {
    #region Private Fields

    private const int _maxBuilderSize = 360;

    [ThreadStatic]
    private static StringBuilder _cachedInstance;

    #endregion Private Fields

    #region Public Methods

    public static StringBuilder Acquire(int capacity = 16)
    {
      StringBuilder result;

      result = null;

      if (capacity <= _maxBuilderSize)
      {
        StringBuilder cachedInstance;

        cachedInstance = _cachedInstance;
        if (cachedInstance != null && capacity <= cachedInstance.Capacity)
        {
          _cachedInstance = null;
          cachedInstance.Length = 0;
          result = cachedInstance;
        }
      }

      return result ?? new StringBuilder(capacity);
    }

    public static string GetStringAndRelease(StringBuilder sb)
    {
      string str;

      str = sb.ToString();
      Release(sb);

      return str;
    }

    public static void Release(StringBuilder sb)
    {
      if (sb.Capacity <= _maxBuilderSize)
      {
        _cachedInstance = sb;
      }
    }

    public static string ToStringAndRelease(this StringBuilder sb)
    {
      return GetStringAndRelease(sb);
    }

    #endregion Public Methods
  }
}