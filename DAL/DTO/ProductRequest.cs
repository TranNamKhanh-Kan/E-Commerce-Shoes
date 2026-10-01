namespace DAL.DTO
{
    public class ProductRequest
    {
        public string? Name { get; set; }

        public string? Type { get; set; }

        public string? Status { get; set; }

        /// <summary>Được gán sau khi upload Cloudinary (client không cần gửi).</summary>
        public string? ImageUrl { get; set; }
        public string? PublicId { get; set; }

        public int? Quantity { get; set; }

        public float? Price { get; set; }

        public string? Size { get; set; }
    }
}
