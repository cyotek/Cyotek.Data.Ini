using NUnit.Framework;
using System.Collections.Generic;
using System.Reflection;

namespace Cyotek.Data.Ini.Tests
{
  [TestFixture]
  internal class IniTokenCollectionTests : TestBase
  {
    #region Public Methods

    public static IEnumerable<TestCaseData> TryGetValueTestCaseSource()
    {
      yield return new TestCaseData("alpha", true, new IniValueToken("alpha", "beta")).SetName("{m}");
      yield return new TestCaseData("GaMmA", true, new IniValueToken("gamma", "delta")).SetName("{m}Case");
      yield return new TestCaseData("beta", false, null).SetName("{m}NotFound");
      yield return new TestCaseData("", false, null).SetName("{m}Empty");
      yield return new TestCaseData(null, false, null).SetName("{m}Null");
    }

    [Test]
    public void AddRangeTest()
    {
      // arrange
      IniTokenCollection target;
      IniTokenCollection expected;
      IEnumerable<IniToken> items;

      target = new IniTokenCollection { new IniValueToken("alpha", "beta"), new IniValueToken("gamma", "delta") };

      expected = new IniTokenCollection { new IniValueToken("alpha", "beta"), new IniValueToken("gamma", "delta"), new IniValueToken("epsilon", "zeta"), new IniValueToken("eta", "theta") };

      items = new[] { new IniValueToken("epsilon", "zeta"), new IniValueToken("eta", "theta") };

      // act
      target.AddRange(items);

      // assert
      IniDocumentAssert.AreEqual(expected, target);
    }

    [Test]
    public void ConstructorWithValuesTest()
    {
      // arrange
      IniTokenCollection expected;
      IniTokenCollection actual;

      expected = new IniTokenCollection { new IniValueToken("alpha", "beta"), new IniValueToken("gamma", "delta") };

      // act
      actual = new IniTokenCollection(expected);

      // assert
      IniDocumentAssert.AreEqual(expected, actual);
    }

    [Test]
    public void IndexerStringTest()
    {
      // arrange
      IniTokenCollection target;
      IniToken expected;
      IniToken actual;

      target = new IniTokenCollection { new IniValueToken("alpha", "beta"), new IniValueToken("gamma", "delta") };

      expected = target[1];

      // act
      actual = target["gamma"];

      // assert
      Assert.AreSame(expected, actual);
    }

    [Test]
    public void IndexOf_returns_index_by_token()
    {
      // arrange
      IniTokenCollection target;
      IniSectionToken token;
      int expected;
      int actual;

      token = new IniSectionToken("Beta");

      target = new IniTokenCollection();
      target.Add(new IniSectionToken("Alpha"));
      target.Add(token);
      target.Add(new IniSectionToken("Gamma"));

      expected = 1;

      // act
      actual = target.IndexOf(token);

      // assert
      Assert.AreEqual(expected, actual);
    }

    [Test]
    public void IndexOfRenamedTest()
    {
      // arrange
      IniTokenCollection target;
      int expected;
      int actual;

      // As not all tokens support names, the collection uses a lookup dictionary to match names to indexes in a list
      // This means that if items are removed, renamed or the list is reordered, the lookup will be invalid. It should
      // update itself if it detects an invalid value, so lets just make sure it does!

      target = new IniTokenCollection();
      target.Add(new IniSectionToken("Alpha"));
      target.Add(new IniSectionToken("Beta"));
      target.Add(new IniSectionToken("Gamma"));

      target["Gamma"].Name = "Delta";

      expected = 2;

      // act
      actual = target.IndexOf("Delta");

      // assert
      Assert.AreEqual(expected, actual);
    }

    [TestCase("alpha", 0, TestName = "{m}")]
    [TestCase("GaMmA", 1, TestName = "{m}Case")]
    [TestCase("beta", -1, TestName = "{m}NotFound")]
    [TestCase("", -1, TestName = "{m}Empty")]
    [TestCase(null, -1, TestName = "{m}Null")]
    public void IndexOfTestCases(string name, int expected)
    {
      // arrange
      IniTokenCollection target;
      int actual;

      target = new IniTokenCollection { new IniValueToken("alpha", "beta"), new IniValueToken("gamma", "delta") };

      // act
      actual = target.IndexOf(name);

      // assert
      Assert.AreEqual(expected, actual);
    }

