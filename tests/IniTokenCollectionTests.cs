using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Cyotek.Data.Ini.Tests
{
  [TestFixture]
  internal class IniTokenCollectionTests : TestBase
  {
    #region Private Properties

    private static IniTokenCollection TestCollection => new IniTokenCollection
    {
      new IniValueToken("longTest", "9223372036854775807"),
      new IniValueToken("shortTest", "32767"),
      new IniValueToken("stringTest", "HELLO WORLD THIS IS A TEST STRING ÅÄÖ!"),
      new IniValueToken("floatTest", "0.4982315"),
      new IniValueToken("intTest", "2147483647"),
      new IniValueToken("byteTest", "127"),
      new IniValueToken("doubleTest", "0.493128713218231"),
      new IniValueToken("colorTest", "Crimson"),
      new IniValueToken("fontTest", "Arial,13,\"Italic, Strikeout\""),
      new IniValueToken("enumTest", "Bold, Italic"),
      new IniValueToken("blank", "")
    };

    #endregion Private Properties

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
    public void SortComparisonExceptionTest()
    {
      Assert.Throws<ArgumentNullException>(() => new IniTokenCollection().Sort((Comparison<IniToken>)null));
    }

    [Test]
    public void SortComparisonTest()
    {
      // arrange
      IniTokenCollection target;
      IniTokenCollection expected;

      target = IniTokenCollectionTests.TestCollection;

      expected = new IniTokenCollection
      {
        new IniValueToken("stringTest", "HELLO WORLD THIS IS A TEST STRING ÅÄÖ!"),
        new IniValueToken("shortTest", "32767"),
        new IniValueToken("longTest", "9223372036854775807"),
        new IniValueToken("intTest", "2147483647"),
        new IniValueToken("fontTest", "Arial,13,\"Italic, Strikeout\""),
        new IniValueToken("floatTest", "0.4982315"),
        new IniValueToken("enumTest", "Bold, Italic"),
        new IniValueToken("doubleTest", "0.493128713218231"),
        new IniValueToken("colorTest", "Crimson"),
        new IniValueToken("byteTest", "127"),
        new IniValueToken("blank", "")
      };

      // act
      target.Sort((x, y) => -string.Compare(x.Name, y.Name));

      // assert
      IniDocumentAssert.AreEqual(expected, target);
    }

    [Test]
    public void SortExceptionTest()
    {
      Assert.Throws<ArgumentNullException>(() => new IniTokenCollection().Sort((IComparer<IniToken>)null));
    }

    [Test]
    public void SortIndexExceptionTest()
    {
      // arrange
      string expected;
      Exception actual;

      expected = "Index cannot be less than zero. (Parameter 'index')";

      // act
      actual = Assert.Throws<ArgumentException>(() => new IniTokenCollection().Sort(-1, 1, IniTokenComparer.Ordinal));

      // assert
      Assert.AreEqual(expected, actual.Message);
    }

    [Test]
    public void SortMaximumCountExceptionTest()
    {
      // arrange
      string expected;
      Exception actual;

      expected = "Invalid count.";

      // act
      actual = Assert.Throws<ArgumentException>(() => new IniTokenCollection().Sort(0, 100, IniTokenComparer.Ordinal));

      // assert
      Assert.AreEqual(expected, actual.Message);
    }

    [Test]
    public void SortMinimumCountExceptionTest()
    {
      // arrange
      string expected;
      Exception actual;

      expected = "Count cannot be less than zero. (Parameter 'count')";

      // act
      actual = Assert.Throws<ArgumentException>(() => new IniTokenCollection().Sort(0, -1, IniTokenComparer.Ordinal));

      // assert
      Assert.AreEqual(expected, actual.Message);
    }

    [Test]
    public void SortParametersTest()
    {
      // arrange
      IniTokenCollection target;
      IniTokenCollection expected;

      target = IniTokenCollectionTests.TestCollection;

      expected = new IniTokenCollection
      {
        new IniValueToken("longTest", "9223372036854775807"),
        new IniValueToken("shortTest", "32767"),
        new IniValueToken("byteTest", "127"),
        new IniValueToken("colorTest", "Crimson"),
        new IniValueToken("doubleTest", "0.493128713218231"),
        new IniValueToken("floatTest", "0.4982315"),
        new IniValueToken("fontTest", "Arial,13,\"Italic, Strikeout\""),
        new IniValueToken("intTest", "2147483647"),
        new IniValueToken("stringTest", "HELLO WORLD THIS IS A TEST STRING ÅÄÖ!"),
        new IniValueToken("enumTest", "Bold, Italic"),
        new IniValueToken("blank", "")
      };

      // act
      target.Sort(2, 7, IniTokenComparer.Ordinal);

      // assert
      IniDocumentAssert.AreEqual(expected, target);
    }

    [Test]
    public void SortTest()
    {
      // arrange
      IniTokenCollection target;
      IniTokenCollection expected;

      target = IniTokenCollectionTests.TestCollection;

      expected = new IniTokenCollection
      {
        new IniValueToken("blank", ""),
        new IniValueToken("byteTest", "127"),
        new IniValueToken("colorTest", "Crimson"),
        new IniValueToken("doubleTest", "0.493128713218231"),
        new IniValueToken("enumTest", "Bold, Italic"),
        new IniValueToken("floatTest", "0.4982315"),
        new IniValueToken("fontTest", "Arial,13,\"Italic, Strikeout\""),
        new IniValueToken("intTest", "2147483647"),
        new IniValueToken("longTest", "9223372036854775807"),
        new IniValueToken("shortTest", "32767"),
        new IniValueToken("stringTest", "HELLO WORLD THIS IS A TEST STRING ÅÄÖ!")
      };

      // act
      target.Sort();

      // assert
      IniDocumentAssert.AreEqual(expected, target);
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