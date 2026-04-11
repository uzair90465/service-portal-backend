namespace SoftSolutions.DTOs.ResponseDTO
{
    public class ReviewResponseDTO:BaseModelResponseDTO
    {
        public int OrderId { get; set; }   // ⭐ ADD THIS (IMPORTANT)

        public int Rating { get; set; }

        public string Comment { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
