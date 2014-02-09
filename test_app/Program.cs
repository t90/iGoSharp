using System.IO;
using iGo;
using Interfaces;

namespace test_app
{
    class Program
    {
        static void Main(string[] args)
        {
            var i = System.Console.Out.xInterfacesIWriteLine();
            Print(i);
        }

        public static void Print(IWriteLine wl)
        {
            wl.WriteLine("some text");
        }

    }




}
