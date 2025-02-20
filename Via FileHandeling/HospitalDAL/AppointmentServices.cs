using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
        public static bool isAppointmentIdPresent(int id)
        {
            List<Appointment> appointments = GetAllAppointmentsFromDatabase();
            foreach (Appointment app in appointments)
            {
                if (app.AppointmentId == id)
                {
                    return true;
                }
            }
            return false;
        }
        public static bool isPatientIdPresent(int id)
        {
            List<Patient> patients = PatientServices.GetAllPatientsFromDatabase();
            foreach (Patient patient in patients)
            {
                if (patient.PatientId == id)
                {
                    return true;
                }
            }
            return false;
        }
        public static bool isDoctorAvailable(int id, DateTime date)
        {
            List<Doctor> doctors = DoctorServices.GetAllDoctorsFromDatabase();
            List<Appointment> appointments = GetAllAppointmentsFromDatabase();
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
            StreamWriter writer = null;
            try
            {
                writer = new StreamWriter("Appointments.txt", append: true);
                string jsonAppointment = JsonSerializer.Serialize(appointment);
                writer.WriteLine(jsonAppointment);
                status = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
            finally
            {
                if (writer != null) 
                { 
                    writer.Close(); 
                }
            }
            return status;
        }
        public static List<Appointment> GetAllAppointmentsFromDatabase()
        {
            List<Appointment> appointments = new List<Appointment>();
            StreamReader reader = null;
            try
            {
                reader = new StreamReader("Appointments.txt");
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    Appointment appointment = JsonSerializer.Deserialize<Appointment>(line);
                    appointments.Add(appointment);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            finally
            {
                if (reader != null)
                { 
                    reader.Close(); 
                }
            }
            return appointments;
        }
       
        public static bool DeleteAppointmentFromDatabase(int appointmentId)
        {
            bool status = false;
            StreamReader reader = null;
            StreamWriter writer = null;
            List<Appointment> appointmentsToKeep = new List<Appointment>();
            try
            {
                reader = new StreamReader("Appointments.txt");
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    Appointment appointment = JsonSerializer.Deserialize<Appointment>(line);
                    if (appointment.AppointmentId != appointmentId)
                    {
                        appointmentsToKeep.Add(appointment);
                    }
                    else
                    {
                        status = true;
                        LogDeletedAppointment(appointment);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            finally
            {
                reader.Close();
            }

            if (status)
            {
                try
                {
                    writer = new StreamWriter("Appointments.txt");
                    if (writer != null)
                    {
                        foreach (Appointment appointment in appointmentsToKeep)
                        {
                            string jsonAppointment = JsonSerializer.Serialize(appointment);
                            writer.WriteLine(jsonAppointment);
                        }
                        status = true;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
                finally
                {
                    if (writer != null)
                    {
                        writer.Close();
                    }
                }
            }
            return status;
        }
        public static List<Appointment> SearchAppointmentsInDatabase(int doctorId, int patientId)
        {
            List<Appointment> appointments = new List<Appointment>();
            StreamReader streamReader = null;
            try
            {
                streamReader = new StreamReader("Appointments.txt");
                string line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    Appointment appointment = JsonSerializer.Deserialize<Appointment>(line);
                    if (appointment.DoctorId == doctorId && appointment.PatientId == patientId)
                    {
                        appointments.Add(appointment);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            finally
            {
                if (streamReader != null)
                {
                    streamReader.Close();
                }
            }
            return appointments;
        }
    }
}
