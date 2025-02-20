using System;
using System.Collections.Generic;
using System.Diagnostics;

//using System.IdentityModel.Protocols.WSTrust;
using System.IO;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace HospitalDAL
{
    public class PatientServices
    {
        private static void LogDeletedPatient(Patient patient)
        {
            StreamWriter deletedWriter = null;
            try
            {
                deletedWriter = new StreamWriter("DeletedPatients.txt", append: true);
                string deletedPatient = $"{JsonSerializer.Serialize(patient)} Time of Deletion: {DateTime.Now}";
                deletedWriter.WriteLine(deletedPatient);
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
        public static bool InsertPatient(Patient patient)
        {
            bool status = false;
            string connStr = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=HospitalManagementSystem;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";

            SqlConnection sqlConnection = new SqlConnection(connStr);
            SqlCommand sqlCommand = null;

            try
            {
                sqlConnection.Open();
                string query = "INSERT INTO Patients (Name, Email, Disease) VALUES (@Name, @Email, @Disease)";
                sqlCommand = new SqlCommand(query, sqlConnection);

                sqlCommand.Parameters.AddWithValue("@Name", patient.Name);
                sqlCommand.Parameters.AddWithValue("@Email", patient.Email);
                sqlCommand.Parameters.AddWithValue("@Disease", patient.Disease);

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
        public static List<Patient> SearchPatientsInDatabase(string name)
        {
            List<Patient> patients = new List<Patient>();
            string connStr = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=HospitalManagementSystem;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";

            SqlConnection sqlConnection = new SqlConnection(connStr);
            SqlCommand sqlCommand = null;
            try
            {
                sqlConnection.Open();

                string query = "SELECT * FROM Patients WHERE Name = @Name";
                sqlCommand = new SqlCommand(query, sqlConnection);

                sqlCommand.Parameters.AddWithValue("@Name", name);
                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                while (sqlDataReader.Read())
                {
                    patients.Add(new Patient { PatientId = sqlDataReader.GetInt32(0), Name = sqlDataReader.GetString(1), Email = sqlDataReader.GetString(2), Disease = sqlDataReader.GetString(3) });
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
            return patients;
        }
        public static bool DeletePatientFromDatabase(int patientId)
        {
            bool status = false;
            string connStr = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=HospitalManagementSystem;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";

            SqlConnection sqlConnection = new SqlConnection(connStr);
            SqlCommand sqlCommand = null;
            try
            {
                string selectQuery = "SELECT * FROM Patients WHERE PatientId = @Id";
                sqlCommand = new SqlCommand(selectQuery, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@Id", patientId);
                sqlConnection.Open();
                SqlDataReader reader = sqlCommand.ExecuteReader();
                if (reader.Read())
                {
                    Patient patient = new Patient { PatientId = reader.GetInt32(0), Name = reader.GetString(1), Email = reader.GetString(2), Disease = reader.GetString(3) };
                    LogDeletedPatient(patient);
                }
                else
                {
                    return status;
                }
                reader.Close();

                string deleteQuery = "DELETE FROM Patients WHERE PatientId = @Id";
                sqlCommand = new SqlCommand(deleteQuery, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@Id", patientId);
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
        public static bool UpdatePatientInDatabase(Patient patientToBeUpdated)
        {
            bool status = false;
            string connStr = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=HospitalManagementSystem;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";
            SqlConnection sqlConnection = new SqlConnection(connStr);
            SqlCommand sqlCommand = null;
            try
            {
                string query = "UPDATE Patients set Name = @name, Email = @email, Disease = @disease WHERE PatientId = @Id";
                sqlConnection.Open();
                sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@Id", patientToBeUpdated.PatientId);
                sqlCommand.Parameters.AddWithValue("@name", patientToBeUpdated.Name);
                sqlCommand.Parameters.AddWithValue("@email", patientToBeUpdated.Email);
                sqlCommand.Parameters.AddWithValue("@disease", patientToBeUpdated.Disease);
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
        public static List<Patient> GetAllPatientsFromDatabase()
        {
            List<Patient> patients = new List<Patient>();
            string connStr = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=HospitalManagementSystem;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";

            SqlConnection sqlConnection = new SqlConnection(connStr);
            SqlCommand sqlCommand = null;
            try
            {
                sqlConnection.Open();

                string query = "SELECT * FROM Patients";
                sqlCommand = new SqlCommand(query, sqlConnection);

                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                while (sqlDataReader.Read())
                {
                    patients.Add(new Patient { PatientId = sqlDataReader.GetInt32(0), Name =  sqlDataReader.GetString(1), Email = sqlDataReader.GetString(2), Disease = sqlDataReader.GetString(3) });
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
            return patients; 
        }
    }
}
