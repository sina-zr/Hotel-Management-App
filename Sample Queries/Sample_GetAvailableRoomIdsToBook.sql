declare @roomTypeId int;
declare @startDate date;
declare @endDate date;

set @startDate = '12/5/2023';
set @endDate = '12/10/2023';
set @roomTypeId = 2;

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