namespace DataAccessLibrary.Models
{
    public class BookingModel
    {
        public int Id { get; set; }
        public GuestModel Geust { get; set; }
        public RoomTypeModel RoomType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool? CheckedIn { get; set; }
        public decimal? TotalPrice { get; set; }
    }
}
