using Cyotek.Testing;
using NUnit.Framework;
using System;
using System.IO;
using System.Reflection;

namespace Cyotek.Data.Ini.Tests
{
  [TestFixture]
  internal class IniDocumentTests : TestBase
  {
    #region Private Fields

    private const int _expectedMissingIndex = -1;

    #endregion Private Fields

    #region Public Methods

    [Test]
    public void CloneObjectTest()
    {
      this.RunCloneObjectTest(TestData.SampleDocument);
    }

    [Test]
    public void CloneTest()
    {
      this.RunCloneTest(TestData.SampleDocument);
    }

    [Test]
    public void ConstructorTest()
    {
      // arrange
      IniDocument target;

      // act
      target = new IniDocument();

      // assert
      Assert.IsNotNull(target);
    }

    [Test]
    public void ConstructorWithFileNameTest()
    {
      // arrange
      IniDocument expected;
      IniDocument actual;
      string fileName;

      expected = TestData.SampleDocument;

      fileName = this.GetDataFileName("settings.ini");

      // act
      actual = new IniDocument(fileName);

      // assert
      Assert.AreEqual(fileName, actual.FileName);
      IniAssert.AreEqual(expected, actual);
    }

    [Test]
    public void CreateSectionExceptionTest()
    {
      // arrange
      IniDocument target;
      string expectedSectionName;

      expectedSectionName = "beta";

      target = new IniDocument();
      target.ChildTokens.Add(new IniValueToken(expectedSectionName, ""));

      // act & assert
      Assert.Throws<InvalidDataException>(() => target.CreateSection(expectedSectionName));
    }

    [Test]
    public void CreateSectionExistingTest()
    {
      // arrange
      IniDocument target;
      IniToken expected;
      IniToken actual;
      string expectedSectionName;

      target = new IniDocument();
      expectedSectionName = "alpha";
      expected = target.CreateSection(expectedSectionName);

      // act
      actual = target.CreateSection(expectedSectionName);

      // assert
      Assert.IsNotNull(actual);
      Assert.AreSame(expected, actual);
    }

    [Test]
    public void CreateSectionNullExceptionTest()
    {
      // arrange
      IniDocument target;

      target = new IniDocument();

      // act & assert
      Assert.Throws<ArgumentNullException>(() => target.CreateSection(null));
    }

    [Test]
    public void CreateSectionTest()
    {
      // arrange
      IniDocument target;
      IniToken actual;
      string expectedSectionName;

      target = new IniDocument();
      expectedSectionName = "beta";

      // act
      actual = target.CreateSection(expectedSectionName);

      // assert
      Assert.IsNotNull(actual);
      Assert.AreEqual(expectedSectionName, actual.Name);
      Assert.AreEqual(IniTokenType.Section, actual.Type);
      Assert.AreNotEqual(_expectedMissingIndex, target.ChildTokens.IndexOf(expectedSectionName));
    }

    [Test]
    public void CreateTokenExceptionTest()
    {
      // arrange
      IniDocument target;
      MethodInfo method;
      Exception actual;

      target = new IniDocument();

      method = typeof(IniDocument).GetMethod("CreateToken", BindingFlags.Instance | BindingFlags.NonPublic);

      // act & assert
      actual = Assert.Throws<TargetInvocationException>(() => method.Invoke(target, new object[] { (IniTokenType)(-1), "" }));
      Assert.IsInstanceOf<ArgumentOutOfRangeException>(actual.InnerException);
    }

    [Test]
    public void DeleteSectionInvalidNullSectionNameExceptionTest()
    {
      // arrange
      IniDocument target;

      target = new IniDocument();

      // act & assert
      Assert.Throws<ArgumentNullException>(() => target.DeleteSection(null));
    }

    [Test]
    public void DeleteSectionNegativeTest()
    {
      // arrange
      IniDocument target;
      string expectedSectionName;
      bool actual;

      target = new IniDocument();
      expectedSectionName = "gamma";

      // act
      actual = target.DeleteSection(expectedSectionName);

      // assert
      Assert.IsFalse(actual);
    }

    [Test]
    public void DeleteSectionTest()
    {
      // arrange
      IniDocument target;
      string expectedSectionName;
      bool actual;

      target = new IniDocument();
      expectedSectionName = "delta";

      target.ChildTokens.Add(new IniSectionToken(expectedSectionName));

      // act
      actual = target.DeleteSection(expectedSectionName);

      // assert
      Assert.IsTrue(actual);
      Assert.AreEqual(_expectedMissingIndex, target.ChildTokens.IndexOf(expectedSectionName));
    }

