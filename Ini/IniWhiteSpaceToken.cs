using System;
using System.ComponentModel;
using System.IO;
using System.Linq;

namespace Cyotek.Ini
{
  public class IniWhitespaceToken : IniToken
  {
    #region Instance Fields

    private string _value;

    #endregion

    #region Public Constructors

    public IniWhitespaceToken()
      : this(string.Empty)
    { }

    public IniWhitespaceToken(string value)
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

    public override string Value
    {
      get { return _value; }
      set
      {
        if (!string.IsNullOrEmpty(value) && !value.All(char.IsWhiteSpace))
        {
          throw new ArgumentException("Value can only contain whitespace characters.", "value");
        }

        _value = value;
      }
    }

    #endregion

    #region Overridden Methods

    public override IniToken Clone()
    {
      return new IniWhitespaceToken(this.Value);
    }

    /// <summary>
    /// Returns a string that represents the current object.
    /// </summary>
    /// <returns>
    /// A string that represents the current object.
    /// </returns>
    public override string ToString()
    {
      return this.Value;
    }

    public override void Write(TextWriter writer)
    {
      writer.WriteLine(this.ToString());

      base.Write(writer);
    }

    #endregion
  }
}
