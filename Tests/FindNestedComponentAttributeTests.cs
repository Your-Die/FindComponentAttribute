namespace Tests
{
    using System;
    using Chinchillada;
    using Chinchillada.Tests;
    using NUnit.Framework;
    using UnityEngine;

    public static class FindNestedComponentAttributeTests
    {
        [TearDown]
        public static void TearDown() => GameObjectMocking.TearDown();
        
        [Test]
        public static void FindNestedComponent()
        {
            var behaviour = GameObjectMocking.WithComponent<BehaviourWithNested>();
            var component = behaviour.gameObject.AddComponent<Component>();

            var fieldInfo = typeof(BehaviourWithNested).GetField(nameof(BehaviourWithNested.classWithField));

            var attribute = new FindNestedComponentsAttribute();
            attribute.Apply(behaviour, fieldInfo);

            Assert.That(behaviour.classWithField.component, Is.EqualTo(component));
        }
        
        public class BehaviourWithNested : MonoBehaviour
        {
            public ClassWithField classWithField = new ClassWithField();
        }

        [Serializable]
        public class ClassWithField
        {
            [FindComponent] public Component component;
        }

        public class Component : MonoBehaviour
        {
        }
    }
}