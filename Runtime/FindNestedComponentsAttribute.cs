using System.Collections;
using System.Reflection;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Chinchillada
{
    public class FindNestedComponentsAttribute : ComponentFinderAttribute
    {
        public override void Apply(MonoBehaviour behaviour, object obj, FieldInfo field)
        {
            ApplyInternal(behaviour, obj, field, null);
        }

        public override void Apply(MonoBehaviour behaviour, object obj, FieldInfo field, SearchStrategy searchStrategy)
        {
            ApplyInternal(behaviour, obj, field, searchStrategy);
        }

        private static void ApplyInternal(MonoBehaviour behaviour, object obj, FieldInfo field, SearchStrategy? strategy)
        {
            if (typeof(Object).IsAssignableFrom(field.FieldType))
                return;

            var nestedObject = field.GetValue(obj);

            if (nestedObject is IEnumerable collection)
                ResolveNestedCollection(behaviour, collection, strategy);
            else
                ResolveNestedObject(behaviour, nestedObject, strategy);
        }

        private static void ResolveNestedCollection(MonoBehaviour behaviour, IEnumerable collection,
                                                    SearchStrategy? strategy = null)
        {
            foreach (var nestedObject in collection)
                ResolveNestedObject(behaviour, nestedObject, strategy);
        }
        
        private static void ResolveNestedObject(MonoBehaviour behaviour, object nestedObject, SearchStrategy? strategy)
        {
            var nestedFields = TypeExtensions.GetAttributedFields<ComponentFinderAttribute>(nestedObject);

            if (strategy == null)
            {
                foreach (var (nestedField, attribute) in nestedFields)
                    attribute.Apply(behaviour, nestedObject, nestedField);
            }
            else
            {
                foreach (var (nestedField, attribute) in nestedFields)
                    attribute.Apply(behaviour, nestedObject, nestedField, strategy.Value);
            }
        }
    }
}