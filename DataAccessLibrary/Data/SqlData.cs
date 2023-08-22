﻿using DataAccessLibrary.DataBase;
using DataAccessLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary.Data
{
    public class SqlData
    {
        private readonly ISqlDataAccess _db;
        private const string connectionStringName = "SqlDb";

        public SqlData(ISqlDataAccess db)
        {
            _db = db;
        }

        public List<RoomTypeModel> GetAvailableRoomTypes(DateTime startDate, DateTime endDate)
        {

            var parameters = new { startDate, endDate };

            return _db.LoadData<RoomTypeModel, dynamic>("dbo.spRoomTypes_GetAvailableRoomTypes",
                                                        parameters,
                                                        connectionStringName,
                                                        true);
        }

        public void BookARoom(string firstName, string lastName, DateTime startDate, DateTime endDate, int roomTypeId)
        {
            // if guest doesn't already exist in Guest Table, INSERT it
            // & anyway return an Id
            var guest = _db.LoadData<GuestModel, dynamic>("dbo.spGuest_Insert",
                                                          new { firstName, lastName },
                                                          connectionStringName,
                                                          true).First();

            RoomTypeModel roomType = _db.LoadData<RoomTypeModel, dynamic>("select * from dbo.RoomTypes where Id = @Id",
                new { Id = roomTypeId }, connectionStringName, false).First();

            TimeSpan timeStaying = endDate.Subtract(startDate);
            

            // Getting the available rooms
            var LoadRoomsParameters = new { startDate, endDate, roomTypeId };
            List<RoomModel> availableRooms = _db.LoadData<RoomModel, dynamic>("dbo.spRooms_GetAvailableRooms",
                                                                              LoadRoomsParameters,
                                                                              connectionStringName,
                                                                              true);

            //Saving the Booking
            var SaveBookingParameters = new { roomId = availableRooms.First().Id,
                startDate = startDate,
                endDate = endDate,
                totalCost = timeStaying.Days * roomType.Price };

            _db.SaveData("dbo.spBookings_Insert", SaveBookingParameters, connectionStringName, true);
        }
    }
}