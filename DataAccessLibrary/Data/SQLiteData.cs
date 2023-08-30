using DataAccessLibrary.DataBase;
using DataAccessLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary.Data
{
	public class SQLiteData : IDataBaseData
	{
		private readonly ISQLiteDataAccess _db;
		private const string connectionStringName = "SQLiteDb";

		public SQLiteData(ISQLiteDataAccess db)
		{
			_db = db;
		}

		public List<RoomTypeModel> GetAvailableRoomTypes(DateTime startDate, DateTime endDate)
		{

			string query = @"SELECT t.Id, t.Title, t.Description, t.Price
			FROM Rooms r 
			inner join RoomTypes t on t.Id = r.RoomTypeId
			WHERE r.Id NOT in (
				SELECT b.RoomId
				FROM Bookings b
				WHERE (@startDate < b.StartDate AND @endDate > b.EndDate)
					 OR (b.StartDate < @endDate AND @endDate < b.EndDate)
					 OR (b.StartDate <= @startDate AND @startDate < b.EndDate))
				group by t.Id, t.Title, t.Description, t.Price";

			var parameters = new { startDate, endDate };

			var output = _db.LoadData<RoomTypeModel, dynamic>(query,
														parameters,
														connectionStringName);

			output.ForEach(x => x.Price = x.Price / 100);

			return output;
		}

		public void BookARoom(string firstName, string lastName, DateTime startDate, DateTime endDate, int roomTypeId)
		{
			// if guest doesn't already exist in Guest Table, INSERT it
			// & anyway return an Id
			var guest = _db.LoadData<GuestModel, dynamic>("dbo.spGuest_Insert",
														  new { firstName, lastName },
														  connectionStringName).First();

			RoomTypeModel roomType = _db.LoadData<RoomTypeModel, dynamic>("select * from dbo.RoomTypes where Id = @Id",
				new { Id = roomTypeId }, connectionStringName).First();

			TimeSpan timeStaying = endDate.Subtract(startDate);


			// Getting the available rooms
			var LoadRoomsParameters = new { startDate, endDate, roomTypeId };
			List<RoomModel> availableRooms = _db.LoadData<RoomModel, dynamic>("dbo.spRooms_GetAvailableRooms",
																			  LoadRoomsParameters,
																			  connectionStringName);

			//Saving the Booking
			var SaveBookingParameters = new
			{
				roomId = availableRooms.First().Id,
				GuestId = guest.Id,
				startDate = startDate,
				endDate = endDate,
				totalCost = timeStaying.Days * roomType.Price
			};

			_db.SaveData("dbo.spBookings_Insert", SaveBookingParameters, connectionStringName);
		}

		public List<FullBookingModel> SerachBookings(string lastName)
		{

			var bookings = _db.LoadData<FullBookingModel, dynamic>("dbo.spBookings_Search",
																   new { lastName, startDate = DateTime.Now.Date },
																   connectionStringName);

			return bookings;
		}

		public void CheckInGuest(int bookingId)
		{
			_db.SaveData("dbo.spBookings_CheckIn", new { Id = bookingId }, connectionStringName);

		}

		public RoomTypeModel GetRoomTypeById(int id)
		{
			return _db.LoadData<RoomTypeModel, dynamic>("dbo.spRoomTypes_GetById",
				 new { id },
				 connectionStringName).First();
		}
	}
}
