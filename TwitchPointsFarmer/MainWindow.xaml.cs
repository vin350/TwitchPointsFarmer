using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public List<User> MyUsers
        {
            get{ return _MyUsers; }
            set { _MyUsers = value; }
        }
        private List<User> _MyUsers;
        /// <summary>
        /// A list with all current subscribed channels
        /// </summary>
        public List<string> MyChannels
        {
            get { return _MyChannels; }
            set { _MyChannels = value; }
        }
        private List<string> _MyChannels;
        /// <summary>
        /// The class that manages the console
        /// </summary>
        public Logger Logger { get; set; }
        /// <summary>
        /// The manager for saving/loading information from the JSON file
        /// </summary>
        public SaveClass Save { get; set; }
        public List<Bot> BotManager
        {
            get { return _BotManager; }
            set { _BotManager = value; }
        }
        private List<Bot> _BotManager;
        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/>
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            BotManager = new();
            Closing += WindowCloseEvent;
            Save = new();
            MyChannels = new();
            MyUsers = new();
            Logger = new Logger(this);
            Save.Load(out _MyUsers, out _MyChannels);
            UpdateUI();
            Logger.Log("System Loaded");
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
            if (inputRead == InputWindow.CancelConst) return;
            bool exists = MyChannels.Any(u => u == inputRead);
            if (exists)
            {
                MessageBox.Show("This channel is already on the list!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            MyChannels.Add(inputRead);
            UpdateUI();
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

        private async void StartButton_Click(object sender, RoutedEventArgs e)
        {
            await Task.Run(() =>
            {
                Logger.Log("Starting...");
                foreach (var MyUser in MyUsers)
                {
                    foreach (var MyChannel in MyChannels)
                    {
                        Logger.Log("trying to connect " + MyUser.Username + " into " + MyChannel);
                        Bot bot = new Bot(MyUser.Username, MyUser.AuthCode, MyChannel, this);
                    }
                }
            });
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            List<Bot> temp = new();
            BotManager.ForEach(x =>
            {
                x.Disconnect();
                temp.Add(x);
            });
            temp.ForEach(x => BotManager.Remove(x));
            GC.Collect();
            GC.WaitForPendingFinalizers();
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

        public void SendMessageToAll(string message)
        {
            foreach (Bot x in BotManager)
            {
                x.SendMessage(message);
            }
        }

        public void SendMessageTo(string Channel, string message)
        {
            foreach (Bot x in BotManager)
            {
                string Ch = x.GetActChannel();
                if (Ch.ToLower() == Channel.ToLower())
                {
                    x.SendMessageTo(Ch, message);
                }
                
            }
        }

        public void Log(string message) => Logger.Log(message);
        public void Warn(string message) => Logger.Warn(message);
        public void Error(string message) => Logger.Error(message);

        #endregion
    }
}
