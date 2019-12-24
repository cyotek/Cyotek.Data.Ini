using Cyotek.Ini;
using Cyotek.Testing;
using NUnit.Framework;
using System;
using System.IO;

namespace Cyotek.Core.Tests.Ini
{
  [TestFixture]
  internal class IniDocumentTests : TestBase
  {
    #region Protected Fields

    protected const string SampleIni = @"; this is a comment

# this is also a comment

[Settings]
longTest=9223372036854775807
shortTest=32767
stringTest=HELLO WORLD THIS IS A TEST STRING ÅÄÖ!
floatTest=0.4982315
intTest=2147483647
byteTest=127
doubleTest=0.493128713218231
  
[ham]
name=Hampus
value=0.75

[egg]
name=Eggbert
value=0.5


this is a bad value";

    #endregion Protected Fields

    #region Private Fields

    private const int _expectedMissingIndex = -1;

    #endregion Private Fields

    #region Public Methods

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
      IniDocument target;

      // act
      target = new IniDocument(this.GetDataFileName("settings.ini"));

      // assert
      this.CompareDocuments(target, this.GetSampleDocument());
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
      expectedSectionName = this.GetRandomString();
      expected = target.CreateSection(expectedSectionName);

      // act
      actual = target.CreateSection(expectedSectionName);

      // assert
      Assert.IsNotNull(actual);
      Assert.AreSame(expected, actual);
    }

    [Test]
    [ExpectedException(typeof(ArgumentNullException), ExpectedMessage = "Value cannot be null.\r\nParameter name: sectionName")]
    public void CreateSectionNullExceptionTest()
    {
      // arrange
      IniDocument target;

      target = new IniDocument();

      // act
      target.CreateSection(null);

      // assert
    }

    [Test]
    public void CreateSectionTest()
    {
      // arrange
      IniDocument target;
      IniToken actual;
      string expectedSectionName;

      target = new IniDocument();
      expectedSectionName = this.GetRandomString();

      // act
      actual = target.CreateSection(expectedSectionName);

      // assert
      Assert.IsNotNull(actual);
      Assert.AreEqual(expectedSectionName, actual.Name);
      Assert.AreEqual(IniTokenType.Section, actual.Type);
      Assert.AreNotEqual(_expectedMissingIndex, target.ChildTokens.IndexOf(expectedSectionName));
    }

    [Test]
    [ExpectedException(typeof(ArgumentNullException), ExpectedMessage = "Value cannot be null.\r\nParameter name: sectionName")]
    public void DeleteSectionInvalidNullSectionNameExceptionTest()
    {
      // arrange
      IniDocument target;

      target = new IniDocument();

      // act
      target.DeleteSection(null);

      // assert
    }

    [Test]
    public void DeleteSectionNegativeTest()
    {
      // arrange
      IniDocument target;
      string expectedSectionName;
      bool actual;

      target = new IniDocument();
      expectedSectionName = this.GetRandomString();

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
      expectedSectionName = this.GetRandomString();

      target.ChildTokens.Add(new IniSectionToken(expectedSectionName));

      // act
      actual = target.DeleteSection(expectedSectionName);

      // assert
      Assert.IsTrue(actual);
      Assert.AreEqual(_expectedMissingIndex, target.ChildTokens.IndexOf(expectedSectionName));
    }

    [Test]
    [ExpectedException(typeof(ArgumentNullException), ExpectedMessage = "Value cannot be null.\r\nParameter name: sectionName")]
    public void DeleteValueInvalidNullSectionNameExceptionTest()
    {
      // arrange
      IniDocument target;
      string expectedValueName;

      target = new IniDocument();
      expectedValueName = "Value";

      // act
      target.DeleteValue(null, expectedValueName);

      // assert
    }

