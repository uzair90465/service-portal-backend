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
    public class RequestOfferController : ControllerBase
    {
        private readonly DB _context;
        private readonly IMapper _mapper;

        public RequestOfferController(DB context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // ================= CREATE OFFER (PROVIDER) =================
        [HttpPost]
        public async Task<IActionResult> CreateOffer(RequestOfferRequestDTO dto)
        {
            var offer = _mapper.Map<RequestOffer>(dto);

            offer.Status = "Pending";

            _context.RequestOffers.Add(offer);
            await _context.SaveChangesAsync();

            var result = _mapper.Map<RequestOfferResponseDTO>(offer);

            return Ok(result);
        }

        // ================= GET OFFERS BY REQUEST (USER SIDE) =================
        [HttpGet("request/{requestId}")]
        public async Task<IActionResult> GetOffersByRequest(int requestId)
        {
            var offers = await _context.RequestOffers
                .Include(o => o.Provider)
                .Where(o => o.RequestId == requestId)
                .ToListAsync();

            var result = _mapper.Map<List<RequestOfferResponseDTO>>(offers);

            return Ok(result);
        }

        // ================= ACCEPT OFFER (USER SELECTS PROVIDER) 💣 =================
        [HttpPost("accept/{offerId}")]
        public async Task<IActionResult> AcceptOffer(int offerId)
        {
            var offer = await _context.RequestOffers
                .Include(o => o.ServiceRequest)
                .FirstOrDefaultAsync(o => o.Id == offerId);

            if (offer == null)
                return NotFound("Offer not found");

            // reject all other offers of same request
            var otherOffers = await _context.RequestOffers
                .Where(o => o.RequestId == offer.RequestId)
                .ToListAsync();

            foreach (var o in otherOffers)
            {
                o.Status = (o.Id == offerId) ? "Accepted" : "Rejected";
            }

            // update request status
            offer.ServiceRequest.Status = "Accepted";

            // ================= CREATE ORDER AUTOMATICALLY 💣 =================
            if (offer == null)
                return NotFound();

            var price = offer.OfferedPrice;

            var order = new Order
            {
                ServiceRequestId = offer.RequestId,
                TotalAmount = price,
                CommissionPercentage = 5,
                CommissionAmount = price * 0.05m,
                ProviderEarning = price - (price * 0.05m),
                PaymentStatus = "Pending",
                Status = "InProgress"
            };

            _context.Orders.Add(order);

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Offer accepted and order created",
                orderId = order.Id
            });
        }

        // ================= REJECT OFFER =================
        [HttpPost("reject/{offerId}")]
        public async Task<IActionResult> RejectOffer(int offerId)
        {
            var offer = await _context.RequestOffers.FindAsync(offerId);

            if (offer == null)
                return NotFound();

            offer.Status = "Rejected";

            await _context.SaveChangesAsync();

            return Ok("Offer rejected");
        }
    }
}