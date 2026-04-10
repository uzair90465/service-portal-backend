namespace SoftSolutions.DTOs.ResponseDTO
{
    public class CategoryResponseDTO:BaseModelResponseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ParentId { get; set; }
    }
}
