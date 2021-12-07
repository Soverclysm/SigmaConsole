using System;
using SigmaConsole;

namespace TestingSuite {
    class Program {
        static void Main() {
            ConsoleClient cc = new ConsoleClient();
            var pb = new ConsoleClient.ProgressBar();
            for (int i = 0; i < 101; i++) {
                pb.ProgressValue = i;
                pb.Run();
                System.Threading.Thread.Sleep(200);
            }
        }

    }
}
