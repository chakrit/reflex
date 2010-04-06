
using System;


namespace Reflex.Tests.Stubs
{
    public class StubFactory
    {
        private Random _rand = new Random();


        public int MakeRandomNumber()
        {
            return _rand.Next();
        }

        public string MakeRandomString()
        {
            return "RandomStr" + MakeRandomNumber().ToString();
        }


        public Foo MakeRandomFoo()
        {
            return new Foo
            {
                A = _rand.Next(int.MaxValue),
                B = Guid.NewGuid().ToString()
            };
        }

        public Bar MakeRandomBar()
        {
            return new Bar
            {
                A = _rand.Next(int.MaxValue),
                B = Guid.NewGuid().ToString()
            };
        }
    }
}
