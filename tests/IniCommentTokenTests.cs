﻿using NUnit.Framework;

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