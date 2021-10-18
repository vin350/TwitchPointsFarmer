using System;
using TwitchLib.Client;
using TwitchLib.Client.Enums;
using TwitchLib.Client.Events;
using TwitchLib.Client.Extensions;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Clients;
using TwitchLib.Communication.Events;
using TwitchLib.Communication.Models;
using TwitchPointsFarmer.Components;
using TwitchPointsFarmer.Models;
using TwitchPointsFarmer.Utils;
namespace TwitchPointsFarmer
{
    public class Bot
    {
        private readonly TwitchClient client;
        private string ActChannel;
        private static string LastWhisper;
        public MainWindow Main { get; set; }
        public string ActUsername { get; set; }

        public Bot(string userName, string token, string channel, MainWindow main)
        {
            Main = main;
            ConnectionCredentials credentials = new(userName, token);
            ClientOptions clientOptions = new()
            {
                MessagesAllowedInPeriod = 750,
                ThrottlingPeriod = TimeSpan.FromSeconds(30)
            };
            WebSocketClient customClient = new(clientOptions);
            client = new TwitchClient(customClient);
            client.Initialize(credentials, channel);

            //client.OnLog += Client_OnLog;

            client.OnJoinedChannel += Client_OnJoinedChannel;
            client.OnWhisperReceived += Client_OnWhisperReceived;
            client.OnMessageReceived += Client_OnMessageReceived;
            client.OnConnected += Client_OnConnected;
            client.OnIncorrectLogin += Client_OnIncorrectLogin;
            client.OnConnectionError += Client_OnConnectionError;
            client.OnFailureToReceiveJoinConfirmation += Client_OnFailureToReceiveJoinConfirmation;
            client.OnDisconnected += Client_OnDisconnected;

            client.Connect();
        }

        public void Disconnect()
        {
            client.Disconnect();
        }

        private void Client_OnDisconnected(object sender, OnDisconnectedEventArgs e)
        {
            Main.Log($"{ActUsername} disconnected from {ActChannel}");
        }

        private void Client_OnMessageReceived(object sender, OnMessageReceivedArgs e)
        {
            if (e.ChatMessage.Message.Contains($"{e.ChatMessage.BotUsername}"))
            {
                Main.Log(e.ChatMessage.Username + ": " + e.ChatMessage.Message);
            }
        }

        private void Client_OnLog(object sender, OnLogArgs e)
        {
            Main.Log($"{e.BotUsername} - {e.Data}");
        }
        private void Client_OnFailureToReceiveJoinConfirmation(object sender, OnFailureToReceiveJoinConfirmationArgs e)
        {
            Main.Error($"Erro ao iniciar a conta em: {e.Exception.Channel}");
        }

        private void Client_OnConnectionError(object sender, OnConnectionErrorArgs e)
        {
            Main.Error($"Erro ao iniciar a conta: {e.BotUsername}");
        }

        private void Client_OnIncorrectLogin(object sender, OnIncorrectLoginArgs e)
        {
            ActChannel = "";
            Main.Error($"Erro ao iniciar a conta: {e.Exception.Username}");
        }


        private void Client_OnConnected(object sender, OnConnectedArgs e)
        {
            //Logger.Log(e.BotUsername + $" connected to {e.AutoJoinChannel}");
        }

        private void Client_OnJoinedChannel(object sender, OnJoinedChannelArgs e)
        {
            Main.Log(e.BotUsername + $" connected to {e.Channel}");
            ActChannel = e.Channel;
            ActUsername = e.BotUsername;
            Main.BotManager.Add(this);
        }

        private void Client_OnWhisperReceived(object sender, OnWhisperReceivedArgs e)
        {
            if (LastWhisper != e.WhisperMessage.Message)
            {
                Main.Log(e.WhisperMessage.Username + ": " + e.WhisperMessage.Message);
                LastWhisper = e.WhisperMessage.Message;
            }
        }


        public void SendMessage(string message)
        {
            client.SendMessage(ActChannel, message);
            Main.Log("Account: " + ActUsername + ", Channel: " + ActChannel + ", Send: " + message);
        }

        public void SendMessageTo(string Channel, string message)
        {
            client.SendMessage(Channel, message);
            Main.Log("Account: " + ActUsername + ", Channel: " + Channel + ", Send: " + message);
        }

        public string GetActChannel()
        {
            return ActChannel;
        }
    }
}