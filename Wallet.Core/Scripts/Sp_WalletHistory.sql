CREATE PROCEDURE sp_WalletHistory_paginated

@customerId uniqueidentifier,
@PageIndex INT, 
@PageTotal INT, 
@GETTOTAL BIT,   
@StartDate nvarchar(50),
@EndDate nvarchar(50),
@TOTALRECORDS INT OUTPUT

AS

-- sp_WalletHistory_paginated '129712e3-9214-4dd3-9c03-cfc4eb9ba979',0,2,1,'2020/03/01','2020/03/04',0

BEGIN
    DECLARE @offset INT
    DECLARE @newsize INT
    DECLARE @sql nvarchar(MAX)
	DECLARE @sort nvarchar(50)
	DECLARE @WalletID uniqueidentifier

	SELECT @WalletID = Id FROM CustomerWallet WHERE CustomerId = @customerId

	SET @sort = 'CreatedOn'
    IF(@PageIndex=0)
      BEGIN
        SET @offset = @PageIndex
        SET @newsize = @PageTotal
       END
    ELSE 
      BEGIN
        SET @offset = @PageIndex * @PageTotal
        SET @newsize = @PageTotal - 1
      END
    SET NOCOUNT ON
    SET @sql = '
     WITH OrderedSet AS
    (
      SELECT *, ROW_NUMBER() OVER (ORDER BY ' + @sort + ') AS ''Index''
      FROM WalletHistory WHERE CONVERT(nvarchar(150), WalletId) = ' + '''' + CONVERT(nvarchar(150), @WalletID) + '''' + 
	  ' AND CreatedOn BETWEEN CONVERT(datetime2, ' + '''' + @StartDate + '''' + ')' +
			+ ' AND CONVERT(datetime2, ' + '''' + @EndDate + '''' + ')' +
    ')
   SELECT * FROM OrderedSet WHERE [Index] BETWEEN ' + CONVERT(NVARCHAR(12), @offset) + ' AND ' + CONVERT(NVARCHAR(12), (@offset + @newsize)) 
   EXEC (@sql)
   SET @TOTALRECORDS = (SELECT COUNT(*) FROM WalletHistory WHERE WalletId = @WalletID AND 
   CreatedOn BETWEEN CONVERT(datetime2, @StartDate) AND CONVERT(datetime2, @EndDate))
END