using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Access_Layer
{
    public class DriversData
    {
        public static bool GetInfoByPersonID(int PersonID, ref int DriverID, ref int CreatedByUserID, ref DateTime CreatedDate)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(DatabaseMeta.connectionString);

            string query = "select *from Drivers where PersonID=@PersonID";

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

                    DriverID = (int)reader["DriverID"];
                    CreatedByUserID = (int)reader["CreatedByUserID"];
                    CreatedDate = (DateTime)reader["CreatedDate"];
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
        public static bool GetInfoByDriverID(int DriverID, ref int PersonID, ref int CreatedByUserID, ref DateTime CreatedDate)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(DatabaseMeta.connectionString);

            string query = "select *from Drivers where DriverID=@DriverID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.Add("@DriverID", SqlDbType.Int).Value = DriverID;

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    // The record was found
                    isFound = true;

                    PersonID = (int)reader["PersonID"];
                    CreatedByUserID = (int)reader["CreatedByUserID"];
                    CreatedDate = (DateTime)reader["CreatedDate"];
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

        public static int addDriver(int PersonID, int CreatedByUserID, DateTime CreatedDate)
        {
            int DriverID = -1;

            SqlConnection connection = new SqlConnection(DatabaseMeta.connectionString);

            string query = @"INSERT Drivers (PersonID, CreatedByUserID, CreatedDate)" +
            "VALUES (@PersonID, @CreatedByUserID, @CreatedDate);" +
            "SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@PersonID", PersonID);
            command.Parameters.AddWithValue("@DriverID", DriverID);
            command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
            command.Parameters.AddWithValue("@CreatedDate", CreatedDate);

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();


                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    DriverID = insertedID;
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
            return DriverID;
        }

        public static DataTable GetAllDrivers()
        {
            DataTable dtIssuedLicenses = new DataTable();

            SqlConnection connection = new SqlConnection(DatabaseMeta.connectionString);

            string query = @"select Drivers.DriverID, People.PersonID, People.NationalNo, ISNULL(People.FirstName, '') 
            + ' ' + ISNULL(People.SecondName, '') + ' ' + ISNULL(People.ThirdName, '') + ' ' + 
            ISNULL(People.LastName, '') AS 'FullName', CreatedDate as Date from Drivers inner join People on drivers.PersonID=People.PersonID";

            SqlCommand command = new SqlCommand(query, connection);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    dtIssuedLicenses.Load(reader);
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

            return dtIssuedLicenses;
        }

    }
}
