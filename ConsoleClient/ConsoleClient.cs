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

        public class ProgressBar {

            public int BarWidth = 40;
            public string ClosingNotation = "]-";
            public bool DisplayOnly;
            public string EmptyNotation = "-";
            public string FilledNotation = "#";
            public int LoadingIndex = 0;
            public List<string> LoadingMessages = new();
            public bool MessageDisplay;
            public string OpeningNotation = "-[";
            public float ProgressValue { get; set; }

            public ProgressBar() {
                MessageDisplay = false;
                ProgressValue = 0;
            }

            public ProgressBar(List<string> loadingMessages) {
                LoadingMessages = loadingMessages;
                MessageDisplay = true;
            }

            public void Display() {
                if (DisplayOnly) Console.Clear();
                else Console.SetCursorPosition(0, Console.CursorTop + getDisplacement());
                Console.Write(CreateBar());
            }

            private int getDisplacement() {
                if (!MessageDisplay) return 0;
                if (Console.CursorTop < 2) return -1;
                else return -2;
            }

            public void Next() {
                if (MessageDisplay) {
                    LoadingIndex++;
                    ProgressValue = 100f * (LoadingIndex + 1f) / LoadingMessages.Count;
                }
                else ProgressValue++;
                Display();
            }
            public void Next(int newValue) {
                ProgressValue = newValue;
                Display();
            }

            private string CreateBar() {
                string barRender = OpeningNotation;
                int discreteSegments = (int)(ProgressValue * (BarWidth / 100f));
                barRender += FilledNotation.RepeatString(discreteSegments);
                barRender += EmptyNotation.RepeatString(BarWidth - discreteSegments);
                barRender += $"{ClosingNotation} {Math.Round(ProgressValue, 2)}%";
                barRender = barRender.FillWithWhitespace();
                if (MessageDisplay) barRender += $"\nLoading {LoadingMessages[LoadingIndex]}...".FillWithWhitespace();
                return barRender;
            }
        }

        public class Menu<T> {

            public int BodyWidth = 40;
            public bool DisplayTitle = true;
            public bool ScrollLock = false;
            public bool DisplayOnly = true;
            public ConsoleColor SelectedColour = Console.ForegroundColor;
            public string UnselectedNotation = "";
            public string SelectedNotation = " >";
            public string NotationBuffer = " ";

            public string Title;
            public T[] ReturnArticles = null;
            public string[] UserArticles;

            private int CurrentIndex = 0;
            private Dictionary<string, T> Articles;
            private int LastIndex = 0;

            public Menu(string[] userArticles, T[] returnArticles = null, string title = null) {
                if (title == null) DisplayTitle = false;
                else Title = title;
                ReturnArticles = returnArticles?? null;
                UserArticles = userArticles;
                Flush();
            }

            enum Direction {
                Up, Down
            }

            public T Run() {
                CurrentIndex = 0;
                if (DisplayOnly) Console.Clear();
                PrintMenu();
                while (true) {
                    if (TakeUserInput() == "RETURN") return ReturnArticles[CurrentIndex];
                    int topIndex = Console.CursorTop - UserArticles.Length + 1;
                    Console.SetCursorPosition(0, topIndex + LastIndex);
                    PrintUnselectedArticle(UserArticles[LastIndex].FillWithWhitespace());
                    Console.SetCursorPosition(0, topIndex + CurrentIndex);
                    PrintSelectedArticle(UserArticles[CurrentIndex].FillWithWhitespace());
                }
            }

            public T Run(out int index) {
                CurrentIndex = 0;
                if (DisplayOnly) Console.Clear();
                PrintMenu();
                while (true) {
                    if (TakeUserInput() == "RETURN") { index = CurrentIndex; return default; }
                    int topIndex = Console.CursorTop - UserArticles.Length + 1;
                    Console.SetCursorPosition(0, topIndex + LastIndex);
                    PrintUnselectedArticle(UserArticles[LastIndex].FillWithWhitespace());
                    Console.SetCursorPosition(0, topIndex + CurrentIndex);
                    PrintSelectedArticle(UserArticles[CurrentIndex].FillWithWhitespace());
                }
            }

            private string TakeUserInput() {
                var arg = Console.ReadKey(true).Key;
                switch (arg) {
                    case ConsoleKey.W:
                        goto case ConsoleKey.UpArrow;
                    case ConsoleKey.UpArrow:
                        AdjustIndex(Direction.Up);
                        break;
                    case ConsoleKey.S:
                        goto case ConsoleKey.DownArrow;
                    case ConsoleKey.DownArrow:
                        AdjustIndex(Direction.Down);
                        break;
                    case ConsoleKey.Enter:
                        return "RETURN";
                    default: return "UNHANDLED";
                }
                return "HANDLED";
            }

            private void PrintMenu() {
                Console.WriteLine(Title.GenerateTitle(BodyWidth, TitleFanciness.Some, AlignmentType.Centre));
                for (int i = 0; i < UserArticles.Length; i++) {
                    if (i == CurrentIndex) PrintSelectedArticle(UserArticles[i]);
                    else PrintUnselectedArticle(UserArticles[i]);
                }
            }

            private void PrintUnselectedArticle(string article) {
                Console.WriteLine($"{UnselectedNotation}{NotationBuffer}{article}");
            }

            private void PrintSelectedArticle(string article) {
                ConsoleColor arg = Console.ForegroundColor;
                Console.ForegroundColor = SelectedColour;
                Console.WriteLine($"{SelectedNotation}{NotationBuffer}{article}");
            }

            // Flushes user inputs to the Articles dict
            private void Flush() {
                if (ReturnArticles != null) {
                    var arg = new Dictionary<string, T>();
                    for (int i = 0; i < UserArticles.Length; i++) {
                        arg.Add(UserArticles[i], ReturnArticles[i]);
                    }
                    Articles = arg;
                }
            }

            private void AdjustIndex(Direction dir) {
                int Max = UserArticles.Length - 1;
                if (dir == Direction.Up) {
                    if (CurrentIndex == 0 && !ScrollLock) { CurrentIndex = Max; LastIndex = 0; }
                    else { LastIndex = CurrentIndex; CurrentIndex--;}
                }
                if (dir == Direction.Down) {
                    if (CurrentIndex == Max && !ScrollLock) { CurrentIndex = 0; LastIndex = Max; }
                    else { LastIndex = CurrentIndex; CurrentIndex++; }
                }
            }

        }

    }
}
