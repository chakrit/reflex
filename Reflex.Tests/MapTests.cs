
using System;
using System.Collections.Generic;
using System.Collections.Specialized;

using MbUnit.Framework;

using Reflex.Tests.Stubs;

namespace Reflex.Tests
{
    [TestFixture]
    public class MapTests : ReflexTestBase
    {
        [Test]
        public void PropertiesShouldCopyAllValues()
        {
            var foo = Stubs.MakeRandomFoo();
            var bar = Stubs.MakeRandomBar();

            Map.Properties(foo, bar);

            Assert.AreEqual(foo.A, bar.A);
            Assert.AreEqual(foo.B, bar.B);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PropertiesShouldThrowIfTargetIsNull()
        {
            Map.Properties(new Foo(), null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PropertiesShouldThrowIfSrcIsNull()
        {
            Map.Properties(null, new Bar());
        }

        [Test]
        public void DictionaryShouldCopyAllValues()
        {
            var foo = Stubs.MakeRandomFoo();
            var foo2 = new Foo();

            var dict = new Dictionary<string, object> {
                { "A", foo.A },
                { "B", foo.B }
            };

            Map.Dictionary(dict, foo2);

            Assert.AreEqual(foo.A, foo2.A);
            Assert.AreEqual(foo.B, foo2.B);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DictionaryShouldThrowIfDictIsNull()
        {
            Map.Dictionary((IDictionary<string, object>)null, new Foo());
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DictionaryShouldThrowIfTargetIsNull()
        {
            var dict = new Dictionary<string, object>();

            Map.Dictionary(dict, null);
        }

        [Test]
        public void StringDictShouldCopyAllValues()
        {
            var foo = Stubs.MakeRandomFoo();
            var foo2 = new Foo();

            var dict = new Dictionary<string, string> {
                { "A", foo.A.ToString() },
                { "B", foo.B.ToString() }
            };

            Map.StringDict(dict, foo2);

            Assert.AreEqual(foo.A, foo2.A);
            Assert.AreEqual(foo.B, foo2.B);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void StringDictShouldThrowIfDictIsNull()
        {
            Map.StringDict(null, new Foo());
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void StringDictShouldThrowIfTargetIsNull()
        {
            Map.StringDict(new Dictionary<string, string>(), null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void StringDictShouldThrowIfFoundUnrecognizedKey()
        {
            var foo = new Foo();
            var dict = new Dictionary<string, string>
            {
                { "NonExistentPropertySurely", "UnparsedValue" }
            };

            Map.StringDict(dict, foo);
        }

        [Test]
        public void NameValueShouldCopyAllValues()
        {
            var foo = Stubs.MakeRandomFoo();
            var foo2 = new Foo();

            var col = new NameValueCollection();
            col.Add("A", foo.A.ToString());
            col.Add("B", foo.B.ToString());

            Map.NameValue(col, foo2);

            Assert.AreEqual(foo.A, foo2.A);
            Assert.AreEqual(foo.B, foo2.B);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NameValueShouldThrowIfColIsNull()
        {
            Map.NameValue(null, new Foo());
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NameValueShouldThrowIfTargetIsNull()
        {
            var col = new NameValueCollection();

            Map.NameValue(col, null);
        }
    }
}
