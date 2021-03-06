using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework_1_4
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = "Server=.\\SQLEXPRESS; Database=Minions; Trusted_Connection=True";
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            string minionInput = Console.ReadLine();
            string villianInput = Console.ReadLine();

            string[] minionData = minionInput.Split(':')[1].Trim().Split(' ');
            string minionName = minionData[0];
            int minionAge = int.Parse(minionData[1]);
            string townName = minionData[2];

            string villianName = villianInput.Split(':')[1].Trim();

            using (connection)
            {
                if (!CheckIfTownExists(townName, connection))
                {
                    AddTown(townName, connection);
                    Console.WriteLine($"Town {townName} was added to the database.");
                }

                if (!CheckIfVillianExists(villianName, connection))
                {
                    AddVillian(villianName, connection);
                    Console.WriteLine($"Villian {villianName} was added to the database.");
                }

                int townId = GetTownByName(townName, connection);
                AddMinion(minionName, minionAge, townId, connection);

                int minionId = GetMinionByName(minionName, connection);
                int villianId = GetVillianByName(villianName, connection);

                AddMinionToVillian(minionId, villianId, connection);
                Console.WriteLine($"Successfully added {minionName} to be minion of {villianName}.");
            }
        }

        private static int GetTownByName(string townName, SqlConnection connection)
        {
            int townId = 0;

            string commandString = "SELECT TownId FROM Towns WHERE Name = @townName";
            SqlCommand command = new SqlCommand(commandString, connection);
            command.Parameters.AddWithValue(@townName, townName);
            townId = (int) command.ExecuteScalar();

            return townId;
        }

        private static void AddMinionToVillian(int minionId, int villianId, SqlConnection connection)
        {
            string commandString = "INSERT INTO MinionsVillians VALUES (@minionId, @villianId)";
            SqlCommand command = new SqlCommand(commandString, connection);
            command.Parameters.AddWithValue("@minionId", minionId);
            command.Parameters.AddWithValue("@villianId", villianId);

            command.ExecuteNonQuery();

        }

        private static int GetVillianByName(string villianName, SqlConnection connection)
        {
            int villianId = 0;

            string commandString = "SELECT VillianId FROM Villians WHERE Name = @villianName";
            SqlCommand command = new SqlCommand(commandString, connection);
            command.Parameters.AddWithValue("@villianName", villianName);
            villianId = (int)command.ExecuteScalar();

            return villianId;
        }

        private static int GetMinionByName(string minionName, SqlConnection connection)
        {
            int minionId = 0;

            string commandString = "SELECT MinionId FROM Minions WHERE Name = @minionName";
            SqlCommand command = new SqlCommand(commandString, connection);
            command.Parameters.AddWithValue("@minionName", minionName);
            minionId = (int)command.ExecuteScalar();

            return minionId;
        }

        private static void AddMinion(string minionName,int age,  int townId, SqlConnection connection)
        {
            string commandString = "INSERT INTO Minions(Name, Age, TownId) VALUES (@minionName, @minionAge, @townId)";
            SqlCommand command = new SqlCommand(commandString, connection);
            command.Parameters.AddWithValue("@minionName", minionName);
            command.Parameters.AddWithValue("@minionAge", age);
            command.Parameters.AddWithValue("@townId", townId);

            command.ExecuteNonQuery();
        }

        private static void AddVillian(string villianName, SqlConnection connection)
        {
            string commandString = "INSERT INTO Villians(Name, EvilnessFactor) VALUES(@villianName, 'evil')";
            SqlCommand command = new SqlCommand(commandString, connection);
            command.Parameters.AddWithValue("@villianName", villianName);

            command.ExecuteNonQuery();

        }

        private static bool CheckIfVillianExists(string villianName, SqlConnection connection)
        {
            string commandString = "SELECT COUNT(*) FROM Villians WHERE [Name] = @villianName";
            SqlCommand command = new SqlCommand(commandString, connection);
            command.Parameters.AddWithValue("@villianName", villianName);

            if ((int)command.ExecuteScalar() == 0)
            {
                return false;
            }
            return true;
        }

        private static void AddTown(string townName, SqlConnection connection)
        {
            string commandString = "INSERT INTO Towns(Name) VALUES(@townName)";
            SqlCommand command = new SqlCommand(commandString, connection);
            command.Parameters.AddWithValue("@townName", townName);

            command.ExecuteNonQuery();
        }

        private static bool CheckIfTownExists(string townName, SqlConnection connection)
        {
            string commandString = "SELECT COUNT(*) FROM Towns WHERE [Name] = @townName";
            SqlCommand command = new SqlCommand(commandString, connection);
            command.Parameters.AddWithValue("@townName", townName);

            if ((int)command.ExecuteScalar() == 0)
            {
                return false;
            }
            return true;
        }
    }
}
