using BusinessInfo.Application.Common.AES;
using BusinessInfo.Application.Common.Exceptions;
using BusinessInfo.Application.Common.Interfaces;
using BusinessInfo.Application.Common.Metrics;
using BusinessInfo.Application.Common.Models.Response;
using BusinessInfo.Common;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace BusinessInfo.Application.RentalPartner.Command.Create
{
    public class CreateRentalPartnerCommandHandler : IRequestHandler<CreateRentalPartnerCommandRequest, ResponseApiBase<Guid>>
    {
        private readonly IBusinessInfoContext _context;
        private readonly ILogger<CreateRentalPartnerCommandHandler> _logger;
        private readonly AesEncryptionService _aesEncryptionService;
        private readonly IHttpContextAccessor _httpContextAccessor;



        public CreateRentalPartnerCommandHandler(IHttpContextAccessor contextAccessor, IBusinessInfoContext context, ILogger<CreateRentalPartnerCommandHandler> logger, AesEncryptionService aesEncryptionService)
        {
            _context = context;
            _logger = logger;
            _aesEncryptionService = aesEncryptionService;
            _httpContextAccessor = contextAccessor;
        }

        public async Task<ResponseApiBase<Guid>> Handle(CreateRentalPartnerCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var response = new ResponseApiBase<Guid>();

                _logger.LogInformation("Stating create RentalPartener {data}", request.ToJson());

                var rentalPartner = await RentalPartnerEntity(request);
                await _context.SaveChangesAsync(cancellationToken);

                response.AddSuccess(rentalPartner.Id);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }

        }

        private async Task<Domain.Entities.RentalPartner> RentalPartnerEntity(CreateRentalPartnerCommandRequest request)
        {

            var namePartnerExisting = await _context.RentalPartners.FirstOrDefaultAsync(c => c.Name == request.Name);

            if (namePartnerExisting is not null)
            {
                throw new BadRequestException($"O {request.Name} já existe.Digite outro.");
            }

            var issuer = _httpContextAccessor.HttpContext?.User?.FindFirst("IssuerId")?.Value;


            if (!Guid.TryParse(issuer, out var issuerId))
            {
                throw new UnauthorizedAccessException("IssuerId inválido ou não encontrado no token.");
            }

            _logger.LogInformation("Cliente logado:", issuer);


            var rentalPartner = new Domain.Entities.RentalPartner
            {
                Name = request.Name,    
                Document = request.Document.UnMask(),
                Active = request.Active,
                IssuerId = issuerId
            };
            await _context.RentalPartners.AddAsync(rentalPartner);
            ApplicationMetrics.ManyVehiclesRegisteredInShortTime(issuerId.ToString());

            return rentalPartner;

        }

    }

}
