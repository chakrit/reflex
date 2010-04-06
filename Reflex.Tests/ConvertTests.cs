
using MbUnit.Framework;

using Reflex.Tests.Stubs;

namespace Reflex.Tests
{
    [TestFixture]
    public class ConvertTests : ReflexTestBase
    {
        [Test]
        public void ToGenericShouldConvertsBasicValueType()
        {
            object o = Stubs.MakeRandomNumber();

            var result = Convert.To<int>(o);

            Assert.IsInstanceOfType<int>(result);
            Assert.AreEqual(o, result);
        }

        [Test]
        public void ToGenericShouldConvertsBasicRefType()
        {
            object o = Stubs.MakeRandomFoo();

            var result = Convert.To<Foo>(o);

            Assert.IsNotNull(result);
            Assert.AreSame(o, result);
        }

        [Test]
        public void ToGenericWithValueTypeShouldReturnsDefaultIfValueIsNull()
        {
            object o = null;

            var result = Convert.To<int>(o);

            Assert.AreEqual(result, default(int));
        }

        [Test]
        public void ToGenericShouldReturnsDefaultIfValueTypeConversionFails()
        {
            object o = Stubs.MakeRandomString();

            var result = Convert.To<int>(o);

            Assert.AreEqual(result, default(int));
        }

        [Test]
        public void ToGenericShouldReturnsDefaultIfRefTypeToValueTypeConversionFails()
        {
            object o = Stubs.MakeRandomFoo();

            var result = Convert.To<int>(o);

            Assert.AreEqual(result, default(int));
        }

        [Test]
        public void ToGenericShouldReturnNullIfValueTypeToRefTypeConversionFails()
        {
            object o = Stubs.MakeRandomNumber();

            var result = Convert.To<Foo>(o);

            Assert.IsNull(result);
        }

        [Test]
        public void ToGenericShouldReturnNullIfRefTypeConversionFails()
        {
            object o = Stubs.MakeRandomBar();

            var result = Convert.To<Foo>(o);

            Assert.IsNull(result);
        }


        [Test]
        public void ToShouldConvertsBasicValueType()
        {
            object o = Stubs.MakeRandomNumber();

            var result = Convert.To(typeof(int), o);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType<int>(result);
            Assert.AreEqual(o, result);
        }

        [Test]
        public void ToShouldConvertsBasicRefType()
        {
            object o = Stubs.MakeRandomFoo();

            var result = Convert.To(typeof(Foo), o);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType<Foo>(result);
            Assert.AreSame(o, result);
        }

        [Test]
        public void ToWithValueTypeShouldReturnDefaultIfValueIsNull()
        {
            object o = null;

            var result = Convert.To(typeof(int), o);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType<int>(result);
            Assert.AreEqual(default(int), result);
        }

        [Test]
        public void ToShouldReturnsDefaultIfValueTypeConversionFails()
        {
            object o = Stubs.MakeRandomString();

            var result = Convert.To(typeof(int), o);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType<int>(result);
            Assert.AreEqual(default(int), result);
        }

        [Test]
        public void ToShouldReturnsDefaultIfRefTypeToValueTypeConversionFails()
        {
            object o = Stubs.MakeRandomFoo();

            var result = Convert.To(typeof(int), o);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType<int>(result);
            Assert.AreEqual(default(int), result);
        }

        [Test]
        public void ToShouldReturnNullIfValueTypeToRefTypeConversionFails()
        {
            object o = Stubs.MakeRandomNumber();

            var result = Convert.To(typeof(Foo), o);

            Assert.IsNull(result);
        }

        [Test]
        public void ToShouldReturnNullIfRefTypeConversionFails()
        {
            object o = Stubs.MakeRandomFoo();

            var result = Convert.To(typeof(Bar), o);

            Assert.IsNull(result);
        }

    }
}
