CREATE PROCEDURE [dbo].[spBookings_CheckIn]
	@Id int
AS
BEGIN
	set nocount on;

	update dbo.Bookings
	set checkedIn = 1
	where Id = @Id;
END