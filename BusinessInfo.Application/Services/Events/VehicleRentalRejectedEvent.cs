using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessInfo.Application.Services.Events
{
    public class VehicleRentalRejectedEvent
    {
        public Guid RentalId { get; set; }
        public string Reason { get; set; }
    }
}
