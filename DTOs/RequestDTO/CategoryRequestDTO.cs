namespace SoftSolutions.DTOs.RequestDTO
{
    public class CategoryRequestDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int? ParentId { get; set; }
    }
}
