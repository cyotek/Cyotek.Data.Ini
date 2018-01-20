﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;

namespace Cyotek.Ini
{
  public class IniSectionToken : IniToken
  {
    #region Constructors

    public IniSectionToken()
      : this(string.Empty)
    { }

    public IniSectionToken(string name)
    {
      this.Name = name;
      this.ChildTokens = new IniTokenCollection();
    }

    #endregion

    #region Properties

    public override IniTokenType Type
    {
      get { return IniTokenType.Section; }
    }

    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public override string Value
    {
      get { return base.Value; }
      set { throw new NotSupportedException(); }
    }

    #endregion

    #region Methods

    public override IniToken Clone()
    {
      IniSectionToken result;

      result = new IniSectionToken();
      foreach (IniToken token in this.ChildTokens)
      {
        result.ChildTokens.Add(token.Clone());
      }

      return result;
    }

    public IEnumerable<string> GetNames()
    {
      return from token in this.ChildTokens
             where token.Type == IniTokenType.Value
             select token.Name;
    }

    public string GetValue(string valueName)
    {
      return this.GetValue(valueName, string.Empty);
    }

    public string GetValue(string name, string defaultValue)
    {
      IniToken valueToken;
      string result;

      valueToken = this.GetValueToken(name);
      result = valueToken == null ? defaultValue : valueToken.Value;

      return result;
    }

    public IniValueToken GetValueToken(string name)
    {
      IniToken valueToken;

      this.ChildTokens.TryGetValue(name, out valueToken);

      if (valueToken != null && valueToken.Type != IniTokenType.Value)
      {
        throw new InvalidDataException(string.Format("A token named '{0}' already exists, but is not a value token.", name));
      }

      return (IniValueToken)valueToken;
    }

    public bool SetValue(string name, string value)
    {
      IniToken valueToken;
      bool result;

      if (string.IsNullOrEmpty(name))
      {
        throw new ArgumentNullException(nameof(name));
      }

      if (!this.ChildTokens.TryGetValue(name, out valueToken))
      {
        this.ChildTokens.Add(new IniValueToken(name, value));
        result = true;
      }
      else if (valueToken.Type != IniTokenType.Value)
      {
        throw new InvalidDataException(string.Format("A token named '{0}' already exists, but is not a value token.", name));
      }
      else if (!string.Equals(valueToken.Value, value))
      {
        valueToken.Value = value;
        result = true;
      }
      else
      {
        result = false;
      }

      return result;
    }

    public override void Write(TextWriter writer)
    {
      writer.WriteLine(string.Concat("[", this.Name, "]"));

      base.Write(writer);

      if (this.ChildTokens.All(t => t.Type == IniTokenType.Value))
      {
        writer.WriteLine();
      }
    }

    #endregion
  }
}
