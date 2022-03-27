using System;
using UnityEngine;
using Random = System.Random;

/// <summary>
/// Custom serializable GUID
/// </summary>
[Serializable]
public struct SerializableGUID : IComparable, IComparable<SerializableGUID>, IEquatable<SerializableGUID>
{
    [SerializeField]
    private int m_Value0;

    [SerializeField]
    private int m_Value1;

    [SerializeField]
    private int m_Value2;

    [SerializeField]
    private int m_Value3;

    public static bool operator ==(SerializableGUID x, SerializableGUID y)
    {
        return x.m_Value0 == y.m_Value0 && x.m_Value1 == y.m_Value1 && x.m_Value2 == y.m_Value2 && x.m_Value3 == y.m_Value3;
    }

    public static bool operator !=(SerializableGUID x, SerializableGUID y)
    {
        return !(x == y);
    }

    public static bool operator <(SerializableGUID x, SerializableGUID y)
    {
        if (x.m_Value0 != y.m_Value0)
            return x.m_Value0 < y.m_Value0;

        if (x.m_Value1 != y.m_Value1)
            return x.m_Value1 < y.m_Value1;

        return x.m_Value2 != y.m_Value2 ? x.m_Value2 < y.m_Value2 : x.m_Value3 < y.m_Value3;
    }

    public static bool operator >(SerializableGUID x, SerializableGUID y)
    {
        return !(x < y) && !(x == y);
    }

    public override bool Equals(object obj)
    {
        return obj is SerializableGUID guid && Equals(guid);
    }

    public bool Equals(SerializableGUID obj)
    {
        return this == obj;
    }

    public override int GetHashCode()
    {
        return (((((m_Value0 * 397) ^ m_Value1) * 397) ^ m_Value2) * 397) ^ m_Value3;
    }

    public int CompareTo(object obj)
    {
        return obj == null ? 1 : CompareTo((SerializableGUID)obj);
    }

    public int CompareTo(SerializableGUID rhs)
    {
        if (this < rhs)
            return -1;

        return this > rhs ? 1 : 0;
    }

    public bool Empty()
    {
        return m_Value0 == 0U && m_Value1 == 0U && m_Value2 == 0U && m_Value3 == 0U;
    }

    private SerializableGUID(int a, int b, int c, int d)
    {
        m_Value0 = a;
        m_Value1 = b;
        m_Value2 = c;
        m_Value3 = d;
    }

    public static SerializableGUID Generate()
    {
        var random = new Random(Guid.NewGuid().GetHashCode());

        return new SerializableGUID(random.Next(), random.Next(), random.Next(), random.Next());
    }

    public override string ToString()
    {
        return m_Value0.ToString("X8") +
               m_Value1.ToString("X8") +
               m_Value2.ToString("X8") +
               m_Value3.ToString("X8");
    }
}