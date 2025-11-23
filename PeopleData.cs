using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Security.Policy;

namespace Data_Access_Layer
{
    public class PeopleData
    {
        public static DataTable GetAllPeopleInfo()
        {
            DataTable dtPeople = new DataTable();

            SqlConnection connection = new SqlConnection(DatabaseMeta.connectionString);

            string query = "SELECT People.PersonID, People.NationalNo, People.FirstName, People.SecondName, " +
                "People.ThirdName, People.LastName, People.Gender, People.DateOfBirth, " +
                "Countries.CountryName as Nationality, People.Phone, People.Email " +
                "FROM People " +
                "INNER JOIN Countries ON People.NationalityCountryID = Countries.CountryID";

            SqlCommand command = new SqlCommand(query, connection);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    dtPeople.Load(reader);
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

            return dtPeople;
        }

        public static bool getPersonInfo(int PersonID, ref string NationalNo, ref string FirstName, ref string SecondName,
             ref string ThirdName, ref string LastName, ref DateTime DateOfBirth, ref string Gender, ref string Address,
              ref string Nationality, ref string Phone, ref string Email, ref string ImagePath, ref int NationalityCountryID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(DatabaseMeta.connectionString);

            string query = "SELECT People.PersonID, People.NationalNo, People.FirstName, People.SecondName, People.ThirdName," +
                " People.LastName, People.DateOfBirth, People.Gender, People.Address, People.Phone, People.Email, " +
                "Countries.CountryName as Nationality, People.ImagePath, Countries.CountryID FROM Countries " +
                "INNER JOIN People ON " +
                "Countries.CountryID = People.NationalityCountryID " +
                "WHERE PersonID = @PersonID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.Add("@PersonID", SqlDbType.Int).Value = PersonID;

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    // The record was found
                    isFound = true;

                    FirstName = (string)reader["FirstName"];
                    NationalNo = (string)reader["NationalNo"];
                    LastName = (string)reader["LastName"];
                    DateOfBirth = (DateTime)reader["DateOfBirth"];
                    Gender = (string)reader["Gender"];
                    Address = (string)reader["Address"];
                    Nationality = (string)reader["Nationality"];
                    Phone = (string)reader["Phone"];
                    NationalityCountryID = (int)reader["CountryID"];

                    if (reader["SecondName"] != DBNull.Value)
                        SecondName = (string)reader["SecondName"];
                    else
                        SecondName = "";

                    if (reader["ThirdName"] != DBNull.Value)
                        ThirdName = (string)reader["ThirdName"];
                    else
                        ThirdName = "";

                    if (reader["Email"] != DBNull.Value)
                        Email = (string)reader["Email"];
                    else
                        Email = "";

                    if (reader["ImagePath"] != DBNull.Value)
                        ImagePath = (string)reader["ImagePath"];
                    else
                        ImagePath = "";

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

        public static bool getPersonInfo(string NationalNo, ref int PersonID, ref string FirstName, ref string SecondName,
            ref string ThirdName, ref string LastName, ref DateTime DateOfBirth, ref string Gender, ref string Address,
             ref string Nationality, ref string Phone, ref string Email, ref string ImagePath, ref int NationalityCountryID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(DatabaseMeta.connectionString);

            string query = @"SELECT People.PersonID, People.NationalNo, People.FirstName, People.SecondName, People.ThirdName," +
                " People.LastName, People.DateOfBirth, People.Gender, People.Address, People.Phone, People.Email, " +
                "Countries.CountryName as Nationality, People.ImagePath, Countries.CountryID FROM Countries " +
                "INNER JOIN People ON " +
                "Countries.CountryID = People.NationalityCountryID " +
                "WHERE NationalNo = @NationalNo";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.Add("@NationalNo", SqlDbType.NVarChar).Value = NationalNo;

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    // The record was found
                    isFound = true;

                    PersonID = (int)reader["PersonID"];
                    FirstName = (string)reader["FirstName"];
                    LastName = (string)reader["LastName"];
                    DateOfBirth = (DateTime)reader["DateOfBirth"];
                    Gender = (string)reader["Gender"];
                    Address = (string)reader["Address"];
                    Nationality = (string)reader["Nationality"];
                    Phone = (string)reader["Phone"];
                    NationalityCountryID = (int)reader["CountryID"];

                    if (reader["SecondName"] != DBNull.Value)
                        SecondName = (string)reader["SecondName"];
                    else
                        SecondName = "";

                    if (reader["ThirdName"] != DBNull.Value)
                        ThirdName = (string)reader["ThirdName"];
                    else
                        ThirdName = "";

                    if (reader["Email"] != DBNull.Value)
                        Email = (string)reader["Email"];
                    else
                        Email = "";

                    if (reader["ImagePath"] != DBNull.Value)
                        ImagePath = (string)reader["ImagePath"];
                    else
                        ImagePath = "";

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


        public static int addPerson(string NationalNo, string FirstName, string SecondName,
             string ThirdName, string LastName, DateTime DateOfBirth, string Gender, string Address,
              int NationalityCountryID, string Phone, string Email, string ImagePath)
        {
            int PersonID = -1;

            SqlConnection connection = new SqlConnection(DatabaseMeta.connectionString);

            string query = @"INSERT People (NationalNo, FirstName, SecondName, ThirdName, LastName, DateOfBirth, Gender," +
            "Address, Phone, Email, NationalityCountryID, ImagePath)" +
            "VALUES (@NationalNo, @FirstName, @SecondName, @ThirdName, @LastName, @DateOfBirth, @Gender, @Address, @Phone, @Email, @NationalityCountryID, @ImagePath);" +
            "SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@NationalNo", NationalNo);
            command.Parameters.AddWithValue("@FirstName", FirstName);
            command.Parameters.AddWithValue("@LastName", LastName);
            command.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);
            command.Parameters.AddWithValue("@Gender", Gender);
            command.Parameters.AddWithValue("@Address", Address);
            command.Parameters.AddWithValue("@NationalityCountryID", NationalityCountryID);
            command.Parameters.AddWithValue("@Phone", Phone);

            // Handle SecondName
            if (!string.IsNullOrEmpty(SecondName))
                command.Parameters.AddWithValue("@SecondName", SecondName);
            else
                command.Parameters.AddWithValue("@SecondName", DBNull.Value);

            // Handle ThirdName
            if (!string.IsNullOrEmpty(ThirdName))
                command.Parameters.AddWithValue("@ThirdName", ThirdName);
            else
                command.Parameters.AddWithValue("@ThirdName", DBNull.Value);

            // Handle Email
            if (!string.IsNullOrEmpty(Email))
                command.Parameters.AddWithValue("@Email", Email);
            else
                command.Parameters.AddWithValue("@Email", DBNull.Value);

            // Handle ImagePath
            if (!string.IsNullOrEmpty(ImagePath))
                command.Parameters.AddWithValue("@ImagePath", ImagePath);
            else
                command.Parameters.AddWithValue("@ImagePath", DBNull.Value);

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();


                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    PersonID = insertedID;
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



            return PersonID;
        }