    [Test]
    [ExpectedException(typeof(ArgumentNullException), ExpectedMessage = "Value cannot be null.\r\nParameter name: valueName")]
    public void DeleteValueInvalidNullValueNameExceptionTest()
    {
      // arrange
      IniDocument target;
      string expectedSectionName;

      target = new IniDocument();
      expectedSectionName = "Value";

      // act
      target.DeleteValue(expectedSectionName, null);

      // assert
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
      expectedSectionName = this.GetRandomString();
      expectedValueName = this.GetRandomString();

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
      expectedSectionName = this.GetRandomString();
      expectedValueName = this.GetRandomString();
      expectedValue = this.GetRandomString();

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
      expectedSectionName = this.GetRandomString();
      expectedValueName = this.GetRandomString();
      expectedValue = this.GetRandomString();

      target.SetValue(expectedSectionName, expectedValueName, expectedValue);

      // act
      actual = target.DeleteValue(expectedSectionName, expectedValueName);

      // assert
      Assert.IsTrue(actual);
      Assert.AreEqual(_expectedMissingIndex, target.ChildTokens[expectedSectionName].ChildTokens.IndexOf(expectedValueName));
    }

    [Test]
    [ExpectedException(typeof(InvalidDataException), ExpectedMessage = "A token named 'Section' already exists, but is not a section token.")]
    public void GetSectionInvalidSectionExceptionTest()
    {
      // arrange
      IniDocument target;
      string expectedSectionName;
      string expectedValue;

      target = new IniDocument();
      expectedSectionName = "Section";
      expectedValue = this.GetRandomString();

      target.ChildTokens.Add(new IniValueToken(expectedSectionName, expectedValue));

      // act
      target.GetSection(expectedSectionName);

      // assert
    }

    [Test]
    [ExpectedException(typeof(ArgumentNullException), ExpectedMessage = "Value cannot be null.\r\nParameter name: sectionName")]
    public void GetSectionNullExceptionTest()
    {
      // arrange
      IniDocument target;

      target = new IniDocument();

      // act
      target.GetSection(null);

      // assert
    }

