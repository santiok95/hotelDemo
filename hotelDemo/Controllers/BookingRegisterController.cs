using hotelDemo.Data;
using hotelDemo.Dtos;
using hotelDemo.Models;
using hotelDemo.Services;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;



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

            var bookinglist = await _context.Bookings.ToListAsync();

            return View(bookinglist);


        }

        [HttpGet]
        public async Task<IActionResult> Register()
        {
            Booking booking = new Booking(string.Empty, DateTime.Today, DateTime.Today.AddDays(7), 0);

            return View(booking);
        }

        [HttpPost]
        public async Task<IActionResult> Register(Booking booking)
        {
            if (ModelState.IsValid)
            {


                booking.Code = BookingCodeGenerator.GetBookingCode();

                booking.Status = Booking.BookingStatus.Pending;


                var qrStream = QRgenerator.QrGenerator(booking.Code);


                var model = new ImageDTO("QRImage", "QRfolder");

                var imageUrl = await FirebaseStorageManager.UploadImage(qrStream, model);

                booking.QR = new Uri(imageUrl);

                await _context.Bookings.AddAsync(booking);
                await _context.SaveChangesAsync();

            }

            return RedirectToAction("Index");


        }

        [HttpPost]
        public async Task<IActionResult> Edit(Guid Id, Booking booking)
        {
            var result = await _context.Bookings.SingleOrDefaultAsync(x => x.Id == Id);

            if (result == null) { return View(booking); }

            result.Status = booking.Status;

            _context.Bookings.Update(result);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "BookingRegister");
        }
        [HttpGet]
        public async Task<IActionResult> Edit(Guid Id)
        {

            var viewmodel = await _context.Bookings.SingleOrDefaultAsync(model => model.Id == Id);
            if (viewmodel == null) { return View(null); };

            return View(viewmodel);





        }


    }

    public static class BookingCodeGenerator
    {
        private static readonly Random _random = new Random();
        private const string _chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        public static string GetBookingCode()
        {
            string code;

            // Genera un código al azar y verifica que tenga al menos un número.
            do
            {
                code = new string(Enumerable.Repeat(_chars, 6)
                    .Select(s => s[_random.Next(s.Length)]).ToArray());
            } while (!code.Any(char.IsDigit));

            return code;
        }


    }


}
