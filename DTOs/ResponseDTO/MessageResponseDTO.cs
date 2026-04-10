namespace SoftSolutions.DTOs.ResponseDTO
{
    public class MessageResponseDTO:BaseModelResponseDTO
    {
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public int ServiceRequestId { get; set; }

        public string MessageText { get; set; }
        public DateTime SentAt { get; set; }
    }
}
