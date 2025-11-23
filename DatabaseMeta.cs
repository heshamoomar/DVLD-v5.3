using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Access_Layer
{
    public class DatabaseMeta
    {
        internal static string connectionString = "Server=.;Database=DVlD;User Id=sa;Password=sa123456;";

        public static bool checkPersonHasReferences(int PersonID)   // true if references in other rows do exist
        {

            bool foundReferences = false;

            SqlConnection connection = new SqlConnection(DatabaseMeta.connectionString);

            string query1 = "select top 1 found=1  from Applications where ApplicantPersonID = @PersonID";
            string query2 = "select top 1 found=1  from Drivers where PersonID = @PersonID";


            SqlCommand command1 = new SqlCommand(query1, connection);
            command1.Parameters.AddWithValue("@PersonID", PersonID);

            SqlCommand command2 = new SqlCommand(query2, connection);
            command2.Parameters.AddWithValue("@PersonID", PersonID);

            try
            {
                connection.Open();

                SqlDataReader reader1 = command1.ExecuteReader();
                SqlDataReader reader2 = command2.ExecuteReader();

                if(reader1.HasRows)
                {
                    foundReferences = true;
                }

                if(reader2.HasRows)
                {
                    foundReferences = true;
                }

                reader1.Close();
                reader2.Close();
            }
            catch (Exception ex)
            {
                // Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return foundReferences;
        }
        public static bool checkUserHasReferences(int userID)   // true if references in other rows do exist
        {

            bool foundReferences = false;

            SqlConnection connection = new SqlConnection(DatabaseMeta.connectionString);

            string query1 = "select top 1 found=1  from Tests where CreatedByUserID = @userID";
            string query2 = "select top 1 found=1  from Licenses where CreatedByUserID = @userID";
            string query3 = "select top 1 found=1  from DetainedLicenses where CreatedByUserID = @userID";
            string query4 = "select top 1 found=1  from InternationalLicenses where CreatedByUserID = @userID";
            string query5 = "select top 1 found=1  from TestAppointments where CreatedByUserID = @userID";
            string query6 = "select top 1 found=1  from Applications where CreatedByUserID = @userID";
            string query7 = "select top 1 found=1  from Drivers where CreatedByUserID = @userID";


            SqlCommand command1 = new SqlCommand(query1, connection);
            command1.Parameters.AddWithValue("@userID", userID);

            SqlCommand command2 = new SqlCommand(query2, connection);
            command2.Parameters.AddWithValue("@userID", userID);

            SqlCommand command3 = new SqlCommand(query3, connection);
            command3.Parameters.AddWithValue("@userID", userID);

            SqlCommand command4 = new SqlCommand(query4, connection);
            command4.Parameters.AddWithValue("@userID", userID);

            SqlCommand command5 = new SqlCommand(query5, connection);
            command5.Parameters.AddWithValue("@userID", userID);

            SqlCommand command6 = new SqlCommand(query6, connection);
            command6.Parameters.AddWithValue("@userID", userID);

            SqlCommand command7 = new SqlCommand(query7, connection);
            command7.Parameters.AddWithValue("@userID", userID);

            try
            {
                connection.Open();

                SqlDataReader reader1 = command1.ExecuteReader();
                SqlDataReader reader2 = command2.ExecuteReader();
                SqlDataReader reader3 = command3.ExecuteReader();
                SqlDataReader reader4 = command4.ExecuteReader();
                SqlDataReader reader5 = command5.ExecuteReader();
                SqlDataReader reader6 = command6.ExecuteReader();
                SqlDataReader reader7 = command7.ExecuteReader();

                if(reader1.HasRows)
                    foundReferences = true;
                if(reader2.HasRows)
                    foundReferences = true;
                if(reader3.HasRows)
                    foundReferences = true;
                if(reader4.HasRows)
                    foundReferences = true;
                if(reader5.HasRows)
                    foundReferences = true;
                if(reader6.HasRows)
                    foundReferences = true;
                if(reader7.HasRows)
                    foundReferences = true;

                reader1.Close();
                reader2.Close();
                reader3.Close();
                reader4.Close();
                reader5.Close();
                reader6.Close();
                reader7.Close();
            }
            catch (Exception ex)
            {
                // Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return foundReferences;
        }

    }
}
