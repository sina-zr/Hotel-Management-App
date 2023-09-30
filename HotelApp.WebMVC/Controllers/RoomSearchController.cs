using DataAccessLibrary.Data;
using DataAccessLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace HotelApp.WebMVC.Controllers
{
    public class RoomSearchController : Controller
    {
        private readonly IDataBaseData _db;

        public RoomSearchController(IDataBaseData db)
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.StartDate = DateTime.Now.ToString("yyyy-MM-dd");
            ViewBag.EndDate = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");

            return View();
        }

        [HttpPost]
        public IActionResult Search(DateTime startDate, DateTime endDate)
        {
            ViewBag.AvailableRoomTypes = _db.GetAvailableRoomTypes(startDate, endDate);

            // Set the ViewBag values for startDate and endDate to preserve the input values
            ViewBag.StartDate = startDate.ToString("yyyy-MM-dd");
            ViewBag.EndDate = endDate.ToString("yyyy-MM-dd");

            return View("Index");
        }
    }
}
