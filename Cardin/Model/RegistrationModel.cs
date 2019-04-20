using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Cardin.Model
{

    public partial class ItemStructure
    {
        [JsonProperty("total")]
        public int total { get; set; }

        [JsonProperty("limit")]
        public int limit { get; set; }

        [JsonProperty("skip")]
        public int skip { get; set; }

        [JsonProperty("data")]
        public List<ItemClass> Data { get; set; }
    }

    public partial class ItemClass
    {
        [JsonProperty("_id")]
        public string id { get; set; }

        [JsonProperty("name")]
        public string name { get; set; }

        [JsonProperty("facilityId")]
        public string facilityId { get; set; }
    }



        public class RegistrationModel
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string facilityName { get; set; }
        public string password { get; set; }

    }

    public partial class PersonStructure
    {
        [JsonProperty("total")]
        public int total { get; set; }

        [JsonProperty("limit")]
        public int limit { get; set; }

        [JsonProperty("skip")]
        public int skip { get; set; }

        [JsonProperty("data")]
        public List<Person> Data { get; set; }
    }



    public partial class RegistrationItem
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("data")]
        public Data Data { get; set; }
    }

    public partial class Data
    {
        [JsonProperty("facility")]
        public Facility Facility { get; set; }

        [JsonProperty("person")]
        public Person Person { get; set; }

        [JsonProperty("auth")]
        public Auth Auth { get; set; }
    }

    public partial class Auth
    {
        [JsonProperty("accessToken")]
        public string AccessToken { get; set; }
    }

    public partial class Facility
    {
        [JsonProperty("isActive")]
        public bool IsActive { get; set; }

        [JsonProperty("paidUserLicence")]
        public int PaidUserLicence { get; set; }

        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string name { get; set; }

        [JsonProperty("email")]
        public string email { get; set; }

        [JsonProperty("createdAt")]
        public DateTimeOffset CreatedAt { get; set; }

        [JsonProperty("updatedAt")]
        public DateTimeOffset UpdatedAt { get; set; }

        [JsonProperty("country")]
        public string country { get; set; }

        [JsonProperty("street")]
        public string street { get; set; }

        [JsonProperty("city")]
        public string city { get; set; }

        [JsonProperty("contact")]
        public string contact { get; set; }

        [JsonProperty("state")]
        public string state { get; set; }

        [JsonProperty("poBox")]
        public string poBox { get; set; }

        [JsonProperty("logo")]
        public string logo { get; set; }
    }

    public partial class Person
    {

        public SolidColorBrush isActiveColour
        {
            get {
                if (this.IsActive)
                {
                    return Brushes.Green;
                }
                else
                {
                    return Brushes.LightGray;
                }
            }

        }

        public string userStatusToolTip
        {
            get
            {
                if (this.IsActive)
                {
                    return "User's active";
                }
                else
                {
                    return "User's not active";
                }
            }

        }


        public string fullName
        {
            get
            {
                return this.firstName + " " + this.lastName;
            }

        }


        [JsonProperty("isActive")]
        public bool IsActive { get; set; }

        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("firstName")]
        public string firstName { get; set; }

        [JsonProperty("lastName")]
        public string lastName { get; set; }

        [JsonProperty("phone")]
        public string phone { get; set; }

        [JsonProperty("profileImagePath")]
        public string profileImagePath { get; set; }

        [JsonProperty("signatureImagePath")]
        public string signatureImagePath { get; set; }


        [JsonProperty("profileImage")]
        public string profileImage { get; set; }

        [JsonProperty("signature")]
        public string signature { get; set; }

        [JsonProperty("email")]
        public string email { get; set; }

        [JsonProperty("facilityId")]
        public string facilityId { get; set; }

        [JsonProperty("engagementType")]
        public string engagementType { get; set; }

        [JsonProperty("post")]
        public string post { get; set; }

        [JsonProperty("extentionNo")]
        public string extentionNo { get; set; }

        [JsonProperty("deativeDate")]
        public string deativeDate { get; set; }

        [JsonProperty("createdAt")]
        public DateTimeOffset CreatedAt { get; set; }

        [JsonProperty("updatedAt")]
        public DateTimeOffset UpdatedAt { get; set; }

        [JsonProperty("clocked")]
        public CardInOut clocked { get; set; }

        [JsonProperty("__v")]
        public long V { get; set; }

    }
}
