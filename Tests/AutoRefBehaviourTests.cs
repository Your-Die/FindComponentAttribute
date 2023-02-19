namespace Chinchillada.Tests
{
    using Chinchillada;
    using NUnit.Framework;
    using UnityEngine;

    public class AutoRefBehaviourTests : UnityObjectTests
    {
        [Test]
        public static void FindsComponentOnCreation()
        {
            var component = UnityTestUtil.CreateGameObjectWith<Component>();
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