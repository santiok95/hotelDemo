using System.ComponentModel.DataAnnotations;
using System.Data;

namespace hotelDemo.Models
{
    public class Booking
    {
        public Booking(string code, string name, DateTime from, DateTime to, int people, Uri url)
        {

            this.Code = code;
            this.Name = name;
            this.From = from;
            this.To = to;
            this.People = people;
            this.Url = url;
        }


        public Booking() { }


        public Guid Id { get; set; }

        [Display(Name = "Code")]
        public string Code { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name="From")]
        [DataType(DataType.DateTime)]
        public DateTime From { get; set; }

        [Display(Name = "To")]
        [DataType(DataType.DateTime)]
        public DateTime To { get; set; }

        [Display(Name = "People")]
        public int People { get; set; }

        [Display(Name = "Url")]
        [DataType(DataType.Url)]
        public Uri Url { get; set; }

        public BookingStatus Status { get; set; }

        public enum BookingStatus
        {
            Pending,
            Confirmed,
            Canceled
        }

    }
}
