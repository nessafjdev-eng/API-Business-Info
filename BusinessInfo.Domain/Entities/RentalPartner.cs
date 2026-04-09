using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessInfo.Domain.Entities
{
    public class RentalPartner : BaseEntity
    {
        public Guid IssuerId { get; set; }
        public Issuer Issuer { get; set; }
        public string Name { get; set; }
        public string Document { get; set; }
        public bool Active { get; set; }
    }
}
