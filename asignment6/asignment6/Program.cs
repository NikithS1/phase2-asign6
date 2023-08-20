using System;
using System.Data.SqlClient;

namespace ProductDetails
{
    class Program
    {
        static string connectionString = "Server=LAPTOP-DLI2FL88;database=ProductInventoryDB;trusted_connection=true";

        static void Main(string[] args)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                bool continueProgram = true;
                while (continueProgram)
                {
                    Console.WriteLine("\nSelect an option:");
                    Console.WriteLine("1. View Product Inventory");
                    Console.WriteLine("2. Add New Product");
                    Console.WriteLine("3. Update Product Quantity");
                    Console.WriteLine("4. Remove Product");
                    Console.WriteLine("5. Exit");

                    int choice = Convert.ToInt32(Console.ReadLine());

                    switch (choice)
                    {
                        case 1:
                            ViewProductInventory(connection);
                            break;
                        case 2:
                            AddNewProduct(connection);
                            break;
                        case 3:
                            UpdateProductQuantity(connection);
                            break;
                        case 4:
                            RemoveProduct(connection);
                            break;
                        case 5:
                            continueProgram = false;
                            break;
                        default:
                            Console.WriteLine("Invalid choice.");
                            break;
                    }
                }

                connection.Close();
            }
        }

        static void ViewProductInventory(SqlConnection connection)
        {
            Console.WriteLine("Product Inventory:");
            string query = "SELECT * FROM Products";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine($"ID: {reader["Productid"]}, Name: {reader["ProductName"]}, Price: {reader["Price"]}, Quantity: {reader["Quantity"]}");
                    }
                }
            }
        }

        static void AddNewProduct(SqlConnection connection)
        {
            Console.WriteLine("Enter new product details:");
            Console.Write("Name: ");
            string productName = Console.ReadLine();
            Console.Write("Price: ");
            decimal price = Convert.ToDecimal(Console.ReadLine());
            Console.Write("Quantity: ");
            int quantity = Convert.ToInt32(Console.ReadLine());

            string insertQuery = "INSERT INTO Products (ProductName, Price, Quantity) VALUES (@ProductName, @Price, @Quantity)";

            using (SqlCommand command = new SqlCommand(insertQuery, connection))
            {
                command.Parameters.AddWithValue("@ProductName", productName);
                command.Parameters.AddWithValue("@Price", price);
                command.Parameters.AddWithValue("@Quantity", quantity);

                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    Console.WriteLine("New product added successfully.");
                }
                else
                {
                    Console.WriteLine("Failed to add new product.");
                }
            }
        }



        static void UpdateProductQuantity(SqlConnection connection)
        {
            Console.Write("Enter Product ID to update quantity: ");
            int productId = Convert.ToInt32(Console.ReadLine());
            Console.Write("Enter new Quantity: ");
            int newQuantity = Convert.ToInt32(Console.ReadLine());

            string updateQuery = "UPDATE Products SET Quantity = @NewQuantity WHERE Productid = @ProductId";

            using (SqlCommand command = new SqlCommand(updateQuery, connection))
            {
                command.Parameters.AddWithValue("@NewQuantity", newQuantity);
                command.Parameters.AddWithValue("@ProductId", productId);

                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    Console.WriteLine("Product quantity updated successfully.");
                }
                else
                {
                    Console.WriteLine("Failed to update product quantity.");
                }
            }
        }

        static void RemoveProduct(SqlConnection connection)
        {
            Console.Write("Enter Product ID to remove: ");
            int productId = Convert.ToInt32(Console.ReadLine());

            string deleteQuery = "DELETE FROM Products WHERE Productid = @ProductId";

            using (SqlCommand command = new SqlCommand(deleteQuery, connection))
            {
                command.Parameters.AddWithValue("@ProductId", productId);

                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    Console.WriteLine("Product removed successfully.");
                }
                else
                {
                    Console.WriteLine("Failed to remove product.");
                }
            }
        }
    }
}