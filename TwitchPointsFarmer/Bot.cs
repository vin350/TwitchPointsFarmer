using System;
using TwitchLib.Client;
using TwitchLib.Client.Enums;
using TwitchLib.Client.Events;
using TwitchLib.Client.Extensions;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Clients;
using TwitchLib.Communication.Events;
using TwitchLib.Communication.Models;

namespace TwitchPointsFarmer
{
    class Bot
    {
        private readonly TwitchClient client;
        private string actChannel;
        private static string LastWhisper;
        public Bot(string userName, string token, string channel)
        {
            ConnectionCredentials credentials = new(userName, token);
            var clientOptions = new ClientOptions
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
            client.OnConnected += Client_OnConnected;
            client.OnIncorrectLogin += Client_OnIncorrectLogin;

            client.Connect();
        }

        private void Client_OnLog(object sender, OnLogArgs e)
        {
            //LogInfo($"{e.DateTime.ToString()}: {e.BotUsername} - {e.Data}");
        }

        private void Client_OnIncorrectLogin(object sender, OnIncorrectLoginArgs e)
        {
            actChannel = "";
            //Program.BotManager.Remove(this);
            //LogError($"Erro ao iniciar a conta: {e.Exception.Username}");
        }


        private void Client_OnConnected(object sender, OnConnectedArgs e)
        {
            //LogSuccess(e.BotUsername + $" connected to {e.AutoJoinChannel}");
        }

        private void Client_OnJoinedChannel(object sender, OnJoinedChannelArgs e)
        {
            actChannel = e.Channel;
        }

        private void Client_OnWhisperReceived(object sender, OnWhisperReceivedArgs e)
        {
            if (e.WhisperMessage.Username.ToLower() == "StreamElements".ToLower())
            {
                if (LastWhisper != e.WhisperMessage.Message)
                {
                    //LogInfo(e.WhisperMessage.Message);
                    LastWhisper = e.WhisperMessage.Message;
                }
            }
        }


        public void SendMessage(string message)
        {
            client.SendMessage(actChannel, message);
            //LogInfo("Channel: " + actChannel + ", Send Message: " + message);
        }

        public void SendMessageTo(string Channel, string message)
        {
            client.SendMessage(Channel, message);
            //LogInfo("Channel: " + Channel + ", Send Message: " + message);
        }

        public string GetActChannel()
        {
            return actChannel;
        }
    }
}