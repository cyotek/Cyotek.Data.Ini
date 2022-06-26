using NUnit.Framework;
using System;
using System.IO;

namespace Cyotek.Data.Ini.Tests
{
  public abstract class TestBase
  {
    #region Protected Properties

    protected string DataDirectory => Path.Combine(TestContext.CurrentContext.WorkDirectory, "data");

    #endregion Protected Properties

    #region Protected Methods

    protected string GetDataFileName(string baseName) => Path.Combine(this.DataDirectory, baseName);

    [Obsolete("Tests should be deterministic.")]
    protected string GetRandomString()
    {
      return Guid.NewGuid().ToString("n");
    }

    #endregion Protected Methods
  }
}