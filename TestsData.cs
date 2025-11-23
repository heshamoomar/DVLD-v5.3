using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using System.Net;

namespace Data_Access_Layer
{
    public class TestsData
    {
        public static DataTable GetTestsTypes()
        {
            DataTable dtTests = new DataTable();

            SqlConnection connection = new SqlConnection(DatabaseMeta.connectionString);

            string query = "select * from TestTypes";

            SqlCommand command = new SqlCommand(query, connection);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    dtTests.Load(reader);
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

            return dtTests;
        }

        public static bool GetTestTypeInfo(int TestTypeID,ref string TestTypeTitle,ref string TestTypeDescription,ref decimal TestTypeFees)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(DatabaseMeta.connectionString);

            string query = "SELECT *from TestTypes " +
                "WHERE TestTypeID = @TestTypeID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.Add("@TestTypeID", SqlDbType.Int).Value = TestTypeID;

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    // The record was found
                    isFound = true;

                    TestTypeTitle = (string)reader["TestTypeTitle"];
                    TestTypeFees = (decimal)reader["TestTypeFees"];

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

        public static bool UpdateTestTypes(int TestTypeID, string TestTypeTitle, string TestTypeDescription, decimal TestTypeFees)
        {
            int rowsAffected = 0;


            SqlConnection connection = new SqlConnection(DatabaseMeta.connectionString);

            string query = @"UPDATE TestTypes
                           SET TestTypeTitle = @TestTypeTitle
                              ,TestTypeDescription = @TestTypeDescription
                              ,TestTypeFees = @TestTypeFees
                               WHERE TestTypeID = @TestTypeID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
            command.Parameters.AddWithValue("@TestTypeTitle", TestTypeTitle);
            command.Parameters.AddWithValue("@TestTypeDescription", TestTypeDescription);
            command.Parameters.AddWithValue("@TestTypeFees", TestTypeFees);

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

        public static DataTable GetTestAppointmentsDatatable_Vision(int LocalDrivingLicenseApplicationID)
        {
            DataTable dtAppointments = new DataTable();

            SqlConnection connection = new SqlConnection(DatabaseMeta.connectionString);

            string query = @"select TestAppointmentID, AppointmentDate, PaidFees, 
                IsLocked from TestAppointments where 
                LocalDrivingLicenseApplicationID =@LocalDrivingLicenseApplicationID and TestTypeID=1";


            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    dtAppointments.Load(reader);
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

            return dtAppointments;
        }

        public static DataTable GetTestAppointmentsDatatable_Written(int LocalDrivingLicenseApplicationID)
        {
            DataTable dtAppointments = new DataTable();

            SqlConnection connection = new SqlConnection(DatabaseMeta.connectionString);

            string query = @"select TestAppointmentID, AppointmentDate, PaidFees, 
                IsLocked from TestAppointments where 
                LocalDrivingLicenseApplicationID =@LocalDrivingLicenseApplicationID and TestTypeID=2";


            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    dtAppointments.Load(reader);
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

            return dtAppointments;
        }

        public static DataTable GetTestAppointmentsDatatable_Street(int LocalDrivingLicenseApplicationID)
        {
            DataTable dtAppointments = new DataTable();

            SqlConnection connection = new SqlConnection(DatabaseMeta.connectionString);

            string query = @"select TestAppointmentID, AppointmentDate, PaidFees, 
                IsLocked from TestAppointments where 
                LocalDrivingLicenseApplicationID =@LocalDrivingLicenseApplicationID and TestTypeID=3";


            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    dtAppointments.Load(reader);
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

            return dtAppointments;
        }

    }
}