    [Test]
    public void LoadIniTest()
    {
      // arrange
      IniDocument target;
      IniDocument expected;

      target = new IniDocument();
      expected = this.GetSampleDocument();

      // act
      target.LoadIni(SampleIni);

      // assert
      this.CompareDocuments(expected, target);
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
      expected = this.GetSampleDocument();

      // act
      using (Stream stream = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
      {
        target.Load(stream);
      }

      // assert
      this.CompareDocuments(expected, target);
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
      expected = this.GetSampleDocument();

      // act
      target.Load(fileName);

      // assert
      this.CompareDocuments(expected, target);
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
    public void SaveTest()
    {
      // arrange
      string workFile;
      IniDocument target;
      IniDocument actual;

      workFile = this.GetWorkFile();
      target = this.GetSampleDocument();

      // act
      try
      {
        target.Save(workFile);
      }
      finally
      {
        actual = new IniDocument();
        actual.Load(workFile);
        this.DeleteFile(workFile);
      }

      // assert
      this.CompareDocuments(target, actual);
    }

    [Test]
    [ExpectedException(typeof(ArgumentNullException), ExpectedMessage = "Value cannot be null.\r\nParameter name: name")]
    public void SetValueInvalidNullValueNameExceptionTest()
    {
      // arrange
      IniDocument target;
      string expectedSectionName;
      string expectedValue;

      target = new IniDocument();
      expectedSectionName = "Value";
      expectedValue = this.GetRandomString();

      // act
      target.SetValue(expectedSectionName, null, expectedValue);

      // assert
    }

    [Test]
    [ExpectedException(typeof(InvalidDataException), ExpectedMessage = "A token named 'Value' already exists, but is not a value token.")]
    public void SetValueInvalidValueExceptionTest()
    {
      // arrange
      IniDocument target;
      string expectedSectionName;
      string expectedValueName;
      string expectedValue;
      IniSectionToken sectionToken;

      target = new IniDocument();
      expectedSectionName = "Section";
      expectedValueName = "Value";
      expectedValue = this.GetRandomString();

      sectionToken = new IniSectionToken(expectedSectionName);
      target.ChildTokens.Add(sectionToken);

      sectionToken.ChildTokens.Add(new IniSectionToken(expectedValueName));

      // act
      target.SetValue(expectedSectionName, expectedValueName, expectedValue);

      // assert
    }

    [Test]
    public void SetValueStaticTest()
    {
      // arrange
      string fileName;
      string expectedSectionName;
      string expectedValueName;
      string expectedValue;
      IniDocument expected;

      fileName = this.GetWorkFile();
      expectedSectionName = this.GetRandomString();
      expectedValueName = this.GetRandomString();
      expectedValue = this.GetRandomString();

      // act
      try
      {
        IniDocument.SetValue(fileName, expectedSectionName, expectedValueName, expectedValue);
      }
      finally
      {
        expected = new IniDocument(fileName);
        expected.Load();
        this.DeleteFile(fileName);
      }

      // assert
      Assert.AreNotEqual(_expectedMissingIndex, expected.ChildTokens.IndexOf(expectedSectionName));
      Assert.AreNotEqual(_expectedMissingIndex, expected.ChildTokens[expectedSectionName].ChildTokens.IndexOf(expectedValueName));
      Assert.AreEqual(expectedValue, expected.ChildTokens[expectedSectionName].ChildTokens[expectedValueName].Value);
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
      expectedSectionName = this.GetRandomString();
      expectedValueName = this.GetRandomString();
      expectedValue = this.GetRandomString();

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
      expectedSectionName = this.GetRandomString();
      expectedValueName = this.GetRandomString();
      expectedValue = this.GetRandomString();
      target.SetValue(expectedSectionName, expectedValueName, this.GetRandomString());

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

      target = this.GetSampleDocument();
      expected = SampleIni;

      // act
      actual = target.ToString();

      // assert
      Assert.AreEqual(expected, actual);
    }

    #endregion Public Methods

    #region Protected Methods

    protected void CompareDocuments(IniDocument expected, IniDocument actual)
    {
      CyoAssert.AreEqual(expected, actual, "Ini", "FileName");
    }

    protected IniDocument GetSampleDocument()
    {
      IniDocument result;
      IniSectionToken section;

      result = new IniDocument();

      result.ChildTokens.Add(new IniCommentToken("; this is a comment"));
      result.ChildTokens.Add(new IniWhitespaceToken());
      result.ChildTokens.Add(new IniCommentToken("# this is also a comment"));
      result.ChildTokens.Add(new IniWhitespaceToken());

      section = new IniSectionToken("Settings");
      section.ChildTokens.Add(new IniValueToken("longTest", "9223372036854775807"));
      section.ChildTokens.Add(new IniValueToken("shortTest", "32767"));
      section.ChildTokens.Add(new IniValueToken("stringTest", "HELLO WORLD THIS IS A TEST STRING ÅÄÖ!"));
      section.ChildTokens.Add(new IniValueToken("floatTest", "0.4982315"));
      section.ChildTokens.Add(new IniValueToken("intTest", "2147483647"));
      section.ChildTokens.Add(new IniValueToken("byteTest", "127"));
      section.ChildTokens.Add(new IniValueToken("doubleTest", "0.493128713218231"));
      section.ChildTokens.Add(new IniWhitespaceToken("  "));
      result.ChildTokens.Add(section);

      section = new IniSectionToken("ham");
      section.ChildTokens.Add(new IniValueToken("name", "Hampus"));
      section.ChildTokens.Add(new IniValueToken("value", "0.75"));
      section.ChildTokens.Add(new IniWhitespaceToken());
      result.ChildTokens.Add(section);

      section = new IniSectionToken("egg");
      section.ChildTokens.Add(new IniValueToken("name", "Eggbert"));
      section.ChildTokens.Add(new IniValueToken("value", "0.5"));
      section.ChildTokens.Add(new IniWhitespaceToken());
      section.ChildTokens.Add(new IniWhitespaceToken());
      section.ChildTokens.Add(new IniRawToken("this is a bad value"));
      result.ChildTokens.Add(section);

      return result;
    }

    #endregion Protected Methods
  }
}