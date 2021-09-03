namespace Tests
{
    using System.Linq;
    using Chinchillada;
    using Chinchillada.Tests;
    using NUnit.Framework;
    using UnityEngine;

    public class GameObjectExtensionsTests
    {
        [TearDown]
        public static void TearDown() => GameObjectMocking.TearDown();

        [Test]
        public static void FindsComponentOnOtherObject()
        {
            var component  = MockWithComponent();

            var gameObject = new GameObject();
            var result     = gameObject.GetComponentInScene<MockBehavior>();
            
            Assert.AreEqual(result, component);
        }

        [Test]
        public static void FindsAllComponentsInScene([Values(1, 5, 300)] int amount)
        {
            for (var i = 0; i < amount; i++) 
                MockWithComponent();

            var gameObject = new GameObject();
            var result = gameObject.GetComponentsInScene<MockBehavior>().ToList();

            Assert.That(result, Has.Count.EqualTo(amount));
        }
        
        private static MockBehavior MockWithComponent() => GameObjectMocking.WithComponent<MockBehavior>();

        private class MockBehavior : MonoBehaviour
        {
        }
    }
}