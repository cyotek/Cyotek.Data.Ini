using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;

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
  internal class IniSectionTokenTests : IniTokenTestBase<IniSectionToken>
  {
    #region Protected Properties

    protected override string DefaultName => string.Empty;

    protected override string ExpectedInnerText => @"[I am the very model of a modern Major-Gineral]
alpha=beta
; line 3

gamma=delta
epsilon
eta=zeta";

    protected override IniTokenType ExpectedType => IniTokenType.Section;

    protected override IniSectionToken SampleToken => new IniSectionToken("I am the very model of a modern Major-Gineral")
    {
      ChildTokens =
      {
        new IniValueToken("alpha", "beta"),
        new IniCommentToken("line 3"),
        new IniWhitespaceToken(),
        new IniValueToken("gamma", "delta"),
        new IniRawToken("epsilon"),
        new CustomIniToken
        {
          Name = "eta",
          Value = "zeta"
        }
      }
    };

    #endregion Protected Properties

    #region Public Methods

    public static IEnumerable<TestCaseData> GetValueTokenTestCaseSource()
    {
      yield return new TestCaseData("alpha", new IniValueToken("alpha", "beta")).SetName("{m}");
      yield return new TestCaseData("beta", null).SetName("{m}Missing");
    }

    public static IEnumerable<TestCaseData> SetValueTestCaseSource()
    {
      yield return new TestCaseData(new IniSectionToken("Settings")
      {
        ChildTokens =
        {
          new IniValueToken("alpha", "beta")
        }
      }, "alpha", "beta", false, "[Settings]\r\nalpha=beta").SetName("{m}NoChange");
      yield return new TestCaseData(new IniSectionToken("Settings")
      {
        ChildTokens =
        {
          new IniValueToken("alpha", "beta")
        }
      }, "alpha", "gamma", true, "[Settings]\r\nalpha=gamma").SetName("{m}Change");
      yield return new TestCaseData(new IniSectionToken("Settings"), "omega", "alpha", true, "[Settings]\r\nomega=alpha").SetName("{m}New");
      yield return new TestCaseData(new IniSectionToken("Settings")
      {
        ChildTokens =
        {
          new IniValueToken("alpha", "beta")
        }
      }, "gamma", "delta", true, "[Settings]\r\nalpha=beta\r\ngamma=delta").SetName("{m}Append");
      yield return new TestCaseData(new IniSectionToken("Settings")
      {
        ChildTokens =
        {
          new IniValueToken("alpha", "beta"),
          new IniWhitespaceToken(" "),
          new IniWhitespaceToken(" ")
        }
      }, "gamma", "delta", true, "[Settings]\r\nalpha=beta\r\ngamma=delta\r\n \r\n ").SetName("{m}Insert");
    }

    [TestCase(null, TestName = "{m}Null")]
    [TestCase("alpha", TestName = "{m}")]
    public void ConstructorWithNameTestCases(string expected)
    {
      // arrange
      IniSectionToken target;

      // act
      target = new IniSectionToken(expected);

      // assert
      Assert.IsNotNull(target);
      Assert.AreEqual(expected, target.Name);
    }

    [Test]
    public void GetNamesTest()
    {
      // arrange
      IniSectionToken target;
      string[] expected;
      IEnumerable<string> actual;

      target = this.SampleToken;

      expected = new[]
      {
        "alpha",
        "gamma"
      };

      // act
      actual = target.GetNames();

      // assert
      CollectionAssert.AreEqual(expected, actual);
    }

    [TestCase("alpha", "beta", TestName = "{m}")]
    [TestCase("epsilon", "", TestName = "{m}Missing")]
    public void GetValueTestCases(string key, string expected)
    {
      // arrange
      IniSectionToken target;
      string actual;

      target = this.SampleToken;

      // act
      actual = target.GetValue(key);

      // assert
      Assert.AreEqual(expected, actual);
    }

    [Test]
    public void GetValueTokenExceptionTest()
    {
      Assert.Throws<InvalidDataException>(() => this.SampleToken.GetValueToken("eta"));
    }

    [TestCaseSource(nameof(GetValueTokenTestCaseSource))]
    public void GetValueTokenTestCases(string name, IniToken expected)
    {
      // arrange
      IniSectionToken target;
      IniToken actual;

      target = this.SampleToken;

      // act
      actual = target.GetValueToken(name);

      // assert
      IniAssert.AreEqual(expected, actual);
    }

    [TestCase("alpha", "gamma", "beta", TestName = "{m}")]
    [TestCase("epsilon", "delta", "delta", TestName = "{m}Missing")]
    public void GetValueWithFallbackTestCases(string key, string fallback, string expected)
    {
      // arrange
      IniSectionToken target;
      string actual;

      target = this.SampleToken;

      // act
      actual = target.GetValue(key, fallback);

      // assert
      Assert.AreEqual(expected, actual);
    }

    [TestCase("alpha", "beta", TestName = "{m}")]
    [TestCase("epsilon", "", TestName = "{m}Missing")]
    public void IndexerGetTestCases(string key, string expected)
    {
      // arrange
      IniSectionToken target;
      string actual;

      target = this.SampleToken;

      // act
      actual = target[key];

      // assert
      Assert.AreEqual(expected, actual);
    }

    [TestCaseSource(nameof(SetValueTestCaseSource))]
    public void IndexerSetTestCases(IniSectionToken target, string name, string value, bool _, string expectedLayout)
    {
      // arrange

      // act
      target[name] = value;

      // assert
      Assert.AreEqual(expectedLayout, target.InnerText);
    }

    [Test]
    public override void NameSetTest()
    {
      // arrange
      IniSectionToken target;
      string expected;

      target = new IniSectionToken();

      expected = "epsilon";

      // act
      target.Name = expected;

      // assert
      Assert.AreEqual(expected, target.Name);
    }

    [Test]
    public void SetValueNullExceptionTest()
    {
      Assert.Throws<ArgumentNullException>(() => new IniSectionToken().SetValue(null, "beta"));
    }

    [TestCaseSource(nameof(SetValueTestCaseSource))]
    public void SetValueTestCases(IniSectionToken target, string name, string value, bool expected, string expectedLayout)
    {
      // arrange
      bool actual;

      // act
      actual = target.SetValue(name, value);

      // assert
      Assert.AreEqual(expected, actual);
      Assert.AreEqual(expectedLayout, target.InnerText);
    }

    [Test]
    public void SetValueTypeExceptionTest()
    {
      Assert.Throws<InvalidDataException>(() => this.SampleToken.SetValue("eta", "theta"));
    }

    #endregion Public Methods
  }
}