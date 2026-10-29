using BusinessInfo.Application.Common.Exceptions;
using BusinessInfo.Application.Common.Interfaces;
using BusinessInfo.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BusinessInfo.Application.Vehicle.Queries.Details
{
    public class DetailsVehicleQueryHandler : IRequestHandler<DetailsVehicleQueryRequest, DetailsVehicleQueryResponse>
    {
        private readonly IBusinessInfoContext _context;
        private readonly ILogger<DetailsVehicleQueryHandler> _logger;
        private readonly IRedisCaching _redis;
        public DetailsVehicleQueryHandler(IRedisCaching redis, IBusinessInfoContext context, ILogger<DetailsVehicleQueryHandler> logger)
        {
            _context = context;
            _logger = logger;
            _redis = redis;
        }

        public async Task<DetailsVehicleQueryResponse> Handle(DetailsVehicleQueryRequest request, CancellationToken cancellationToken)
        {

            try
            {
                _logger.LogInformation("Starting Details Vehicle {data}", request.ToJson());

                var query = await _context.Vehicles
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == request.VehicleId || x.Plate == request.Plate);
             
                if (query == null)
                {
                    throw new BadRequestException("Dados não encontrado");
                }
                else
                {
                    var response = new DetailsVehicleQueryResponse
                    {
                        Plate = query.Plate,
                        Year = query.Year,
                        NameVehicle = query.NameVehicle,
                        Renavam = query.Renavam,
                        TypeVechicle = query.TypeVechicle,
                        StatusVehicle = query.IsRented ? "Alugado" : "Disponivel para Locação",
                        Color = query.Collor,
                        ModelCar = query.Model,
                        Brand = query.Brand
                    };

                    return response;
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }

        }

    }
}