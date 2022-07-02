using System.IO;

namespace Cyotek.Data.Ini.Tests
{
  internal sealed class CustomIniToken : IniToken
  {
    #region Public Properties

    public override IniTokenType Type => (IniTokenType)99;

    #endregion Public Properties

    #region Public Methods

    public override IniToken Clone()
    {
      return new CustomIniToken
      {
        Name = this.Name,
        Value = this.Value
      };
    }

    public override void Write(TextWriter writer)
    {
      writer.WriteLine(this.Name + "=" + this.Value.ToEscapedLiteral());

      base.Write(writer);
    }

    #endregion Public Methods
  }
}