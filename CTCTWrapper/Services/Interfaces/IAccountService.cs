using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CTCT.Components.Contacts;
using CTCT.Components.Account;

namespace CTCT.Services
{
    /// <summary>
    /// Interface for account service
    /// </summary>
    public interface  IAccountService
    {
        /// <summary>
        /// Retrieve a list of all the account owner's email addresses
        /// </summary>
        /// <param name="accessToken">Constant Contact OAuth2 access token.</param>
        /// <param name="apiKey">The API key for the application</param>
        /// <returns>list of all verified account owner's email addresses</returns>
        IList<VerifiedEmailAddress> GetVerifiedEmailAddress(string accessToken, string apiKey);

        /// <summary>
        /// Retrieve the summary account-related information listed in the Structure section for the authorized Constant Contact account. 
        /// </summary>
        /// <param name="accessToken">Constant Contact OAuth2 access token.</param>
        /// <param name="apiKey">The API key for the application</param>
        /// <returns>Summary account-related information listed in the Structure section for the authorized Constant Contact account.</returns>
        AccountInformation GetInformation(string accessToken, string apiKey);
    }
}
