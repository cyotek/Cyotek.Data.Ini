
namespace Cyotek.Data.Ini.Tests
{
  [TestFixture]
  internal class IniCommentTokenTests : TestBase
  {
    #region  Tests

    [Test]
    public void ConstuctorTest()
    {
      // arrange
      IniCommentToken target;
      IniTokenType expectedType;

      expectedType = IniTokenType.Comment;

      // act
      target = new IniCommentToken();

      // assert
      Assert.IsNotNull(target);
      Assert.AreEqual(expectedType, target.Type);
      Assert.IsNull(target.Value);
    }

    [Test]
    public void ConstuctorWithValueTest()
    {
      // arrange
      IniCommentToken target;
      string expected;
      IniTokenType expectedType;

      expectedType = IniTokenType.Comment;
      expected = this.GetRandomString();

      // act
      target = new IniCommentToken(expected);

      // assert
      Assert.IsNotNull(target);
      Assert.AreEqual(expectedType, target.Type);
      Assert.AreEqual(expected, target.Value);
    }

    [Test]
    public void ToStringTest()
    {
      // arrange
      IniCommentToken target;
      string expected;
      string actual;
      string value;

      value = this.GetRandomString();
      expected = string.Concat("; ", value);
      target = new IniCommentToken(value);

      // act
      actual = target.ToString();

      // assert
      Assert.AreEqual(expected, actual);
    }

    #endregion
  }
}
