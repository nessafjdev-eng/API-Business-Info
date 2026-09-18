using BusinessInfo.Application.Common.Interfaces;
using BusinessInfo.Application.Services.Events;
using BusinessInfo.Application.Services.Interfaces;
using BusinessInfo.Common;
using BusinessInfo.Common.Broker;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessInfo.Application.ProcessLocationVehicle.Command
{
    
    public class ProcessVehicleRentalHandler : IRequestHandler<ProcessVehicleRentalCommandRequest, ProcessVehicleRentalCommandResponse>
    {
        private readonly IBusinessInfoContext _context;
        private readonly IMessageQueueService _queueService;

        public ProcessVehicleRentalHandler(IBusinessInfoContext context,IMessageQueueService queueService)
        {
            _context = context;
            _queueService = queueService;
        }

        public async Task<ProcessVehicleRentalCommandResponse> Handle(ProcessVehicleRentalCommandRequest request, CancellationToken cancellationToken)
        {
            var vehicle = await _context.Vehicles.FirstOrDefaultAsync(x => x.Id.Equals(request.VehicleId));

            if (vehicle == null)
            {
                Reject(request.RentalId, "Veículo não encontrado.");
                return new ProcessVehicleRentalCommandResponse { Message = "Veiculo não encontrado" };
                
            }

            if (vehicle.IsRented)
            {
                Reject(request.RentalId, "Veículo indisponível.");

                return new ProcessVehicleRentalCommandResponse { Message = "Veiculo indisponivel." };
            }

            Approve(request);

            return new ProcessVehicleRentalCommandResponse { Message = "Locação Aprovada" };
        }

        private void Approve(
            ProcessVehicleRentalCommandRequest request)
        {
            var evt =
                new VehicleRentalApprovedEvent
                {
                    RentalId = request.RentalId,
                    VehicleId = request.VehicleId,
                    ApprovedAt = DateTime.UtcNow
                };

            _queueService.Publish(
                evt,
                Configuration.RabbitExchange,
                RabbitRoutingKeys.VehicleRentalApproved);
        }

        private void Reject(
            Guid rentalId,
            string reason)
        {
            var evt =
                new VehicleRentalRejectedEvent
                {
                    RentalId = rentalId,
                    Reason = reason
                };

            _queueService.Publish(
                evt,
                Configuration.RabbitExchange,
                RabbitRoutingKeys
                    .VehicleRentalRejected);
        }
    }
}
