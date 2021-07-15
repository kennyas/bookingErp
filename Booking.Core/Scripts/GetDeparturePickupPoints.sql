select distinct pt.* from RoutePoint routePt 
join Point pt on routePt.PointId = pt.Id where (routePt.PointType = 1 or routePt.PointType = 2)
and pt.IsDeleted = 0 and ({0} is null or pt.AreaId = {0})
order by pt.CreatedOn desc

 OFFSET ({1} * {2}) ROWS FETCH NEXT {2} ROWS ONLY