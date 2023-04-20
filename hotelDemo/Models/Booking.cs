using System.ComponentModel.DataAnnotations;
using System.Data;

namespace hotelDemo.Models
{
    public class Booking
    {
        public Booking(string name, DateTime checkIn, DateTime checkOut, int people)
        {

            this.Name = name;
            this.CheckIn = checkIn;
            this.CheckOut = checkOut;
            this.People = people;
        }
        public Booking()
        {
        }

        public Guid Id { get; set; }

        [Display(Name = "Code")]
        public string? Code { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Check In")]
        [DataType(DataType.DateTime)]
        public DateTime CheckIn { get; set; }

        [Display(Name = "Check Out")]
        [DataType(DataType.DateTime)]
        public DateTime CheckOut { get; set; }

        [Display(Name = "People")]
        public int People { get; set; }

        [Display(Name = "Url")]
        [DataType(DataType.Url)]
        public Uri? QR { get; set; }

        public BookingStatus Status { get; set; }

        public enum BookingStatus
        {
            Pending,
            Confirmed,
            Canceled
        }

    }
}
