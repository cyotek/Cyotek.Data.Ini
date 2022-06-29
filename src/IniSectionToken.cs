using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;

namespace Cyotek.Data.Ini
{
  public class IniSectionToken : IniToken
  {
    #region Constructors

    public IniSectionToken()
      : this(string.Empty)
    {
    }

    public IniSectionToken(string name)
    {
      this.Name = name;
      this.ChildTokens = new IniTokenCollection();
    }

    #endregion

    #region Properties

    public override IniTokenType Type => IniTokenType.Section;

    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public override string Value
    {
      get => base.Value;
      set => throw new NotSupportedException();
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
      foreach(IniToken token in this.ChildTokens)
      {
        if(token.Type == IniTokenType.Value)
        {
          yield return token.Name;
        }
      }
    }

    public string GetValue(string valueName)
    {
      return this.GetValue(valueName, string.Empty);
    }

    public string GetValue(string name, string defaultValue)
    {
      IniToken valueToken;

      valueToken = this.GetValueToken(name);

      return valueToken == null ? defaultValue : valueToken.Value;
    }

    public IniValueToken GetValueToken(string name)
    {
      this.ChildTokens.TryGetValue(name, out IniToken valueToken);

      if (valueToken != null && valueToken.Type != IniTokenType.Value)
      {
        throw new InvalidDataException(string.Format("A token named '{0}' already exists, but is not a value token.", name));
      }

      return (IniValueToken)valueToken;
    }

    public bool SetValue(string name, string value)
    {
      bool result;
      IniTokenCollection children;

      if (string.IsNullOrEmpty(name))
      {
        throw new ArgumentNullException(nameof(name));
      }

      children = this.ChildTokens;

      if (!children.TryGetValue(name, out IniToken valueToken))
      {
        int index;

        index = IniSectionToken.GetInsertionIndex(children);

        if (index != -1)
        {
          children.Insert(index, new IniValueToken(name, value));
        }
        else
        {
          children.Add(new IniValueToken(name, value));
        }
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

    private static int GetInsertionIndex(IniTokenCollection children)
    {
      int index;

      index = -1;

      for (int i = children.Count - 1; i >= 0; i--)
      {
        if (children[i].Type == IniTokenType.Whitespace || children[i].Type == IniTokenType.Comment)
        {
          index = i;
        }
        else
        {
          break;
        }
      }

      return index;
    }

    public override void Write(TextWriter writer)
    {
      writer.WriteLine(string.Concat("[", this.Name, "]"));

      base.Write(writer);

      if (this.AreAllChildrenValues())
      {
        writer.WriteLine();
      }
    }

    private bool AreAllChildrenValues()
    {
      bool result;

      result = true;

      foreach(IniToken token in this.ChildTokens)
      {
        if(token.Type!= IniTokenType.Value)
        {
          result = false;
          break;
        }
      }

      return result;
    }

    #endregion
  }
}
