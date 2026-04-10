namespace SoftSolutions.DTOs.ResponseDTO
{
    public class LocationResponseDTO:BaseModelResponseDTO
    {
        public string Name { get; set; }
        public int? ParentId { get; set; }
    }
}
