namespace SoftSolutions.DTOs.ResponseDTO
{
    public class ServiceResponseDTO:BaseModelResponseDTO
    {
        public string Title { get; set; }
        public decimal? BasePrice { get; set; }
        public string CategoryName { get; set; }
    }
}
