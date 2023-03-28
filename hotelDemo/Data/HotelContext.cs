using hotelDemo.Models;
using Microsoft.EntityFrameworkCore;

namespace hotelDemo.Data
{
    public class HotelContext : DbContext
    {
        public HotelContext(DbContextOptions<HotelContext> options) : base(options) { }

        public DbSet<Booking> Bookings { get; set; }
    }
}
