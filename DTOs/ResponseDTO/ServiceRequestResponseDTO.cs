namespace SoftSolutions.DTOs.ResponseDTO
{
    public class ServiceRequestResponseDTO:BaseModelResponseDTO
    {
        public int Id { get; set; }
        public string ServiceTitle { get; set; }
        public string ProblemDescription { get; set; }
        public string Status { get; set; }
        public string LocationName { get; set; }
    }
}
