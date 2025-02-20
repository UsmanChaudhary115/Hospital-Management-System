using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace HospitalDAL
{
    public class AppointmentServices
    {
        private static void LogDeletedAppointment(Appointment appointment)
        {
            StreamWriter deletedWriter = null;
            try
            {
                deletedWriter = new StreamWriter("DeletedAppointments.txt", append: true);
                string deletedAppointment = $"{JsonSerializer.Serialize(appointment)} Time of Deletion: {DateTime.Now}";
                deletedWriter.WriteLine(deletedAppointment);
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
        public static bool isPatientIdPresent(int id)
        {
            bool status = false;
            List<Patient> patients = PatientServices.GetAllPatientsFromDatabase();
            foreach (Patient patient in patients)
            {
                if (patient.PatientId == id)
                {
                    status = true;
                }
            }
            return status;
        }
        public static bool isDoctorAvailable(int id, DateTime date)
        {
            List<Doctor> doctors = DoctorServices.GetAllDoctorsFromDatabase();
            List<Appointment> appointments = AppointmentServices.GetAllAppointmentsFromDatabase();
            foreach (Doctor doc in doctors)
            {
                if (doc.DoctorId == id)
                {
                    foreach (Appointment appointment in appointments)
                    {
                        if (appointment.DoctorId == id && appointment.AppointmentDate.Date == date.Date)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }
        public static bool InsertAppointment(Appointment appointment)
        {
            bool status = false;
            string connStr = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=HospitalManagementSystem;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";

            SqlConnection sqlConnection = new SqlConnection(connStr);
            SqlCommand sqlCommand = null;

            try
            {
                sqlConnection.Open();
                string query = "INSERT INTO Appointments (DoctorId, PatientId, AppointmentDate) VALUES (@docId, @patId, @appDate)";
                sqlCommand = new SqlCommand(query, sqlConnection);

                sqlCommand.Parameters.AddWithValue("@docId", appointment.DoctorId);
                sqlCommand.Parameters.AddWithValue("@patId", appointment.PatientId);
                sqlCommand.Parameters.AddWithValue("@appDate", appointment.AppointmentDate);

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
        public static List<Appointment> GetAllAppointmentsFromDatabase()
        {
            List<Appointment> appointments = new List<Appointment>();
            string connStr = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=HospitalManagementSystem;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";

            SqlConnection sqlConnection = new SqlConnection(connStr);
            SqlCommand sqlCommand = null;
            try
            {
                sqlConnection.Open();
                string query = "SELECT * FROM Appointments";
                sqlCommand = new SqlCommand(query, sqlConnection);

                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                while (sqlDataReader.Read())
                {
                    appointments.Add(new Appointment { AppointmentId = sqlDataReader.GetInt32(0), DoctorId = sqlDataReader.GetInt32(1), PatientId = sqlDataReader.GetInt32(2), AppointmentDate = sqlDataReader.GetDateTime(3) });
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
            return appointments;
        }
        public static bool DeleteAppointmentFromDatabase(int appointmentId)
        {
            bool status = false;
            string connStr = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=HospitalManagementSystem;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";

            SqlConnection sqlConnection = new SqlConnection(connStr);
            SqlCommand sqlCommand = null;
            try
            {
                string selectQuery = "SELECT * FROM Appointments WHERE AppointmentId = @Id";
                sqlCommand = new SqlCommand(selectQuery, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@Id", appointmentId);
                sqlConnection.Open();
                SqlDataReader reader = sqlCommand.ExecuteReader();
                if (reader.Read())
                {
                    Appointment appointment = new Appointment { AppointmentId = reader.GetInt32(0), DoctorId = reader.GetInt32(1), PatientId = reader.GetInt32(2), AppointmentDate = reader.GetDateTime(3) };
                    LogDeletedAppointment(appointment);
                }
                else
                {
                    return status;
                }
                reader.Close();

                string deleteQuery = "DELETE FROM Appointments WHERE AppointmentId = @Id";
                sqlCommand = new SqlCommand(deleteQuery, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@Id", appointmentId);
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
        public static List<Appointment> SearchAppointmentsInDatabase(int doctorId, int patientId)
        {
            List<Appointment> appointments = new List<Appointment>();
            string connStr = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=HospitalManagementSystem;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";

            SqlConnection sqlConnection = new SqlConnection(connStr);
            SqlCommand sqlCommand = null;
            try
            {
                sqlConnection.Open();

                string query = "SELECT * FROM Appointments WHERE DoctorId = @docId AND PatientId = @patId";
                sqlCommand = new SqlCommand(query, sqlConnection);

                sqlCommand.Parameters.AddWithValue("@docId", doctorId);
                sqlCommand.Parameters.AddWithValue("@patId", patientId);
                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                while (sqlDataReader.Read())
                {
                    appointments.Add(new Appointment { AppointmentId = sqlDataReader.GetInt32(0), DoctorId = sqlDataReader.GetInt32(1), PatientId = sqlDataReader.GetInt32(2), AppointmentDate = sqlDataReader.GetDateTime(3) });
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
            return appointments;
        }
    }
}
