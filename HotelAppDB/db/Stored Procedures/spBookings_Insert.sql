CREATE PROCEDURE [dbo].[spBookings_Insert]
	@roomId int,
	@guestID int,
	@startDate date,
	@endDate date,
	@totalCost money
AS

BEGIN
	set nocount on;

	insert into dbo.Bookings (RoomId, GuestId, EndDate, TotalCost)
	values (@roomId, @guestID, @startDate, @endDate, @totalCost);

END