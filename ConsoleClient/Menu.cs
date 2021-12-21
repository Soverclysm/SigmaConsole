using System;

namespace SigmaConsole {
    public class Menu<T> {

        #region Properties

        public bool DisplayTitle = true;
        public bool DisplayOnly = true;

        public string UnselectedNotation = " ";
        public string BufferNotation = " ";
        public string SelectedNotation = " >";
        public string ClosingUnselectedNotation = "";
        public string ClosingBufferNotation = "";
        public string ClosingSelectedNotation = "";

        public ConsoleColor PrimaryColour = Console.ForegroundColor;
        public ConsoleColor SelectedColour = Console.ForegroundColor;
        public TitleFanciness TitleFanciness = TitleFanciness.Some;
        public AlignmentType TitleAlignment = AlignmentType.Centre;

        public string Title;
        public T[] ReturnArticles;
        public string[] UserArticles;

        private int CurrentIndex;
        private (int, int) IndexBounds;
        private readonly bool IndexReturn = false;

        #endregion

        #region Logic

        public Menu(string[] userArticles, T[] returnArticles = null, string title = null) {
            if (title == null) DisplayTitle = false;
            else Title = title.GenerateTitle(Console.WindowWidth/2, TitleFanciness.Some, AlignmentType.Centre);
            UserArticles = userArticles;
            if (returnArticles == null) IndexReturn = true;
            else ReturnArticles = returnArticles;
        }

        public T GetGenericOutput() {
            if (IndexReturn) throw new NullReferenceException("You have not specified any ReturnArticles T[], so you cannot receive a generic output. Include the ReturnArticles T[] in the constructor of SigmaConsole.Menu()");
            return ReturnArticles[GetOutput()];
        }
        public int GetOutput() {
            int output = Run();
            Console.SetCursorPosition(0, IndexBounds.Item2 + 1);
            return output;
        }

        private int Run() {
            CurrentIndex = 0;
            SetupMenu();
            ConsoleKey ki;
            int handleCode = 1;
            while (handleCode != 0) {
                ki = Console.ReadKey(true).Key;
                Console.SetCursorPosition(0, IndexBounds.Item1 + CurrentIndex);
                PrintUnselectedArticle(UserArticles[CurrentIndex], false);
                handleCode = HandleInput(ki);
                if (handleCode == 0) return CurrentIndex;
                Console.SetCursorPosition(0, IndexBounds.Item1 + CurrentIndex);
                PrintSelectedArticle(UserArticles[CurrentIndex], false);
            }
            return default;
        }

        private void SetupMenu() {
            if (DisplayOnly) Console.Clear();
            if (DisplayTitle) Console.WriteLine(Title);
            for (int i = 0; i < UserArticles.Length; i++) {
                if (CurrentIndex == i) PrintSelectedArticle(UserArticles[i], true);
                else PrintUnselectedArticle(UserArticles[i], true);
            }
            var msi = Console.CursorTop - UserArticles.Length;
            var ism = Console.CursorTop - 1;
            IndexBounds = (msi, ism);
        }

        private int HandleInput(ConsoleKey key) {
            switch (key) {
                case ConsoleKey.UpArrow or ConsoleKey.W:
                    CurrentIndex--; break;
                case ConsoleKey.DownArrow or ConsoleKey.S:
                    CurrentIndex++; break;
                case ConsoleKey.Enter:
                    return 0;
            }
            if (CurrentIndex < 0) CurrentIndex = UserArticles.Length - 1;
            else if (CurrentIndex > UserArticles.Length - 1) CurrentIndex = 0;
            return 1;
        }

        #endregion

        #region Output

        private void PrintUnselectedArticle(string article, bool newLine) {
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write($"{UnselectedNotation}{BufferNotation}{article}{ClosingBufferNotation}{ClosingUnselectedNotation}{(newLine ? "\n" : "")}".FillWithWhitespace());
        }

        private void PrintSelectedArticle(string article, bool newLine) {
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write($"{SelectedNotation}{BufferNotation}{article}{ClosingBufferNotation}{ClosingSelectedNotation}{(newLine? "\n":"")}".FillWithWhitespace());
        }

        #endregion

    }
}
