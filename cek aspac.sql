Declare @Product	VarChar(20),
	@WrhsType	VarChar(20),
	@Year		Int,
	@Month		Int

SELECT @Product = 'SF05', @WrhsType = 'Production', @Year = 2015, @month = 1

Declare @Start	DateTime,
	@End	DateTime
EXEC S_getdateStart @Year, @Month, @Start OUT
EXEC S_getdateEnd @Year, @Month, @End OUT

Select W.WrhsType, (A.TotalIn + A.TotalOut) / (A.QtyIN + A.QtyOut) AS Price, A.* 
from V_STStockAllDetail A INNER JOIN MsWarehouse W ON A.Warehouse = W.WrhsCode
WHERE Product = @Product AND W.WrhsType = @WrhsType AND A.TransDate BETWEEN @Start AND @End
Order by transdate

Select W.WrhsType, SUM(QtyIn - QtyOut), SUM(TotalIn - TotalOut), CASE WHEN SUM(QtyIn - QtyOut) = 0 THEN 0 ELSE SUM(TotalIn - TotalOut)  / SUM(QtyIn - QtyOut) END Price
from V_STStockAllDetail A INNER JOIN MsWarehouse W ON A.Warehouse = W.WrhsCode
WHERE Product = @Product AND W.WrhsType = @WrhsType AND A.TransDate < @Start
Group By W.WrhsType

Select W.WrhsType, W.WorkCtr, SUM(QtyIn - QtyOut), SUM(TotalIn - TotalOut),  CASE WHEN SUM(QtyIn - QtyOut) = 0 THEN 0 ELSE SUM(TotalIn - TotalOut)  / SUM(QtyIn - QtyOut) END Price
from V_STStockAllDetail A INNER JOIN MsWarehouse W ON A.Warehouse = W.WrhsCode
WHERE Product = @Product AND W.WrhsType = 'Owner' AND A.TransDate < @Start
Group By W.WrhsType, W.WorkCtr 

Select W.WrhsType, W.WorkCtr, SUM(QtyIn - QtyOut), SUM(TotalIn - TotalOut)  
from V_STStockAllDetail A INNER JOIN MsWarehouse W ON A.Warehouse = W.WrhsCode
WHERE Product = @Product AND W.WrhsType = @WrhsType AND A.TransDate < @Start
Group By W.WrhsType, W.WorkCtr 