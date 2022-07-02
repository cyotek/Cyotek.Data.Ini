using NUnit.Framework;

namespace Cyotek.Data.Ini.Tests
{
  internal static class IniAssert
  {
    #region Public Methods

    public static void AreEqual(IniTokenCollection expected, IniTokenCollection actual)
    {
      if (expected != null && actual != null)
      {
        Assert.AreEqual(expected.Count, actual.Count);

        for (int i = 0; i < expected.Count; i++)
        {
          IniAssert.AreEqual(expected[i], actual[i]);
        }
      }
      else
      {
        Assert.IsTrue(object.ReferenceEquals(expected, actual));
      }
    }

    public static void AreEqual(IniDocument expected, IniDocument actual)
    {
      if (expected != null && actual != null)
      {
        // note: Note comparing filename, deliberately
        IniAssert.AreEqual(expected.ChildTokens, actual.ChildTokens);
      }
      else
      {
        Assert.IsTrue(object.ReferenceEquals(expected, actual));
      }
    }

    public static void AreEqual(IniToken expected, IniToken actual)
    {
      if (expected != null && actual != null)
      {
        Assert.AreEqual(expected.Type, actual.Type, nameof(IniToken.Type));
        Assert.AreEqual(expected.Name, actual.Name, nameof(IniToken.Name));
        Assert.AreEqual(expected.Value, actual.Value, nameof(IniToken.Value));
        IniAssert.AreEqual(expected.ChildTokens, actual.ChildTokens);
      }
      else
      {
        Assert.IsTrue(object.ReferenceEquals(expected, actual));
      }
    }

    #endregion Public Methods
  }
}