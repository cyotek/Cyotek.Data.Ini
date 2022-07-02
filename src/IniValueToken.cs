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
  public class IniValueToken : IniToken
  {
    #region Constructors

    public IniValueToken()
    {
    }

    public IniValueToken(string name, string value)
      : this()
    {
      this.Name = name;
      this.Value = value;
    }

    #endregion

    #region Properties

    public override IniTokenType Type => IniTokenType.Value;

    #endregion

    #region Methods

    public override IniToken Clone()
    {
      return new IniValueToken(this.Name, this.Value);
    }

    public override void Write(TextWriter writer)
    {
      writer.WriteLine(this.Name + "=" + this.Value.ToEscapedLiteral());

      base.Write(writer);
    }

    #endregion
  }
}
