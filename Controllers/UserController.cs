using Microsoft.AspNetCore.Mvc;
using SoftSolutions.Database;
using SoftSolutions.DTOs.RequestDTO;
using SoftSolutions.DTOs.ResponseDTO;
using SoftSolutions.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace SoftSolutions.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly DB _context;
        private readonly IMapper _mapper;

        public UsersController(DB context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRequestDTO dto)
        {
            var exists = await _context.Users
                .AnyAsync(u => u.Email == dto.Email);
            if (exists)
                return BadRequest("Email already exists");

            var user = _mapper.Map<User>(dto);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var response = _mapper.Map<UserResponseDTO>(user);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _context.Users.ToListAsync();
            var result = _mapper.Map<List<UserResponseDTO>>(users);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound();
            var result = _mapper.Map<UserResponseDTO>(user);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UserRequestDTO dto)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound();
            _mapper.Map(dto, user);
            await _context.SaveChangesAsync();
            var response = _mapper.Map<UserResponseDTO>(user);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound();
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return Ok("User deleted successfully");
        }
    }
}