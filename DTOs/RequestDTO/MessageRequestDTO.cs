namespace SoftSolutions.DTOs.RequestDTO
{
    public class MessageRequestDTO
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public int ServiceRequestId { get; set; }

        public string MessageText { get; set; }
        public DateTime SentAt { get; set; }
    }
}
