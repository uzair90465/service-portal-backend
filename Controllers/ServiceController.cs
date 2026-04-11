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
    public class ServiceController : ControllerBase
    {
        private readonly DB _context;
        private readonly IMapper _mapper;

        public ServiceController(DB context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // ================= GET ALL SERVICES =================
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var services = await _context.Services
                .Include(s => s.Category)
                .ToListAsync();

            var result = _mapper.Map<List<ServiceResponseDTO>>(services);

            return Ok(result);
        }

        // ================= GET BY ID =================
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var service = await _context.Services
                .Include(s => s.Category)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (service == null)
                return NotFound();

            var result = _mapper.Map<ServiceResponseDTO>(service);

            return Ok(result);
        }

        // ================= GET BY CATEGORY =================
        [HttpGet("category/{categoryId}")]
        public async Task<IActionResult> GetByCategory(int categoryId)
        {
            var services = await _context.Services
                .Where(s => s.CategoryId == categoryId)
                .Include(s => s.Category)
                .ToListAsync();

            var result = _mapper.Map<List<ServiceResponseDTO>>(services);

            return Ok(result);
        }

        // ================= CREATE SERVICE (OPTIONAL ADMIN/TEST) =================
        [HttpPost]
        public async Task<IActionResult> Create(ServiceRequestDTO dto)
        {
            var service = _mapper.Map<Service>(dto);

            _context.Services.Add(service);
            await _context.SaveChangesAsync();

            var result = _mapper.Map<ServiceResponseDTO>(service);

            return Ok(result);
        }
    }
}