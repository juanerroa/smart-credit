CREATE PROCEDURE [dbo].[AddPayment]
    @CreditCardId UNIQUEIDENTIFIER,
    @Amount DECIMAL(18, 2),
    @Date DATETIME
AS
BEGIN
    BEGIN TRANSACTION
    BEGIN TRY

		DECLARE @TransactionId UNIQUEIDENTIFIER = NEWID();

        INSERT INTO [Transactions] (Id, CreditCardId, Description, Amount, Type, Date, CreatedAt)
        VALUES (@TransactionId, @CreditCardId, 'Pago de tarjeta', @Amount, 2, @Date, GETUTCDATE()); -- 2 para "Pago"

        UPDATE CreditCards
        SET 
            Balance = Balance - @Amount,
            AvailableBalance = AvailableBalance + @Amount
        WHERE Id = @CreditCardId;

        COMMIT TRANSACTION

		-- Retorna el registro insertado
        SELECT 
			t.Id AS Id,
			t.CreditCardId,
			t.Description,
			t.Amount,
			t.Type,
			t.Date,
			t.CreatedAt,
			t.UpdatedAt,
			t.DeletedAt
        FROM [Transactions] t
        WHERE Id = @TransactionId;

    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION
        THROW;
    END CATCH
END