using NUnit.Framework;

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
  internal class IniCommentTokenTests : IniTokenTestBase<IniCommentToken>
  {
    #region Protected Properties

    protected override string ExpectedInnerText => "; alpha beta 2022";

    protected override IniTokenType ExpectedType => IniTokenType.Comment;

    protected override IniCommentToken SampleToken => new IniCommentToken("alpha beta 2022");

    #endregion Protected Properties

    #region Public Methods

    [TestCase(null, TestName = "{m}Null")]
    [TestCase("alpha", TestName = "{m}")]
    public void ConstructorWithValueTestCases(string expected)
    {
      // arrange
      IniCommentToken target;

      // act
      target = new IniCommentToken(expected);

      // assert
      Assert.IsNotNull(target);
      Assert.AreEqual(expected, target.Value);
    }

    #endregion Public Methods
  }
}