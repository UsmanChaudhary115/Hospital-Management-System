using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace HospitalDAL
{
    public class DoctorServices
    {
        private static void LogDeletedDoctor(Doctor doctor)
        {
            StreamWriter deletedWriter = null;
            try
            {
                deletedWriter = new StreamWriter("DeletedDoctors.txt", append: true);
                string deletedDoctor = $"{JsonSerializer.Serialize(doctor)} Time of Deletion: {DateTime.Now}";
                deletedWriter.WriteLine(deletedDoctor);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while logging deleted patient: {ex.Message}");
            }
            finally
            {
                if (deletedWriter != null)
                {
                    deletedWriter.Close();
                }
            }
        }
        public static bool InsertDoctor(Doctor doctor)
        {
            bool status = false;
            string connStr = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=HospitalManagementSystem;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";

            SqlConnection sqlConnection = new SqlConnection(connStr);
            SqlCommand sqlCommand = null;

            try
            {
                sqlConnection.Open();
                string query = "INSERT INTO Doctors (Name, Specialization) VALUES (@Name, @Specialization)";
                sqlCommand = new SqlCommand(query, sqlConnection);

                sqlCommand.Parameters.AddWithValue("@Name", doctor.Name);
                sqlCommand.Parameters.AddWithValue("@Specialization", doctor.Specialization);

                int count = sqlCommand.ExecuteNonQuery();
                if (count > 0)
                {
                    status = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
            finally
            {

                if (sqlCommand != null)
                {
                    sqlCommand.Dispose();
                }
                if (sqlConnection != null)
                {
                    sqlConnection.Close();
                }
            }

            return status;
        }
        public static List<Doctor> GetAllDoctorsFromDatabase()
        {
            List<Doctor> doctors = new List<Doctor>();
            string connStr = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=HospitalManagementSystem;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";

            SqlConnection sqlConnection = new SqlConnection(connStr);
            SqlCommand sqlCommand = null;
            try
            {
                sqlConnection.Open();
                string query = "SELECT * FROM Doctors";
                sqlCommand = new SqlCommand(query, sqlConnection);

                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                while (sqlDataReader.Read())
                {
                    doctors.Add(new Doctor { DoctorId = sqlDataReader.GetInt32(0), Name = sqlDataReader.GetString(1), Specialization = sqlDataReader.GetString(2) });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            finally
            {
                if (sqlCommand != null)
                {
                    sqlCommand.Dispose();
                }
                if (sqlConnection != null)
                {
                    sqlConnection.Close();
                }
            }
            return doctors;
        }
        public static bool UpdateDoctorInDatabase(Doctor doctorToBeUpdated)
        {
            bool status = false;
            string connStr = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=HospitalManagementSystem;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";
            SqlConnection sqlConnection = new SqlConnection(connStr);
            SqlCommand sqlCommand = null;
            try
            {
                string query = "UPDATE Doctors set Name = @name, Specialization = @Specialization WHERE DoctorId = @Id";
                sqlConnection.Open();
                sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@Id", doctorToBeUpdated.DoctorId);
                sqlCommand.Parameters.AddWithValue("@name", doctorToBeUpdated.Name);
                sqlCommand.Parameters.AddWithValue("@Specialization", doctorToBeUpdated.Specialization);
                int count = sqlCommand.ExecuteNonQuery();
                if (count > 0)
                {
                    status = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
            finally
            {
                if (sqlCommand != null)
                {
                    sqlCommand.Dispose();
                }
                if (sqlConnection != null)
                {
                    sqlConnection.Close();
                }
            }
            return status;
        }
        public static bool DeleteDoctorFromDatabase(int doctorId)
        {
            bool status = false;
            string connStr = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=HospitalManagementSystem;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";

            SqlConnection sqlConnection = new SqlConnection(connStr);
            SqlCommand sqlCommand = null;
            try
            {
                string selectQuery = "SELECT * FROM Doctors WHERE DoctorId = @Id";
                sqlCommand = new SqlCommand(selectQuery, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@Id", doctorId);
                sqlConnection.Open();
                SqlDataReader reader = sqlCommand.ExecuteReader();
                if (reader.Read())
                {
                    Doctor doctor = new Doctor { DoctorId = reader.GetInt32(0), Name = reader.GetString(1), Specialization = reader.GetString(2)};
                    LogDeletedDoctor(doctor);
                }
                else
                {
                    return status;
                }
                reader.Close();

                string deleteQuery = "DELETE FROM Doctors WHERE DoctorId = @Id";
                sqlCommand = new SqlCommand(deleteQuery, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@Id", doctorId);
                int count = sqlCommand.ExecuteNonQuery();
                if (count > 0)
                {
                    status = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
            finally
            {
                if (sqlCommand != null)
                {
                    sqlCommand.Dispose();
                }
                if (sqlConnection != null)
                {
                    sqlConnection.Close();
                }
            }
            return status;
        }
        public static List<Doctor> SearchDoctorsInDatabase(string specialization)
        {
            List<Doctor> doctors = new List<Doctor>();
            string connStr = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=HospitalManagementSystem;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";

            SqlConnection sqlConnection = new SqlConnection(connStr);
            SqlCommand sqlCommand = null;
            try
            {
                sqlConnection.Open();

                string query = "SELECT * FROM Doctors WHERE Specialization = @Specialization";
                sqlCommand = new SqlCommand(query, sqlConnection);

                sqlCommand.Parameters.AddWithValue("@Specialization", specialization);
                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                while (sqlDataReader.Read())
                {
                    doctors.Add(new Doctor { DoctorId = sqlDataReader.GetInt32(0), Name = sqlDataReader.GetString(1), Specialization = sqlDataReader.GetString(2) });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
            finally
            {

                if (sqlCommand != null)
                {
                    sqlCommand.Dispose();
                }
                if (sqlConnection != null)
                {
                    sqlConnection.Close();
                }
            }
            return doctors;
        }
    }
}
