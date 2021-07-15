 CREATE PROC [dbo].[Sp_WalletDebitCredit]


@Amount decimal(18,2),
@WalletId uniqueidentifier
,@CreatedOn datetime2(7)
,@CreatedBy uniqueidentifier
,@ModifiedBy uniqueidentifier
,@PaymentReference nvarchar(50)
,@CustomerId uniqueidentifier
 ,@TransactionDesc nvarchar(max)
 ,@TransType nvarchar(10)
 ,@ModifiedOn datetime2(7)
 ,@NewBalance decimal(18,2)
 ,@Reference nvarchar(50)

AS
 
 Begin
      Begin tran

        INSERT INTO [dbo].[WalletHistory]
           ([Id],
           [CreatedOn]
           ,[ModifiedOn]
           ,[CreatedBy]
          ,[ModifiedBy]
            ,[IsDeleted]
           ,[Reference]
           ,[Amount]
           ,[WalletId]
          ,TransactionDesc
           ,[TransType], 
           [PaymentReference])
        VALUES
           (NEWID(),
            @CreatedOn
           ,@ModifiedOn
           ,@CreatedBy
           ,@ModifiedBy       
           ,0        
           ,@Reference
           ,@Amount
           ,@WalletId
           ,@TransactionDesc
           ,@TransType, 
            @PaymentReference)
	               
        if(@@ERROR=0)
          begin
            commit Tran
            UPDATE CustomerWallet SET AvailableBalance = @newBalance , LedgerBalance = @newBalance, ModifiedBy = @ModifiedBy, ModifiedOn = @ModifiedOn
     WHERE CustomerId = @CustomerId AND Id = @WalletId
	   IF(@@ROWCOUNT > 0)
	   BEGIN
		SELECT * FROM CustomerWallet WHERE CustomerId = @CustomerId AND Id = @WalletId	 
	   END
          end
        else
          begin
            rollback Tran
            SELECT * FROM CustomerWallet WHERE CustomerId is null
          end
    end
GO


