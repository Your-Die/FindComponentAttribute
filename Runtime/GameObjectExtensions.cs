namespace Chinchillada
{
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    public static class GameObjectExtensions
    {
        public static T GetComponentInScene<T>(this GameObject gameObject, bool includeInactive = false)
        {
            var components = gameObject.GetComponentsInScene<T>(includeInactive);
            return components.First();
        }

        public static IEnumerable<T> GetComponentsInScene<T>(this GameObject gameObject, bool includeInactive = false)
        {
            var rootObjects = gameObject.scene.GetRootGameObjects();
            foreach (var root in rootObjects)
            {
                var components = root.GetComponentsInChildren<T>(includeInactive);
                foreach (var component in components)
                    yield return component;
            }
        }
    }
}