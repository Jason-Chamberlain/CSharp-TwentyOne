using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using Casino;
using Casino.TwentyOne;

namespace TwentyOne
{
    class Program
    {
        static void Main(string[] args)
        {
            const string CASINO_NAME = "Grand Hotel and Casino"; // The name of the casino

            // Welcome message and get player's name
            Console.WriteLine("Welcome to the {0}. \nLet's start by entering your name:", CASINO_NAME);
            string playerName = Console.ReadLine();

            // Check if user is an admin and display exception log
            if (playerName.ToLower() == "admin")
            {
                List<ExceptionEntity> Exceptions = ReadExceptions();
                foreach (var exception in Exceptions)
                {
                    Console.Write(exception.Id + " | ");
                    Console.Write(exception.ExceptionType + " | ");
                    Console.Write(exception.ExceptionMessage + " | ");
                    Console.Write(exception.TimeStamp);
                    Console.WriteLine();
                }
                Console.Read();
                return;
            }

            bool validAnswer = false;
            int bank = 0;

            // Get and validate the amount of money the player brought
            while (!validAnswer)
            {
                Console.WriteLine("And how much money did you bring today?");
                validAnswer = int.TryParse(Console.ReadLine(), out bank);
                if (!validAnswer) Console.WriteLine("Please enter digits only, no decimals.");
            }

            // Prompt the player to join a game of 21
            Console.WriteLine("Hello, {0}. Would you like to join a game of 21 right now?", playerName);
            string answer = Console.ReadLine().ToLower();

            // Start the game if the player wants to play
            if (answer == "yes" || answer == "y" || answer == "yeah" || answer == "ya")
            {
                // Create a new player
                Player player = new Player(playerName, bank);
                player.Id = Guid.NewGuid(); // Assign a unique ID to the player

                // Log the player's ID and timestamp
                using (StreamWriter file = new StreamWriter(@"..\Logs\log.txt", true))
                {
                    file.Write(DateTime.Now + ": ");
                    file.WriteLine(player.Id);
                }

                // Initialize the game and add the player to it
                Game game = new TwentyOneGame();
                game += player;
                player.isActivelyPlaying = true;

                // Play the game while the player wants to and has money
                while (player.isActivelyPlaying && player.Balance > 0)
                {
                    try
                    {
                        game.Play();
                    }
                    // Handle FraudException
                    catch (FraudException ex)
                    {
                        Console.WriteLine(ex.Message);
                        UpdateDbWithException(ex);
                        Console.ReadLine();
                        return;
                    }
                    // Handle other exceptions
                    catch (Exception ex)
                    {
                        Console.WriteLine("An error occurred. Please contact your System Administrator.");
                        UpdateDbWithException(ex);
                        Console.ReadLine();
                        return;
                    }
                }

                game -= player; // Remove the player from the game
                Console.WriteLine("Thank you for playing!"); // End of the game
            }

            Console.WriteLine("Feel free to look around the casino. Bye for now."); // Casino exit message
            Console.ReadLine(); // Wait for user input before exiting
        }

        // Method to update the database with information about the exception
        private static void UpdateDbWithException(Exception ex)
        {
            // Define the connection string to the database
            string connectionString = @"Data Source=(localdb)\ProjectsV13;Initial Catalog=TwentyOneGame;
                                Integrated Security=True;Connect Timeout=30;Encrypt=False;
                                TrustServerCertificate=False;ApplicationIntent=ReadWrite;
                                MultiSubnetFailover=False";

            // SQL query string to insert the exception information into the database
            string queryString = @"INSERT INTO Exceptions (ExceptionType, ExceptionMessage, TimeStamp) VALUES
                            (@ExceptionType, @ExceptionMessage, @TimeStamp)";

            // Create a connection to the database
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.Add("@ExceptionType", SqlDbType.VarChar);
                command.Parameters.Add("@ExceptionMessage", SqlDbType.VarChar);
                command.Parameters.Add("@TimeStamp", SqlDbType.DateTime);

                // Set parameter values based on the exception
                command.Parameters["@ExceptionType"].Value = ex.GetType().ToString();
                command.Parameters["@ExceptionMessage"].Value = ex.Message;
                command.Parameters["@TimeStamp"].Value = DateTime.Now;

                // Open the connection, execute the query, and close the connection
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        // Method to read exceptions from the database and return a list of ExceptionEntity objects
        private static List<ExceptionEntity> ReadExceptions()
        {
            // Define the connection string to the database
            string connectionString = @"Data Source=(localdb)\ProjectsV13;Initial Catalog=TwentyOneGame;
                                Integrated Security=True;Connect Timeout=30;Encrypt=False;
                                TrustServerCertificate=False;ApplicationIntent=ReadWrite;
                                MultiSubnetFailover=False";

            // SQL query string to select exception information from the database
            string queryString = @"SELECT Id, ExceptionType, ExceptionMessage, TimeStamp FROM Exceptions";

            List<ExceptionEntity> Exceptions = new List<ExceptionEntity>();

            // Create a connection to the database
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);

                connection.Open();

                // Execute the query and read the results
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    // Create an ExceptionEntity object and populate its properties
                    ExceptionEntity exception = new ExceptionEntity();
                    exception.Id = Convert.ToInt32(reader["Id"]);
                    exception.ExceptionType = reader["ExceptionType"].ToString();
                    exception.ExceptionMessage = reader["ExceptionMessage"].ToString();
                    exception.TimeStamp = Convert.ToDateTime(reader["TimeStamp"]);
                    Exceptions.Add(exception);
                }

                // Close the connection and return the list of exceptions
                connection.Close();
                return Exceptions;
            }
        }
    }
}