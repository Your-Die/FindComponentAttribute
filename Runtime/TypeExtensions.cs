namespace Chinchillada
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using UnityEngine;
    using Object = UnityEngine.Object;

    public static class TypeExtensions
    {
        public static IEnumerable<Type> GetBaseClasses(this Type type)
        {
            for (var current = type.BaseType; current != null; current = current.BaseType)
                yield return current;
        }
        
        public static IEnumerable<FieldInfo> GetAllFields(this Type type)
        {
            const BindingFlags bindingFlags = BindingFlags.Instance     |
                                              BindingFlags.DeclaredOnly |
                                              BindingFlags.NonPublic    |
                                              BindingFlags.Public;

            var types = type.GetBaseClasses().Prepend(type);
            return types.SelectMany(baseType => baseType.GetFields(bindingFlags));
        }

        public static IEnumerable<(FieldInfo, TAttribute)> GetFieldsWithAttribute<TAttribute>(this Type type) 
            where TAttribute : Attribute
        {
            var fields = type.GetAllFields();

            foreach (var field in fields)
            {
                var attributes = field.GetCustomAttributes(typeof(TAttribute)).ToList();
                var attribute  = (TAttribute) attributes.FirstOrDefault();

                if (attribute != null)
                    yield return (field, attribute);
            }
        }

        public static IEnumerable<(FieldInfo field, TAttribute)> GetAttributedFields<TAttribute>(object obj)
            where TAttribute : PropertyAttribute
        {
            var type   = obj.GetType();
            var fields = type.GetAllFields();

            foreach (var field in fields)
            {
                var value = field.GetValue(obj);
                if (!IsNull(value))
                    continue;

                var attributes = field.GetCustomAttributes(typeof(TAttribute)).ToList();
                if (!attributes.Any())
                    continue;

                var attribute = (TAttribute) attributes.First();
                yield return (field, attribute);
            }

            static bool IsNull(object item)
            {
                if (item == null)
                    return true;

                if (item is Object unityObject)
                    return unityObject == null;

                return false;
            }
        }
    }
}