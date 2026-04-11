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
    public class ReviewsController : ControllerBase
    {
        private readonly DB _context;
        private readonly IMapper _mapper;

        public ReviewsController(DB context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // ================= ADD REVIEW =================
        [HttpPost]
        public async Task<IActionResult> CreateReview(ReviewRequestDTO dto)
        {
            // check if order exists
            var order = await _context.Orders
                .Include(o => o.ServiceRequest)
                .FirstOrDefaultAsync(o => o.Id == dto.OrderId);

            if (order == null)
                return NotFound("Order not found");

            // only completed orders can be reviewed
            if (order.Status != "Completed")
                return BadRequest("Order not completed yet");

            // only one review per order
            var exists = await _context.Reviews
                .AnyAsync(r => r.OrderId == dto.OrderId);

            if (exists)
                return BadRequest("Review already exists");

            var review = _mapper.Map<Review>(dto);

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            var result = _mapper.Map<ReviewResponseDTO>(review);

            return Ok(result);
        }

        // ================= GET REVIEWS BY ORDER =================
        [HttpGet("order/{orderId}")]
        public async Task<IActionResult> GetByOrder(int orderId)
        {
            var review = await _context.Reviews
                .Where(r => r.OrderId == orderId)
                .FirstOrDefaultAsync();

            if (review == null)
                return NotFound();

            var result = _mapper.Map<ReviewResponseDTO>(review);

            return Ok(result);
        }

        // ================= GET ALL REVIEWS =================
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var reviews = await _context.Reviews.ToListAsync();

            var result = _mapper.Map<List<ReviewResponseDTO>>(reviews);

            return Ok(result);
        }
    }
}