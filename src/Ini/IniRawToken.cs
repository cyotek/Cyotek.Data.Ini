﻿using System;
using System.ComponentModel;
using System.IO;

namespace Cyotek.Ini
{
  public class IniRawToken : IniToken
  {
    #region Constructors

    public IniRawToken()
      : this(string.Empty)
    { }

    public IniRawToken(string value)
    {
      this.Value = value;
    }

    #endregion

    #region Properties

    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public override string Name
    {
      get { return base.Name; }
      set { throw new NotSupportedException(); }
    }

    public override IniTokenType Type
    {
      get { return IniTokenType.Raw; }
    }

    #endregion

    #region Methods

    public override IniToken Clone()
    {
      return new IniRawToken(this.Value);
    }

    public override void Write(TextWriter writer)
    {
      writer.WriteLine(this.Value);

      base.Write(writer);
    }

    #endregion
  }
}