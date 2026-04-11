using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SoftSolutions.Database;
using SoftSolutions.DTOs.RequestDTO;
using SoftSolutions.DTOs.ResponseDTO;
using SoftSolutions.Models;
using AutoMapper;

namespace SoftSolutions.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly DB _context;
        private readonly IMapper _mapper;

        public MessagesController(DB context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // ================= SEND MESSAGE =================
        [HttpPost]
        public async Task<IActionResult> SendMessage(MessageRequestDTO dto)
        {
            var message = _mapper.Map<Message>(dto);

            // backend will set time
            message.SentAt = DateTime.Now;

            _context.Messages.Add(message);
            await _context.SaveChangesAsync();

            var result = _mapper.Map<MessageResponseDTO>(message);

            return Ok(result);
        }

        // ================= GET CHAT BY SERVICE REQUEST =================
        [HttpGet("request/{requestId}")]
        public async Task<IActionResult> GetChat(int requestId)
        {
            var messages = await _context.Messages
                .Include(m => m.Sender)
                .Include(m => m.Receiver)
                .Where(m => m.ServiceRequestId == requestId) // ✅ FIXED
                .OrderBy(m => m.SentAt)
                .ToListAsync();

            var result = _mapper.Map<List<MessageResponseDTO>>(messages);

            return Ok(result);
        }

        // ================= GET USER CHAT HISTORY (OPTIONAL BONUS) =================
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserMessages(int userId)
        {
            var messages = await _context.Messages
                .Include(m => m.Sender)
                .Include(m => m.Receiver)
                .Where(m => m.SenderId == userId || m.ReceiverId == userId)
                .OrderByDescending(m => m.SentAt)
                .ToListAsync();

            var result = _mapper.Map<List<MessageResponseDTO>>(messages);

            return Ok(result);
        }
    }
}