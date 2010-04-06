
using System;
using System.Reflection;

using MbUnit.Framework;

using Reflex.Tests.Stubs;

namespace Reflex.Tests
{
    [TestFixture]
    public class GetTests : ReflexTestBase
    {
        [Test]
        public void PropertyShouldReturnPropertyInfo()
        {
            var foo = Stubs.MakeRandomFoo();

            var prop = Get.Property(foo, "A");

            Assert.IsNotNull(prop);
            Assert.AreEqual("A", prop.Name);
            Assert.AreEqual(foo.A, prop.GetValue(foo, new object[] { }));
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PropertyShouldThrowIfObjIsNull()
        {
            Get.Property(null, "A");
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PropertyShouldThrowIfNameIsNull()
        {
            Get.Property(new Foo(), null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PropertyShouldThrowIfNameIsEmpty()
        {
            Get.Property(new Foo(), "");
        }


        [Test]
        public void PropertyValueShouldReturnPropertyValue()
        {
            var foo = Stubs.MakeRandomFoo();

            var result = Get.PropertyValue(foo, "A");
            Assert.AreEqual(result, foo.A);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PropertyValueShouldThrowIfObjIsNull()
        {
            Get.PropertyValue(null, "A");
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PropertyValueShouldThrowIfNameIsNull()
        {
            Get.PropertyValue(new Foo(), null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PropertyValueShouldThrowIfNameIsEmpty()
        {
            Get.PropertyValue(new Foo(), "");
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void PropertyValueShouldThrowIfPropNotExists()
        {
            var foo = new Foo();

            Get.PropertyValue(foo, "Some_randomPropERty_xxx_Names");
        }


        [Test]
        public void PropertiesShouldReturnPropertiesSet()
        {
            var foo = new Foo();
            var properties = Get.Properties(foo);

            Assert.AreEqual(properties.Length, 2);
            Assert.AreEqual(properties[0].Name, "A");
            Assert.AreEqual(properties[1].Name, "B");
        }

        [Test]
        public void PropertiesWithPrivateFlagsShouldReturnPrivatePropertiesSet()
        {
            var foo = new Foo();
            var privateProps = Get.Properties(foo, BindingFlags.NonPublic | BindingFlags.Instance);

            Assert.AreEqual(privateProps.Length, 2);
            Assert.IsNotNull(privateProps[0]);
            Assert.AreEqual(privateProps[0].Name, "PV");
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PropertiesShouldThrowIfObjectIsNull()
        {
            Get.Properties(null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Properties2ShouldThrowIfObjectIsNull()
        {
            Get.Properties(null, BindingFlags.NonPublic | BindingFlags.Instance);
        }


        [Test]
        public void MethodsShouldReturnCorrectMethodSets()
        {
            var bar = new Bar();

            var methods = Get.Methods(bar);

            Assert.IsNotNull(methods);
            Assert.AreEqual(2, methods.Length);
            Assert.IsNotNull(methods[0]);
            Assert.AreEqual("SayHello", methods[0].Name);
        }

        [Test]
        public void MethodsWithFlagsShouldReturnCorrectMethodsSet()
        {
            var bar = new Bar();

            var methods = Get.Methods(bar, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);

            Assert.IsNotNull(methods);
            Assert.AreEqual(2, methods.Length);
            Assert.IsNotNull(methods[0]);
            Assert.AreEqual("sayHi", methods[0].Name);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void MethodsShouldThrowIfObjectIsNull()
        {
            Get.Methods(null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void MethodsWithFlagsShouldThrowIfObjectIsNull()
        {
            Get.Methods(null, BindingFlags.NonPublic | BindingFlags.Instance);
        }


        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void MethodShouldThrowIfObjIsNull()
        {
            Get.Method(null, "hello");
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void MethodShouldThrowIfNameIsNull()
        {
            Get.Method(new Bar(), null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void MethodShouldThrowIfNameIsEmpty()
        {
            Get.Method(new Bar(), "");
        }

        [Test]
        public void MethodShouldReturnCorrectMethodInfo()
        {
            var bar = new Bar();

            var method = Get.Method(bar, "SayHello");

            Assert.IsNotNull(method);

            // try to invoke and see if the result are equal
            var expected = bar.SayHello("John Doe");
            var actual = method.Invoke(bar, new[] { "John Doe" });

            Assert.AreEqual("SayHello", method.Name);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void MethodShouldReturnNullIfMatchPrivateMethod()
        {
            var bar = new Bar();

            var method = Get.Method(bar, "sayHi");

            Assert.IsNull(method);
        }

    }
}
