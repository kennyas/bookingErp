CREATE PROC Sp_GetCustomerWalletByMobileNo

@mobile_number nvarchar(50)

-- Sp_GetCustomerWalletByMobileNo '08062066852'

AS

SELECT Id WalletId, W.CustomerId, W.PhoneNumber, W.Fullname FROM CustomerWallet W
WHERE  W.PhoneNumber = @mobile_number