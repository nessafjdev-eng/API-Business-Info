using BusinessInfo.Domain.Enumerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessInfo.Application.Vehicle.Queries.Details
{
    public class DetailsVehicleQueryResponse
    {
        public string Plate { get; set; }
        public int Year { get; set; }
        public string NameVehicle { get; set; }
        public string Renavam { get; set; }
        public TypeVechicle TypeVechicle { get; set; }
        public string NameTypeVehicle { get; set; }
        public string StatusVehicle { get; set; }
        public string Color { get; set; }
        public string ModelCar { get; set; }
        public string Brand { get; set; }

        public DetailsVehicleQueryResponse()
        { }

        public DetailsVehicleQueryResponse(Domain.Entities.Vehicle vehicle)
        {
            Plate = vehicle.Plate;
            Year = vehicle.Year;
            NameVehicle = vehicle.NameVehicle;
            Renavam = vehicle.Renavam;
            TypeVechicle = vehicle.TypeVechicle;
            StatusVehicle = vehicle.TypeVechicle.ToString();
            Color = vehicle.Collor;
            ModelCar = vehicle.Model;
            Brand = vehicle.Brand;
            
        }

    }
}
