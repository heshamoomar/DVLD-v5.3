using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Data_Access_Layer
{
    public class LicensesData
    {
        public static DataTable GetAllLicensesByDriverID(int DriverID)
        {
            DataTable dtIssuedLicenses = new DataTable();

            SqlConnection connection = new SqlConnection(DatabaseMeta.connectionString);

            string query = "select * from Licenses WHERE DriverID = @DriverID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.Add("@DriverID", SqlDbType.Int).Value = DriverID;


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


        public static bool getInfoByLicenseID(int LicenseID,ref int ApplicationID,ref int DriverID,ref int LicenseClass,
                ref DateTime IssueDate,ref DateTime ExpirationDate,ref string Notes,ref decimal PaidFees,
                ref bool IsActive,ref byte IssueReason,ref int CreatedByUserID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(DatabaseMeta.connectionString);

            string query = "select *from Licenses where LicenseID=@LicenseID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.Add("@LicenseID", SqlDbType.Int).Value = LicenseID;

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    // The record was found
                    isFound = true;

                    ApplicationID = (int)reader["ApplicationID"];
                    DriverID = (int)reader["DriverID"];
                    LicenseClass = (int)reader["LicenseClass"];
                    IssueDate = (DateTime)reader["IssueDate"];
                    ExpirationDate = (DateTime)reader["ExpirationDate"];
                    PaidFees = (decimal)reader["PaidFees"];
                    IsActive = (bool)reader["IsActive"];
                    IssueReason = (byte)reader["IssueReason"];
                    CreatedByUserID = (int)reader["CreatedByUserID"];

                    if (reader["Notes"] != DBNull.Value)
                        Notes = (string)reader["Notes"];
                    else
                        Notes = "";
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

        public static bool getInfoByApplicationID(int ApplicationID, ref int LicenseID, ref int DriverID,ref int LicenseClass,
                ref DateTime IssueDate,ref DateTime ExpirationDate,ref string Notes,ref decimal PaidFees,
                ref bool IsActive,ref byte IssueReason,ref int CreatedByUserID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(DatabaseMeta.connectionString);

            string query = "select *from Licenses where ApplicationID=@ApplicationID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.Add("@ApplicationID", SqlDbType.Int).Value = ApplicationID;

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    // The record was found
                    isFound = true;

                    LicenseID = (int)reader["LicenseID"];
                    DriverID = (int)reader["DriverID"];
                    LicenseClass = (int)reader["LicenseClass"];
                    IssueDate = (DateTime)reader["IssueDate"];
                    ExpirationDate = (DateTime)reader["ExpirationDate"];
                    PaidFees = (decimal)reader["PaidFees"];
                    IsActive = (bool)reader["IsActive"];
                    IssueReason = (byte)reader["IssueReason"];
                    CreatedByUserID = (int)reader["CreatedByUserID"];

                    if (reader["Notes"] != DBNull.Value)
                        Notes = (string)reader["Notes"];
                    else
                        Notes = "";
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

        public static int addLicense(int ApplicationID, int DriverID, int LicenseClass,
                 DateTime IssueDate, DateTime ExpirationDate, string Notes, decimal PaidFees,
                 bool IsActive, byte IssueReason, int CreatedByUserID)
        {
            int LicenseID = -1;

            SqlConnection connection = new SqlConnection(DatabaseMeta.connectionString);

            string query = @"INSERT INTO Licenses (ApplicationID, DriverID, LicenseClass, IssueDate, ExpirationDate, Notes, PaidFees,
                    IsActive, IssueReason, CreatedByUserID) 
                    VALUES (@ApplicationID, @DriverID, @LicenseClass, @IssueDate, @ExpirationDate, @Notes, @PaidFees, 
                    @IsActive, @IssueReason, @CreatedByUserID);
                    SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
            command.Parameters.AddWithValue("@DriverID", DriverID);
            command.Parameters.AddWithValue("@LicenseClass", LicenseClass);
            command.Parameters.AddWithValue("@IssueDate", IssueDate);
            command.Parameters.AddWithValue("@ExpirationDate", ExpirationDate);
            command.Parameters.AddWithValue("@PaidFees", PaidFees);
            command.Parameters.AddWithValue("@IsActive", IsActive);
            command.Parameters.AddWithValue("@IssueReason", IssueReason);
            command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

            // Handle Notes
            if (!string.IsNullOrEmpty(Notes))
                command.Parameters.AddWithValue("@Notes", Notes);
            else
                command.Parameters.AddWithValue("@Notes", DBNull.Value);

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    LicenseID = insertedID;
                }
            }
            catch (Exception ex)
            {
                // Handle exception (log it or rethrow as needed)
            }
            finally
            {
                connection.Close();
            }
            return LicenseID;
        }

        public static bool ActiveLicenseExistsForApplication(int ApplicationID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(DatabaseMeta.connectionString);

            string query = "SELECT Found=1 FROM Licenses WHERE ApplicationID = @ApplicationID AND IsActive=1";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ApplicationID", ApplicationID);

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

        public static bool ActiveLicenseExistsForDriver(int DriverID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(DatabaseMeta.connectionString);

            string query = "SELECT Found=1 FROM Licenses WHERE DriverID = @DriverID AND IsActive=1";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@DriverID", DriverID);

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


        public static bool updateLicense(int LicenseID, int ApplicationID, int DriverID, int LicenseClass,
                DateTime IssueDate, DateTime ExpirationDate, string Notes, decimal PaidFees,
                bool IsActive, byte IssueReason, int CreatedByUserID)
        {
            int rowsAffected = 0;

            SqlConnection connection = new SqlConnection(DatabaseMeta.connectionString);

            string query = @"UPDATE Licenses
                           SET ApplicationID = @ApplicationID
                              ,DriverID = @DriverID
                              ,LicenseClass = @LicenseClass
                              ,IssueDate = @IssueDate
                              ,ExpirationDate = @ExpirationDate
                              ,Notes = @Notes
                              ,PaidFees = @PaidFees
                              ,IsActive = @IsActive
                              ,IssueReason = @IssueReason
                              ,CreatedByUserID = @CreatedByUserID
                               WHERE LicenseID = @LicenseID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LicenseID", LicenseID);
            command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
            command.Parameters.AddWithValue("@DriverID", DriverID);
            command.Parameters.AddWithValue("@LicenseClass", LicenseClass);
            command.Parameters.AddWithValue("@IssueDate", IssueDate);
            command.Parameters.AddWithValue("@ExpirationDate", ExpirationDate);
            command.Parameters.AddWithValue("@PaidFees", PaidFees);
            command.Parameters.AddWithValue("@IsActive", IsActive);
            command.Parameters.AddWithValue("@IssueReason", IssueReason);
            command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

            // Handle Notes
            if (!string.IsNullOrEmpty(Notes))
                command.Parameters.AddWithValue("@Notes", Notes);
            else
                command.Parameters.AddWithValue("@Notes", DBNull.Value);

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

    }
}
