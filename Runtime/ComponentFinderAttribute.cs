using System.Reflection;
using UnityEngine;

namespace Chinchillada
{
    /// <summary>
    /// Base class for attributes that find component references.
    /// </summary>
    public abstract class ComponentFinderAttribute : PropertyAttribute
    {
        public void Apply(MonoBehaviour behaviour, FieldInfo field)
        {
            this.Apply(behaviour, behaviour, field);
        }

        public void Apply(MonoBehaviour behaviour, FieldInfo field, SearchStrategy strategy)
        {
            this.Apply(behaviour, behaviour, field, strategy);
        }
        
        public abstract void Apply(MonoBehaviour behaviour, object obj, FieldInfo field);

        public abstract void Apply(MonoBehaviour behaviour, object obj, FieldInfo field, SearchStrategy searchStrategy);
    }
}