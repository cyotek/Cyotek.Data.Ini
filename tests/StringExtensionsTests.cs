using NUnit.Framework;
using System.Collections.Generic;

namespace Cyotek.Data.Ini.Tests
{
  [TestFixture]
  internal class StringExtensionsTests
  {
    #region Public Methods

    public static IEnumerable<TestCaseData> StartsWithAnyTestCaseSource()
    {
      yield return new TestCaseData(null, new[] { 'a', 'b' }, false).SetName("{m}Null");
      yield return new TestCaseData(string.Empty, new[] { 'a', 'b' }, false).SetName("{m}Empty");
      yield return new TestCaseData("alpha", new[] { 'a', 'b' }, true).SetName("{m}");
      yield return new TestCaseData("beta", new[] { 'a', 'b' }, true).SetName("{m}DifferentChar");
      yield return new TestCaseData("gamma", new[] { 'a', 'b' }, false).SetName("{m}NoMatch");
    }

    [TestCaseSource(nameof(StartsWithAnyTestCaseSource))]
    public void StartsWithAnyTestCases(string target, char[] anyOf, bool expected)
    {
      Assert.AreEqual(expected, target.StartsWithAny(anyOf));
    }

    [TestCase("\tHello\r\n\tWorld!\r\n", @"\tHello\r\n\tWorld!\r\n", TestName = "{m}")]
    [TestCase("\'\"\\\0\a\b\f\n\r\t\v", @"\'""\\\0\a\b\f\n\r\t\v", TestName = "{m}All")]
    [TestCase("\u0001\tHello\r\n\tWorld!\r\n\u2026\u2400", @"\u0001\tHello\r\n\tWorld!\r\n…␀", TestName = "{m}Unicode")]
    public void ToEscapedLiteralTestCases(string target, string expected)
    {
      // arrange
      string actual;

      // act
      actual = target.ToEscapedLiteral();

      // assert
      Assert.AreEqual(expected, actual);
    }

    [TestCase(@"\t""Hello""\r\n\tWorld!\r\n", "\t\"Hello\"\r\n\tWorld!\r\n", TestName = "{m}")]
    [TestCase(@"\'""\\\0\a\b\f\n\r\t\v", "\'\"\\\0\a\b\f\n\r\t\v", TestName = "{m}All")]
    [TestCase(@"\u0001\tHello\r\n\tWorld!\r\n…␀", "\u0001\tHello\r\n\tWorld!\r\n\u2026\u2400", TestName = "{m}Unicode")]
    public void ToLiteralTestCases(string target, string expected)
    {
      // arrange
      string actual;

      // act
      actual = target.ToLiteral();

      // assert
      Assert.AreEqual(expected, actual);
    }

    #endregion Public Methods
  }
}