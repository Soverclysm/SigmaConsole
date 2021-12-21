using System;
using SigmaConsole;

namespace TestingSuite {
    class Program {
        static void Main() {

            string[] articles = new string[] { "poo", "one", "pee" };
            int[] eArticles = new int[] { 6, 8, 9 };
            Menu<int> menu = new(articles, eArticles, "your mother");
            int num = menu.GetGenericOutput();
            Console.Write(num);

        }

    }
}
