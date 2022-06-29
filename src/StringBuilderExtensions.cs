using System.Text;

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