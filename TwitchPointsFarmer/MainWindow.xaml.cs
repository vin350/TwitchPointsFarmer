using System.Collections.Generic;
using System.Windows;
using TwitchPointsFarmer.Components;
using TwitchPointsFarmer.Models;
using System.Linq;

namespace TwitchPointsFarmer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// A list with all
        /// </summary>
        public List<User> MyUsers { get; set; }
        public List<string> MyChannels { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            //depois mudar isso pra pegar direto dos arquivos JSON, assim a lista começa vazia!!!
            MyUsers = new();
            MyChannels = new();
        }

        private void AddChannelButton_Click(object sender, RoutedEventArgs e)
        {
            var p = new InputWindow(content: "Channel name:",
                                    windowTitle:"Add new");
            string inputRead = p.GetInput();
            if (inputRead == InputWindow.CancelConst)
            {
                return;
            }
            if (!string.IsNullOrWhiteSpace(inputRead))
            {
                ChannelsListBox.Items.Add(inputRead);
                MyChannels.Add(inputRead);
            }
        }

        private void RemChannelButton_Click(object sender, RoutedEventArgs e)
        {
            //remove from the backend
            MyChannels.Remove(ChannelsListBox.SelectedItem as string);

            //remove from the UI
            ChannelsListBox.Items.Remove(ChannelsListBox.SelectedItem);
        }

        private void AddAccountButton_Click(object sender, RoutedEventArgs e)
        {
            InputWindow usernameInput = new(content: "Username: ", windowTitle: "Add new");
            InputWindow authcodeInput = new(content: "AuthCode: ", windowTitle: "Add new");
            string username;
            string authcode;

            //get username
            username = usernameInput.GetInput();
            if (username == InputWindow.CancelConst) return;
            //check if user already exists using LINQ
            bool exists = MyUsers.Any(u => u.Username == username);
            if (exists)
            {
                MessageBox.Show("This user is already on the list!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            authcode = authcodeInput.GetInput();
            if (authcode == InputWindow.CancelConst) return;
            User user = new User() { Username = username, AuthCode = authcode };

            //checks if exists again, just to be sure, but this time checks if both username and authcode
            if (MyUsers.Contains(user))
            {
                MessageBox.Show("This user is already on the list!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }


            //Add on the UI
            AccountsListBox.Items.Add(user.Username);
        }

        private void RemAccountButton_Click(object sender, RoutedEventArgs e)
        {
            /* OBS.: Aqui eu usei LINQ pra pegar o username
             * pq cada user tem o nome e o código e esse não
             * aparece no UI. Então tem q fazer essas macumbas
             * pra conseguir deletar todos os q batem com o nome 
             */
             
            //get username
            int index = AccountsListBox.SelectedIndex;
            var username = AccountsListBox.Items[index] as string;

            //LINQ
            MyUsers.RemoveAll(x => x.Username == username);

            //remove from the UI
            AccountsListBox.Items.Remove(AccountsListBox.SelectedItem);
        }
    }
}
