// Copyright (c) IdeaTech. All rights reserved.

internal class MockIReadOnlyDictionary(IDictionary<string, object?> innerDictionary) : IReadOnlyDictionary<string, object?>
{
    private readonly IDictionary<string, object?> _innerDictionary = innerDictionary;

    public object? this[string key] => this._innerDictionary[key];

    public IEnumerable<string> Keys => this._innerDictionary.Keys;

    public IEnumerable<object?> Values => this._innerDictionary.Values;

    public int Count => this._innerDictionary.Count;

    public bool ContainsKey(string key) => this._innerDictionary.ContainsKey(key);

    public IEnumerator<KeyValuePair<string, object?>> GetEnumerator() => this._innerDictionary.GetEnumerator();

    public bool TryGetValue(string key, [MaybeNullWhen(false)] out object? value) => this._innerDictionary.TryGetValue(key, out value);

    IEnumerator IEnumerable.GetEnumerator() => this._innerDictionary.GetEnumerator();
}
