﻿using System.IO;

namespace Cyotek.Ini
{
  public class IniValueToken : IniToken
  {
    #region Public Constructors

    public IniValueToken()
    { }

    public IniValueToken(string name, string value)
      : this()
    {
      this.Name = name;
      this.Value = value;
    }

    #endregion

    #region Overridden Properties

    public override IniTokenType Type
    {
      get { return IniTokenType.Value; }
    }

    #endregion

    #region Overridden Methods

    public override void Write(TextWriter writer)
    {
      writer.WriteLine(string.Concat(this.Name, "=", this.Value.ToEscapedLiteral()));

      base.Write(writer);
    }

    #endregion
  }
}