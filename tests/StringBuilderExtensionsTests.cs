using NUnit.Framework;
using System.Collections.Generic;
using System.Text;

namespace Cyotek.Data.Ini.Tests
{
  [TestFixture]
  public class StringBuilderExtensionsTests
  {
    #region Public Methods

    public static IEnumerable<TestCaseData> RemoveTrailingLinebreaksTestCaseSource()
    {
      yield return new TestCaseData(string.Empty, string.Empty).SetName("{m}Empty");
      yield return new TestCaseData("\r\n", string.Empty).SetName("{m}Clear");
      yield return new TestCaseData("alpha\r\nbeta", "alpha\r\nbeta").SetName("{m}Inner");
      yield return new TestCaseData("\nalpha", "\nalpha").SetName("{m}Leading");
      yield return new TestCaseData("alpha\n", "alpha").SetName("{m}Lf");
      yield return new TestCaseData("alpha\r\n", "alpha").SetName("{m}CrLf");
      yield return new TestCaseData("alpha\n\n", "alpha").SetName("{m}LfLf");
      yield return new TestCaseData("alpha\r\n\r\n", "alpha").SetName("{m}CrLfCrLf");
      yield return new TestCaseData("alpha\n\r\n", "alpha").SetName("{m}LfCrLf");
      yield return new TestCaseData("alpha\r\n\n", "alpha").SetName("{m}CrLfLf");
    }

    [TestCaseSource(nameof(RemoveTrailingLinebreaksTestCaseSource))]
    public void RemoveTrailingLinebreaksTestCases(string input, string expected)
    {
      // arrange
      StringBuilder target;
      string actual;

      target = new StringBuilder();
      target.Append(input);

      // act
      target.RemoveTrailingLinebreaks();

      // assert
      actual = target.ToString();
      Assert.AreEqual(expected, actual);
    }

    #endregion Public Methods
  }
}