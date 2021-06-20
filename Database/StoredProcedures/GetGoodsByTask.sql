CREATE PROCEDURE [dbo].[GetGoodsByTask]
	@taskId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	--SET NOCOUNT ON;

SELECT distinct
		SG.*, g1.BarCode as BoxName, d.*,
		IsNull(GR1.QuantityInLocation, 0) as QuantityInLocation,
		l.LocationId,
		l.LocationName,
		Case When bog.LocationId is null Then 0 else 1 end as IsDeliveryOrder,

		Case When IsNull(GR1.QuantityInLocation, 0) = 0 and bog.LocationId is null then 'На витрину' else 
		Case When IsNull(GR1.QuantityInLocation, 0) > 0 and bog.LocationId is null then 'Для хранения' else 
			Case When bog.LocationId is not null Then 'Клиентский' end 
		End
		End as DeliveryCategoryDesc -- разделение товара на 3 кучи (На витрину, Для хранения, Клиентский)

		FROM 
		Scaner_Goods as SG
		LEFT JOIN Defects d on d.Id = SG.DefectId
		left join Scaner_Goods g1 on g1.Id =SG.BoxId 

		inner join [dbo].[Tasks] as T with (nolock)
		on T.Id = SG.TaskId
		left join [dbo].[Scaner_1cDocData] as S with (nolock)   
		on (S.Article = SG.GoodArticle and S.PlanNum = T.PlanNum)
		left join [KAZ-FOLSNR].[WebProject].[dbo].[Locations] as L with (nolock)
		on L.LocationGuid = S.LocationGuid
		left join (
			Select SUM(GR.Quantity-CASE WHEN GR.Reserve<0 THEN 0 ELSE GR.Reserve END) as QuantityInLocation, GR.GoodId, GR.LocationId  
			from  [KAZ-FOLSNR].[WebProject].[dbo].GoodsRemains GR with (nolock) 
			where (gr.Quantity - CASE WHEN gr.Reserve<0 THEN 0 ELSE gr.Reserve END) > 0
			group by GR.GoodId, GR.LocationId
		) AS GR1
		on GR1.GoodId = SG.GoodId and GR1.LocationId = L.LocationId
		left join (Select distinct bog.GoodId, bog.LocationId 
		from [KAZ-FOLSNR].[WebProject].[dbo].[BasketOrderGoods] as bog with (nolock)
		left join [KAZ-FOLSNR].[WebProject].[dbo].BasketOrders as bo with (nolock)
		on bo.id = bog.BasketOrderId 
		where bo.SaleTypeId = 2) as bog --SaleTypeId = 2 - Покупка с доставкой
		on bog.GoodId = SG.GoodId and bog.LocationId = L.LocationId
		WHERE SG.TaskId = @taskId 
		order by SG.Created desc
END
