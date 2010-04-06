
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

using MbUnit.Framework;

using Reflex.Tests.Stubs;

namespace Reflex.Tests
{
    [TestFixture]
    public class TransformTests : ReflexTestBase
    {
        [Test]
        public void ToDictionaryShouldProduceCorrectDictionary()
        {
            var foo = Stubs.MakeRandomFoo();

            var dict = Transform.ToDictionary(foo);

            Assert.IsTrue(dict.Keys.Contains("A"));
            Assert.IsInstanceOfType(foo.A.GetType(), dict["A"]);
            Assert.AreEqual(dict["A"], foo.A);

            Assert.IsTrue(dict.Keys.Contains("B"));
            Assert.IsInstanceOfType(foo.B.GetType(), dict["B"]);
            Assert.AreEqual(dict["B"], foo.B);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ToDictionaryShouldThrowIfSrcIsNull()
        {
            Transform.ToDictionary(null);
        }


        [Test]
        public void ToDictionaryGenericShouldProduceCorrectDictionary()
        {
            var foo = Stubs.MakeRandomFoo();

            // try with foo.B as a string representation of number
            foo.B = Stubs.MakeRandomNumber().ToString();

            var dict = Transform.ToDictionary<int>(foo);

            Assert.IsTrue(dict.Keys.Contains("A"));
            Assert.AreEqual(foo.A, dict["A"]);

            Assert.IsTrue(dict.Keys.Contains("B"));
            Assert.AreEqual(int.Parse(foo.B), dict["B"]);
        }

        [Test]
        public void ToDictionaryGenericShouldUseDefaultWhenConversionFails()
        {
            var foo = Stubs.MakeRandomFoo();

            foo.B = "not a number surely";

            var dict = Transform.ToDictionary<int>(foo);

            Assert.IsTrue(dict.Keys.Contains("A"));
            Assert.AreEqual(foo.A, dict["A"]);

            Assert.IsTrue(dict.Keys.Contains("B"));
            Assert.AreEqual(default(int), dict["B"]);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ToDictionaryGenericShouldThrowIfSrcIsNull()
        {
            Transform.ToDictionary<int>(null);
        }


        [Test]
        public void ToStringDictShouldProduceCorrectDictionary()
        {
            var foo = Stubs.MakeRandomFoo();

            var dict = Transform.ToStringDict(foo);

            Assert.IsTrue(dict.Keys.Contains("A"));
            Assert.AreEqual(foo.A.ToString(), dict["A"]);

            Assert.IsTrue(dict.Keys.Contains("B"));
            Assert.AreEqual(foo.B, dict["B"]);
        }

        [Test]
        public void ToStringDictWithNameValueShouldProduceEquivalentDict()
        {
            var col = new NameValueCollection();

            col.Add("A", "hello");
            col.Add("B", "world");

            var dict = Transform.ToStringDict(col);

            Assert.IsNotNull(dict);

            Assert.IsTrue(dict.ContainsKey("A"));
            Assert.AreEqual(col["A"], dict["A"]);

            Assert.IsTrue(dict.ContainsKey("B"));
            Assert.AreEqual(col["B"], dict["B"]);
        }

        [Test]
        public void ToStringDictShouldPertainNulls()
        {
            var foo = new Foo { B = null };

            var dict = Transform.ToStringDict(foo);

            Assert.IsNull(dict["B"]);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ToStringDictShouldThrowIfSrcIsNull()
        {
            Transform.ToStringDict((object)null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ToStringDictWithNameValueShouldThrowIfColIsNull()
        {
            Transform.ToStringDict((NameValueCollection)null);
        }


        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ToNameValueShouldThrowIfSrcIsNull()
        {
            Transform.ToNameValue((object)null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ToNameValueWithStringDictShouldThrowIfDictIsNull()
        {
            Transform.ToNameValue((IDictionary<string, string>)null);
        }

        [Test]
        public void ToNameValueShouldProduceCorrectCollection()
        {
            var foo = Stubs.MakeRandomFoo();

            var col = Transform.ToNameValue(foo);

            Assert.IsTrue(col.AllKeys.Contains("A"));
            Assert.AreEqual(foo.A.ToString(), col["A"]);

            Assert.IsTrue(col.AllKeys.Contains("B"));
            Assert.AreEqual(foo.B, col["B"]);
        }

        [Test]
        public void ToNameValueWithStringDictShouldProduceEquivalentCollection()
        {
            var dict = new Dictionary<string, string>();

            dict.Add("A", "hello");
            dict.Add("B", "world");

            var col = Transform.ToNameValue(dict);

            Assert.IsNotNull(col);

            Assert.IsTrue(col.AllKeys.Contains("A"));
            Assert.AreEqual(col["A"], dict["A"]);

            Assert.IsTrue(col.AllKeys.Contains("B"));
            Assert.AreEqual(col["B"], dict["B"]);
        }

        [Test]
        public void ToNameValueShouldPertainNulls()
        {
            var foo = new Foo { B = null };

            var col = Transform.ToNameValue(foo);

            Assert.IsNull(col["B"]);
        }


        [Test]
        public void ToObjectShouldProduceNewObject()
        {
            // build source data
            var dict = new Dictionary<string, object>();
            dict.Add("A", 123);
            dict.Add("B", new object());
            dict.Add("C", "a string");

            // perform the transform
            var obj = Transform.ToObject(dict);

            // verify results
            var type = obj.GetType();
            var dud = new object[] { };

            foreach (var item in dict)
            {
                var prop = type.GetProperty(item.Key);

                var dictValue = dict[item.Key];
                var propValue = prop.GetValue(obj, dud);

                Assert.AreEqual(dictValue.GetType(), propValue.GetType());
                Assert.AreEqual(dictValue, propValue);
            }
        }

        [Test]
        public void ToObjectShouldWorkWithComplexTypes()
        {
            // build source data
            var dict = new Dictionary<string, object>();
            dict.Add("Simple", "string");
            dict.Add("Complex", new
            {
                Complex = new { ComplexValue = "Complex" }
            });

            // perform the transformation
            var obj = Transform.ToObject(dict);

            // verify results
            var type = obj.GetType();
            var dud = new object[] { };

            foreach (var item in dict)
            {
                var prop = type.GetProperty(item.Key);

                var dictValue = dict[item.Key];
                var propValue = prop.GetValue(obj, dud);

                Assert.AreEqual(dictValue.GetType(), propValue.GetType());
                Assert.AreEqual(dictValue, propValue);
            }
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ToObjectShouldThrowIfArgumentIsNull()
        {
            Transform.ToObject((IDictionary<string, object>)null);
        }


        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ToDelegateShouldThrowIfMethodInfoIsNull()
        {
            Transform.ToDelegate(null);
        }

        [Test]
        public void ToDelegateWithTargetShouldProduceCorrectDelegate()
        {
            var bar = new Bar();

            var type = typeof(Bar);
            var method = type.GetMethod("SayHello");

            var del = Transform.ToDelegate(method, bar);

            Assert.IsNotNull(del);

            // verify that results match
            var expected = bar.SayHello("John Doe");
            var actual = del.DynamicInvoke(new[] { "John Doe" });

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ToDelegateShouldProduceCorrectDelegate()
        {
            var type = typeof(Bar);
            var method = type.GetMethod("StaticHello");

            var del = Transform.ToDelegate(method);

            Assert.IsNotNull(del);

            // verify that results match
            var expected = Bar.StaticHello("John Doe");
            var actual = del.DynamicInvoke(new[] { "John Doe" });

            Assert.AreEqual(expected, actual);
        }
    }
}
