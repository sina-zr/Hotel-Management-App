namespace DataAccessLibrary.Models
{
    public class FullBookingModel
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public int GuestId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string RoomNumber { get; set; }
        public int RoomTypeId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool? CheckedIn { get; set; }
        public decimal? TotalPrice { get; set; }
    }
}
