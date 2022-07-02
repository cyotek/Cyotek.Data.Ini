namespace Cyotek.Data.Ini.Tests
{
  internal static class TestData
  {
    #region Public Properties

    public static IniDocument SampleDocument
    {
      get
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
        section.ChildTokens.Add(new IniValueToken("colorTest", "Crimson"));
        section.ChildTokens.Add(new IniValueToken("fontTest", "Arial,13,\"Italic, Strikeout\""));
        section.ChildTokens.Add(new IniValueToken("enumTest", "Bold, Italic"));
        section.ChildTokens.Add(new IniValueToken("blank", string.Empty));
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
    }

    public static string SampleIni = @"; this is a comment

# this is also a comment

[Settings]
longTest=9223372036854775807
shortTest=32767
stringTest=HELLO WORLD THIS IS A TEST STRING ÅÄÖ!
floatTest=0.4982315
intTest=2147483647
byteTest=127
doubleTest=0.493128713218231
colorTest=Crimson
fontTest=Arial,13,""Italic, Strikeout""
enumTest=Bold, Italic
blank=
  
[ham]
name=Hampus
value=0.75

[egg]
name=Eggbert
value=0.5


this is a bad value";

    #endregion Public Properties
  }
}