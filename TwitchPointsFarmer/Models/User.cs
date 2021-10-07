namespace TwitchPointsFarmer.Models
{
    public class User
    {
        /// <summary>
        /// The public username of this account
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// The private authentication code for this account
        /// </summary>
        public string AuthCode { get; set; }
    }
}
