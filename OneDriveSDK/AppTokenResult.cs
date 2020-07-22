using System.Collections.Generic;

namespace OneDrive
{
    using Newtonsoft.Json;
    
    public class AppTokenResult
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
        [JsonProperty("expires_in")]
        public int AccessTokenExpirationDuration { get; set; }
        [JsonProperty("authentication_token")]
        public string AuthenticationToken { get; set; }
        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }
        [JsonProperty("scope")]
        public IEnumerable<string> Scopes { get; set; }
        [JsonProperty("token_type")]
        public string TokenType { get; set; }
    }
}