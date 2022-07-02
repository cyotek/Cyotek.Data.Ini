using NUnit.Framework;
using System;
using System.IO;

// Cyotek Ini Reader / Writer Library
// https://github.com/cyotek/Cyotek.Data.Ini

// Copyright © 2014-2022 Cyotek Ltd.

// This work is licensed under the MIT License.
// See LICENSE.TXT for the full text

// Found this code useful?
// https://www.cyotek.com/contribute

namespace Cyotek.Data.Ini.Tests
{
  public abstract class TestBase
  {
    #region Protected Properties

    protected string DataDirectory => Path.Combine(TestContext.CurrentContext.TestDirectory, "data");

    #endregion Protected Properties

    #region Protected Methods

    protected string GetDataFileName(string baseName) => Path.Combine(this.DataDirectory, baseName);

    protected void RunCloneObjectTest<T>(T expected)
      where T : IniToken
    {
      // arrange
      object actual;

      // act
      actual = ((ICloneable)expected).Clone();

      // assert
      Assert.IsNotNull(actual);
      Assert.AreNotSame(expected, actual);
      Assert.IsInstanceOf<T>(actual);
      IniAssert.AreEqual(expected, (T)actual);
    }

    protected void RunCloneTest<T>(T expected)
          where T : IniToken
    {
      // arrange
      IniToken actual;

      // act
      actual = expected.Clone();

      // assert
      Assert.IsNotNull(actual);
      Assert.AreNotSame(expected, actual);
      Assert.IsInstanceOf<T>(actual);
      IniAssert.AreEqual(expected, actual);
    }

    #endregion Protected Methods
  }
}