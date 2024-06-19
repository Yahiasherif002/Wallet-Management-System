using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;



internal class Program
{
    private static void Main(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsetting.json")
            .Build();

        SqlConnection connection = new SqlConnection(configuration.GetSection("constr").Value);

        Console.ForegroundColor= ConsoleColor.Green;

        while (true)
        {
            Console.WriteLine("Select an option:");
            Console.WriteLine("1. Add User");
            Console.WriteLine("2. Update User");
            Console.WriteLine("3. Delete User");
            Console.WriteLine("4. View All Users");
            Console.WriteLine("5. Transfer Money");
            Console.WriteLine("6. Exit");

            var choice = Console.ReadLine();
            if (choice == null) return;
            switch (choice)
            {
                case "1":
                    addWallet(connection);
                    break;
                case "2":
                    updateWallet(connection);
                    break;
                case "3":
                    deleteWallet(connection);
                    break;
                case "4":
                    getWallets(connection);
                    break;
                case "5":
                    TransferMoney(connection);
                    break;
                case "6":
                    return;
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }

        }




        }

    public static void getWallets(SqlConnection connection)
    {
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        SqlCommand cmd = new SqlCommand("GetAllWallets", connection);
        cmd.CommandType = CommandType.StoredProcedure;
        
        connection.Open();
        SqlDataReader reader = cmd.ExecuteReader();

       
        while (reader.Read())
        {
            Wallet wallet = new Wallet
            {
                Id = reader.GetInt32("Id"),
                Holder = reader.GetString("Holder"),
                Balance = reader.GetDecimal("Balance")
            };
            Console.WriteLine(wallet);
            Console.WriteLine("========================================");
        }
        connection.Close();
        Console.ForegroundColor = ConsoleColor.Green;
    }

    public static void addWallet(SqlConnection connection)
    {
        Console.ForegroundColor = ConsoleColor.DarkBlue;
        Console.WriteLine("Enter the wallet holder name: ");
        string holder = Console.ReadLine();

        Console.WriteLine("Enter the balance: ");
        decimal balance = decimal.Parse(Console.ReadLine());

        using (var cmd = new SqlCommand("AddWallet", connection))
        {
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Holder", holder);
            cmd.Parameters.AddWithValue("@Balance", balance);

            connection.Open();
            cmd.ExecuteNonQuery();
            connection.Close();
            Console.WriteLine($"{holder}'s wallet added successfully");
            Console.WriteLine("========================================");


        }
        Console.ForegroundColor = ConsoleColor.Green;
    }

    public static void updateWallet(SqlConnection connection)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;

        Console.WriteLine("Enter the wallet id that u want to change :");
        int id = int.Parse(Console.ReadLine());

        Console.WriteLine("Enter the new wallet name :");
        string holder = Console.ReadLine();

        Console.WriteLine("Enter the new wallet balance :");
        decimal balance = decimal.Parse(Console.ReadLine());




        using (var cmd = new SqlCommand("UpdateWallet", connection))
        {
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Id", id);
            cmd.Parameters.AddWithValue("@Holder", holder);
            cmd.Parameters.AddWithValue("@Balance", balance);

            connection.Open();
            cmd.ExecuteNonQuery();
            connection.Close();

            Console.WriteLine("Wallet updated successfly");
            Console.WriteLine("========================================");


        }
        Console.ForegroundColor = ConsoleColor.Green;
    }

    public static void deleteWallet(SqlConnection connection)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Enter the wallet id that you want to delete : ");
        int id = int.Parse(Console.ReadLine());
        using(var cmd = new SqlCommand("DeleteWallet", connection))
        {
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Id", id);

            connection.Open();
            cmd.ExecuteNonQuery();
            connection.Close();

            Console.WriteLine("User deleted successfully.");
            Console.WriteLine("========================================");
        }
        Console.ForegroundColor = ConsoleColor.Green;
    }

    public static void TransferMoney(SqlConnection connection)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write ("Enter the source wallet id: ");
        int sourceId;
        while (!int.TryParse(Console.ReadLine(), out sourceId))
        {
            Console.WriteLine("Invalid input. Please enter a valid integer value for the source wallet id.");
        }

        Console.Write("Enter the destination wallet id: ");
        int destinationId;
        while (!int.TryParse(Console.ReadLine(), out destinationId))
        {
            Console.WriteLine("Invalid input. Please enter a valid integer value for the destination wallet id.");
        }

        Console.Write ("Enter the amount to transfer: ");
        decimal amount;
        while (!decimal.TryParse(Console.ReadLine(), out amount))
        {
            Console.WriteLine("Invalid input. Please enter a valid decimal value for the amount.");
        }
        try
        {
            using (var cmd = new SqlCommand("TransferMoney", connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SourceWalletId", sourceId);
                cmd.Parameters.AddWithValue("@DestinationWalletId", destinationId);
                cmd.Parameters.AddWithValue("@Amount", amount);

                connection.Open();
                cmd.ExecuteNonQuery();
                Console.WriteLine("Money transferred successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        finally
        {
            connection.Close();
            Console.ForegroundColor = ConsoleColor.Green;
        }
    }
}