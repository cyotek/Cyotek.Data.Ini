using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

// Cyotek Ini Reader / Writer Library
// https://github.com/cyotek/Cyotek.Data.Ini

// Copyright © 2014-2022 Cyotek Ltd.

// This work is licensed under the MIT License.
// See LICENSE.TXT for the full text

// Found this code useful?
// https://www.cyotek.com/contribute

namespace Cyotek.Data.Ini
{
  public class IniTokenCollection
    : Collection<IniToken>
  {
    #region Private Fields

    private readonly IDictionary<string, int> _nameToIndexLookup;

    #endregion Private Fields

    #region Public Constructors

    public IniTokenCollection()
    {
      _nameToIndexLookup = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
    }

    public IniTokenCollection(IEnumerable<IniToken> items)
      : this()
    {
      this.AddRange(items);
    }

    #endregion Public Constructors

    #region Public Indexers

    public IniToken this[string name] => this[this.IndexOf(name)];

    #endregion Public Indexers

    #region Public Methods

    public void AddRange(IEnumerable<IniToken> collection)
    {
      foreach (IniToken value in collection)
      {
        this.Add(value);
      }
    }

    public int IndexOf(string name)
    {
      int index;

      if (name != null)
      {
        if (!_nameToIndexLookup.TryGetValue(name, out index) || index < 0 || index > this.Count - 1 || !string.Equals(this[index].Name, name, StringComparison.OrdinalIgnoreCase))
        {
          // missing index, or not found, out of bounds, etc
          index = -1;

          _nameToIndexLookup.Remove(name);

          for (int i = 0; i < this.Count; i++)
          {
            if (string.Equals(this[i].Name, name, StringComparison.OrdinalIgnoreCase))
            {
              _nameToIndexLookup.Add(name, i);
              index = i;
              break;
            }
          }
        }
      }
      else
      {
        index = -1;
      }

      return index;
    }

    public void InsertRange(int index, IEnumerable<IniToken> collection)
    {
      foreach (IniToken value in collection)
      {
        this.Insert(index, value);
        index++;
      }
    }

    public bool Remove(string name)
    {
      int index;

      index = this.IndexOf(name);

      if (index != -1)
      {
        this.RemoveAt(index);
      }

      return index != -1;
    }

    public void RemoveRange(IEnumerable<IniToken> collection)
    {
      List<IniToken> itemsToRemove;

      itemsToRemove = new List<IniToken>(collection);

      for (int i = 0; i < itemsToRemove.Count; i++)
      {
        this.Remove(itemsToRemove[i]);
      }
    }

    public void Sort()
    {
      this.Sort(IniTokenComparer.Ordinal);
    }

    public void Sort(IComparer<IniToken> comparer)
    {
      this.Sort(0, this.Count, comparer);
    }

    public void Sort(Comparison<IniToken> comparison)
    {
      if (comparison == null)
      {
        throw new ArgumentNullException(nameof(comparison));
      }

      if (this.Count > 0)
      {
        IniToken[] tokens;

        tokens = this.ToArray();

        Array.Sort(tokens, comparison);

        this.Replace(tokens);
      }
    }

    public void Sort(int index, int count, IComparer<IniToken> comparer)
    {
      IniToken[] tokens;
      
      if (comparer == null)
      {
        throw new ArgumentNullException(nameof(comparer));
      }

      if (index < 0)
      {
        throw new ArgumentException("Index cannot be less than zero.", nameof(index));
      }

      if (count < 0)
      {
        throw new ArgumentException("Count cannot be less than zero.", nameof(count));
      }

      if (this.Count - index < count)
      {
        throw new ArgumentException("Invalid count.");
      }

      tokens = this.ToArray();

      Array.Sort(tokens, index, count, comparer);

      this.Replace(tokens);
    }

    public IniToken[] ToArray()
    {
      IniToken[] result;
      int count;

      count = this.Count;

      if (count > 1)
      {
        result = new IniToken[count];
        this.CopyTo(result, 0);
      }
      else
      {
#if NET462_OR_GREATER || NETSTANDARD2_0_OR_GREATER || NET || NETCOREAPP
        result = Array.Empty<IniToken>();
#else
        result = new IniToken[0];
#endif
      }

      return result;
    }

    public void TryAddRange(IEnumerable<IniToken> collection)
    {
      // ReSharper disable once LoopCanBePartlyConvertedToQuery
      foreach (IniToken value in collection)
      {
        if (!this.Contains(value))
        {
          this.Add(value);
        }
      }
    }

    public bool TryGetValue(string name, out IniToken value)
    {
      int index;

      index = this.IndexOf(name);
      value = index != -1
        ? this[index]
        : null;

      return index != -1;
    }

    #endregion Public Methods

    #region Protected Methods

    protected override void ClearItems()
    {
      _nameToIndexLookup.Clear();

      base.ClearItems();
    }

    protected override void RemoveItem(int index)
    {
      IniToken token;

      token = this.Items[index];

      if (!string.IsNullOrEmpty(token?.Name))
      {
        _nameToIndexLookup.Remove(token.Name);
      }

      base.RemoveItem(index);
    }

    protected override void SetItem(int index, IniToken item)
    {
      IniToken previousItem;

      previousItem = this.Items[index];

      if (!string.IsNullOrEmpty(previousItem?.Name))
      {
        _nameToIndexLookup.Remove(previousItem.Name);

        if (!string.IsNullOrEmpty(item.Name))
        {
          _nameToIndexLookup.Add(item.Name, index);
        }
      }

      base.SetItem(index, item);
    }

    #endregion Protected Methods

    #region Private Methods

    private void Replace(IniToken[] tokens)
    {
      IList<IniToken> items;

      _nameToIndexLookup.Clear();

      items = this.Items;

      items.Clear();

      for (int i = 0; i < tokens.Length; i++)
      {
        items.Add(tokens[i]);
      }
    }

    #endregion Private Methods
  }
}