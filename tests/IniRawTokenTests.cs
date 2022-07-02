using NUnit.Framework;

namespace Cyotek.Data.Ini.Tests
{
  [TestFixture]
  internal class IniRawTokenTests : IniTokenTestBase<IniRawToken>
  {
    #region Protected Properties

    protected override string ExpectedInnerText => "I am the very model of a modern Major-Gineral";

    protected override IniTokenType ExpectedType => IniTokenType.Raw;

    protected override IniRawToken SampleToken => new IniRawToken("I am the very model of a modern Major-Gineral");

    #endregion Protected Properties

    #region Public Methods

    [TestCase(null, TestName = "{m}Null")]
    [TestCase("alpha", TestName = "{m}")]
    public void ConstructorWithValueTestCases(string expected)
    {
      // arrange
      IniRawToken target;

      // act
      target = new IniRawToken(expected);

      // assert
      Assert.IsNotNull(target);
      Assert.AreEqual(expected, target.Value);
    }

    #endregion Public Methods
  }
}