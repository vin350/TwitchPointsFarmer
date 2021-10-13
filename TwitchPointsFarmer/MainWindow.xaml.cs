using System.Collections.Generic;
using System.Linq;
using System.Windows;
using TwitchPointsFarmer.Components;
using TwitchPointsFarmer.Models;
using TwitchPointsFarmer.Utils;

namespace TwitchPointsFarmer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// A list with all the current Accounts
        /// </summary>
        public List<User> MyUsers { get; set; }
        /// <summary>
        /// A list with all current subscribed channels
        /// </summary>
        public List<string> MyChannels { get; set; }
        /// <summary>
        /// The class that manages the console
        /// </summary>
        public Logger Logger { get; set; }
        /// <summary>
        /// The manager for saving/loading information from the JSON file
        /// </summary>
        public SaveClass Save { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/>
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            Closing += WindowCloseEvent;
            Save = new();
            MyChannels = new();
            MyUsers = new();
            Save.Load(MyUsers, MyChannels);
            Logger = new Logger(this);
        }



        #region Events

        private void WindowCloseEvent(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();
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
                MyChannels.Add(inputRead);
                UpdateUI();
            }
        }

        private void RemChannelButton_Click(object sender, RoutedEventArgs e)
        {
            //if it hasnt any entries, ignore click
            if (MyChannels == null || MyChannels.Count == 0 || ChannelsListBox.SelectedItem == null || ChannelsListBox.SelectedIndex == -1)
            {
                return;
            }

            //remove from the backend
            MyChannels.Remove(ChannelsListBox.SelectedItem as string);

            UpdateUI();
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
            User user = new() { Username = username, AuthCode = authcode };

            //checks if exists again, just to be sure, but this time checks if both username and authcode
            if (MyUsers.Contains(user))
            {
                MessageBox.Show("This user is already on the list!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            MyUsers.Add(user);
            UpdateUI();
        }

        private void RemAccountButton_Click(object sender, RoutedEventArgs e)
        {
            /* OBS.: Aqui eu usei LINQ pra pegar o username
             * pq cada user tem o nome e o código e esse não
             * aparece no UI. Então tem q fazer essas macumbas
             * pra conseguir deletar todos os q batem com o nome 
             */
             
            //if it hasnt any entries, ignore click
            if(MyUsers==null || MyUsers.Count == 0 || AccountsListBox.SelectedItem==null || AccountsListBox.SelectedIndex==-1)
            {
                return;
            }

            //get username
            int index = AccountsListBox.SelectedIndex;
            var username = AccountsListBox.Items[index] as string;

            //LINQ
            MyUsers.RemoveAll(x => x.Username == username);
            UpdateUI();
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {

        }

        #endregion

        #region Methods
        
        /// <summary>
        /// Updates the UI to match the current lists
        /// </summary>
        public void UpdateUI()
        {
            //sync channels
            ChannelsListBox.Items.Clear();
            MyChannels.ForEach(c => ChannelsListBox.Items.Add(c));

            //sync users
            AccountsListBox.Items.Clear();
            MyUsers.ForEach(u => AccountsListBox.Items.Add(u.Username));

            //save to file, just to be shure everything is nicely saved :)
            Save.Save(MyUsers, MyChannels);
        }

        #endregion
    }
}
