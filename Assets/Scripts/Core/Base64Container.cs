using System;
using UnityEngine;

namespace ProjectU.Core
{
    [Serializable]
    public class Base64Container
    {
        [SerializeField]
        private string data;

        public Base64Container(byte[] binary) => data = Convert.ToBase64String(binary);

        public static implicit operator byte[](Base64Container base64) => Convert.FromBase64String(base64.data);
        public static implicit operator Base64Container(byte[] binary) => new Base64Container(binary);

        public static bool operator ==(Base64Container lhs, Base64Container rhs)
        {
            return
                rhs is null
                    ? string.IsNullOrEmpty(lhs?.data)
                    : lhs!.Equals(rhs);
        }

        public static bool operator !=(Base64Container lhs, Base64Container rhs) => !(lhs == rhs);

        public override bool Equals(object obj)
        {
            if (obj is Base64Container base64)
                return Equals(base64);
            
            return false;
        }

        public override int GetHashCode()
        {
            return data != null ? data.GetHashCode() : 0;
        }

        protected bool Equals(Base64Container other)
        {
            return data == other.data;
        }

        public override string ToString() => data;
    }
}