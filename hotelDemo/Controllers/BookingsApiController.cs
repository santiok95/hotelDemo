using hotelDemo.Data;
using hotelDemo.Dtos;
using hotelDemo.Hubs;
using hotelDemo.Models;
using hotelDemo.Services;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace hotelDemo.Controllers
{

    [Route("api/Bookings")]
    [ApiController]
    public class BookingsApiController : ControllerBase
    {
        private readonly HotelContext _context;
        private readonly IHubContext<NotificationHub> _hubContext;

        public BookingsApiController(HotelContext context, IHubContext<NotificationHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }


        // GET: api/Bookings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Booking>>> GetBookings()
        {
            return await _context.Bookings.ToListAsync();
        }

        // GET: api/Bookings/5
        [HttpGet("{code}")]
        public async Task<ActionResult<Booking>> GetBooking(string code)
        {
            var booking = await _context.Bookings.FirstOrDefaultAsync(x => x.Code == code);

            if (booking == null)
            {
                return NotFound();
            }

            if (booking.CheckIn < DateTime.Today)
            {
                booking.Status = Booking.BookingStatus.Archived;
            }

            return booking;
        }


        // DELETE: api/Bookings/5
        [HttpDelete("{code}")]
        public async Task<ActionResult> CancelBooking(string code)
        {
            var booking = await _context.Bookings.FirstOrDefaultAsync(x => x.Code == code);

            if (booking == null)
            {
                return NotFound();
            }

            booking.Status = Booking.BookingStatus.Canceled;

            _context.Bookings.Update(booking);

            await _context.SaveChangesAsync();

            return Ok();
        }

        // PUT: api/Bookings/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBooking(Guid id, Booking booking)
        {
            if (id != booking.Id)
            {
                return BadRequest();
            }

            _context.Entry(booking).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookingExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Bookings
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Booking>> PostBooking(Booking booking)
        {
            booking.Code = BookingCodeGenerator.GetBookingCode();

            booking.Status = Booking.BookingStatus.Pending;


            var qrStream = QRgenerator.QrGenerator(booking.Code);


            var model = new ImageDTO("QRImage", "QRfolder");

            var imageUrl = await FirebaseStorageManager.UploadImage(qrStream, model);

            booking.QR = new Uri(imageUrl);

            await _context.Bookings.AddAsync(booking);
            await _context.SaveChangesAsync();

            await _hubContext.Clients.All.SendAsync("ReceiveBooking", booking);

            return CreatedAtAction("GetBooking", new { id = booking.Id }, booking);
        }

        //// DELETE: api/Bookings/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteBooking(Guid id)
        //{
        //    var booking = await _context.Bookings.FindAsync(id);
        //    if (booking == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Bookings.Remove(booking);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        private bool BookingExists(Guid id)
        {
            return _context.Bookings.Any(e => e.Id == id);
        }


    }
}