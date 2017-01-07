using System;
using System.Collections.Generic;

namespace Cyotek.Ini
{
  public partial class IniTokenCollection
  {
    #region Fields

    private IDictionary<string, int> _nameToIndexLookup;

    #endregion

    #region Properties

    public IniToken this[string name]
    {
      get { return this[this.IndexOf(name)]; }
    }

    #endregion

    #region Methods

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
      value = index != -1 ? _items[index] : null;

      return index != -1;
    }

    partial void OnClearingItems()
    {
      _nameToIndexLookup.Clear();
    }

    partial void OnInitialize()
    {
      _nameToIndexLookup = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
    }

    partial void OnRemovingItem(int index)
    {
      IniToken token;

      token = _items[index];
      if (!string.IsNullOrEmpty(token?.Name))
      {
        _nameToIndexLookup.Remove(token.Name);
      }
    }

    partial void OnSettingItem(int index, IniToken item)
    {
      IniToken previousItem;

      previousItem = _items[index];

      if (!string.IsNullOrEmpty(previousItem?.Name))
      {
        _nameToIndexLookup.Remove(previousItem.Name);

        if (!string.IsNullOrEmpty(item.Name))
        {
          _nameToIndexLookup.Add(item.Name, index);
        }
      }
    }

    #endregion
  }
}
