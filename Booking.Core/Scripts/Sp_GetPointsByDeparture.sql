


CREATE  PROCEDURE [dbo].[Sp_GetPointsByDeparture]
	@departurePickupPointId uniqueidentifier,
	@PageIndex [INT] = 1,
	@PageSize [INT] = 10
AS
BEGIN
        select pt.*,
		ROW_NUMBER() OVER(ORDER BY pt.Id DESC) as RowNo,
		COUNT(*) OVER () TotalCount from  RoutePoint rtPt   join
        (select RouteId, PointId , OrderNo  From RoutePoint rp join Point pt on rp.PointId = pt.Id  where PointId = @departurePickupPointId
         and (pt.Isdeleted = 0 and rp.isDeleted = 0)  group by RouteId, PointId, OrderNo ) AllRoutes
        on rtPt.RouteId = AllRoutes.RouteID
        left join Point pt on rtPt.PointId = pt.ID
        where (rtPt.PointType =  2 or rtPt.PointType = 3) and (rtPt.IsDeleted = 0) and rtPt.OrderNo >  AllRoutes.OrderNo
        order by Longitude , Latitude 
   
   
        OFFSET (@PageIndex * @PageSize) ROWS FETCH NEXT @PageSize ROWS ONLY
END
