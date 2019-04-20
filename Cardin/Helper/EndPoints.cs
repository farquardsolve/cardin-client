using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cardin.Helper
{
    public class EndPoints
    {
        public const string host = "http://localhost:3030";
        public const string registration = host + "/registrations";
        public const string facilities = host + "/facilities";
        public const string savefacilities = host + "/save-facilities";
        public const string persons = host + "/persons";
        public const string authentication = host + "/authentication";
        public const string workerPosts = host + "/worker-posts";
        public const string engagementTypes = host + "/engagement-types";
        public const string carLots = host + "/car-lots";
        public const string fiiterWorker = host + "/filter-workers";
        public const string clockActivities = host + "/clock-activities";
    }
}
