using System;
using System.ComponentModel;
using System.IO;

// Cyotek Ini Reader / Writer Library
// https://github.com/cyotek/Cyotek.Data.Ini

// Copyright © 2014-2022 Cyotek Ltd.

// This work is licensed under the MIT License.
// See LICENSE.TXT for the full text

// Found this code useful?
// https://www.cyotek.com/contribute

namespace Cyotek.Data.Ini
{
  public class IniRawToken : IniToken
  {
    #region Constructors

    public IniRawToken()
      : this(null)
    {
    }

    public IniRawToken(string value)
      : base(value)
    {
    }

    #endregion

    #region Properties

    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public override string Name
    {
      get => base.Name;
      set => throw new NotSupportedException();
    }

    public override IniTokenType Type => IniTokenType.Raw;

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
