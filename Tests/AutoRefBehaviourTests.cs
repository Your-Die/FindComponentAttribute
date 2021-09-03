namespace Tests
{
    using Chinchillada;
    using Chinchillada.Tests;
    using NUnit.Framework;
    using UnityEngine;

    public static class AutoRefBehaviourTests
    {
        [TearDown]
        public static void TearDown() => GameObjectMocking.TearDown();

        [Test]
        public static void FindsComponentOnCreation()
        {
            var component = GameObjectMocking.WithComponent<Component>();

            var behaviour = component.gameObject.AddComponent<BehaviourWithField>();
            
            Assert.That(behaviour.component, Is.EqualTo(component));
        }

        public class BehaviourWithField : AutoRefBehaviour
        {
            [FindComponent] public Component component;
        }

        public class Component : MonoBehaviour
        {
        }
    }
}