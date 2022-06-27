using NUnit.Framework;
using System.IO;

namespace Cyotek.Data.Ini.Tests
{
  public abstract class TestBase
  {
    #region Protected Properties

    protected string DataDirectory => Path.Combine(TestContext.CurrentContext.TestDirectory, "data");

    #endregion Protected Properties

    #region Protected Methods

    protected string GetDataFileName(string baseName) => Path.Combine(this.DataDirectory, baseName);

    #endregion Protected Methods
  }
}