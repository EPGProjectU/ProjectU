using System;

public interface ITypeNode
{
    public Type ValueType { get; }

    public bool HasRoot();

    public void SetValue(object value);
}
