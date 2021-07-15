CREATE PROC [dbo].[Sp_WalletTransfer]

-- @Id uniqueidentifier,
@Amount decimal(18,2),
@DebitWalletId uniqueidentifier,
@CreditWalletId uniqueidentifier
,@CreatedOn datetime2(7)
,@ModifiedOn datetime2(7)
,@CreatedBy uniqueidentifier
,@ModifiedBy uniqueidentifier
,@PaymentReference nvarchar(50)
,@CustomerId uniqueidentifier
 ,@TransactionDesc nvarchar(max)
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
           ,[PaymentReference]
           ,[Amount]
           ,[WalletId]
          ,TransactionDesc
           ,[TransType]
		   ,Reference)  --I don't know what you called it.))
        VALUES
           (NEWID(),
            @CreatedOn
           ,@ModifiedOn
           ,@CreatedBy
           ,@ModifiedBy        
           ,0         
           ,@PaymentReference
           ,@Amount
           ,@DebitWalletId
           ,@TransactionDesc
           ,'DR'
		   ,@Reference)

		INSERT INTO [dbo].[WalletHistory]
           ([Id],
            [CreatedOn]
           ,[ModifiedOn]
           ,[CreatedBy]
           ,[ModifiedBy]       
           ,[IsDeleted]         
           ,[PaymentReference]
           ,[Amount]
           ,[WalletId]
           ,[TransactionDesc]
           ,[TransType]
		   ,Reference) 
        VALUES
           (NEWID(),
            @CreatedOn
           ,@ModifiedOn
           ,@CreatedBy
           ,@ModifiedBy          
           ,0          
           ,@PaymentReference
           ,@Amount
           ,@CreditWalletId
            ,@TransactionDesc
           ,'CR'
		   ,@Reference)

	    ---SET @RECID = SCOPE_IDENTITY()                  
        if(@@ERROR=0)
          begin
            commit Tran
            UPDATE CustomerWallet SET AvailableBalance = AvailableBalance - @Amount , LedgerBalance = LedgerBalance - @Amount,
			ModifiedBy = @ModifiedBy, ModifiedOn = @ModifiedOn WHERE CustomerId = @CustomerId AND Id = @DebitWalletId
		   IF(@@ROWCOUNT > 0)
		   BEGIN
			 UPDATE CustomerWallet SET AvailableBalance = AvailableBalance + @Amount , LedgerBalance = LedgerBalance + @Amount,
			 ModifiedBy = @ModifiedBy, ModifiedOn = @ModifiedOn WHERE Id = @CreditWalletId

			 SELECT * FROM CustomerWallet WHERE  CustomerId = @CustomerId AND Id = @DebitWalletId
		   END
          end
        else
          begin
            rollback Tran
            SELECT * FROM CustomerWallet WHERE CustomerId is null
          end
    end
GO