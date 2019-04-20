using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cardin.Model
{
    public partial class CarLotStructure
    {
        [JsonProperty("total")]
        public int total { get; set; }

        [JsonProperty("limit")]
        public int limit { get; set; }

        [JsonProperty("skip")]
        public int skip { get; set; }

        [JsonProperty("data")]
        public ObservableCollection<CarLot> Data { get; set; }
    }

    public partial class CarLot
    {
        [JsonProperty("_id")]
        public string id { get; set; }

        [JsonProperty("name")]
        public string name { get; set; }

        [JsonProperty("facilityId")]
        public string facilityId { get; set; }

        [JsonProperty("spaces")]
        public List<CarParkSpace> spaces { get; set; }

        public string spaceItems
        {
            get
            {
                string result = "";
                if (spaces != null)
                {
                    for (int i = 0; i < spaces.Count; i++)
                    {
                        if (i != spaces.Count - 1)
                            result += spaces[i].name + ",";
                        else
                            result += spaces[i].name;
                    }
                }
                
                return result;
            }

        }
    }

    public partial class CarParkSpace
    {
        [JsonProperty("_id")]
        public string id { get; set; }

        [JsonProperty("name")]
        public string name { get; set; }

        [JsonProperty("isOccupied")]
        public bool isOccupied { get; set; }
        
    }
}
