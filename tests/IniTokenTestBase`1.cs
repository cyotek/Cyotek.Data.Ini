﻿using NUnit.Framework;
using System;

namespace Cyotek.Data.Ini.Tests
{
  public abstract class IniTokenTestBase<T> : TestBase
    where T : IniToken, new()
  {
    #region Protected Properties

    protected virtual string DefaultName => null;

    protected virtual string DefaultValue => null;

    protected abstract string ExpectedInnerText { get; }

    protected abstract IniTokenType ExpectedType { get; }

    protected abstract T SampleToken { get; }

    #endregion Protected Properties

    #region Public Methods

    [Test]
    public void CloneObjectTest()
    {
      // arrange
      object actual;
      T expected;

      expected = this.SampleToken;

      // act
      actual = ((ICloneable)expected).Clone();

      // assert
      Assert.IsNotNull(actual);
      Assert.AreNotSame(expected, actual);
      Assert.IsInstanceOf<T>(actual);
      IniDocumentAssert.AreEqual(expected, (T)actual);
    }

    [Test]
    public void CloneTest()
    {
      // arrange
      IniToken actual;
      T expected;

      expected = this.SampleToken;

      // act
      actual = expected.Clone();

      // assert
      Assert.IsNotNull(actual);
      Assert.AreNotSame(expected, actual);
      Assert.IsInstanceOf<T>(actual);
      IniDocumentAssert.AreEqual(expected, actual);
    }

    [Test]
    public void ConstructorTest()
    {
      // arrange
      T target;

      // act
      target = new T();

      // assert
      Assert.IsNotNull(target);
      Assert.AreEqual(this.DefaultName, target.Name);
      Assert.AreEqual(this.DefaultValue, target.Value);
    }

    [Test]
    public void InnerTextTest()
    {
      Assert.AreEqual(this.ExpectedInnerText, this.SampleToken.InnerText);
    }

    [Test]
    public virtual void NameSetTest()
    {
      Assert.Throws<NotSupportedException>(() => new T().Name = "alpha");
    }

    [Test]
    public void ToStringTest()
    {
      Assert.AreEqual(this.ExpectedInnerText, this.SampleToken.ToString());
    }

    [Test]
    public void TypeTest()
    {
      Assert.AreEqual(this.ExpectedType, new T().Type);
    }

    #endregion Public Methods
  }
}