using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessInfo.Application.Common.Metrics
{
    using Prometheus;
    public static class ApplicationMetrics
    {
        private static readonly Counter ManyVehiclesRegisteredCounter = Metrics.CreateCounter(
            "business_info_many_vehicles_registered_in_short_time_total",
            "Counts many vehicles registered in short time by client",
            new[] { "clientId" }
        );

        public static void ManyVehiclesRegisteredInShortTime(string issuerId)
        {
            ManyVehiclesRegisteredCounter.WithLabels(issuerId).Inc();
        }
    }


}
