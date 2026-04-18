using BusinessInfo.Application.Common.Models.Response;
using BusinessInfo.Domain.Enumerators;
using MediatR;
using System.Text.Json.Serialization;

namespace BusinessInfo.Application.RentalPartner.Command.Create
{
    public class CreateRentalPartnerCommandRequest : IRequest<ResponseApiBase<Guid>>
    {
        [JsonIgnore]
        public Guid Id { get; set; }
        public Guid IssuerId { get; set; }
        public string Name { get; set; }
        public string Document { get; set; }
        public bool Active { get; set; }

    }
}
