namespace SoftSolutions.DTOs.ResponseDTO
{
    public class BaseModelResponseDTO
    {
        public class BaseResponseDto
        {
            public int Id { get; set; }
            public bool IsActive { get; set; }
            public Guid GlobalId { get; set; }
            public DateTime CreatedAt { get; set; }
        }
    }
}
