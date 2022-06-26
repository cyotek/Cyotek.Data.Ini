using System.Collections.Generic;
using System.Reflection;
using Cyotek.Ini;
using Cyotek.Testing;
using NUnit.Framework;

namespace Cyotek.Data.Ini.Tests
{
  [TestFixture]
  internal class IniTokenCollectionTests : TestBase
  {
    #region  Tests

    [Test]
    public void IndexOf_returns_index_by_name()
    {
      // arrange
      IniTokenCollection target;
      int expected;
      int actual;

      target = new IniTokenCollection();
      target.Add(new IniSectionToken("Alpha"));
      target.Add(new IniSectionToken("Beta"));
      target.Add(new IniSectionToken("Gamma"));

      expected = 1;

      // act
      actual = target.IndexOf("Beta");

      // assert
      Assert.AreEqual(expected, actual);
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
    public void IndexOfNegativeTest()
    {
      // arrange
      IniTokenCollection target;
      int expected;
      int actual;

      target = new IniTokenCollection();
      target.Add(new IniSectionToken("Alpha"));
      target.Add(new IniSectionToken("Beta"));

      expected = -1;

      // act
      actual = target.IndexOf("Gamma");

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
      // This means that if items are removed, renamed or the list is reorded, the lookup will be invalid. It should
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

    #endregion
  }
}
