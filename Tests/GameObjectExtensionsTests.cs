namespace Chinchillada.Tests
{
    using System.Linq;
    using Chinchillada;
    using NUnit.Framework;
    using UnityEngine;

    public class GameObjectExtensionsTests : UnityObjectTests
    {
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

            Assert.That(result, NUnit.Framework.Has.Count.EqualTo(amount));
        }
        
        private static MockBehavior MockWithComponent() => UnityTestUtil.CreateGameObjectWith<MockBehavior>();

        private class MockBehavior : MonoBehaviour
        {
        }
    }
}