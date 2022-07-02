using System;
using System.IO;
using System.Text;

// Cyotek Ini Reader / Writer Library
// https://github.com/cyotek/Cyotek.Data.Ini

// Copyright © 2014-2022 Cyotek Ltd.

// This work is licensed under the MIT License.
// See LICENSE.TXT for the full text

// Found this code useful?
// https://www.cyotek.com/contribute

namespace Cyotek.Data.Ini
{
  public abstract class IniToken : ICloneable
  {
    #region Properties

    public virtual IniTokenCollection ChildTokens { get; protected set; }

    public string InnerText => this.ToString();

    public virtual string Name { get; set; }

    public abstract IniTokenType Type { get; }

    public virtual string Value { get; set; }

    protected IniToken()
    {

    }

    protected IniToken(string value)
    {
      this.Value = value;
    }

    #endregion

    #region Methods

    public abstract IniToken Clone();

    /// <summary>
    /// Returns a string that represents the current object.
    /// </summary>
    /// <returns>
    /// A string that represents the current object.
    /// </returns>
    public override string ToString()
    {
      StringBuilder result;

      result = StringBuilderCache.Acquire();

      using (StringWriter writer = new StringWriter(result))
      {
        this.Write(writer);
      }

      result.RemoveTrailingLinebreaks();

      return result.ToStringAndRelease();
    }

    public virtual void Write(TextWriter writer)
    {
      if (this.ChildTokens != null)
      {
        foreach (IniToken token in this.ChildTokens)
        {
          token.Write(writer);
        }
      }
    }

    #endregion

    #region ICloneable Interface

    object ICloneable.Clone()
    {
      return this.Clone();
    }

    #endregion
  }
}
