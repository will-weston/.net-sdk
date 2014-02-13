using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace CTCT.Components.Account
{
    /// <summary>
    /// Summary account-related information listed in the Structure section for the authorized Constant Contact account. 
    /// </summary>
    [Serializable]
    [DataContract]
    public class AccountInformation : Component
    {
        /// <summary>
        /// Standard 2 letter ISO 3166-1 code of the country associated with the account owner
        /// </summary>
        [DataMember(Name = "country_code")]
        public string CountryCode { get; set; }

        /// <summary>
        /// Email address associated with the account owner
        /// </summary>
        [DataMember(Name = "email")]
        public string Email { get; set; }

        /// <summary>
        /// The account owner's first name
        /// </summary>
        [DataMember(Name = "first_name")]
        public string FirstName { get; set; }

        /// <summary>
        /// The account owner's last name
        /// </summary>
        [DataMember(Name = "last_name")]
        public string LastName { get; set; }

        /// <summary>
        /// An array of organization street addresses; currently, only a single address is supported. This is not a 
        /// required attribute, but if you include organization_addresses in a put, it must include the country_code,
        /// city, and line1 fields at minimum
        /// </summary>
        [DataMember(Name = "organization_addresses")]
        public IList<OrganizationAddress> OrganizationAddresses { get; set; }

        /// <summary>
        /// Name of the organization associated with the account
        /// </summary>
        [DataMember(Name = "organization_name")]
        public string OrganizationName { get; set; }

        /// <summary>
        /// Phone number associated with the account owner
        /// </summary>
        [DataMember(Name = "phone")]
        public string Phone { get; set; }

        /// <summary>
        /// 2 letter code for USA state or Canadian province ONLY, 
        /// available only if country_code = US or CA associated with the account owner
        /// </summary>
        [DataMember(Name = "state_code")]
        public string StateCode { get; set; }

        /// <summary>
        /// The time zone associated with the account
        /// </summary>
        [DataMember(Name = "time_zone")]
        public string TimeZone { get; set; }

        /// <summary>
        /// The URL of the Web site associated with the account
        /// </summary>
        [DataMember(Name = "website")]
        public string Website { get; set; }
    }
}
