namespace Tests
{
    using System;
    using System.Linq;
    using Chinchillada;
    using NUnit.Framework;

    public static class TypeExtensionsTests
    {
        [Test]
        public static void GetBaseClass()
        {
            var type = typeof(InheritingFromTestClass);

            var baseClasses = type.GetBaseClasses().ToList();

            Assert.Contains(typeof(TestClass), baseClasses);
        }

        #region Get All Fields

        [Test]
        public static void GetsPublicFields()
        {
            var type = typeof(TestClass);

            var fields = type.GetAllFields().ToList();

            Assert.IsTrue(fields.Any(field => field.IsPublic));
        }

        [Test]
        public static void GetsPrivateFields()
        {
            var type = typeof(TestClass);

            var fields = type.GetAllFields().ToList();

            Assert.IsTrue(fields.Any(field => field.IsPrivate));
        }

        [Test]
        public static void GetsBaseClassPrivateField()
        {
            var type = typeof(InheritingFromTestClass);

            var fields = type.GetAllFields().ToList();

            Assert.IsTrue(fields.Any(field => field.IsPrivate));
        }

        [Test]
        public static void GetsBaseClassPublicField()
        {
            var type = typeof(InheritingFromTestClass);

            var fields = type.GetAllFields().ToList();

            Assert.IsTrue(fields.Any(field => field.IsPublic));
        }

        #endregion

        [TestCase(typeof(TestClass), ExpectedResult                    = 2)]
        [TestCase(typeof(InheritingFromTestClass), ExpectedResult      = 2)]
        [TestCase(typeof(TestClassWithMixedAttributes), ExpectedResult = 1)]
        public static int GetsFieldsWithAttribute(Type type)
        {
            var fieldsWithAttributes = type.GetFieldsWithAttribute<MyTestAttribute>().ToList();
            return fieldsWithAttributes.Count;
        }

        #region classes

#pragma warning disable 169
#pragma warning disable 649
        private class InheritingFromTestClass : TestClass
        {
        }

        private class TestClass
        {
            [MyTest] public  int publicField;
            [MyTest] private int privateField;
        }

        private class TestClassWithMixedAttributes
        {
            public  int publicField;
            private int privateField;

            [MyTest] public int fieldWithAttribute;
        }

        private class MyTestAttribute : Attribute
        {
        }


#pragma warning restore 169
#pragma warning restore 649

        #endregion
    }
}