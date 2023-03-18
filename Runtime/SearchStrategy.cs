using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Chinchillada
{
    /// <summary>
    /// Enum containing possible search strategies.
    /// </summary>
    public enum SearchStrategy
    {
        FindComponent,
        InParent,
        InChildren,
        InScene
    }

    public static class SearchStrategyExtensions
    {
        public static T FindComponent<T>(this SearchStrategy strategy, GameObject gameObject,
                                         bool                includeInactive = false)
        {
            switch (strategy)
            {
                case SearchStrategy.FindComponent: return gameObject.GetComponent<T>();
                case SearchStrategy.InParent:      return gameObject.GetComponentInParent<T>(includeInactive);
                case SearchStrategy.InChildren:    return gameObject.GetComponentInChildren<T>(includeInactive);
                case SearchStrategy.InScene:       return gameObject.GetComponentInScene<T>(includeInactive);
                default:
                    throw new ArgumentOutOfRangeException(nameof(strategy), strategy, null);
            }
        }

        public static IEnumerable<T> FindComponents<T>(this SearchStrategy strategy,
                                                       GameObject          gameObject,
                                                       bool                includeInactive = false)
        {
            switch (strategy)
            {
                case SearchStrategy.FindComponent: return gameObject.GetComponents<T>();
                case SearchStrategy.InParent:      return gameObject.GetComponentsInParent<T>(includeInactive);
                case SearchStrategy.InChildren:    return gameObject.GetComponentsInChildren<T>(includeInactive);
                case SearchStrategy.InScene:       return gameObject.GetComponentsInScene<T>(includeInactive);

                default: throw new ArgumentOutOfRangeException(nameof(strategy), strategy, null);
            }
        }

        public static Component FindComponent(this SearchStrategy strategy, GameObject gameObject, Type type)
        {
            switch (strategy)
            {
                case SearchStrategy.FindComponent: return gameObject.GetComponent(type);
                case SearchStrategy.InParent:      return gameObject.GetComponentInParent(type);
                case SearchStrategy.InChildren:    return gameObject.GetComponentInChildren(type);
                case SearchStrategy.InScene:       return (Component)Object.FindObjectOfType(type);
                default:                           throw new ArgumentOutOfRangeException();
            }
        }

        public static IEnumerable<Component> FindComponents(this SearchStrategy strategy, GameObject gameObject,
                                                            Type                type)
        {
            switch (strategy)
            {
                case SearchStrategy.FindComponent:
                    return gameObject.GetComponents(type);
                case SearchStrategy.InParent:
                    return gameObject.GetComponentsInParent(type);
                case SearchStrategy.InChildren:
                    return gameObject.GetComponentsInChildren(type);
                case SearchStrategy.InScene:
                    return Object.FindObjectsOfType(type).Cast<Component>();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}