using System.Text;

namespace Cyotek.Data.Ini
{
  internal static class StringBuilderExtensions
  {
    #region Public Methods

    public static void RemoveTrailingLinebreaks(this StringBuilder sb)
    {
      while (sb.Length > 0 && (sb[sb.Length - 1] == '\r' || sb[sb.Length - 1] == '\n'))
      {
        sb.Length -= 1;
      }
    }

    #endregion Public Methods
  }
}