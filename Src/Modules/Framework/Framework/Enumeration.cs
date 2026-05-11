using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Framework.NumberHelper
{
    public abstract class Enumeration : IComparable
    {
        private readonly long _Key;
        private readonly string _displayName;

        protected Enumeration()
        {
        }

        protected Enumeration(long key, string displayName)
        {
            _Key = key;
            _displayName = displayName;
        }

        public  long Key
        {
            get { return _Key; }
        }

        public string DisplayName
        {
            get { return _displayName; }
        }

        public override string ToString()
        {
            return DisplayName;
        }

        public static IEnumerable<T> GetAll<T>() where T : Enumeration, new()
        {
            var type = typeof(T);
            var fields = type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);

            foreach (var info in fields)
            {
                var instance = new T();
                var locatedValue = info.GetValue(instance) as T;

                if (locatedValue != null)
                {
                    yield return locatedValue;
                }
            }
        }

#pragma warning disable CS8765 // Nullability of type of parameter doesn't match overridden member (possibly because of nullability attributes).
        public override bool Equals(object obj)
#pragma warning restore CS8765 // Nullability of type of parameter doesn't match overridden member (possibly because of nullability attributes).
        {
            var otherValue = obj as Enumeration;

            if (otherValue == null)
            {
                return false;
            }

            var typeMatches = GetType().Equals(obj.GetType());
            var valueMatches = _Key.Equals(otherValue.Key);

            return typeMatches && valueMatches;
        }

        public override int GetHashCode()
        {
            return _Key.GetHashCode();
        }

        public static long AbsoluteDifference(Enumeration firstValue, Enumeration secondValue)
        {
            var absoluteDifference = Math.Abs(firstValue.Key - secondValue.Key);
            return absoluteDifference;
        }

        public static T FromValue<T>(int value) where T : Enumeration, new()
        {
            var matchingItem = parse<T, int>(value, "value", item => item.Key == value);
            return matchingItem;
        }

        public static T FromDisplayName<T>(string displayName) where T : Enumeration, new()
        {
            var matchingItem = parse<T, string>(displayName, "display name", item => item.DisplayName == displayName);
            return matchingItem;
        }

        private static T parse<T, K>(K value, string description, Func<T, bool> predicate) where T : Enumeration, new()
        {
            var matchingItem = GetAll<T>().FirstOrDefault(predicate);

            if (matchingItem == null)
            {
                var message = string.Format("'{0}' is not a valid {1} in {2}", value, description, typeof(T));
                throw new ApplicationException(message);
            }

            return matchingItem;
        }

#pragma warning disable CS8767 // Nullability of reference types in type of parameter doesn't match implicitly implemented member (possibly because of nullability attributes).
        public int CompareTo(object other)
#pragma warning restore CS8767 // Nullability of reference types in type of parameter doesn't match implicitly implemented member (possibly because of nullability attributes).
        {
            return Key.CompareTo(((Enumeration)other).Key);
        }
    }
}
