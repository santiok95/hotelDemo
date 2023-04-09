using hotelDemo.Data;
using hotelDemo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static hotelDemo.Models.Booking;

namespace hotelDemo.Controllers
{
    public class BookingRegisterController : Controller
    {
        public readonly HotelContext _context;

        public BookingRegisterController(HotelContext context)
        {
            _context = context;
        }

        public async Task<ActionResult> Index()
        {   
            
            var bookinglist =  await _context.Bookings.ToListAsync();
            
            return View(bookinglist);


        }

        [HttpGet]
        public async Task<IActionResult> Register()
        {
            Booking booking = new Booking();

            return View(booking);
        }

        [HttpPost]
        public async Task<IActionResult> Register(Booking booking)
        {
            if(ModelState.IsValid)
            {
               
                
                booking.Status = Booking.BookingStatus.Pending;
                await _context.Bookings.AddAsync(booking);
                await _context.SaveChangesAsync();
                
            }

            return RedirectToAction("Index", "Home");


        }

        [HttpPost]
        public async Task<IActionResult> Edit(Guid Id, Booking booking)
        {
            var result = await _context.Bookings.SingleOrDefaultAsync(x => x.Id == Id);

            if (result == null) { return  View(booking); }

            result.Status = booking.Status;

            _context.Bookings.Update(result);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "BookingRegister");
        }
        [HttpGet]
        public async Task<IActionResult> Edit(Guid Id) {
        
            var viewmodel = await _context.Bookings.SingleOrDefaultAsync(model => model.Id == Id);
            if (viewmodel == null) { return View(null); };

            return View(viewmodel);




        
        }
        

    }
}
