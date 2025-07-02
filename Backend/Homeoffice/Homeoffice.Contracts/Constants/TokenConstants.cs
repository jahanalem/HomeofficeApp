namespace Homeoffice.Contracts.Constants
{
    /// <summary>
    /// Contains constant values related to Tokens.
    /// </summary>
    public static class TokenConstants
    {
        /// <summary>
        /// Represents the name of the Refresh Token.
        /// </summary>
        public const string RefreshToken = "RefreshToken";

        /// <summary>
        /// Represents the name of the Access Token.
        /// </summary>
        public const string AccessToken = "AccessToken";

        /// <summary>
        /// Represents the name of the JWT Bearer Token.
        /// </summary>
        public const string BearerToken = "Bearer";

        /// <summary>
        /// Represents the default expiration time of tokens in seconds.
        /// </summary>
        public const int DefaultExpirationSeconds = 6;

        /// <summary>
        /// Represents the local provider
        /// </summary>
        public const string LocalProvider = "HomeofficeRefresh";

        /// <summary>
        /// Represents the name of the DeviceId used to distinguish different devices.
        /// </summary>
        public const string DeviceId = "DeviceId";
    }
}
