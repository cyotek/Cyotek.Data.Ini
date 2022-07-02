using System.Text;

// Cyotek Ini Reader / Writer Library
// https://github.com/cyotek/Cyotek.Data.Ini

// Copyright © 2014-2022 Cyotek Ltd.

// This work is licensed under the MIT License.
// See LICENSE.TXT for the full text

// Found this code useful?
// https://www.cyotek.com/contribute

namespace Cyotek.Data.Ini
{
  internal static class StringBuilderExtensions
  {
    public static void RemoveTrailingLinebreaks(this StringBuilder sb)
    {
#if NETCOREAPP || NET
      while (sb.Length > 0 && (sb[^1] == '\r' || sb[^1] == '\n'))
#else
        while (sb.Length > 0 && (sb[sb.Length - 1] == '\r' || sb[sb.Length - 1] == '\n'))
#endif
      {
        sb.Length--;
      }
    }
  }
}