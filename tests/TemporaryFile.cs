using NUnit.Framework;
using System;
using System.Diagnostics;
using System.IO;

// Cyotek Ini Reader / Writer Library
// https://github.com/cyotek/Cyotek.Data.Ini

// Copyright © 2014-2022 Cyotek Ltd.

// This work is licensed under the MIT License.
// See LICENSE.TXT for the full text

// Found this code useful?
// https://www.cyotek.com/contribute

namespace Cyotek.Testing
{
  public sealed class TemporaryFile : IDisposable
  {
    #region Constants

    private readonly string _fileName;

    #endregion

    #region Fields

    private bool _isDisposed;

    #endregion

    #region Finalizers

    ~TemporaryFile()
    {
      this.Dispose(false);
    }

    #endregion

    #region Constructors

    public TemporaryFile()
      : this(Path.GetTempFileName())
    { }

    public TemporaryFile(string fileName)
    {
      if (string.IsNullOrEmpty(fileName))
      {
        throw new ArgumentNullException(nameof(fileName));
      }

      _fileName = fileName;
    }

    #endregion

    #region Operators

    public static explicit operator string(TemporaryFile value)
    {
      return value.FileName;
    }

    #endregion

    #region Static Methods

    /// <summary>Creates a small temp file returns the file name.</summary>
    public static string Create()
    {
      return Create(Path.GetTempFileName());
    }

    public static string Create(string fileName)
    {
      string contents = "You can delete this file." + Environment.NewLine;
      return CreateWithContents(contents, fileName);
    }

    public static string CreateWithContents(string contents)
    {
      return CreateWithContents(contents, Path.GetTempFileName());
    }

    public static string CreateWithContents(string contents, string fileName)
    {
      // ensure the directory exists
      Directory.CreateDirectory(Path.GetDirectoryName(fileName));

      // write the text into the temp file.
      using (FileStream f = new FileStream(fileName, FileMode.Create))
      {
        StreamWriter s = new StreamWriter(f);
        s.Write(contents);
        s.Close();
        f.Close();
      }

      if (!File.Exists(fileName))
      {
        Assert.Fail("TempFile: {0} wasn't created.", fileName);
      }

      return fileName;
    }

    public static string Read(string fileName)
    {
      string contents;
      using (StreamReader s = File.OpenText(fileName))
      {
        contents = s.ReadToEnd();
        s.Close();
      }
      return contents;
    }

    #endregion

    #region Properties

    public string FileName
    {
      get
      {
        if (this.IsDisposed)
        {
          throw new ObjectDisposedException(this.GetType().
                                                 Name);
        }

        return _fileName;
      }
    }

    public bool IsDisposed
    {
      get { return _isDisposed; }
    }

    #endregion

    #region Methods

    public void CopyFrom(string fileName)
    {
      File.Copy(fileName, _fileName, true);
    }

    public bool Delete()
    {
      bool result;

      if (!this.IsDisposed)
      {
        Debug.Print("Deleting temporary file '{0}'", _fileName);
        try
        {
          File.Delete(_fileName);
          result = true;
        }
        catch
        {
          result = false;
        }
      }
      else
      {
        result = true;
      }

      return result;
    }

    public void Move(string fileName)
    {
      Debug.Print("Moving '{0}' to '{1}'", _fileName, fileName);
      File.Copy(_fileName, fileName, true);
      this.Delete();
    }

    private void Dispose(bool disposing)
    {
      if (disposing)
      {
        GC.SuppressFinalize(this);
      }

      if (!this.IsDisposed && !string.IsNullOrEmpty(_fileName) && File.Exists(_fileName))
      {
        if (this.Delete())
        {
          _isDisposed = true;
        }
      }
    }

    #endregion

    #region IDisposable Interface

    public void Dispose()
    {
      this.Dispose(true);

      // Unregister object for finalization.
      GC.SuppressFinalize(this);
    }

    #endregion
  }
}
