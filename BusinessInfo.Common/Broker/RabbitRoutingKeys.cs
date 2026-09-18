using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessInfo.Common.Broker
{
    public static class RabbitRoutingKeys
    {
        public const string VehicleRentalRequested = "vehicle.rental.requested";

        public const string VehicleRentalApproved =  "vehicle.rental.approved";

        public const string VehicleRentalRejected =  "vehicle.rental.rejected";
    }
}
