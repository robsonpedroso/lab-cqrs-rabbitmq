namespace CQRS.Core.Domain.VO
{
    public class ProductItem
    {
        public string InternalCode { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }

        public ProductItem() { }

        public ProductItem(DTO.ProductItem item)
        {
            InternalCode = item.InternalCode;
            Name = item.Name;
            Description = item.Description;
            Quantity = item.Quantity;
            Price = item.Price;
        }
    }
}
