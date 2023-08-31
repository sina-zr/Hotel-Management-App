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
			firstName.ToLower();
			lastName.ToLower();

			// if guest doesn't already exist in Guest Table, INSERT it
			// & anyway return an Id
			// we have to do its in multiple calls as well cause SQLite doesn't have if begin end.
			string query = @"select 1 from Guests where FirstName = @firstName and LastName = @lastName";
			int results = _db.LoadData<dynamic, dynamic>(query, new { firstName, lastName }, connectionStringName).Count();

			if (results == 0)
			{
				query = @"insert into Guests (FirstName, LastName) values (@firstName, @lastName);";
				_db.SaveData(query, new { firstName, lastName }, connectionStringName);
			}

			query = @"select [Id], [FirstName], [LastName]
					from Guests
					where FirstName = @firstName and LastName = @lastName LIMIT 1;";

			GuestModel guest = _db.LoadData<GuestModel, dynamic>(query,
														  new { firstName, lastName },
														  connectionStringName).First();

			RoomTypeModel roomType = _db.LoadData<RoomTypeModel, dynamic>("select * from RoomTypes where Id = @Id",
				new { Id = roomTypeId }, connectionStringName).First();

			TimeSpan timeStaying = endDate.Subtract(startDate);


			// Getting the available rooms
			query = @"select r.*
			from Rooms r
			inner join RoomTypes t on t.Id = r.RoomTypeId
			WHERE r.RoomTypeId = @roomTypeId
				AND r.Id NOT in (
				SELECT b.RoomId
				FROM Bookings b
				WHERE (@startDate < b.StartDate AND @endDate > b.EndDate)
					 OR (b.StartDate < @endDate AND @endDate < b.EndDate)
					 OR (b.StartDate <= @startDate AND @startDate < b.EndDate));";
			var LoadRoomsParameters = new { startDate, endDate, roomTypeId };
			List<RoomModel> availableRooms = _db.LoadData<RoomModel, dynamic>(query,
																			  LoadRoomsParameters,
																			  connectionStringName);

			//Saving the Booking
			query = @"insert into Bookings (RoomId, GuestId, StartDate, EndDate, TotalCost)
			values (@roomId, @guestId, @startDate, @endDate, @totalCost);";
			var SaveBookingParameters = new
			{
				roomId = availableRooms.First().Id,
				guestId = guest.Id,
				startDate = startDate.ToString("yyyy-MM-dd"),
				endDate = endDate.ToString("yyyy-MM-dd"),
				totalCost = timeStaying.Days * roomType.Price
			};

			_db.SaveData(query, SaveBookingParameters, connectionStringName);
		}

		public List<FullBookingModel> SerachBookings(string lastName)
		{
			lastName.ToLower();

			string query = @"
			select [b].[Id], [b].[RoomId], [b].[GuestId], [b].[StartDate], [b].[EndDate], [b].[checkedIn], [b].[TotalCost],
				g.FirstName, g.LastName,
				r.RoomNumber, r.RoomTypeId,
				[rt].[Title], [rt].[Description]
			from Bookings b
			inner join Guests g on g.Id = b.GuestId
			inner join Rooms r on r.Id = b.RoomId
			inner join RoomTypes rt on rt.Id = r.RoomTypeId
			where b.StartDate = @startDate AND g.LastName = @lastName;";


			var bookings = _db.LoadData<FullBookingModel, dynamic>(query,
																   new { lastName , startDate = DateTime.Now.Date.ToString("yyyy-MM-dd") },
																   connectionStringName);

			return bookings;
		}

		public void CheckInGuest(int bookingId)
		{
			_db.SaveData("dbo.spBookings_CheckIn", new { Id = bookingId }, connectionStringName);

		}

		public RoomTypeModel GetRoomTypeById(int id)
		{
			return _db.LoadData<RoomTypeModel, dynamic>("Select * from RoomTypes where Id = @id;",
				 new { id },
				 connectionStringName).First();
		}
	}
}
