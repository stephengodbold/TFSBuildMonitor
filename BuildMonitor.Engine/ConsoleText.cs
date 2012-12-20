using System.ComponentModel;
using System.Text;

namespace BuildMonitor.Engine
{
    public class ConsoleText : INotifyPropertyChanged, ILog
    {
        private readonly StringBuilder loggedData = new StringBuilder();
        public event PropertyChangedEventHandler PropertyChanged;

        public void WriteLine(string text)
        {
            LastMessage = text;
            loggedData.AppendLine(text);
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs("Text"));
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs("LastMessage"));
        }

        public string LastMessage { get; private set; }

        public string Text
        {
            get { return loggedData.ToString(); }
        }

        public void Information(string text)
        {
            WriteLine(text);
        }

        public void Warning(string text)
        {
            WriteLine(text);
        }

        public void Error(string text)
        {
            WriteLine(text);
        }
    }
}