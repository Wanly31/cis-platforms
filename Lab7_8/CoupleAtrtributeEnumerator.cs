using System.Collections;

namespace Lab7_8;


public class CoupleAttributeEnumerator : IEnumerator<CoupleAttribute>
{
    private readonly CoupleAttribute[] _items;
    private int _index = -1;

    public CoupleAttributeEnumerator(CoupleAttribute[] items)
        => _items = items;

    public CoupleAttribute Current
    {
        get
        {
            if (_index < 0 || _index >= _items.Length)
                throw new InvalidOperationException("Enumerator is out of range.");
            return _items[_index];
        }
    }

    object IEnumerator.Current => Current;

    public bool MoveNext() => ++_index < _items.Length;

    public void Reset() => _index = -1;

    public void Dispose() {  }
}

// IEnumerable<CoupleAttribute>

public class CoupleAttributeCollection : IEnumerable<CoupleAttribute>
{
    private readonly CoupleAttribute[] _attributes;

    public CoupleAttributeCollection(Type type)
    {
        _attributes = (CoupleAttribute[])type.GetCustomAttributes(typeof(CoupleAttribute), false);
    }

    public IEnumerator<CoupleAttribute> GetEnumerator()
        => new CoupleAttributeEnumerator(_attributes);

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
