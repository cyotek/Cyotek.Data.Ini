﻿using System;
using System.ComponentModel;
using System.IO;

namespace Cyotek.Ini
{
  public class IniCommentToken : IniToken
  {
    #region Public Constructors

    public IniCommentToken()
      : this(null)
    { }

    public IniCommentToken(string value)
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
      get { return IniTokenType.Comment; }
    }

    #endregion

    #region Overridden Methods

    public override void Write(TextWriter writer)
    {
      if (!this.Value.StartsWithAny(IniDocument.DefaultCommentCharacters))
      {
        writer.Write(IniDocument.DefaultCommentCharacters[0]);
        writer.Write(Characters.Space);
      }

      writer.WriteLine(this.Value);

      base.Write(writer);
    }

    #endregion
  }
}
