using System;
using System.ComponentModel;
using System.IO;

namespace Cyotek.Ini
{
  public class IniRawToken : IniToken
  {
    #region Public Constructors

    public IniRawToken()
      : this(string.Empty)
    { }

    public IniRawToken(string value)
    {
      this.Value = value;
    }

    #endregion

    #region Overridden Properties

    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public override string Name
    {
      get { return base.Name; }
      set { throw new NotSupportedException(); }
    }

    public override IniTokenType Type
    {
      get { return IniTokenType.Whitespace; }
    }

    #endregion

    #region Overridden Methods

    public override void Write(TextWriter writer)
    {
      writer.WriteLine(this.Value);

      base.Write(writer);
    }

    #endregion
  }
}
