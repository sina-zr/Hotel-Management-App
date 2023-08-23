CREATE PROCEDURE [dbo].[spBookings_Search]
	@lastName nvarchar(50),
	@startDate date
AS
BEGIN
	set nocount on;

	select [b].[Id], [b].[RoomId], [b].[GuestId], [b].[StartDate], [b].[EndDate], [b].[checkedIn], [b].[TotalCost],
		g.FirstName, g.LastName,
		r.RoomNumber, r.RoomTypeId,
		[rt].[Title], [rt].[Description], [rt].[Price]
	from dbo.Bookings b
	inner join dbo.Guests g on g.Id = b.GuestId
	inner join dbo.Rooms r on r.Id = b.RoomId
	inner join dbo.RoomTypes rt on rt.Id = r.RoomTypeId
	where b.StartDate = @startDate AND g.LastName = @lastName;

END
