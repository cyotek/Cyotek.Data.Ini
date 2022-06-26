using NUnit.Framework;

namespace Cyotek.Data.Ini.Tests
{
  internal static class IniDocumentAssert
  {
    #region Public Methods

    public static void AreEqual(IniTokenCollection expected, IniTokenCollection actual)
    {
      if (expected != null && actual != null)
      {
        for (int i = 0; i < expected.Count; i++)
        {
          IniDocumentAssert.AreEqual(expected[i], actual[i]);
        }
      }
    }

    public static void AreEqual(IniDocument expected, IniDocument actual)
    {
      Assert.AreEqual(expected.ChildTokens.Count, actual.ChildTokens.Count);
      IniDocumentAssert.AreEqual(expected.ChildTokens, actual.ChildTokens);
    }

    public static void AreEqual(IniToken expected, IniToken actual)
    {
      Assert.AreEqual(expected.Type, actual.Type, nameof(IniToken.Type));
      Assert.AreEqual(expected.Name, actual.Name, nameof(IniToken.Name));
      Assert.AreEqual(expected.Value, actual.Value, nameof(IniToken.Value));
      IniDocumentAssert.AreEqual(expected.ChildTokens, actual.ChildTokens);
    }

    #endregion Public Methods
  }
}