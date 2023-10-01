CREATE PROCEDURE [dbo].[spBookings_CheckIn]
	@Id int
AS
BEGIN
	set nocount on;

	update dbo.Bookings
	set CheckedIn = 1
	where Id = @Id;
END