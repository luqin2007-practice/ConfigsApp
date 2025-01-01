using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace Configs.app;

public class Types : IEnumerable<IType>
{
    public static readonly Types Default = [];

    private static readonly Dictionary<string, IType> DefaultTypes = new()
    {
        ["string"] = StringType.Default,
        ["bool"] = BoolType.Default,
        ["boolean"] = BoolType.Default,
        ["int"] = IntType.Default,
        ["integer"] = IntType.Default,
        ["long"] = IntType.Default,
        ["directory"] = DirectoryType.Default,
        ["file"] = FileType.Default,
        ["list"] = ListType.Default,
        ["array"] = ArrayType.Default,
    };

    private readonly Dictionary<string, IType> _types = [];
    private readonly List<IType> _typeList = [];

    public void Add(EnumType type)
    {
        _typeList.Add(type);
        _types[type.Type] = type;
    }

    public bool ContainsKey(string type)
    {
        return _types.ContainsKey(type) || DefaultTypes.ContainsKey(type);
    }

    public bool TryGetValue(string type, [MaybeNullWhen(false)] out IType value)
    {
        return _types.TryGetValue(type, out value) || DefaultTypes.TryGetValue(type, out value);
    }

    public IType this[string key]
    {
        get => _types.TryGetValue(key, out var item) ? item : DefaultTypes[key];
        set
        {
            _typeList.Add(value);
            _types[key] = value;
        }
    }

    public IEnumerator<IType> GetEnumerator()
    {
        return _typeList.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}