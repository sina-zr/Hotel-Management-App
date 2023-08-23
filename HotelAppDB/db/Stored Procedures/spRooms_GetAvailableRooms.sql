CREATE PROCEDURE [dbo].[spRooms_GetAvailableRooms]
	@startDate date,
	@endDate date,
	@roomTypeId int
AS

BEGIN
	set nocount on;

	select r.*
	from dbo.Rooms r
	inner join dbo.RoomTypes t on t.Id = r.RoomTypeId
	WHERE r.RoomTypeId = @roomTypeId
		AND r.Id NOT in (
		SELECT b.RoomId
		FROM dbo.Bookings b
		WHERE (@startDate < b.StartDate AND @endDate > b.EndDate)
			 OR (b.StartDate < @endDate AND @endDate < b.EndDate)
			 OR (b.StartDate <= @startDate AND @startDate < b.EndDate));
END