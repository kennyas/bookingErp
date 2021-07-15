-- CREATE FUNCTION [dbo].[Getpointinroute] (@pointId   UNIQUEIDENTIFIER,
--                                        @pointType INT,
--                                        @routeId   UNIQUEIDENTIFIER)
--returns @point TABLE (
--  Id        UNIQUEIDENTIFIER,
--  PointId   UNIQUEIDENTIFIER,
--  Title     NVARCHAR (500),
--  Longitude DECIMAL(8, 2),
--  Latitude  DECIMAL(8, 2),
--  OrderNo   INT,
--  PointType INT)
--AS
--  BEGIN
--      INSERT INTO @point
--      SELECT rp.id,
--             p.id pointId,
--             p.title,
--             p.longitude,
--             p.latitude,
--             rp.orderno,
--             rp.pointtype
--      FROM   point p
--             JOIN routepoint rp
--               ON ( rp.pointid = p.id
--                    AND rp.routeid = @routeId )
--      WHERE  p.id = @pointId
--             AND ( rp.pointtype = @pointType
--                    OR rp.pointtype = 3 )
--      ORDER  BY orderno

--      RETURN
--  END

CREATE PROCEDURE [dbo].[Sp_GetRouteWithPoints] (
@pickUPpointId UNIQUEIDENTIFIER,
@destinationPointId  UNIQUEIDENTIFIER) 
AS 
  BEGIN 
      SELECT pp.routeid, 
             pp.id      PickUpPointId, 
             dp.id      DestinationPointId, 
             pp.orderno PickupPointOrderNo, 
             dp.orderno DestinationOrderNo 
      FROM   routepoint pp 
             LEFT JOIN routepoint dp 
                    ON ( dp.routeid = pp.routeid ) 
      WHERE  (( pp.pointtype = 1 OR pp.pointtype = 3 ) 
               AND pp.pointid = @pickUPpointId ) 
             AND (( dp.pointtype = 2 OR dp.pointtype = 3 ) 
                   AND dp.pointid = @destinationPointId ) 
             AND pp.orderno < dp.orderno
             AND dp.routeid = pp.routeid
  END 