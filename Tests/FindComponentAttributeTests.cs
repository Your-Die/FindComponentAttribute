namespace Tests
{
    using System;
    using System.Collections.Generic;
    using Chinchillada;
    using Chinchillada.Tests;
    using NUnit.Framework;
    using UnityEngine;

    public static class FindComponentAttributeTests
    {
        [TearDown]
        public static void TearDown()
        {
            GameObjectMocking.TearDown();
        }

        #region Fields

        [TestCase(SearchStrategy.FindComponent, ExpectedResult = true)]
        [TestCase(SearchStrategy.InChildren, ExpectedResult    = true)]
        [TestCase(SearchStrategy.InParent, ExpectedResult      = true)]
        [TestCase(SearchStrategy.InScene, ExpectedResult       = true)]
        public static bool FindsAndAssignsComponent(SearchStrategy strategy)
        {
            var behavior  = GameObjectMocking.WithComponent<BehaviorWithField>();
            var component = behavior.gameObject.AddComponent<TestComponent>();

            return ExecuteFindFieldTest(behavior, strategy, component);
        }

        [TestCase(SearchStrategy.FindComponent, ExpectedResult = false)]
        [TestCase(SearchStrategy.InChildren, ExpectedResult    = true)]
        [TestCase(SearchStrategy.InParent, ExpectedResult      = false)]
        [TestCase(SearchStrategy.InScene, ExpectedResult       = true)]
        public static bool FindsAndAssignsComponentInChild(SearchStrategy strategy)
        {
            var behavior = GameObjectMocking.WithComponent<BehaviorWithField>();
            var child    = GameObjectMocking.WithComponent<TestComponent>();

            child.transform.parent = behavior.transform;

            return ExecuteFindFieldTest(behavior, strategy, child);
        }

        [TestCase(SearchStrategy.FindComponent, ExpectedResult = false)]
        [TestCase(SearchStrategy.InChildren, ExpectedResult    = false)]
        [TestCase(SearchStrategy.InParent, ExpectedResult      = true)]
        [TestCase(SearchStrategy.InScene, ExpectedResult       = true)]
        public static bool FindsAndAssignsComponentInParent(SearchStrategy strategy)
        {
            var behavior = GameObjectMocking.WithComponent<BehaviorWithField>();
            var parent   = GameObjectMocking.WithComponent<TestComponent>();

            behavior.transform.parent = parent.transform;

            return ExecuteFindFieldTest(behavior, strategy, parent);
        }

        [TestCase(SearchStrategy.FindComponent, ExpectedResult = false)]
        [TestCase(SearchStrategy.InChildren, ExpectedResult    = false)]
        [TestCase(SearchStrategy.InParent, ExpectedResult      = false)]
        [TestCase(SearchStrategy.InScene, ExpectedResult       = true)]
        public static bool FindsAndAssignsComponentInScene(SearchStrategy strategy)
        {
            var behavior = GameObjectMocking.WithComponent<BehaviorWithField>();
            var other    = GameObjectMocking.WithComponent<TestComponent>();

            return ExecuteFindFieldTest(behavior, strategy, other);
        }

        private static bool ExecuteFindFieldTest(MonoBehaviour behavior, SearchStrategy strategy,
                                                 TestComponent expected)
        {
            var fieldInfo = typeof(BehaviorWithField).GetField(nameof(BehaviorWithField.component));

            var attribute = new FindComponentAttribute(strategy);
            attribute.Apply(behavior, fieldInfo);

            var value = fieldInfo.GetValue(behavior);
            return ReferenceEquals(value, expected);
        }

        #endregion

        #region Collections

        [TestCase(SearchStrategy.FindComponent, ExpectedResult = true)]
        [TestCase(SearchStrategy.InChildren, ExpectedResult = true)]
        [TestCase(SearchStrategy.InParent, ExpectedResult = true)]
        [TestCase(SearchStrategy.InScene, ExpectedResult = true)]
        public static bool FillsCollectionOfComponents(SearchStrategy strategy)
        {
            const int componentCount = 5;
            
            var behavior = GameObjectMocking.WithComponent<BehaviorWithCollection>();

            for (var i = 0; i < componentCount; i++)
                behavior.gameObject.AddComponent<TestComponent>();

            return ExecuteFindCollectionTest(behavior, strategy, componentCount);
        }


        [TestCase(SearchStrategy.FindComponent, ExpectedResult = false)]
        [TestCase(SearchStrategy.InChildren, ExpectedResult    = true)]
        [TestCase(SearchStrategy.InParent, ExpectedResult      = false)]
        [TestCase(SearchStrategy.InScene, ExpectedResult       = true)]
        public static bool FillsCollectionOfComponentsInChildren(SearchStrategy strategy)
        {
            const int componentCount = 5;
            
            var behavior = CreateBehaviorAndComponents(componentCount, MakeChild);

            return ExecuteFindCollectionTest(behavior, strategy, componentCount);
            
            static void MakeChild(TestComponent component, BehaviorWithCollection parent)
            {
                component.transform.parent = parent.transform;
            }
        }
        
        [TestCase(SearchStrategy.FindComponent, ExpectedResult = false)]
        [TestCase(SearchStrategy.InChildren, ExpectedResult    = false)]
        [TestCase(SearchStrategy.InParent, ExpectedResult      = true)]
        [TestCase(SearchStrategy.InScene, ExpectedResult       = true)]
        public static bool FillsCollectionOfComponentsInParent(SearchStrategy strategy)
        {
            const int componentCount = 5;

            var behavior             = GameObjectMocking.WithComponent<BehaviorWithCollection>();
            var objectWithComponents = CreateComponents();

            behavior.transform.parent = objectWithComponents.transform;

            return ExecuteFindCollectionTest(behavior, strategy, componentCount);
            
            static GameObject CreateComponents()
            {
                var gameObject = GameObjectMocking.Empty();

                for (int i = 0; i < componentCount; i++) 
                    gameObject.AddComponent<TestComponent>();

                return gameObject;
            }
        }
        
        [TestCase(SearchStrategy.FindComponent, ExpectedResult = false)]
        [TestCase(SearchStrategy.InChildren, ExpectedResult    = false)]
        [TestCase(SearchStrategy.InParent, ExpectedResult      = false)]
        [TestCase(SearchStrategy.InScene, ExpectedResult       = true)]
        public static bool FillsCollectionOfComponentsInScene(SearchStrategy strategy)
        {
            const int componentCount = 5;
            
            var behavior = CreateBehaviorAndComponents(componentCount, DoNothing);

            return ExecuteFindCollectionTest(behavior, strategy, componentCount);
            
            static void DoNothing(TestComponent __, BehaviorWithCollection _)
            {
            }
        }


        private static MonoBehaviour CreateBehaviorAndComponents(int componentCount,
                                                                 Action<TestComponent, BehaviorWithCollection>
                                                                     componentInitializer)
        {
            var behavior = GameObjectMocking.WithComponent<BehaviorWithCollection>();
            
            for (var i = 0; i < componentCount; i++)
            {
                var component = GameObjectMocking.WithComponent<TestComponent>();
                componentInitializer.Invoke(component, behavior);
            }

            return behavior;
        }

        private static bool ExecuteFindCollectionTest(MonoBehaviour  behavior,
                                                      SearchStrategy strategy,
                                                      int            expectedAmount)
        {
            var fieldInfo = typeof(BehaviorWithCollection).GetField(nameof(BehaviorWithCollection.components));

            var attribute = new FindComponentAttribute(strategy);
            attribute.Apply(behavior, fieldInfo);

            var collection = (List<TestComponent>) fieldInfo.GetValue(behavior);
            return collection.Count == expectedAmount;
        }

        #endregion

        public class TestComponent : MonoBehaviour
        {
        }

        public class BehaviorWithField : MonoBehaviour
        {
            [FindComponent] public TestComponent component;
        }

        public class BehaviorWithCollection : MonoBehaviour
        {
            [FindComponent] public List<TestComponent> components = new List<TestComponent>();
        }
    }
}