        public static bool updatePerson(int PersonID, string NationalNo, string FirstName, string SecondName,
             string ThirdName, string LastName, DateTime DateOfBirth, string Gender, string Address,
              int NationalityCountryID, string Phone, string Email, string ImagePath)
        {
            int rowsAffected = 0;

            SqlConnection connection = new SqlConnection(DatabaseMeta.connectionString);

            string query = @"UPDATE People
                           SET NationalNo = @NationalNo
                              ,FirstName = @FirstName
                              ,SecondName = @SecondName
                              ,ThirdName = @ThirdName
                              ,LastName = @LastName
                              ,DateOfBirth = @DateOfBirth
                              ,Gender = @Gender
                              ,Address = @Address
                              ,Phone = @Phone
                              ,Email = @Email
                              ,NationalityCountryID = @NationalityCountryID
                              ,ImagePath = @ImagePath
                               WHERE PersonID = @PersonID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@PersonID", PersonID);
            command.Parameters.AddWithValue("@NationalNo", NationalNo);
            command.Parameters.AddWithValue("@FirstName", FirstName);
            command.Parameters.AddWithValue("@LastName", LastName);
            command.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);
            command.Parameters.AddWithValue("@Gender", Gender);
            command.Parameters.AddWithValue("@Address", Address);
            command.Parameters.AddWithValue("@Phone", Phone);
            command.Parameters.AddWithValue("@NationalityCountryID", NationalityCountryID);

            // Handle SecondName
            if (!string.IsNullOrEmpty(SecondName))
                command.Parameters.AddWithValue("@SecondName", SecondName);
            else
                command.Parameters.AddWithValue("@SecondName", DBNull.Value);

            // Handle ThirdName
            if (!string.IsNullOrEmpty(ThirdName))
                command.Parameters.AddWithValue("@ThirdName", ThirdName);
            else
                command.Parameters.AddWithValue("@ThirdName", DBNull.Value);

            // Handle Email
            if (!string.IsNullOrEmpty(Email))
                command.Parameters.AddWithValue("@Email", Email);
            else
                command.Parameters.AddWithValue("@Email", DBNull.Value);

            // Handle ImagePath
            if (!string.IsNullOrEmpty(ImagePath))
                command.Parameters.AddWithValue("@ImagePath", ImagePath);
            else
                command.Parameters.AddWithValue("@ImagePath", DBNull.Value);

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

        public static string getImagePathForPerson(int PersonID)
        {
            string ImagePath = "";
            SqlConnection connection = new SqlConnection(DatabaseMeta.connectionString);

            string query = "select ImagePath from People where PersonID = @PersonID";


            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.Add("@PersonID", SqlDbType.Int).Value = PersonID;

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();


                if (result != null)
                {
                    ImagePath = (string)result;
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
            return ImagePath;
        }


        public static bool IsPersonExists(string NationalNo)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(DatabaseMeta.connectionString);

            string query = "select found=1 from People where NationalNo=@NationalNo";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@NationalNo", NationalNo);

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

        public static bool IsPersonUser(int PersonID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(DatabaseMeta.connectionString);

            string query = "select * from users where PersonID =@PersonID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.Add("@PersonID", SqlDbType.Int).Value = PersonID;

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


        public static bool DeletePerson(int PersonID)
        {
            int rowsAffected = 0;

            SqlConnection connection = new SqlConnection(DatabaseMeta.connectionString);

            string query = @"DELETE FROM People WHERE PersonID = @PersonID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@PersonID", PersonID);

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

    }
}
