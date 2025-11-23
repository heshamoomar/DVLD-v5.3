using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Access_Layer
{
    public class Local_License_Data
    {
        public static int addLocalLicense(int ApplicationID, int LicenseClassID)
        {
            int LocalDrivingLicenseApplicationID = -1;

            SqlConnection connection = new SqlConnection(DatabaseMeta.connectionString);

            string query = @"INSERT LocalDrivingLicenseApplications (ApplicationID, LicenseClassID)" +
            "VALUES (@ApplicationID, @LicenseClassID);" +
            "SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
            command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();


                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    LocalDrivingLicenseApplicationID = insertedID;
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



            return LocalDrivingLicenseApplicationID;
        }

        public static DataTable GetAllLocalLicenseInfo()
        {
            DataTable dtLocalLicenses = new DataTable();

            SqlConnection connection = new SqlConnection(DatabaseMeta.connectionString);

            string query = @"
                SELECT 
                    LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID, 
                    LicenseClasses.ClassName, 
                    People.NationalNo, 
                    ISNULL(People.FirstName, '') + ' ' + ISNULL(People.SecondName, '') + ' ' + ISNULL(People.ThirdName, '') + ' ' + ISNULL(People.LastName, '') AS FullName, 
                    Applications.ApplicationDate, 
                    COUNT(CASE WHEN Tests.TestResult = 1 THEN 1 END) AS 'PassedTests',   -- Counting passed tests
                    ApplicationStatus.ApplicationStatus as 'Status'
                FROM 
                    LocalDrivingLicenseApplications
                LEFT JOIN 
                    LicenseClasses ON LocalDrivingLicenseApplications.LicenseClassID = LicenseClasses.LicenseClassID
                LEFT JOIN 
                    Applications ON LocalDrivingLicenseApplications.ApplicationID = Applications.ApplicationID
                LEFT JOIN 
                    People ON Applications.ApplicantPersonID = People.PersonID
                LEFT JOIN 
                    TestAppointments ON LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = TestAppointments.LocalDrivingLicenseApplicationID
                LEFT JOIN 
                    Tests ON TestAppointments.TestAppointmentID = Tests.TestAppointmentID
                LEFT JOIN 
                    ApplicationStatus ON Applications.ApplicationStatus = ApplicationStatus.ApplicationStatusID
                GROUP BY 
                    LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID, 
                    LicenseClasses.ClassName, 
                    People.NationalNo,
                    People.FirstName, 
                    People.SecondName, 
                    People.ThirdName, 
                    People.LastName, 
                    Applications.ApplicationDate, 
                    ApplicationStatus.ApplicationStatus;
                ";


            SqlCommand command = new SqlCommand(query, connection);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    dtLocalLicenses.Load(reader);
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

            return dtLocalLicenses;
        }

        public static bool GetLicenseInfo(int LocalDrivingLicenseApplicationID, ref int ApplicationID, ref int LicenseClassID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(DatabaseMeta.connectionString);

            string query = "select * from LocalDrivingLicenseApplications where LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.Add("@LocalDrivingLicenseApplicationID", SqlDbType.NVarChar).Value = LocalDrivingLicenseApplicationID;

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    // The record was found
                    isFound = true;

                    ApplicationID = (int)reader["ApplicationID"];
                    LicenseClassID = (int)reader["LicenseClassID"];

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

        public static bool DeleteApplication(int ldl)
        {
            int rowsAffected = 0;

            SqlConnection connection = new SqlConnection(DatabaseMeta.connectionString);

            string query = @"DELETE FROM LocalDrivingLicenseApplications WHERE LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", ldl);

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
