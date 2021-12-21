using System;
using System.Collections.Generic;

namespace SigmaConsole {
    public class ConsoleClient {

        public ConsoleClient() {

        }

        public T TakeInput<T>(string question) {
            bool inputValidated;
            T result = default;
            do {
                Console.Clear();
                Console.Write($"{question}\n> ");
                string arg = Console.ReadLine();
                try {
                    result = (T)Convert.ChangeType(arg, typeof(T));
                    inputValidated = true;
                }
                catch (FormatException) { inputValidated = false; }
            } while (!inputValidated);
            return result;
        }

        public T TakeInput<T>(string question, Func<T, bool> condition) {
            bool inputValidated;
            T result = default;
            do {
                Console.Clear();
                Console.Write($"{question}\n> ");
                string arg = Console.ReadLine();
                try {
                    result = (T)Convert.ChangeType(arg, typeof(T));
                    inputValidated = true;
                } catch (FormatException) { inputValidated = false; }
            } while (!inputValidated && !condition(result));
            return result;
        }

    }
}
