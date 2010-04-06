
namespace Reflex.Tests.Stubs
{
    public class Bar
    {
        public int A { get; set; }
        public string B { get; set; }

        public string SayHello(string name) { return "Hello " + name; }
        public string SayGoodbye(string name) { return "Goodbye " + name; }

        private string sayHi(string name) { return "Hi " + name; }
        private string sayBye(string name) { return "Bye " + name; }

        public static string StaticHello(string name) { return "Bzz... " + name; }
    }
}
