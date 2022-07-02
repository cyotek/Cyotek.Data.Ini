using NUnit.Framework;
using System.Collections.Generic;

// Cyotek Ini Reader / Writer Library
// https://github.com/cyotek/Cyotek.Data.Ini

// Copyright © 2014-2022 Cyotek Ltd.

// This work is licensed under the MIT License.
// See LICENSE.TXT for the full text

// Found this code useful?
// https://www.cyotek.com/contribute

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

    [TestCase(null, null, TestName = "{m}Null")]
    [TestCase("", "", TestName = "{m}Empty")]
    [TestCase("alpha", "alpha", TestName = "{m}")]
    [TestCase("\talpha", "alpha", TestName = "{m}Leading")]
    [TestCase("alpha\n", "alpha", TestName = "{m}Trailing")]
    [TestCase("\r\nalpha  ", "alpha", TestName = "{m}Both")]
    public void TrimWhitespaceTestCases(string value, string expected)
    {
      // arrange
      string actual;

      // act
      actual = value.TrimWhitespace();

      // assert
      Assert.AreEqual(expected, actual);
    }

    #endregion Public Methods
  }
}