namespace DAL.DTO
{
    public class ProductResponse
    {
        public Guid ProductId { get; set; }

        public string Type { get; set; }

        public string Name { get; set; }

        public int? Quantity { get; set; }

        public double? Price { get; set; }

        public string Size { get; set; }

        public string Status { get; set; }

        public string ImageUrl { get; set; }
    }
}
