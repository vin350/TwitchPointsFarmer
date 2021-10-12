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
            Save.Load(MyUsers, MyChannels);
            MyChannels = new();
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
        
        public void UpdateUI()
        {
            /*
             * É pra isso funcionar, mas eu não testei kk
             */

            //sync channels
            var faltaCh = from x in MyChannels
                          where !ChannelsListBox.Items.Contains(x)
                          select x;
            var temAMaisCh = from x in ChannelsListBox.Items.Cast<string>()
                             where !MyChannels.Contains(x)
                             select x;
            faltaCh.ToList().ForEach(x => ChannelsListBox.Items.Add(x));
            temAMaisCh.ToList().ForEach(x => MyChannels.Add(x));

            //sync accounts
            var faltaAcc = from x in MyUsers
                           where !AccountsListBox.Items.Contains(x.Username)
                           select x;
            var temAMaisAcc = from x in AccountsListBox.Items.Cast<string>()
                              where !MyUsers.Any(y => y.Username == x)
                              select x;
            faltaAcc.ToList().ForEach(x => AccountsListBox.Items.Add(x));
            //nao da pra add sem saber o auth code, se tiver só no UI, é descartado
            //temAMaisAcc.ToList().ForEach(x => MyUsers.Add(x));

        }

        #endregion
    }
}
