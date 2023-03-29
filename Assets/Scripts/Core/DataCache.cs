using System;

public class DataCache<T>
{
    private T _cache;

    private readonly Func<T> _cacheLoader;

    public DataCache(Func<T> cacheLoader)
    {
        _cacheLoader = cacheLoader;
    }

    public T Get()
    {
        if (_cache != null)
            return _cache;

        return _cache = _cacheLoader.Invoke();
    }

    public void Clear() => _cache = default;
}