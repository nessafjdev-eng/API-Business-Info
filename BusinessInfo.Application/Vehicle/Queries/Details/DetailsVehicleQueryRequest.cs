using BusinessInfo.Application.Common.Models.Response;
using BusinessInfo.Application.VehicleSaved.Queries.List;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessInfo.Application.Vehicle.Queries.Details
{
    public class DetailsVehicleQueryRequest : IRequest<DetailsVehicleQueryResponse>
    {
        public Guid? VehicleId { get; set; }
        public string Plate { get; set; }
    }
}
