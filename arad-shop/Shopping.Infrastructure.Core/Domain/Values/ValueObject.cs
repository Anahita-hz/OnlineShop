﻿using System;
using System.Linq;
using System.Reflection;

namespace Shopping.Infrastructure.Core.Domain.Values
{
    public abstract class ValueObject<TValueObject> : IEquatable<TValueObject>
        where TValueObject : ValueObject<TValueObject>
    {
        public bool Equals(TValueObject other)
        {
            if (other == null)
            {
                return false;
            }
            var publicProperties = GetPropertiesForCompare();
            if (!publicProperties.Any())
            {
                return true;
            }
            return publicProperties.All(property =>
                Equals(property.GetValue(this, null), property.GetValue(other, null)));
        }
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            var item = obj as ValueObject<TValueObject>;
            return (object)item != null && Equals((TValueObject)item);

        }
        public override int GetHashCode()
        {
            const int index = 1;
            const int initialHasCode = 31;
            var publicProperties = GetPropertiesForCompare();
            if (!publicProperties.Any())
            {
                return initialHasCode;
            }
            var hashCode = initialHasCode;
            var changeMultiplier = false;
            foreach (var property in publicProperties)
            {
                var value = property.GetValue(this, null);
                if (value == null)
                {
                    hashCode = hashCode ^ (index * 13);
                    continue;
                }
                hashCode = hashCode * (changeMultiplier ? 59 : 114) + value.GetHashCode();
                changeMultiplier = !changeMultiplier;
            }
            return hashCode;
        }
        public static bool operator ==(ValueObject<TValueObject> x, ValueObject<TValueObject> y)
        {
            if (ReferenceEquals(x, y))
            {
                return true;
            }

            if (((object)x == null) || ((object)y == null))
            {
                return false;
            }

            return x.Equals(y);
        }
        public static bool operator !=(ValueObject<TValueObject> x, ValueObject<TValueObject> y)
        {
            return !(x == y);
        }
        private PropertyInfo[] GetPropertiesForCompare()
        {
            return GetType().GetTypeInfo().GetProperties().Where(t =>
                ReflectionHelper.GetSingleAttributeOrDefault<IgnoreOnCompareAttribute>(t) == null).ToArray();
        }
        protected virtual void Validate()
        {
        }
    }
}