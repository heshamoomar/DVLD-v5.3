using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Net;
using System.Security.Policy;
using System.Data;
using System.Reflection;
using System.Security.Cryptography;


namespace Data_Access_Layer
{
    public class UsersData
    {
        public static bool getUserInfoByUsername(string UserName, ref int UserID, ref int PersonID, ref string Password, ref bool IsActive)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(DatabaseMeta.connectionString);

            string query = "SELECT * FROM Users WHERE UserName = @UserName";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.Add("@UserName", SqlDbType.NVarChar).Value = UserName;

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    // The record was found
                    isFound = true;

                    UserID = (int)reader["UserID"];
                    PersonID = reader["PersonID"] != DBNull.Value ? (int)reader["PersonID"] : -1;
                    Password = (string)reader["Password"];
                    IsActive = (bool)reader["IsActive"];
                }
                else
                {
                    // The record was not found
                    isFound = false;
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                // Log or handle the error as needed
                isFound = false;
            }
            finally
            {
                connection.Close();
            }
            return isFound;
        }

        public static bool getUserInfoByUserID(int UserID, ref string UserName, ref int PersonID, ref string Password, ref bool IsActive)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(DatabaseMeta.connectionString);

            string query = "SELECT * FROM Users WHERE UserID = @UserID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.Add("@UserID", SqlDbType.NVarChar).Value = UserID;

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    // The record was found
                    isFound = true;

                    // personID could be null in case no person is assigned to this user 
                    UserName = (string)reader["UserName"];

                    //PersonID = reader["PersonID"] != DBNull.Value ? (int?)reader["PersonID"] : null;  // Nullable int handling

                    if (reader["PersonID"] == DBNull.Value)
                        PersonID = -1;
                    else
                        PersonID = (int)reader["PersonID"];


                    Password = (string)reader["Password"];
                    IsActive = (bool)reader["IsActive"];

                }
                else
                {
                    // The record was not found
                    isFound = false;
                }
                reader.Close();

            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
                isFound = false;
            }
            finally
            {
                connection.Close();
            }
            return isFound;
        }

        public static bool getUserInfoByUserNameAndPassword(string UserName, string Password, ref int UserID, ref int PersonID, ref bool IsActive)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(DatabaseMeta.connectionString);

            string query = "select* from Users where UserName= @UserName and Password=@Password";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.Add("@UserName", SqlDbType.NVarChar).Value = UserName;
            command.Parameters.Add("@Password", SqlDbType.NVarChar).Value = Password;

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    // The record was found
                    isFound = true;

                    UserID = (int)reader["UserID"];
                    PersonID = reader["PersonID"] != DBNull.Value ? (int)reader["PersonID"] : -1;
                    IsActive = (bool)reader["IsActive"];

                }
                else
                {
                    // The record was not found
                    isFound = false;
                }
                reader.Close();

            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
                isFound = false;
            }
            finally
            {
                connection.Close();
            }
            return isFound;
        }

        public static bool IsUserExists(string UserName)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(DatabaseMeta.connectionString);

            string query = "select found=1 from Users where UserName= @UserName";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.Add("@UserName", SqlDbType.NVarChar).Value = UserName;

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                isFound = reader.HasRows;

                reader.Close();

            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
                isFound = false;
            }
            finally
            {
                connection.Close();
            }
            return isFound;
        }

        public static int addUser(int PersonID, string UserName, string Password, bool IsActive)
        {
            Password = ComputeHash(Password);

            int UserID = -1;

            SqlConnection connection = new SqlConnection(DatabaseMeta.connectionString);

            string query = @"INSERT INTO Users (PersonID, UserName, Password, IsActive) " +
            "VALUES (@PersonID, @UserName, @Password, @IsActive) " +
            "SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@UserName", UserName);
            command.Parameters.AddWithValue("@Password", Password);
            command.Parameters.AddWithValue("@IsActive", IsActive);

            // Handle nullable fields

            // Handle PersonID
            if (String.IsNullOrEmpty(PersonID.ToString()) || PersonID == -1) // insert user that doesn't correspond to a person in database
                command.Parameters.AddWithValue("@PersonID", DBNull.Value);
            else
                command.Parameters.AddWithValue("@PersonID", PersonID);
            try
            {
                connection.Open();

                object result = command.ExecuteScalar();


                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    UserID = insertedID;
                }
            }

            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);

            }

            finally
            {
                connection.Close();
            }

            return UserID;
        }

        public static DataTable GetAllUsersInfo()
        {
            DataTable dtUsers = new DataTable();

            SqlConnection connection = new SqlConnection(DatabaseMeta.connectionString);

            //string query = "SELECT Users.UserID, Users.PersonID, " +
            //    "People.FirstName+ People.SecondName+ People.ThirdName+ People.LastName as FullName, " +
            //    "Users.UserName, Users.Password, Users.IsActive " +
            //    "FROM People INNER JOIN Users ON People.PersonID = Users.PersonID";

            string query = "select * from Users";

            SqlCommand command = new SqlCommand(query, connection);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    dtUsers.Load(reader);
                }
                else
                {

                }
                reader.Close();

            }
            catch (Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }

            return dtUsers;
        }

        public static bool DeleteUser(int UserID)
        {
            int rowsAffected = 0;

            SqlConnection connection = new SqlConnection(DatabaseMeta.connectionString);

            string query = @"DELETE FROM Users WHERE UserID = @UserID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@UserID", UserID);

            try
            {
                connection.Open();

                rowsAffected = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                // Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return (rowsAffected > 0);
        }

        public static bool UpdateUser(int UserID, int PersonID, string UserName, string Password, bool IsActive)
        {
            Password = ComputeHash(Password);

            int rowsAffected = 0;

            SqlConnection connection = new SqlConnection(DatabaseMeta.connectionString);

            string query = @"UPDATE Users
                           SET PersonID = @PersonID
                              ,UserName = @UserName
                              ,Password = @Password
                              ,IsActive = @IsActive
                               WHERE UserID = @UserID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@UserID", UserID);
            command.Parameters.AddWithValue("@UserName", UserName);
            command.Parameters.AddWithValue("@Password", Password);
            command.Parameters.AddWithValue("@IsActive", IsActive);

            // Handle nullable fields

            // Handle PersonID
            if (String.IsNullOrEmpty(PersonID.ToString()) || PersonID == -1)
                command.Parameters.AddWithValue("@PersonID", DBNull.Value);

            else
                command.Parameters.AddWithValue("@PersonID", PersonID);

            try
            {
                connection.Open();
                rowsAffected = command.ExecuteNonQuery();
            }

            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
                return false;
            }

            finally
            {
                connection.Close();
            }

            return (rowsAffected > 0);

        }

        static string ComputeHash(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                // Compute the hash value from the UTF-8 encoded input string
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));

                // Convert the byte array to a lowercase hexadecimal string
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }
    }
}
