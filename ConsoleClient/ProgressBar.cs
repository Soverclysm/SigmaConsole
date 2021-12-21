using System;
using System.Collections.Generic;

namespace SigmaConsole {
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
}
