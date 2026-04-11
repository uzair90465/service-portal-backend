using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SoftSolutions.Database;
using SoftSolutions.Models;
using SoftSolutions.DTOs.RequestDTO;
using SoftSolutions.DTOs.ResponseDTO;
using AutoMapper;

namespace SoftSolutions.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationsController : ControllerBase
    {
        private readonly DB _context;
        private readonly IMapper _mapper;

        public LocationsController(DB context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // ================= CREATE LOCATION =================
        [HttpPost]
        public async Task<IActionResult> Create(LocationRequestDTO dto)
        {
            var location = _mapper.Map<Location>(dto);

            _context.Locations.Add(location);
            await _context.SaveChangesAsync();

            var result = _mapper.Map<LocationResponseDTO>(location);

            return Ok(result);
        }

        // ================= GET ALL =================
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var locations = await _context.Locations.ToListAsync();

            var result = _mapper.Map<List<LocationResponseDTO>>(locations);

            return Ok(result);
        }

        // ================= GET BY ID =================
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var location = await _context.Locations.FindAsync(id);

            if (location == null)
                return NotFound();

            var result = _mapper.Map<LocationResponseDTO>(location);

            return Ok(result);
        }
    }
}