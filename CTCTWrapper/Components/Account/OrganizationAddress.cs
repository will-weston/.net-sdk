using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace CTCT.Components.Account
{
    /// <summary>
    /// Organization address information.
    /// </summary>
    [DataContract]
    [Serializable]
    public class OrganizationAddress : Component
    {
        /// <summary>
        /// The city the organization is located in
        /// </summary>
        [DataMember(Name = "city")]
        public string City { get; set; }

        /// <summary>
        /// Standard 2 letter ISO 3166-1 code for the organization_addresses
        /// </summary>
        [DataMember(Name = "country_code")]
        public string CountryCode { get; set; }

        /// <summary>
        /// Line 1 of the organization's street address
        /// </summary>
        [DataMember(Name = "line1")]
        public string Line1 { get; set; }

        /// <summary>
        /// Line 2 of the organization's street address
        /// </summary>
        [DataMember(Name = "line2")]
        public string Line2 { get; set; }

        /// <summary>
        /// Line 3 of the organization's street address
        /// </summary>
        [DataMember(Name = "line3")]
        public string Line3 { get; set; }

        /// <summary>
        /// Postal (zip) code of the organization's street address
        /// </summary>
        [DataMember(Name = "postal_code")]
        public string PostalCode { get; set; }

        /// <summary>
        /// Name of the state or province for the organization_addresses; for country = CA or US, this field
        /// is overwritten by the state or province name derived from the state_code, if entered.
        /// </summary>
        [DataMember(Name = "state")]
        public string State { get; set; }

        /// <summary>
        /// Use ONLY for the standard 2 letter abbreviation for the US state or Canadian province for organization_addresses; 
        /// NOTE: A data validation error occurs if state_code is populated and country_code does not = US or CA.
        /// </summary>
        [DataMember(Name = "state_code")]
        public string StateCode { get; set; }
    }
}
