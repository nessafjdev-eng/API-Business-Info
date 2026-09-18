using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessInfo.Application.Services.Events
{
    public class VehicleRentalRequestedEvent
    {
        public Guid RentalId { get; set; }
        public Guid VehicleId { get; set; }
        public Guid ClientId { get; set; }
        public decimal ValueTotal { get; set; }
    }
}
