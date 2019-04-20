using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cardin.Model
{
    
    public partial class CardInOut
    {
        [JsonProperty("_id")]
        public string id { get; set; }

        [JsonProperty("personId")]
        public string personId { get; set; }

        [JsonProperty("facilityId")]
        public string facilityId { get; set; }

        [JsonProperty("isDateClose")]
        public bool isDateClose { get; set; }

        [JsonProperty("clockIn")]
        public Clocked clockIn { get; set; }

        [JsonProperty("clockOut")]
        public Clocked clockOut { get; set; }
    }

    public partial class Clocked
    {
        [JsonProperty("isCard")]
        public bool isCard { get; set; }

        [JsonProperty("comment")]
        public string comment { get; set; }

        [JsonProperty("clockTime")]
        public DateTime clockTime { get; set; }
    }
}
