using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace TwitchPointsFarmer.Utils
{
    /// <summary>
    /// A class to manage logging information, warning and error displays on the console output
    /// </summary>
    public class Logger
    {
        private MainWindow main;
        /// <summary>
        /// Creates a new instance of the logger
        /// </summary>
        /// <param name="main"></param>
        public Logger(MainWindow main)
        {
            this.main = main;
        }
        /// <summary>
        /// Outputs a normal message on the console text box
        /// </summary>
        /// <param name="message">The message to be displayed</param>
        public void Log(string message)
        {
            DateTime date = DateTime.Now;
            message = $"[{date.Day}/{date.Month} {date.Hour}:{date.Minute}:{date.Second} - LOG] {message}";
            main.ConsoleBox.Text += "\n" + message;
        }
        /// <summary>
        /// Outputs a warning on the console
        /// </summary>
        /// <param name="message">The message of the warning</param>
        public void Warn(string message)
        {
            DateTime date = DateTime.Now;
            message = $"[{date.Day}/{date.Month} {date.Hour}:{date.Minute}:{date.Second} - WARN] {message}";
            main.ConsoleBox.Text += "\n" + message;
        }
        /// <summary>
        /// Outputs an error message on the console
        /// </summary>
        /// <param name="message">The message of the error</param>
        public void Error(string message)
        {
            DateTime date = DateTime.Now;
            message = $"[{date.Day}/{date.Month} {date.Hour}:{date.Minute}:{date.Second} - ERROR] {message}";
            main.ConsoleBox.Text += "\n" + message;
        }
    }
}

