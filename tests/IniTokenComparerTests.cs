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
  internal class IniTokenComparerTests
  {
    #region Public Methods

    public static IEnumerable<TestCaseData> CompareTestCaseSource()
    {
      yield return new TestCaseData(null, null, 0).SetName("{m}Null");
      yield return new TestCaseData(null, new IniValueToken(), -1).SetName("{m}XNull");
      yield return new TestCaseData(new IniValueToken(), null, 1).SetName("{m}YNull");
      yield return new TestCaseData(new IniValueToken("alpha", "beta"), new IniValueToken("alpha", "beta"), 0).SetName("{m}");
      yield return new TestCaseData(new IniWhitespaceToken(), new IniValueToken(), 1).SetName("{m}Type");
      yield return new TestCaseData(new IniValueToken("alpha", "beta"), new IniValueToken("gamma", "beta"), -1).SetName("{m}Name");
      yield return new TestCaseData(new IniValueToken("alpha", "beta"), new IniValueToken("alpha", "delta"), -1).SetName("{m}Value");
    }

    [TestCaseSource(nameof(CompareTestCaseSource))]
    public void CompareTestCases(IniToken x, IniToken y, int expected)
    {
      // arrange
      IniTokenComparer target;
      int actual;

      target = new OrdinalIniTokenComparer();

      // act
      actual = target.Compare(x, y);

      // assert
      if (expected == 0)
      {
        Assert.Zero(actual);
      }
      else if (expected < 0)
      {
        Assert.Less(actual, 0);
      }
      else
      {
        Assert.Greater(actual, 0);
      }
    }

    #endregion Public Methods
  }
}