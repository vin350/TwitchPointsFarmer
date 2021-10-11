using System.Collections.Generic;
using Newtonsoft.Json;

namespace TwitchPointsFarmer.Models
{
    public class SaveContainer
    {
        /// <summary>
        /// A list with all the current Accounts
        /// </summary>
        [JsonProperty("users")]
        public List<User> MyUsers { get; set; }
        /// <summary>
        /// A list with all current subscribed channels
        /// </summary>
        [JsonProperty("channels")]
        public List<string> MyChannels { get; set; }
    }
}
