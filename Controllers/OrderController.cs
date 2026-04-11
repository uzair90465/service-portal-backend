using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SoftSolutions.Database;
using SoftSolutions.DTOs.ResponseDTO;
using SoftSolutions.Models;
using AutoMapper;

namespace SoftSolutions.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly DB _context;
        private readonly IMapper _mapper;

        public OrdersController(DB context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // ================= GET ALL ORDERS =================
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var orders = await _context.Orders
                .Include(o => o.ServiceRequest)
                .ToListAsync();

            var result = _mapper.Map<List<OrderResponseDTO>>(orders);

            return Ok(result);
        }

        // ================= GET BY ID =================
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var order = await _context.Orders
                .Include(o => o.ServiceRequest)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
                return NotFound();

            var result = _mapper.Map<OrderResponseDTO>(order);

            return Ok(result);
        }

        // ================= GET BY USER =================
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUser(int userId)
        {
            var orders = await _context.Orders
                .Include(o => o.ServiceRequest)
                .Where(o => o.ServiceRequest.UserId == userId)
                .ToListAsync();

            var result = _mapper.Map<List<OrderResponseDTO>>(orders);

            return Ok(result);
        }

        // ================= UPDATE STATUS =================
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, string status)
        {
            var order = await _context.Orders.FindAsync(id);

            if (order == null)
                return NotFound();

            order.Status = status;

            await _context.SaveChangesAsync();

            return Ok("Order status updated");
        }

        // ================= COMPLETE ORDER =================
        [HttpPost("{id}/complete")]
        public async Task<IActionResult> CompleteOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);

            if (order == null)
                return NotFound();

            order.Status = "Completed";
            order.PaymentStatus = "Paid";

            await _context.SaveChangesAsync();

            return Ok("Order completed successfully");
        }
    }
}