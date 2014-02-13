using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Configuration;
using System.Web.UI;
using CTCT.Components;
using System.Runtime.Serialization;

namespace CTCT.Authentication
{
    /// <summary>
    /// Class used to authenticate from web application
    /// </summary>
    public sealed class Authentication
    {
        #region Constants

        private const string RequestAuthorizationUrl = "https://oauth2.constantcontact.com/oauth2/oauth/siteowner/authorize?response_type=code&client_id={0}&redirect_uri={1}&state={2}";
        private const string RequestAccessTokenUrl = "https://oauth2.constantcontact.com/oauth2/oauth/token?grant_type=authorization_code&client_id={0}&client_secret={1}&code={2}&redirect_uri={3}";

        #endregion

        #region Private Properties

        private readonly string _redirectUrl;
        private readonly string _apiKey;
        private readonly string _clientSecret;
        
        #endregion

        #region Constructor

        /// <summary>
        /// Initialize new instance of AuthenticationWebForm class
        /// </summary>
        /// <param name="apiKey">The api key</param>
        /// <param name="redirectUrl">The redirect url.</param>
        /// <param name="secret">The secret token.</param>
        public Authentication(string apiKey, string redirectUrl, string secret)
        {
            _apiKey = apiKey;
            _redirectUrl = redirectUrl;
            _clientSecret = secret;  
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Gets authorization code
        /// </summary>
        public string GetAuthorizationUrl(string state) {
            var url = String.Format(RequestAuthorizationUrl, 
                HttpUtility.UrlEncode(_apiKey),
                HttpUtility.UrlEncode(_redirectUrl),
                HttpUtility.UrlEncode(state));

            return url;
        }

        /// <summary>
        /// Gets access token by code
        /// </summary>
        /// <param name="code">authorization code</param>
        /// <returns>access token</returns>
        public string GetAccessTokenByCode(string code, string state)
        {
            var url = String.Format(RequestAccessTokenUrl,
                HttpUtility.UrlEncode(_apiKey),
                HttpUtility.UrlEncode(_clientSecret),
                HttpUtility.UrlEncode(code),
                HttpUtility.UrlEncode(_redirectUrl));

            var request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = "POST";

            string accessToken;
            using(var response = request.GetResponse() as HttpWebResponse)
            using (var reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                var responseText = reader.ReadToEnd();

                var info = Component.FromJSON<AuthenticationRequest>(responseText);

                accessToken = info.AccessToken;
            }

            return accessToken;
        }

        #endregion 
    }

    /// <summary>
    /// AuthenticationRequest class.
    /// </summary>
    [DataContract]
    [Serializable]
    class AuthenticationRequest
    {
        /// <summary>
        /// Activity id.
        /// </summary>
        [DataMember(Name = "access_token", EmitDefaultValue = false)]
        public string AccessToken { get; set; }

        /// <summary>
        /// Activity id.
        /// </summary>
        [DataMember(Name = "expires_in", EmitDefaultValue = false)]
        public string ExpiresIn { get; set; }

        /// <summary>
        /// Activity id.
        /// </summary>
        [DataMember(Name = "token_type", EmitDefaultValue = false)]
        public string TokenType { get; set; }
    }
}
