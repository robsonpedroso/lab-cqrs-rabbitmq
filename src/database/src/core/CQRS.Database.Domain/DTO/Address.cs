using DO = CQRS.Database.Domain.Entities;

namespace CQRS.Database.Domain.DTO
{
    public class Address
    {
        public virtual string Street { get; set; }
        public virtual string District { get; set; }
        public virtual string Complement { get; set; }
        public virtual string Number { get; set; }
        public virtual string ZipCode { get; set; }
        public virtual string State { get; set; }
        public virtual string City { get; set; }

        public Address()
        {

        }

        public Address(DO.Address address)
        {
            this.Street = address.Street;
            this.District = address.District;
            this.Number = address.Number;
            this.Complement = address.Complement;
            this.ZipCode = address.ZipCode;
            this.State = address.State;
            this.City = address.City;
        }
    }
}
