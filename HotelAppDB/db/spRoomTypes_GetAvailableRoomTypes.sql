CREATE PROCEDURE [dbo].[spRoomTypes_GetAvailableRoomTypes]
	@startDate date,
	@endDate date
AS
BEGIN
	set nocount on;

	SELECT t.Id, t.Title, t.Description, t.Price
	FROM dbo.Rooms r 
	inner join dbo.RoomTypes t on t.Id = r.RoomTypeId
	WHERE r.Id NOT in (
		SELECT b.RoomId
		FROM dbo.Bookings b
		WHERE (@startDate < b.StartDate AND @endDate > b.EndDate)
			 OR (b.StartDate < @endDate AND @endDate < b.EndDate)
			 OR (b.StartDate <= @startDate AND @startDate < b.EndDate))
		group by t.Id, t.Title, t.Description, t.Price

END
