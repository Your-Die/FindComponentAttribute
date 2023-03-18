using Sirenix.OdinInspector;
using UnityEngine;

namespace Chinchillada
{
    using System.Collections.Generic;
    using System.Reflection;

    /// <summary>
    /// Base class for MonoBehaviours.
    /// Automatically applies <see cref="FindComponentAttribute"/> on awake,
    /// and also extends a Button to manually trigger it from the Unity editor.
    /// </summary>
    public abstract class AutoRefBehaviour : MonoBehaviour
    {
        protected virtual void Awake() => this.FindComponents();

        /// <summary>
        /// Applies the <see cref="FindComponentAttribute"/> on this <see cref="UnityEngine.MonoBehaviour"/>
        /// </summary>
        [Button]
        protected virtual void FindComponents()
        {
            var attributedFields = this.GetComponentFinderFields();

            foreach (var (field, attribute) in attributedFields)
                attribute.Apply(this, field);
        }

        /// <summary>
        /// Applies the <see cref="FindComponentAttribute"/> on each <see cref="AutoRefBehaviour"/>
        /// on the hosting <see cref="GameObject"/>.
        /// </summary>
        [ContextMenu("Find All Components")]
        private void FindAllComponents()
        {
            var behaviours = this.GetComponentsInChildren<AutoRefBehaviour>();
            
            foreach (var chinchillada in behaviours)
                chinchillada.FindComponents();
        }

        /// <summary>
        /// <see cref="FindComponentsCustom"/> with <see cref="SearchStrategy.InChildren"/>.
        /// </summary>
        [ContextMenu("Find Components In Children")]
        private void FindComponentsInChildren() => this.FindComponentsCustom(SearchStrategy.InChildren);

        /// <summary>
        /// <see cref="FindComponentsCustom"/> with <see cref="SearchStrategy.InParent"/>.
        /// </summary>
        [ContextMenu("Find Components In parents")]
        private void FindComponentsInParents() => this.FindComponentsCustom(SearchStrategy.InParent);

        /// <summary>
        /// <see cref="FindComponentsCustom"/> with <see cref="SearchStrategy.InScene"/>.
        /// </summary>
        [ContextMenu("Find Components Anywhere")]
        private void FindComponentsAnywhere() => this.FindComponentsCustom(SearchStrategy.InScene);

        /// <summary>
        /// Try to find components for each <see cref="FindComponentAttribute"/>, but using the
        /// <paramref name="strategy"/>.
        /// </summary>
        private void FindComponentsCustom(SearchStrategy strategy)
        {
            var attributedFields = this.GetComponentFinderFields();

            foreach (var (field, attribute) in attributedFields)
                attribute.Apply(this, field, strategy);
        }

        /// <summary>
        /// Get all fields with the <see cref="FindComponentAttribute"/>.
        /// </summary>
        /// <returns></returns>
        private IEnumerable<(FieldInfo, ComponentFinderAttribute)> GetComponentFinderFields()
        {
            var type = this.GetType();
            return type.GetFieldsWithAttribute<ComponentFinderAttribute>();
        }
    }
}