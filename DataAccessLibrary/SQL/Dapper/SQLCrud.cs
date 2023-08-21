using DataAccessLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary.SQL.Dapper
{
    public class SQLCrud
    {
        private readonly string _connectionString;
        private SQLDataAccess db = new SQLDataAccess();
        public SQLCrud(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Get All Rooms available in that Date (async)
        public List<RoomModel> GetAvailableRooms(DateTime startDate, DateTime endDate)
        {
            string query = @"
                SELECT r.*
                FROM dbo.Rooms r
                WHERE NOT EXISTS (
                    SELECT 1
                    FROM dbo.Bookings b
                    WHERE r.Id = b.RoomId
                    AND (@StartDate BETWEEN b.StartDate AND b.EndDate
                         OR @EndDate BETWEEN b.StartDate AND b.EndDate
                         OR b.StartDate BETWEEN @StartDate AND @EndDate
                         OR b.EndDate BETWEEN @StartDate AND @EndDate)
                )";

            var parameters = new { StartDate = startDate, EndDate = endDate };

            return db.LoadData<RoomModel, dynamic>(query, parameters, _connectionString);
        }

        // Get the RoomType By RoomId
        public RoomTypeModel GetTypeByRoomId(int roomId)
        {
            string query = @"
                SELECT rt.*
                FROM dbo.RoomTypes rt
                INNER JOIN dbo.Rooms r ON rt.Id = r.RoomTypeId
                WHERE r.RoomNumber = @RoomId
            ";

            var parameters = new { RoomId = roomId };

            return db.LoadData<RoomTypeModel, dynamic>(query, parameters, _connectionString).First();
        }

        // Save a Booking
        public void SaveBooking(BookingModel booking)
        {

        }

        // Get a Booking By LastName
        public BookingModel GetBookingByLastName(string lastname)
        {
            string query = @"SELECT b.*
                FROM dbo.Bookings b
                INNER JOIN dbo.Guests g ON b.GuestId = g.Id
                WHERE g.LastName = @LastName;";

            var parameters = new { LastName = lastname };

            return db.LoadData<BookingModel, dynamic>(query, parameters, _connectionString).First();
        }

        // Update a Booking
    }
}