    [Test]
    public void DeleteValueInvalidNullSectionNameExceptionTest()
    {
      // arrange
      IniDocument target;
      string expectedValueName;

      target = new IniDocument();
      expectedValueName = "Value";

      // act & assert
      Assert.Throws<ArgumentNullException>(() => target.DeleteValue(null, expectedValueName));
    }

    [Test]
    public void DeleteValueInvalidNullValueNameExceptionTest()
    {
      // arrange
      IniDocument target;
      string expectedSectionName;

      target = new IniDocument();
      expectedSectionName = "Value";

      // act & assert
      Assert.Throws<ArgumentNullException>(() => target.DeleteValue(expectedSectionName, null));
    }

    [Test]
    public void DeleteValueMissingSectionTest()
    {
      // arrange
      IniDocument target;
      string expectedSectionName;
      string expectedValueName;
      bool actual;

      target = new IniDocument();
      expectedSectionName = "epsilon";
      expectedValueName = "zeta";

      // act
      actual = target.DeleteValue(expectedSectionName, expectedValueName);

      // assert
      Assert.IsFalse(actual);
    }

    [Test]
    public void DeleteValueNegativeTest()
    {
      // arrange
      IniDocument target;
      string expectedSectionName;
      string expectedValueName;
      string expectedValue;
      bool actual;

      target = new IniDocument();
      expectedSectionName = "eta";
      expectedValueName = "theta";
      expectedValue = "iota";

      target.SetValue(expectedSectionName, expectedValueName, expectedValue);
      target.DeleteValue(expectedSectionName, expectedValueName);

      // act
      actual = target.DeleteValue(expectedSectionName, expectedValueName);

      // assert
      Assert.IsFalse(actual);
    }

    [Test]
    public void DeleteValueTest()
    {
      // arrange
      IniDocument target;
      string expectedSectionName;
      string expectedValueName;
      string expectedValue;
      bool actual;

      target = new IniDocument();
      expectedSectionName = "kappa";
      expectedValueName = "lambda";
      expectedValue = "mu";

      target.SetValue(expectedSectionName, expectedValueName, expectedValue);

      // act
      actual = target.DeleteValue(expectedSectionName, expectedValueName);

      // assert
      Assert.IsTrue(actual);
      Assert.AreEqual(_expectedMissingIndex, target.ChildTokens[expectedSectionName].ChildTokens.IndexOf(expectedValueName));
    }

    [Test]
    public void FileNameTest()
    {
      // arrange
      IniDocument target;
      string expected;
      string actual;

      target = new IniDocument();

      expected = "alpha.ini";

      // act
      target.FileName = expected;

      // assert
      actual = target.FileName;
      Assert.AreEqual(expected, actual);
    }

    [Test]
    public void GetSectionInvalidSectionExceptionTest()
    {
      // arrange
      IniDocument target;
      string expectedSectionName;
      string expectedValue;

      target = new IniDocument();
      expectedSectionName = "Section";
      expectedValue = "nu";

      target.ChildTokens.Add(new IniValueToken(expectedSectionName, expectedValue));

      // TODO: Check this makes sense

      // act & assert
      Assert.Throws<InvalidDataException>(() => target.GetSection(expectedSectionName));
    }

    [Test]
    public void GetSectionNullExceptionTest()
    {
      // arrange
      IniDocument target;

      target = new IniDocument();

      // act & assert
      Assert.Throws<ArgumentNullException>(() => target.GetSection(null));
    }

    [TestCase("Settings", "shortTest", null, "32767", TestName = "{m}")]
    [TestCase("OldSettings", "shortTest", null, null, TestName = "{m}MissingSection")]
    [TestCase("Settings", "quickTest", null, null, TestName = "{m}MissingValue")]
    [TestCase("Settings", "fallbackTest", "alpha", "alpha", TestName = "{m}Fallback")]
    public void GetValueFallbackTestCases(string section, string name, string defaultValue, string expected)
    {
      // arrange
      IniDocument target;
      string actual;

      target = TestData.SampleDocument;

      // act
      actual = target.GetValue(section, name, defaultValue);

      // assert
      Assert.AreEqual(expected, actual);
    }

    [Test]
    public void GetValueNullExceptionTest()
    {
      Assert.Throws<ArgumentNullException>(() => new IniDocument().GetValue("alpha", null));
    }

    [TestCase("Settings", "shortTest", null, "32767", TestName = "{m}")]
    [TestCase("OldSettings", "shortTest", null, null, TestName = "{m}MissingSection")]
    [TestCase("Settings", "quickTest", null, null, TestName = "{m}MissingValue")]
    [TestCase("Settings", "fallbackTest", "alpha", "alpha", TestName = "{m}Fallback")]
    public void GetValueStaticTestCases(string section, string name, string defaultValue, string expected)
    {
      // arrange
      string fileName;
      string actual;

      fileName = this.GetDataFileName("settings.ini");

      // act
      actual = IniDocument.GetValue(fileName, section, name, defaultValue);

      // assert
      Assert.AreEqual(expected, actual);
    }

