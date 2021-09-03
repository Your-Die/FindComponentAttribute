namespace Tests
{
    using Chinchillada;
    using Chinchillada.Tests;
    using NUnit.Framework;
    using UnityEngine;

    public static class SearchStrategyTests
    {
        [TearDown]
        public static void TearDown() => GameObjectMocking.TearDown();

        [TestCase(SearchStrategy.FindComponent, ExpectedResult = true)]
        [TestCase(SearchStrategy.InChildren, ExpectedResult    = true)]
        [TestCase(SearchStrategy.InParent, ExpectedResult      = true)]
        [TestCase(SearchStrategy.InScene, ExpectedResult       = true)]
        public static bool FindsComponentGeneric(SearchStrategy strategy)
        {
            var behaviour = GameObjectMocking.WithComponent<Behaviour>();

            var result = strategy.FindComponent<Behaviour>(behaviour.gameObject);

            return result == behaviour;
        }
        
        [TestCase(SearchStrategy.FindComponent, ExpectedResult = false)]
        [TestCase(SearchStrategy.InChildren, ExpectedResult    = true)]
        [TestCase(SearchStrategy.InParent, ExpectedResult      = false)]
        [TestCase(SearchStrategy.InScene, ExpectedResult       = true)]
        public static bool FindsComponentInChildren(SearchStrategy strategy)
        {
            var gameObject = GameObjectMocking.Empty();
            var child     = GameObjectMocking.WithComponent<Behaviour>();
            
            child.transform.parent = gameObject.transform;

            var result = strategy.FindComponent<Behaviour>(gameObject);

            return result == child;
        }
        
        [TestCase(SearchStrategy.FindComponent, ExpectedResult = false)]
        [TestCase(SearchStrategy.InChildren, ExpectedResult    = false)]
        [TestCase(SearchStrategy.InParent, ExpectedResult      = true)]
        [TestCase(SearchStrategy.InScene, ExpectedResult       = true)]
        public static bool FindsComponentInParent(SearchStrategy strategy)
        {
            var gameObject = GameObjectMocking.Empty();
            var parent      = GameObjectMocking.WithComponent<Behaviour>();
            
            gameObject.transform.parent = parent.transform;

            var result = strategy.FindComponent<Behaviour>(gameObject);

            return result == parent;
        }
        
        [TestCase(SearchStrategy.FindComponent, ExpectedResult = false)]
        [TestCase(SearchStrategy.InChildren, ExpectedResult    = false)]
        [TestCase(SearchStrategy.InParent, ExpectedResult      = false)]
        [TestCase(SearchStrategy.InScene, ExpectedResult       = true)]
        public static bool FindsComponentInScene(SearchStrategy strategy)
        {
            var gameObject = GameObjectMocking.Empty();
            var other     = GameObjectMocking.WithComponent<Behaviour>();
            
            var result = strategy.FindComponent<Behaviour>(gameObject);

            return result == other;
        }

        public class Behaviour : MonoBehaviour
        {
        }
    }
}