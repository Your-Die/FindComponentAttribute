using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Reflection;

namespace Chinchillada
{
    /// <summary>
    /// Attribute meant for removing the boilerplate code that is often found in Unity Monobehaviour classes.
    /// This attribute automates the setting up of references to other components, instead of having to manually write the <see cref="GetComponent"/>
    /// for each component reference that is necessary.
    /// </summary>
    public class FindComponentAttribute : ComponentFinderAttribute
    {
        /// <summary>
        /// The search <see cref="SearchStrategy"/> that we want to use when looking for matching components.
        /// </summary>
        private readonly SearchStrategy strategy;

        /// <summary>
        /// Constructs a new <see cref="FindComponentAttribute"/>.
        /// </summary>
        /// <param name="strategy">The search <see cref="SearchStrategy"/> that we want to use when looking for matching components.</param>
        public FindComponentAttribute(SearchStrategy strategy = SearchStrategy.FindComponent)
        {
            this.strategy = strategy;
        }

        /// <inheritdoc />
        public override void Apply(MonoBehaviour behaviour, object obj, FieldInfo field)
        {
            this.Apply(behaviour, obj, field, this.strategy);
        }

        public override void Apply(MonoBehaviour behaviour, object obj, FieldInfo field, SearchStrategy searchStrategy)
        {
            var value = field.GetValue(obj);

            if (value is IList)
                ResolveCollection(behaviour, obj, field, this.strategy);
            else
                ResolveField(behaviour, obj, field, searchStrategy);
        }



        private static void ResolveField(Component behaviour, object obj, FieldInfo field, SearchStrategy strategy)
        {
            var fieldValue = field.GetValue(obj);

            if (fieldValue != null)
                return;

            var result = strategy.FindComponent(behaviour.gameObject, field.FieldType);
            field.SetValue(obj, result);
        }

        private static void ResolveCollection(Component behaviour, object obj, FieldInfo field, SearchStrategy strategy)
        {
            if (field.FieldType.IsGenericType == false)
            {
                var message = $"{typeof(FindComponentAttribute)} found on non-generic collection field: {field}. " +
                              "This is not currently supported.";
                Debug.LogWarning(message);
                return;
            }

            var itemTypes = field.FieldType.GetGenericArguments();
            if (itemTypes.Length > 1)
            {
                var message = $"{typeof(FindComponentAttribute)} found on collection field with multiple type arguments: {field}  " +
                              "This is not currently supported.";
                Debug.LogWarning(message);
                return;
            }

            var itemType = itemTypes.First();

            var fieldValue = field.GetValue(obj);
            if (fieldValue == null)
                return;

            var list = (IList) fieldValue;
            
            var items = strategy.FindComponents(behaviour.gameObject, itemType);

            foreach (var item in items)
            {
                if (!list.Contains(item)) 
                    list.Add(item);
            }
        }
    }
}