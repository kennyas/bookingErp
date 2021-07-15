  SELECT pt.*,
		ROW_NUMBER() OVER(ORDER BY pt.Id DESC) as RowNo,
		COUNT(*) OVER () TotalCount from  RoutePoint rtPt   join
        (select RouteId, PointId , OrderNo  From RoutePoint rp join Point pt on rp.PointId = pt.Id  where PointId = {0}
         and (pt.Isdeleted = 0 and rp.isDeleted = 0)  group by RouteId, PointId, OrderNo ) AllRoutes
        ON rtPt.RouteId = AllRoutes.RouteID
        left join Point pt on rtPt.PointId = pt.ID
        where (rtPt.PointType =  1 or rtPt.PointType = 2) and (rtPt.IsDeleted = 0) and rtPt.OrderNo <  AllRoutes.OrderNo
        order by Longitude , Latitude 
   
   
        OFFSET ({1} * {2}) ROWS FETCH NEXT {2} ROWS ONLY