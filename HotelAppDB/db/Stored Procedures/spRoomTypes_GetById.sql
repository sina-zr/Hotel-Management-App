CREATE PROCEDURE [dbo].[spRoomTypes_GetById]
	@id int
AS
BEGIN
	set nocount on;

	Select * from dbo.RoomTypes where Id = @id;

END