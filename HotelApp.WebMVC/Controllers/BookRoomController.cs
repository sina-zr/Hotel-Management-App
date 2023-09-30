using DataAccessLibrary.Data;
using DataAccessLibrary.Models;
using Microsoft.AspNetCore.Mvc;

namespace HotelApp.WebMVC.Controllers
{
    public class BookRoomController : Controller
    {
        private readonly IDataBaseData _db;
        public BookRoomController(IDataBaseData db)
        {
            _db = db;
        }


        //[BindProperty]
        //public string FirstName { get; set; }
        //[BindProperty]
        //public string LastName { get; set; }

        [BindProperty(SupportsGet = true)]
        public static DateTime StartDate { get; set; }
        [BindProperty(SupportsGet = true)]
        public static DateTime EndDate { get; set; }

        [BindProperty(SupportsGet = true)]
        public RoomTypeModel RoomType { get; set; }

        [BindProperty(SupportsGet = true)]
        public int RoomTypeId { get; set; }

        [HttpGet]
        public IActionResult Index(DateTime startDate, DateTime endDate, int roomTypeId)
        {
            StartDate = startDate;
            EndDate = endDate;
            RoomTypeId = roomTypeId;

            if (roomTypeId > 0)
            {
                RoomType = _db.GetRoomTypeById(roomTypeId);
            }

            return View(new { RoomType, StartDate, EndDate });
        }

        [HttpPost]
        public IActionResult Book(string firstName, string lastName, int roomTypeId)
        {
            _db.BookARoom(firstName, lastName, StartDate, EndDate, roomTypeId);
            return RedirectToAction("Index","Home");
        }
    }
}
