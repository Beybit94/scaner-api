CREATE PROCEDURE [dbo].[GetAddressLocationByTask]
	@taskId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	--SET NOCOUNT ON;

	select top 1 D.[Name] from Scaner_1cDocData  S with (nolock)
	inner join [KAZ-FOLSNR].[WebProject].[dbo].[Locations] as L with (nolock)
	on L.LocationGuid = S.LocationGuid
	inner join [KAZ-FOLSNR].[WebProject].[dbo].[Divisions] as D with (nolock)
	on D.Id = L.DivisionId
	inner join Tasks as T with (nolock)
	on S.PlanNum = T.PlanNum
	where T.Id = @taskId
END
