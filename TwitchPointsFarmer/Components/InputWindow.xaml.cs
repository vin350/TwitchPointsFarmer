using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace TwitchPointsFarmer.Components
{
    /// <summary>
    /// Lógica interna para InputWindow.xaml
    /// </summary>
    public partial class InputWindow : Window
    {
        public bool IsCancelled { get; set; }
        public static string CancelConst { get; } = @"%%CANCELLED%%";
        /// <summary>
        /// Creates a new instance of a <see cref="InputWindow"/>
        /// </summary>
        /// <param name="content">The text that will be displayed for the user in the window</param>
        /// <param name="windowTitle">The window title, somthing generic</param>
        /// <param name="defaultValue">The default value for the input box, if not written, will be <code>""</code></param>
        /// <param name="fontFamily">The Font family string, defaults to Arial</param>
        /// <param name="fontSize">The font size, the default is 20</param>
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
