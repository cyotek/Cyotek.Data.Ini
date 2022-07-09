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
  public class IniDocument : IniToken
  {
    #region Public Fields

    public static readonly char[] DefaultCommentCharacters =
    {
      ';',
      '#'
    };

    #endregion Public Fields

    #region Private Fields

    private string _fileName;

    private bool _isLoadDeferred;

    #endregion Private Fields

    #region Public Constructors

    public IniDocument()
    {
      base.ChildTokens = new IniTokenCollection();
    }

    public IniDocument(string fileName)
      : this()
    {
      _isLoadDeferred = true;
      _fileName = fileName;
    }

    #endregion Public Constructors

    #region Public Properties

    public override IniTokenCollection ChildTokens
    {
      get
      {
        if (_isLoadDeferred)
        {
          _isLoadDeferred = false;

          if (File.Exists(_fileName))
          {
            this.Load(_fileName);
          }
        }

        return base.ChildTokens;
      }
      protected set => base.ChildTokens = value;
    }

    public string FileName
    {
      get => _fileName;
      set => _fileName = value;
    }

    public override IniTokenType Type => IniTokenType.Document;

    #endregion Public Properties

    #region Protected Properties

    protected IniTokenCollection CurrentTokenCollection { get; set; }

    #endregion Protected Properties

    #region Public Methods

    public static string GetValue(string fileName, string sectionName, string valueName, string defaultValue)
    {
      return new IniDocument(fileName).GetValue(sectionName, valueName, defaultValue);
    }

    public static void SetValue(string fileName, string sectionName, string valueName, string value)
    {
      IniDocument document;

      document = new IniDocument(fileName);

      document.SetValue(sectionName, valueName, value);
      document.Save();
    }

    public override IniToken Clone()
    {
      IniDocument result;

      result = new IniDocument();

      foreach (IniToken token in this.ChildTokens)
      {
        result.ChildTokens.Add(token.Clone());
      }

      return result;
    }

    public IniToken CreateSection(string sectionName)
    {
      if (string.IsNullOrEmpty(sectionName))
      {
        throw new ArgumentNullException(nameof(sectionName));
      }

      if (!this.ChildTokens.TryGetValue(sectionName, out IniToken sectionToken))
      {
        sectionToken = new IniSectionToken(sectionName);
        this.ChildTokens.Add(sectionToken);
      }
      else if (sectionToken.Type != IniTokenType.Section)
      {
        throw new InvalidDataException(string.Format("A token named '{0}' already exists, but is not a section token.", sectionName));
      }

      return sectionToken;
    }

    public bool DeleteSection(string sectionName)
    {
      bool result;
      int sectionIndex;

      if (string.IsNullOrEmpty(sectionName))
      {
        throw new ArgumentNullException(nameof(sectionName));
      }

      result = false;

      sectionIndex = this.ChildTokens.IndexOf(sectionName);

      if (sectionIndex != -1)
      {
        this.ChildTokens.RemoveAt(sectionIndex);
        result = true;
      }

      return result;
    }

    public bool DeleteValue(string sectionName, string valueName)
    {
      bool result;

      if (string.IsNullOrEmpty(sectionName))
      {
        throw new ArgumentNullException(nameof(sectionName));
      }

      if (string.IsNullOrEmpty(valueName))
      {
        throw new ArgumentNullException(nameof(valueName));
      }

      result = false;

      if (this.ChildTokens.TryGetValue(sectionName, out IniToken sectionToken))
      {
        int valueIndex;

        valueIndex = sectionToken.ChildTokens.IndexOf(valueName);

        if (valueIndex != -1)
        {
          sectionToken.ChildTokens.RemoveAt(valueIndex);
          result = true;
        }
      }

      return result;
    }

    public IniToken GetSection(string sectionName)
    {
      if (string.IsNullOrEmpty(sectionName))
      {
        throw new ArgumentNullException(nameof(sectionName));
      }

      this.ChildTokens.TryGetValue(sectionName, out IniToken sectionToken);

      if (sectionToken != null && sectionToken.Type != IniTokenType.Section)
      {
        throw new InvalidDataException(string.Format("A token named '{0}' already exists, but is not a section token.", sectionName));
      }

      return sectionToken;
    }

    public string GetValue(string sectionName, string valueName)
    {
      return this.GetValue(sectionName, valueName, string.Empty);
    }

    public string GetValue(string sectionName, string valueName, string defaultValue)
    {
      IniToken sectionToken;

      if (string.IsNullOrEmpty(valueName))
      {
        throw new ArgumentNullException(nameof(valueName));
      }

      sectionToken = this.GetSection(sectionName);

      return sectionToken != null ? ((IniSectionToken)sectionToken).GetValue(valueName, defaultValue) : defaultValue;
    }

    public void Load()
    {
      this.Load(_fileName);
    }

    public void Load(string fileName)
    {
      this.Load(fileName, Encoding.UTF8);
    }

    public void Load(string fileName, Encoding encoding)
    {
      using (Stream stream = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
      {
        this.Load(stream, encoding);
      }
    }

    public void Load(Stream stream)
    {
      this.Load(stream, Encoding.UTF8);
    }

    public void Load(Stream stream, Encoding encoding)
    {
      using (TextReader reader = new StreamReader(stream, encoding))
      {
        this.Load(reader);
      }
    }

    public void Load(TextReader reader)
    {
      try
      {
        string line;

        this.ChildTokens.Clear();
        this.CurrentTokenCollection = this.ChildTokens;

        do
        {
          line = reader.ReadLine();

          if (line != null)
          {
            IniTokenType tokenType;
            IniToken token;

            tokenType = this.GetTokenType(line);
            token = this.CreateToken(tokenType, line);

            if (token != null)
            {
              if (token.Type == IniTokenType.Section)
              {
                this.ChildTokens.Add(token);
                this.CurrentTokenCollection = token.ChildTokens;
              }
              else
              {
                this.CurrentTokenCollection.Add(token);
              }
            }
          }
        } while (line != null);
      }
      finally
      {
        this.CurrentTokenCollection = null;
      }
    }

    public void LoadIni(string ini)
    {
      using (TextReader reader = new StringReader(ini))
      {
        this.Load(reader);
      }
    }

    public void Save()
    {
      this.Save(_fileName);
    }

    public void Save(TextWriter writer)
    {
      if (writer == null)
      {
        throw new ArgumentNullException(nameof(writer));
      }

      writer.Write(this.ToString());
      writer.Flush();
    }

    public void Save(Stream stream)
    {
      TextWriter writer;

      if (stream == null)
      {
        throw new ArgumentNullException(nameof(stream));
      }

      writer = new StreamWriter(stream);

      this.Save(writer);
    }

    public void Save(string fileName)
    {
      FileMode mode;

      if (string.IsNullOrEmpty(fileName))
      {
        throw new ArgumentNullException(nameof(fileName));
      }

      // need to use FileMode.Truncate if you want to overwrite a hidden file
      mode = File.Exists(fileName) ? FileMode.Truncate : FileMode.Create;

      using (Stream stream = new FileStream(fileName, mode, FileAccess.Write))
      {
        this.Save(stream);
      }
    }

    public void SetValue(string sectionName, string valueName, string value)
    {
      IniToken sectionToken;

      sectionToken = this.CreateSection(sectionName);

      ((IniSectionToken)sectionToken).SetValue(valueName, value);
    }

    #endregion Public Methods

    #region Protected Methods

    protected IniToken CreateToken(IniTokenType tokenType, string value)
    {
      IniToken token;

      switch (tokenType)
      {
        case IniTokenType.Comment:
          token = new IniCommentToken(value);
          break;

        case IniTokenType.Whitespace:
          token = new IniWhitespaceToken(value);
          break;

        case IniTokenType.Value:
          string name;
          this.GetNameAndValue(value, out name, out value);
          token = new IniValueToken(name, value);
          break;

        case IniTokenType.Section:
          token = new IniSectionToken(this.GetSectionName(value));
          break;

        case IniTokenType.Unknown:
          token = new IniRawToken(value);
          break;

        default: throw new ArgumentOutOfRangeException(nameof(tokenType));
      }

      return token;
    }

    protected virtual char[] GetCommentCharacters()
    {
      return DefaultCommentCharacters;
    }

    #endregion Protected Methods

    #region Private Methods

    private void GetNameAndValue(string line, out string name, out string value)
    {
      int valueStart;

      valueStart = line.IndexOf('=');

      name = line.Substring(0, valueStart).TrimWhitespace();
      value = line.Substring(valueStart + 1).TrimWhitespace().ToLiteral();
    }

    private string GetSectionName(string line)
    {
      int nameStart;
      int nameEnd;

      nameStart = line.IndexOf('[');
      nameEnd = line.IndexOf(']');

      return line.Substring(nameStart + 1, nameEnd - (nameStart + 1));
    }

    private IniTokenType GetTokenType(string line)
    {
      IniTokenType result;

#if NET452_OR_GREATER || NETSTANDARD2_0_OR_GREATER || NET || NETCOREAPP
      if (string.IsNullOrWhiteSpace(line))
      {
        result = IniTokenType.Whitespace;
      }
#else
      if (string.IsNullOrEmpty(line) || line.TrimWhitespace().Length == 0)
      {
        result = IniTokenType.Whitespace;
      }
#endif
      else
      {
        string trimmedLine;

        trimmedLine = line.TrimWhitespace();

        if (trimmedLine.StartsWithAny(this.GetCommentCharacters()))
        {
          result = IniTokenType.Comment;
        }
        else if (trimmedLine[0] == '[' && trimmedLine[trimmedLine.Length - 1] == ']')
        {
          result = IniTokenType.Section;
        }
        else if (trimmedLine.IndexOf('=') != -1)
        {
          result = IniTokenType.Value;
        }
        else
        {
          result = IniTokenType.Unknown;
        }
      }

      return result;
    }

#endregion Private Methods
  }
}