    [TestCase("Settings", "shortTest", "32767", TestName = "{m}")]
    [TestCase("OldSettings", "shortTest", "", TestName = "{m}MissingSection")]
    [TestCase("Settings", "quickTest", "", TestName = "{m}MissingValue")]
    public void GetValueTestCases(string section, string name, string expected)
    {
      // arrange
      IniDocument target;
      string actual;

      target = TestData.SampleDocument;

      // act
      actual = target.GetValue(section, name);

      // assert
      Assert.AreEqual(expected, actual);
    }

    [Test]
    public void LoadIniTest()
    {
      // arrange
      IniDocument target;
      IniDocument expected;

      target = new IniDocument();
      expected = TestData.SampleDocument;

      // act
      target.LoadIni(TestData.SampleIni);

      // assert
      IniAssert.AreEqual(expected, target);
    }

    [Test]
    public void LoadStreamTest()
    {
      // arrange
      IniDocument target;
      IniDocument expected;
      string fileName;

      fileName = this.GetDataFileName("settings.ini");
      target = new IniDocument();
      expected = TestData.SampleDocument;

      // act
      using (Stream stream = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
      {
        target.Load(stream);
      }

      // assert
      IniAssert.AreEqual(expected, target);
    }

    [Test]
    public void LoadTest()
    {
      // arrange
      IniDocument target;
      IniDocument expected;
      string fileName;

      fileName = this.GetDataFileName("settings.ini");
      target = new IniDocument();
      expected = TestData.SampleDocument;

      // act
      target.Load(fileName);

      // assert
      IniAssert.AreEqual(expected, target);
    }

    [Test]
    public void RawLoadTest()
    {
      // arrange
      IniDocument target;
      string expected;
      string actual;
      StringWriter output;

      expected = @"[Meta]
alpha=beta

[Tags]
gamma
delta
epsilon";

      target = new IniDocument();
      target.LoadIni(expected);

      output = new StringWriter();

      // act
      target.Save(output);

      // assert
      actual = output.ToString();
      output.Dispose();
      Assert.AreEqual(expected, actual);
    }

    [Test]
    public void RawSaveTest()
    {
      // arrange
      IniDocument target;
      string expected;
      string actual;
      StringWriter output;
      IniSectionToken tags;

      target = new IniDocument();
      target.SetValue("Meta", "alpha", "beta");

      tags = (IniSectionToken)target.CreateSection("Tags");
      tags.ChildTokens.Add(new IniRawToken("gamma"));
      tags.ChildTokens.Add(new IniRawToken("delta"));
      tags.ChildTokens.Add(new IniRawToken("epsilon"));

      output = new StringWriter();

      expected = @"[Meta]
alpha=beta

[Tags]
gamma
delta
epsilon";

      // act
      target.Save(output);

      // assert
      actual = output.ToString();
      output.Dispose();
      Assert.AreEqual(expected, actual);
    }

    [Test]
    public void Save_WithHiddenFile_Test()
    {
      using (TemporaryFile workFile = new TemporaryFile())
      {
        // arrange
        IniDocument target;
        IniDocument actual;

        target = TestData.SampleDocument;
        target.Save(workFile.FileName);
        target.SetValue("Settings", "stringTest", "SAD FACE");

        File.SetAttributes(workFile.FileName, FileAttributes.Hidden);

        // act
        target.Save(workFile.FileName);

        // assert
        actual = new IniDocument();
        actual.Load(workFile.FileName);
        Assert.IsTrue((File.GetAttributes(workFile.FileName) & FileAttributes.Hidden) != 0);
        IniAssert.AreEqual(target, actual);
      }
    }

    [Test]
    public void SaveFileNameExceptionTest()
    {
      Assert.Throws<ArgumentNullException>(() => new IniDocument().Save((string)null));
    }

    [Test]
    public void SaveStreamExceptionTest()
    {
      Assert.Throws<ArgumentNullException>(() => new IniDocument().Save((Stream)null));
    }

    [Test]
    public void SaveTest()
    {
      using (TemporaryFile workFile = new TemporaryFile())
      {
        // arrange
        IniDocument target;
        IniDocument actual;

        target = TestData.SampleDocument;

        // act
        target.Save(workFile.FileName);

        // assert
        actual = new IniDocument();
        actual.Load(workFile.FileName);
        IniAssert.AreEqual(target, actual);
      }
    }

    [Test]
    public void SaveTextWriterExceptionTest()
    {
      Assert.Throws<ArgumentNullException>(() => new IniDocument().Save((TextWriter)null));
    }

    [Test]
    public void SetChildTokensTest()
    {
      // arrange
      IniDocument target;
      PropertyInfo property;
      IniTokenCollection expected;

      target = new IniDocument();

      property = typeof(IniDocument).GetProperty(nameof(IniDocument.ChildTokens));

      expected = new IniTokenCollection();

      // act
      property.SetValue(target, expected, null);

      // assert
      Assert.AreSame(expected, target.ChildTokens);
    }

    [Test]
    public void SetValue_WithExistingWhitespace_InsertsBefore()
    {
      // arrange
      IniDocument target;
      string expected;
      string actual;
      StringWriter writer;

      target = new IniDocument();
      target.LoadIni(@"[Meta]
Created=2019-12-24T21:04:13.5777065+00:00
Modified=2019-12-24T21:42:36.1605544Z
Options=None

[Properties]
[Tags]
animation
gif");

      writer = new StringWriter();

      expected = @"[Meta]
Created=2019-12-24T21:04:13.5777065+00:00
Modified=2019-12-24T21:42:36.1605544Z
Options=None

[Properties]
alpha=beta

[Tags]
animation
gif";

      // act
      target.SetValue("Properties", "alpha", "beta");

      // assert
      target.Save(writer);
      actual = writer.ToString();
      Assert.AreEqual(expected, actual);
    }

    [Test]
    public void SetValueStaticTest()
    {
      using (TemporaryFile workFile = new TemporaryFile())
      {
        // arrange
        string expectedSectionName;
        string expectedValueName;
        string expectedValue;
        IniDocument actual;

        expectedSectionName = "pi";
        expectedValueName = "rho";
        expectedValue = "sigma";

        // act
        IniDocument.SetValue(workFile.FileName, expectedSectionName, expectedValueName, expectedValue);

        // assert
        actual = new IniDocument(workFile.FileName);
        actual.Load();
        Assert.AreNotEqual(_expectedMissingIndex, actual.ChildTokens.IndexOf(expectedSectionName));
        Assert.AreNotEqual(_expectedMissingIndex, actual.ChildTokens[expectedSectionName].ChildTokens.IndexOf(expectedValueName));
        Assert.AreEqual(expectedValue, actual.ChildTokens[expectedSectionName].ChildTokens[expectedValueName].Value);
      }
    }

    [Test]
    public void SetValueTest()
    {
      // arrange
      IniDocument target;
      string expectedSectionName;
      string expectedValueName;
      string expectedValue;

      target = new IniDocument();
      expectedSectionName = "tau";
      expectedValueName = "upsilon";
      expectedValue = "phi";

      // act
      target.SetValue(expectedSectionName, expectedValueName, expectedValue);

      // assert
      Assert.AreNotEqual(_expectedMissingIndex, target.ChildTokens.IndexOf(expectedSectionName));
      Assert.AreNotEqual(_expectedMissingIndex, target.ChildTokens[expectedSectionName].ChildTokens.IndexOf(expectedValueName));
      Assert.AreEqual(expectedValue, target.ChildTokens[expectedSectionName].ChildTokens[expectedValueName].Value);
    }

    [Test]
    public void SetValueUpdateTest()
    {
      // arrange
      IniDocument target;
      string expectedSectionName;
      string expectedValueName;
      string expectedValue;

      target = new IniDocument();
      expectedSectionName = "chi";
      expectedValueName = "psi";
      expectedValue = "omega";
      target.SetValue(expectedSectionName, expectedValueName, "alpha");

      // act
      target.SetValue(expectedSectionName, expectedValueName, expectedValue);

      // assert
      Assert.AreEqual(expectedValue, target.ChildTokens[expectedSectionName].ChildTokens[expectedValueName].Value);
    }

    [Test]
    public void SimpleBreaksTest()
    {
      // arrange
      IniDocument target;
      string actual;
      string expected;

      target = new IniDocument();
      target.SetValue("Alpha", "Value1", "One");
      target.SetValue("Alpha", "Value2", "Two");
      target.SetValue("Beta", "Value1", "Three");

      expected = @"[Alpha]
Value1=One
Value2=Two

[Beta]
Value1=Three";

      // act
      actual = target.ToString();

      // assert
      Assert.AreEqual(expected, actual);
    }

    [Test]
    public void ToStringTest()
    {
      // arrange
      IniDocument target;
      string expected;
      string actual;

      target = TestData.SampleDocument;
      expected = TestData.SampleIni;

      // act
      actual = target.ToString();

      // assert
      Assert.AreEqual(expected, actual);
    }

    #endregion Public Methods
  }
}