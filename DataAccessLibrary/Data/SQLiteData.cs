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
        public void BookARoom(string firstName, string lastName, DateTime startDate, DateTime endDate, int roomTypeId)
        {
            throw new NotImplementedException();
        }

        public void CheckInGuest(int bookingId)
        {
            throw new NotImplementedException();
        }

        public List<RoomTypeModel> GetAvailableRoomTypes(DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }

        public RoomTypeModel GetRoomTypeById(int id)
        {
            throw new NotImplementedException();
        }

        public List<FullBookingModel> SerachBookings(string lastName)
        {
            throw new NotImplementedException();
        }
    }
}