    [Test]
    public void InsertRangeTest()
    {
      // arrange
      IniTokenCollection target;
      IniTokenCollection expected;
      IEnumerable<IniToken> items;

      target = new IniTokenCollection
      {
        new IniValueToken("alpha", "beta"),
        new IniValueToken("gamma", "delta")
      };

      expected = new IniTokenCollection
      {
        new IniValueToken("alpha", "beta"),
        new IniValueToken("epsilon", "zeta"),
        new IniValueToken("eta", "theta"),
        new IniValueToken("gamma", "delta")
      };

      items = new[]
      {
        new IniValueToken("epsilon", "zeta"),
        new IniValueToken("eta", "theta")
      };

      // act
      target.InsertRange(1, items);

      // assert
      IniDocumentAssert.AreEqual(expected, target);
    }

    [TestCase("alpha", true, TestName = "{m}")]
    [TestCase("GaMmA", true, TestName = "{m}Case")]
    [TestCase("beta", false, TestName = "{m}NotFound")]
    [TestCase("", false, TestName = "{m}Empty")]
    [TestCase(null, false, TestName = "{m}Null")]
    public void RemoveByNameTestCases(string name, bool expected)
    {
      // arrange
      IniTokenCollection target;
      bool actual;

      target = new IniTokenCollection
      {
        new IniValueToken("alpha", "beta"),
        new IniValueToken("gamma", "delta")
      };

      // act
      actual = target.Remove(name);

      // assert
      Assert.AreEqual(expected, actual);
    }

    [Test]
    public void RemoveRangeTest()
    {
      // arrange
      IniTokenCollection target;
      IniTokenCollection expected;
      IEnumerable<IniToken> items;
      IniValueToken a;
      IniValueToken b;
      IniValueToken c;
      IniValueToken d;

      a = new IniValueToken("alpha", "epsilon");
      b = new IniValueToken("beta", "zeta");
      c = new IniValueToken("gamma", "eta");
      d = new IniValueToken("delta", "theta");

      target = new IniTokenCollection { a, b, c, d };

      expected = new IniTokenCollection { a, d };

      items = new[] { b, c };

      // act
      target.RemoveRange(items);

      // assert
      IniDocumentAssert.AreEqual(expected, target);
    }

    [Test]
    public void Setting_item_removes_lookup()
    {
      // arrange
      IniTokenCollection target;
      IDictionary<string, int> lookup;
      bool actual;

      target = new IniTokenCollection();
      target.Add(new IniSectionToken("Alpha"));
      target.Add(new IniSectionToken("Beta"));
      target.Add(new IniSectionToken("Gamma"));

      target.IndexOf("Gamma"); // make sure the lookup entry is cached

      lookup = (IDictionary<string, int>)target.GetType().GetField("_nameToIndexLookup", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(target);

      // act
      target[2] = new IniSectionToken("Delta");

      // assert
      actual = lookup.ContainsKey("Gamma");
      Assert.IsFalse(actual);
    }

    [Test]
    public void ToArrayEmptyTest()
    {
      // arrange
      IniTokenCollection target;
      IniToken[] actual;

      target = new IniTokenCollection();

      // act
      actual = target.ToArray();

      // assert
      CollectionAssert.IsEmpty(actual);
    }

    [Test]
    public void ToArrayTest()
    {
      // arrange
      IniTokenCollection target;
      IniToken[] expected;
      IniToken[] actual;
      IniToken a;
      IniToken b;

      a = new IniValueToken("alpha", "beta");
      b = new IniValueToken("gamma", "delta");

      target = new IniTokenCollection { a, b };

      expected = new[] { a, b };

      // act
      actual = target.ToArray();

      // assert
      CollectionAssert.AreEqual(expected, target);
    }

    [Test]
    public void TryAddRangeTest()
    {
      // arrange
      IniTokenCollection target;
      IniTokenCollection expected;
      IEnumerable<IniToken> items;
      IniValueToken a;
      IniValueToken b;
      IniValueToken c;
      IniValueToken d;

      a = new IniValueToken("alpha", "epsilon");
      b = new IniValueToken("beta", "zeta");
      c = new IniValueToken("gamma", "eta");
      d = new IniValueToken("delta", "theta");

      target = new IniTokenCollection { a, b };

      expected = new IniTokenCollection { a, b, c, d };

      items = new[] { a, b, c, d };

      // act
      target.TryAddRange(items);

      // assert
      IniDocumentAssert.AreEqual(expected, target);
    }

    [TestCaseSource(nameof(TryGetValueTestCaseSource))]
    public void TryGetValueTestCases(string name, bool expected, IniToken expectedToken)
    {
      // arrange
      IniTokenCollection target;
      bool actual;

      target = new IniTokenCollection
      {
        new IniValueToken("alpha", "beta"),
        new IniValueToken("gamma", "delta")
      };

      // act
      actual = target.TryGetValue(name, out IniToken atualToken);

      // assert
      Assert.AreEqual(expected, actual);
      IniDocumentAssert.AreEqual(expectedToken, atualToken);
    }

    #endregion Public Methods
  }
}