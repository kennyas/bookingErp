


DECLARE @g geography = 'POINT(' + cast({4} as nvarchar) + ' ' + cast({3} as nvarchar) + ')';

SELECT distinct p.*, 
       cast('POINT(' + cast(Longitude as nvarchar) + ' ' + cast(Latitude as nvarchar) + ')' as geography).STDistance(@g) As Cordinate
FROM RoutePoint rp join Point p on rp.PointId = p.Id join Area area on p.AreaId = area.Id
where (rp.PointType = 1 or rp.PointType = 2)
and ({0} is null or (({0} like '%' + p.Title + '%') or ({0} like '%' + area.title + '%')))
 and p.IsDeleted = 0
ORDER BY Cordinate ASC
 OFFSET ({1} * {2}) ROWS FETCH NEXT {2} ROWS ONLY

