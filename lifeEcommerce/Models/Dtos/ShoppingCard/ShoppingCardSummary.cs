using System.IO;

namespace lifeEcommerce.Models.Dtos.ShoppingCard
{
    public class ShoppingCardSummary
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Title { get; set; }
        public string Quantity { get; set; }
        public string Total { get; set; }
    }
}
