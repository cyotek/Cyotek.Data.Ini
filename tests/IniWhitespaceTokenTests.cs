using NUnit.Framework;
using System;

namespace Cyotek.Data.Ini.Tests
{
  [TestFixture]
  internal class IniWhitespaceTokenTests : IniTokenTestBase<IniWhitespaceToken>
  {
    #region Protected Properties

    protected override string DefaultValue => string.Empty;

    protected override string ExpectedInnerText => "\t  \t";

    protected override IniTokenType ExpectedType => IniTokenType.Whitespace;

    protected override IniWhitespaceToken SampleToken => new IniWhitespaceToken("\t  \t");

    #endregion Protected Properties

    #region Public Methods

    [TestCase(null, TestName = "{m}Null")]
    [TestCase("\t", TestName = "{m}")]
    public void ConstructorWithValueTestCases(string expected)
    {
      // arrange
      IniWhitespaceToken target;

      // act
      target = new IniWhitespaceToken(expected);

      // assert
      Assert.IsNotNull(target);
      Assert.AreEqual(expected, target.Value);
    }

    [Test]
    public void ValueWithNonWhitespaceThrowsTest()
    {
      Assert.Throws<ArgumentException>(() => new IniWhitespaceToken().Value = "alpha");
    }

    #endregion Public Methods
  }
}