using System;
using System.IO;
using System.Text;

namespace Cyotek.Ini
{
  public abstract class IniToken : ICloneable
  {
    #region Overridden Methods

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

    #endregion

    #region Public Properties

    public virtual IniTokenCollection ChildTokens { get; protected set; }

    public string InnerText
    {
      get { return this.ToString(); }
    }

    public virtual string Name { get; set; }

    public abstract IniTokenType Type { get; }

    public virtual string Value { get; set; }

    #endregion

    #region Public Members

    public abstract IniToken Clone();

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

    #region ICloneable Members

    object ICloneable.Clone()
    {
      return this.Clone();
    }

    #endregion
  }
}
