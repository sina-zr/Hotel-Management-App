using DataAccessLibrary.Data;
using DataAccessLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HotelApp.Web.Pages
{
    public class BookRoomModel : PageModel
    {
        private readonly IDataBaseData _db;
        public BookRoomModel(IDataBaseData db)
        {
            _db = db;
        }

        [BindProperty(SupportsGet = true)]
        public int RoomTypeId { get; set; }

        [BindProperty]
        public string FirstName { get; set; }
        [BindProperty]
        public string LastName { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime StartDate { get; set; }
        [BindProperty(SupportsGet = true)]
        public DateTime EndDate { get; set; }

        [BindProperty]
        public RoomTypeModel RoomType { get; set; }

        public void OnGet(string firstName, string lastName, DateTime startDate, DateTime endDate, int roomTypeId)
        {
            FirstName = firstName;
            LastName = lastName;
            StartDate = startDate;
            EndDate = endDate;
            RoomTypeId = roomTypeId;

            if(roomTypeId > 0)
            {
                RoomType = _db.GetRoomTypeById(roomTypeId);
            }
        }
        public IActionResult OnPost()
        {
            _db.BookARoom(FirstName, LastName, StartDate, EndDate, RoomTypeId);
            return RedirectToPage("Index");
        }
    }
}
