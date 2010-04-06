
// IMPORTANT: Make sure you know wether optimization flags
//            are on or off prior to utilizing benchmark results

using System;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;

using Reflex.Tests.Stubs;

namespace Reflex.PerfTests
{
    class Program
    {
        const int MILLIONS = 1024 * 1024;
        const int THOUSANDS = 1024;


        static void Main(string[] args)
        {
            while (true)
            {
                Console.Clear();

                // To benchmark individual methods, use the Benchmark function:
                //Benchmark(4 * MILLIONS, () => Get.PropertyValue(new Foo(), "A"));

                // To benchmark the .NET framework and/or algorithms, write the benchmark in a set:
                //BenchmarkTypeOfVersusGetType();
                //BenchmarkGenericsWithTypeOfVersusObjectWithGetType();

                // To benchmark operations of a class as a whole, use the following sets:
                //BenchmarkGets();
                //BenchmarkMaps();
                BenchmarkTransforms();

                Console.ReadKey();
            }
        }


        #region typeof(Foo) **versus** foo.GetType()

        static void BenchmarkTypeOfVersusGetType()
        {
            const int ITERATIONS = 100 * MILLIONS;

            var factory = new StubFactory();
            var foo = factory.MakeRandomFoo();

            Benchmark(ITERATIONS, () => usingTypeOf(foo));
            Benchmark(ITERATIONS, () => usingGetType(foo));
        }

        static void usingTypeOf(Foo foo)
        {
            var type = typeof(Foo);

            // dummy checks to try to get around code elimination optimization
            if (type.Name != "Foo") throw new Exception();
            if (foo.B == null) throw new Exception();
        }

        static void usingGetType(Foo foo)
        {
            var type = foo.GetType();

            // dummy checks to try to get around code elimination optimization
            if (type.Name != "Foo") throw new Exception();
            if (foo.B == null) throw new Exception();
        }

        #endregion

        #region generic<T>(T obj) { typeof(T) } **versus** get(object obj) { obj.GetType() }

        static void BenchmarkGenericsWithTypeOfVersusObjectWithGetType()
        {
            var ITERATIONS = 3 * MILLIONS;

            var factory = new StubFactory();
            var foo = factory.MakeRandomFoo();

            Benchmark(ITERATIONS, () => getValueGeneric(foo, "A"));
            Benchmark(ITERATIONS, () => getValueObject(foo, "A"));
        }

        static object getValueGeneric<T>(T obj, string propName)
        {
            var type = typeof(T);
            var prop = type.GetProperty(propName);

            return prop.GetValue(obj, Consts.Dud);
        }

        static object getValueObject(object obj, string propName)
        {
            var type = obj.GetType();
            var prop = type.GetProperty(propName);

            return prop.GetValue(obj, Consts.Dud);
        }

        #endregion


        #region Reflex class benchmarks

        static void BenchmarkGets()
        {
            const int ITERATIONS = 4 * MILLIONS;

            var factory = new StubFactory();
            var foo = factory.MakeRandomFoo();

            Benchmark(ITERATIONS, () => Get.Property(foo, "A"));
            Benchmark(ITERATIONS, () => Get.PropertyValue(foo, "A"));
            Benchmark(ITERATIONS, () => Get.Properties(foo));
            Benchmark(ITERATIONS, () => Get.Properties(foo, BindingFlags.NonPublic | BindingFlags.Instance));
        }

        static void BenchmarkMaps()
        {
            const int ITERATIONS = 1 * MILLIONS;

            var factory = new StubFactory();
            var foo = factory.MakeRandomFoo();
            var bar = factory.MakeRandomBar();

            var fooDict = Transform.ToDictionary(foo);
            var fooStringDict = Transform.ToStringDict(foo);
            // var fooNameValue = Transform.ToNameValue(foo);

            Benchmark(ITERATIONS, () => Map.Properties(foo, bar));
            Benchmark(ITERATIONS, () => Map.Dictionary(fooDict, bar));
            Benchmark(ITERATIONS, () => Map.StringDict(fooStringDict, bar));
        }

        static void BenchmarkTransforms()
        {
            const int ITERATIONS = 1 * MILLIONS;

            var factory = new StubFactory();
            var foo = factory.MakeRandomFoo();

            var fooDict = Transform.ToDictionary(foo);

            Benchmark(ITERATIONS, () => Transform.ToObject(fooDict));
        }

        #endregion


        static void Benchmark(int iterations, Expression<Action> testFunc)
        {


            // extract info from the expression
            var callExpr = (MethodCallExpression)testFunc.Body;
            var methodName = callExpr.Method.Name;


            // prep for tests
            var func = testFunc.Compile();

            var sw = new Stopwatch();
            sw.Reset();


            // run the tests
            Console.Write("Benchmarking \n\t{0}\n\t", methodName);

            sw.Start();
            for (int i = 0; i < iterations; i++) func();
            sw.Stop();

            Console.Write("{0} iterations in {1}ms\n",
                iterations, sw.ElapsedMilliseconds.ToString());
        }
    }
}
