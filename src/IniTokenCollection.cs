using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Cyotek.Data.Ini
{
  public class IniTokenCollection
    : IList<IniToken>, IList
#if NET452_OR_GREATER || NETSTANDARD || NET
      , IReadOnlyList<IniToken>
#endif
  {
#region Constants

    private readonly IList<IniToken> _items;




#endregion

#region Fields



#endregion

#region Constructors

    public IniTokenCollection()
    {
      _items = new List<IniToken>();
      _nameToIndexLookup = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
    }


    public IniTokenCollection(IEnumerable<IniToken> items)
      : this()
    {
      this.AddRange(items);
    }

#endregion

#region Events


#endregion

#region Properties


    protected IList<IniToken> Items
    {
      get { return _items; }
    }

#endregion

#region Methods



    public void AddRange(IEnumerable<IniToken> collection)
    {
      foreach (IniToken value in collection)
      {
        this.Add(value);
      }
    }

    public void InsertRange(int index, IEnumerable<IniToken> collection)
    {
      foreach (IniToken value in collection)
      {
        this.Insert(index, value);
        index++;
      }
    }

    public void RemoveRange(IEnumerable<IniToken> collection)
    {
      IniToken[] itemsToRemove;

      itemsToRemove = collection.ToArray();
      foreach (IniToken item in itemsToRemove)
      {
        this.Remove(item);
      }
    }

    public void Sort()
    {
      this.Sort(Comparer<IniToken>.Default);
    }

    public void Sort(IComparer<IniToken> comparer)
    {
      this.Sort(0, _items.Count, comparer);
    }

    public void Sort(Comparison<IniToken> comparison)
    {
      if (comparison == null)
      {
        throw new ArgumentNullException(nameof(comparison));
      }

      if (_items.IsReadOnly)
      {
        throw new NotSupportedException("Collection is read-only.");
      }

      if (_items.Count > 0)
      {
        ((List<IniToken>)_items).Sort(comparison);
      }
    }

    public void Sort(int index, int count, IComparer<IniToken> comparer)
    {
      if (_items.IsReadOnly)
      {
        throw new NotSupportedException("Collection is read-only.");
      }

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

      if (_items.Count - index < count)
      {
        throw new ArgumentException("Invalid count.");
      }

      ((List<IniToken>)_items).Sort(index, count, comparer);
    }

    public IniToken[] ToArray()
    {
      IniToken[] result;
      int count;

      count = _items.Count;

      result = new IniToken[count];

      if (count != 0)
      {
        this.CopyTo(result, 0);
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

    protected virtual void ClearItems()
    {
      _nameToIndexLookup.Clear();

      _items.Clear();
    }

    protected virtual void InsertItem(int index, IniToken item)
    {
      _items.Insert(index, item);
    }

    protected virtual void RemoveItem(int index)
    {
      IniToken token;

      token = _items[index];

      if (!string.IsNullOrEmpty(token?.Name))
      {
        _nameToIndexLookup.Remove(token.Name);
      }

      _items.RemoveAt(index);
    }

    protected virtual void SetItem(int index, IniToken item)
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
      _items[index] = item;
    }

#endregion

#region IList<IniToken> Interface

    public IniToken this[int index]
    {
      get { return _items[index]; }
      set
      {
        if (value == null)
        {
          throw new ArgumentNullException(nameof(value));
        }

        if (_items.IsReadOnly)
        {
          throw new NotSupportedException("Collection is read-only.");
        }

        if (index < 0 || index >= _items.Count)
        {
          throw new ArgumentOutOfRangeException(nameof(index), index, "Index was out of range. Must be non-negative and less than the size of the collection.");
        }

        this.SetItem(index, value);
      }
    }

    public void Add(IniToken item)
    {
      if (_items.IsReadOnly)
      {
        throw new NotSupportedException("Collection is read-only.");
      }

      if (item == null)
      {
        throw new ArgumentNullException(nameof(item));
      }

      this.InsertItem(_items.Count, item);
    }

    public void Clear()
    {
      if (_items.IsReadOnly)
      {
        throw new NotSupportedException("Collection is read-only.");
      }

      this.ClearItems();
    }

    public bool Contains(IniToken item)
    {
      return _items.Contains(item);
    }

    public void CopyTo(IniToken[] array, int index)
    {
      _items.CopyTo(array, index);
    }

    public IEnumerator<IniToken> GetEnumerator()
    {
      return _items.GetEnumerator();
    }

    public int IndexOf(IniToken item)
    {
      return _items.IndexOf(item);
    }

    public void Insert(int index, IniToken item)
    {
      if (item == null)
      {
        throw new ArgumentNullException(nameof(item));
      }

      if (_items.IsReadOnly)
      {
        throw new NotSupportedException("Collection is read-only.");
      }

      if (index < 0 || index > _items.Count)
      {
        throw new ArgumentOutOfRangeException(nameof(index), index, "Index was out of range. Must be non-negative and less than the size of the collection.");
      }

      this.InsertItem(index, item);
    }

    public bool Remove(IniToken item)
    {
      bool result;
      int index;

      if (_items.IsReadOnly)
      {
        throw new NotSupportedException("Collection is read-only.");
      }

      index = _items.IndexOf(item);
      result = index != -1;

      if (result)
      {
        this.RemoveItem(index);
      }

      return result;
    }

    public void RemoveAt(int index)
    {
      if (_items.IsReadOnly)
      {
        throw new NotSupportedException("Collection is read-only.");
      }

      if (index < 0 || index >= _items.Count)
      {
        throw new ArgumentOutOfRangeException(nameof(index), index, "Index was out of range. Must be non-negative and less than the size of the collection.");
      }

      this.RemoveItem(index);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return _items.GetEnumerator();
    }

    public int Count
    {
      get { return _items.Count; }
    }

    public bool IsReadOnly
    {
      get { return _items.IsReadOnly; }
    }

#endregion


#region IList & ICollection

    int IList.Add(object value)
    {
      if (value is IniToken item)
      {
        this.Add(item);
        return this.IndexOf(item);
      }
      else
      {
        throw new ArgumentException("Value must be of type IniToken.", nameof(value));
      }
    }

    bool IList.Contains(object value)
    {
      return value is IniToken item && this.Contains(item);
    }

    int IList.IndexOf(object value)
    {
      return value is IniToken item
        ? this.IndexOf(item)
        : -1;
    }

    void IList.Insert(int index, object value)
    {
      if (value is IniToken item)
      {
        this.Insert(index, item);
      }
      else
      {
        throw new ArgumentException("Value must be of type IniToken.", nameof(value));
      }
    }

    void IList.Remove(object value)
    {
      if (value is IniToken item)
      {
        this.Remove(item);
      }
      else
      {
        throw new ArgumentException("Value must be of type IniToken.", nameof(value));
      }
    }

    void ICollection.CopyTo(Array array, int index)
    {
      for (int i = 0; i < _items.Count; i++)
      {
        array.SetValue(_items[i], index + i);
      }
    }

    bool IList.IsFixedSize
    {
      get { return false; }
    }

    object ICollection.SyncRoot
    {
      get { return this; }
    }

    bool ICollection.IsSynchronized
    {
      get { return false; }
    }

    object IList.this[int index]
    {
      get { return this[index]; }
      set
      {
        if (value is IniToken item)
        {
          this[index] = item;
        }
        else
        {
          throw new ArgumentException("Value must be of type IniToken.", nameof(value));
        }
      }
    }

#endregion
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

    
 
    

#endregion
  }
}
