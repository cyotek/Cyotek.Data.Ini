﻿using System;
using System.ComponentModel;
using System.IO;

namespace Cyotek.Data.Ini
{
  public class IniWhitespaceToken : IniToken
  {
    #region Fields

    private string _value;

    #endregion

    #region Constructors

    public IniWhitespaceToken()
      : this(string.Empty)
    {
    }

    public IniWhitespaceToken(string value)
    {
      this.Value = value;
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

    public override IniTokenType Type => IniTokenType.Whitespace;

    public override string Value
    {
      get => _value;
      set
      {
#if NET452_OR_GREATER || NETSTANDARD || NET
        if (!string.IsNullOrWhiteSpace(value))
#else
        if (value != null && !string.IsNullOrEmpty(value.Trim()))
#endif
        {
          throw new ArgumentException("Value can only contain whitespace characters.", nameof(value));
        }

        _value = value;
      }
    }

    #endregion

    #region Methods

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
