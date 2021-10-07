using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TwitchPointsFarmer.Components
{
    /// <summary>
    /// Lógica interna para InputWindow.xaml
    /// </summary>
    public partial class InputWindow : Window
    {
        public bool IsCancelled { get; set; }
        public static string CancelConst { get; } = @"%%CANCELLED%%";
        public InputWindow(string content = "Default Value",
                            string windowTitle = "TwitchPointFarmer",
                            string defaultValue = "",
                            string fontFamily = "Arial",
                            int fontSize = 20)
        {
            InitializeComponent();

            IsCancelled = false;

            TitleTextBox.Text = content;
            InputTextBox.Text = defaultValue;
            this.Title = windowTitle;

            TitleTextBox.FontFamily = new FontFamily(fontFamily);
            InputTextBox.FontFamily = new FontFamily(fontFamily);
            TitleTextBox.FontSize = fontSize;
            InputTextBox.FontSize = fontSize;

            //event handling
            OkButton.Click += OkClicked;
            CancelButtton.Click += CancelClicked;
            this.KeyDown += KeyPressed;

            //foca na text box pra n precisar clicar
            InputTextBox.Focus();
        }

        private void KeyPressed(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                OkClicked(this,new RoutedEventArgs());
            }else if(e.Key == Key.Escape)
            {
                CancelClicked(this, new RoutedEventArgs());
            }
        }

        public string GetInput()
        {
            this.ShowDialog();
            if (IsCancelled)
            {
                return CancelConst;
            }
            return InputTextBox.Text.Trim();
        }
        private void OkClicked(object sender, RoutedEventArgs e)
        {
            //check if result is empty
            if (string.IsNullOrWhiteSpace(InputTextBox.Text))
            {
                MessageBox.Show("O campo está vazio!", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                this.Close();
            }
        }
        private void CancelClicked(object sender, RoutedEventArgs e)
        {
            this.IsCancelled = true;
            this.Close();
        }
    }
}
