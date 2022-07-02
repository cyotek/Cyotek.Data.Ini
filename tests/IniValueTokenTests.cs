using NUnit.Framework;
using System;

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
  internal class IniValueTokenTests : IniTokenTestBase<IniValueToken>
  {
    #region Protected Properties

    protected override string ExpectedInnerText => "gamma=delta";

    protected override IniTokenType ExpectedType => IniTokenType.Value;

    protected override IniValueToken SampleToken => new IniValueToken("gamma", "delta");

    #endregion Protected Properties

    #region Public Methods

    [TestCase(null, null, TestName = "{m}Null")]
    [TestCase("alpha", null, TestName = "{m}NameOnly")]
    [TestCase(null, "beta", TestName = "{m}ValueOnly")]
    [TestCase("alpha", "beta", TestName = "{m}")]
    public void ConstructorWithNameAndValueTestCases(string expectedName, string expectedValue)
    {
      // arrange
      IniValueToken target;

      // act
      target = new IniValueToken(expectedName, expectedName);

      // assert
      Assert.IsNotNull(target);
      Assert.AreEqual(expectedName, target.Value);
    }

    [Test]
    public void ValueSetTest()
    {
      Assert.Throws<NotSupportedException>(() => new IniSectionToken().Value = "alpha");
    }


    [Test]
    public override void NameSetTest()
    {
      // arrange
      IniValueToken target;
      string expected;

      target = new IniValueToken();

      expected = "epsilon";

      // act
      target.Name = expected;

      // assert
      Assert.AreEqual(expected, target.Name);
    }

    #endregion Public Methods
  }
}