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
    class Bot
    {
        private readonly TwitchClient client;
        private string actChannel;
        private static string LastWhisper;
        public Logger Logger { get; set; }

        public Bot(string userName, string token, string channel, Logger logger)
        {
            this.Logger = logger;
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
            client.OnConnectionError += Client_OnConnectionError;
            client.OnFailureToReceiveJoinConfirmation += Client_OnFailureToReceiveJoinConfirmation;

            client.Connect();
        }


        private void Client_OnLog(object sender, OnLogArgs e)
        {
            Logger.Log($"{e.BotUsername} - {e.Data}");
        }
        private void Client_OnFailureToReceiveJoinConfirmation(object sender, OnFailureToReceiveJoinConfirmationArgs e)
        {
            Logger.Error($"Erro ao iniciar a conta em: {e.Exception.Channel}");
        }

        private void Client_OnConnectionError(object sender, OnConnectionErrorArgs e)
        {
            Logger.Error($"Erro ao iniciar a conta: {e.BotUsername}");
        }

        private void Client_OnIncorrectLogin(object sender, OnIncorrectLoginArgs e)
        {
            actChannel = "";
            //Program.BotManager.Remove(this);
            Logger.Error($"Erro ao iniciar a conta: {e.Exception.Username}");
        }


        private void Client_OnConnected(object sender, OnConnectedArgs e)
        {
            //Logger.Log(e.BotUsername + $" connected to {e.AutoJoinChannel}");
        }

        private void Client_OnJoinedChannel(object sender, OnJoinedChannelArgs e)
        {
            Logger.Log(e.BotUsername + $" connected to {e.Channel}");
            actChannel = e.Channel;
        }

        private void Client_OnWhisperReceived(object sender, OnWhisperReceivedArgs e)
        {
            if (e.WhisperMessage.Username.ToLower() == "StreamElements".ToLower())
            {
                if (LastWhisper != e.WhisperMessage.Message)
                {
                    Logger.Log(e.WhisperMessage.Message);
                    LastWhisper = e.WhisperMessage.Message;
                }
            }
        }


        public void SendMessage(string message)
        {
            client.SendMessage(actChannel, message);
            Logger.Log("Channel: " + actChannel + ", Send Message: " + message);
        }

        public void SendMessageTo(string Channel, string message)
        {
            client.SendMessage(Channel, message);
            Logger.Log("Channel: " + Channel + ", Send Message: " + message);
        }

        public string GetActChannel()
        {
            return actChannel;
        }
    }
}