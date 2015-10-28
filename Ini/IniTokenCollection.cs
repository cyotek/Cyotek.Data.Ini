using System;
using System.Collections.Generic;
using Cyotek.Collections;

namespace Cyotek.Ini
{
  public class IniTokenCollection : Collection<IniToken>
  {
    #region Instance Fields

    private readonly IDictionary<string, int> _nameToIndexLookup;

    #endregion

    #region Public Constructors

    public IniTokenCollection()
    {
      _nameToIndexLookup = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
    }

    #endregion

    #region Overridden Methods

    /// <summary>
    /// Removes all elements from the <see cref="T:System.Collections.ObjectModel.Collection`1"/>.
    /// </summary>
    protected override void ClearItems()
    {
      _nameToIndexLookup.Clear();

      base.ClearItems();
    }

    /// <summary>
    /// Removes the element at the specified index of the <see cref="T:System.Collections.ObjectModel.Collection`1"/>.
    /// </summary>
    /// <param name="index">The zero-based index of the element to remove.</param><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index"/> is less than zero.-or-<paramref name="index"/> is equal to or greater than <see cref="P:System.Collections.ObjectModel.Collection`1.Count"/>.</exception>
    protected override void RemoveItem(int index)
    {
      IniToken token;

      token = this[index];
      if (token != null && !string.IsNullOrEmpty(token.Name))
      {
        _nameToIndexLookup.Remove(token.Name);
      }

      base.RemoveItem(index);
    }

    /// <summary>
    /// Replaces the element at the specified index.
    /// </summary>
    /// <param name="index">The zero-based index of the element to replace.</param><param name="item">The new value for the element at the specified index. The value can be null for reference types.</param><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index"/> is less than zero.-or-<paramref name="index"/> is greater than <see cref="P:System.Collections.ObjectModel.Collection`1.Count"/>.</exception>
    protected override void SetItem(int index, IniToken item)
    {
      if (item != null && !string.IsNullOrEmpty(item.Name))
      {
        _nameToIndexLookup.Remove(item.Name);
      }

      base.SetItem(index, item);
    }

    #endregion

    #region Public Properties

    public IniToken this[string name]
    {
      get { return this[this.IndexOf(name)]; }
    }

    #endregion

    #region Public Members

    public int IndexOf(string name)
    {
      int index;

      if (!_nameToIndexLookup.TryGetValue(name, out index) || index < 0 || index > this.Count - 1 || !this[index].Name.Equals(name, StringComparison.OrdinalIgnoreCase))
      {
        // missing index, or not found, out of bounds, etc
        index = -1;
        _nameToIndexLookup.Remove(name);

        for (int i = 0; i < this.Count; i++)
        {
          string itemName;

          itemName = this[i].Name;

          if (itemName != null && itemName.Equals(name, StringComparison.OrdinalIgnoreCase))
          {
            _nameToIndexLookup.Add(name, i);
            index = i;
            break;
          }
        }
      }

      return index;
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

    public bool TryGetValue(string name, out IniToken value)
    {
      int index;

      index = this.IndexOf(name);
      value = index != -1 ? this[index] : null;

      return index != -1;
    }

    #endregion
  }
}
