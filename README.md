# Wallet Management System

A simple console-based wallet management system built with C#. This application allows you to manage wallets and perform transactions such as adding, updating, deleting, viewing, and transferring money between wallets.

## Features

- **Add Wallet**: Create a new wallet with a holder name and balance.
- **Update Wallet**: Modify an existing wallet's holder name and balance.
- **Delete Wallet**: Remove an existing wallet from the database.
- **View All Wallets**: Display a list of all wallets.
- **Transfer Money**: Transfer money from one wallet to another with transactional integrity.

## Technologies Used

- **C#**
- **.NET Core**
- **SQL Server**
- **Microsoft.Data.SqlClient**
- **Microsoft.Extensions.Configuration**

## Prerequisites

- .NET Core SDK
- SQL Server
- A SQL Server database with the necessary tables and stored procedures

## Setup and Installation

1. **Clone the Repository**

    ```bash
    git clone https://github.com/yourusername/wallet-management-system.git
    cd wallet-management-system
    ```

2. **Set Up the Database**

    - Create a SQL Server database.
    - Create the `Wallets` table:
        ```sql
        CREATE TABLE Wallets (
            Id INT PRIMARY KEY IDENTITY,
            Holder NVARCHAR(100),
            Balance DECIMAL(18, 2)
        );
        ```

    - Create the stored procedures:
        ```sql
        CREATE PROCEDURE GetAllWallets
        AS
        BEGIN
            SET NOCOUNT ON;
            SELECT * FROM Wallets;
        END;
        ```

        ```sql
        CREATE PROCEDURE AddWallet
            @Holder NVARCHAR(100),
            @Balance DECIMAL(18, 2)
        AS
        BEGIN
            SET NOCOUNT ON;
            INSERT INTO Wallets (Holder, Balance)
            VALUES (@Holder, @Balance);
        END;
        ```

        ```sql
        CREATE PROCEDURE UpdateWallet
            @Id INT,
            @Holder NVARCHAR(100),
            @Balance DECIMAL(18, 2)
        AS
        BEGIN
            SET NOCOUNT ON;
            UPDATE Wallets
            SET Holder = @Holder, Balance = @Balance
            WHERE Id = @Id;
        END;
        ```

        ```sql
        CREATE PROCEDURE DeleteWallet
            @Id INT
        AS
        BEGIN
            SET NOCOUNT ON;
            DELETE FROM Wallets
            WHERE Id = @Id;
        END;
        ```

        ```sql
        CREATE PROCEDURE TransferMoney
            @SourceWalletId INT,
            @DestinationWalletId INT,
            @Amount DECIMAL(18, 2)
        AS
        BEGIN
            SET NOCOUNT ON;

            BEGIN TRY
                BEGIN TRANSACTION;

                DECLARE @SourceBalance DECIMAL(18, 2);
                SELECT @SourceBalance = Balance FROM Wallets WHERE Id = @SourceWalletId;

                IF @SourceBalance < @Amount
                BEGIN
                    THROW 50000, 'Insufficient balance in source wallet.', 1;
                END

                UPDATE Wallets
                SET Balance = Balance - @Amount
                WHERE Id = @SourceWalletId;

                UPDATE Wallets
                SET Balance = Balance + @Amount
                WHERE Id = @DestinationWalletId;

                COMMIT TRANSACTION;
            END TRY
            BEGIN CATCH
                ROLLBACK TRANSACTION;
                THROW;
            END CATCH
        END;
        ```

3. **Configure the Application**

    - Update the `appsettings.json` file with your database connection string:
        ```json
        {
            "constr": "YourConnectionStringHere"
        }
        ```

4. **Run the Application**

    ```bash
    dotnet run
    ```

## Usage

Follow the on-screen instructions to add, update, delete, view, or transfer money between wallets.

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for more details.

## Contributing

Contributions are welcome! Please open an issue or submit a pull request.

## Contact

For any questions or feedback, please contact [yahyasheriif@gmail.com](mailto:yahyasheriif@gmail.com).
