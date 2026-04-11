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
    public class ServiceRequestController : ControllerBase
    {
        private readonly DB _context;
        private readonly IMapper _mapper;

        public ServiceRequestController(DB context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // ================= CREATE REQUEST (USER) =================
        [HttpPost]
        public async Task<IActionResult> Create(ServiceRequestRequestDTO dto)
        {
            var request = _mapper.Map<ServiceRequest>(dto);

            request.Status = "Pending";

            _context.ServiceRequests.Add(request);
            await _context.SaveChangesAsync();

            var result = _mapper.Map<ServiceRequestResponseDTO>(request);

            return Ok(result);
        }

        // ================= GET ALL REQUESTS (USER SIDE) =================
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUser(int userId)
        {
            var requests = await _context.ServiceRequests
                .Include(r => r.Service)
                .Include(r => r.Location)
                .Where(r => r.UserId == userId)
                .ToListAsync();

            var result = _mapper.Map<List<ServiceRequestResponseDTO>>(requests);

            return Ok(result);
        }

        // ================= GET FOR PROVIDER (SMART FILTER) =================
        [HttpGet("provider/{providerId}")]
        public async Task<IActionResult> GetForProvider(int providerId)
        {
            // step 1: get provider services
            var providerServices = await _context.ProviderServices
                .Where(p => p.ProviderId == providerId)
                .Select(p => p.ServiceId)
                .ToListAsync();

            // step 2: filter requests
            var requests = await _context.ServiceRequests
                .Include(r => r.Service)
                .Include(r => r.Location)
                .Where(r =>
                    r.Status == "Pending"
                    && providerServices.Contains(r.ServiceId)
                )
                .ToListAsync();

            var result = _mapper.Map<List<ServiceRequestResponseDTO>>(requests);

            return Ok(result);
        }

        // ================= UPDATE STATUS (OPTIONAL CONTROL) =================
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, string status)
        {
            var request = await _context.ServiceRequests.FindAsync(id);

            if (request == null)
                return NotFound();

            request.Status = status;

            await _context.SaveChangesAsync();

            return Ok("Status updated");
        }
    }
}