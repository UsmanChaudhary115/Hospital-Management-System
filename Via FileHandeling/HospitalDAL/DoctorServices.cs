using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

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

        public static bool isDoctorIdPresent(int id)
        {
            List<Doctor> Doctors = GetAllDoctorsFromDatabase();
            foreach (Doctor doc in Doctors)
            {
                if (doc.DoctorId == id)
                {
                    return true;
                }
            }
            return false;
        }
        private static bool isAlreadyPresent(Doctor doctor)
        {
            bool status = false;
            List<Doctor> doctors = GetAllDoctorsFromDatabase();
            foreach (Doctor d in doctors)
            {
                if (d.DoctorId == doctor.DoctorId)
                {
                    status = true;
                    break;
                }
            }
            return status;
        }
        public static bool InsertDoctor(Doctor doctor)
        {
            bool status = false;
            StreamWriter writer = null;
            try
            {
                if (!(isAlreadyPresent(doctor)))
                {
                    writer = new StreamWriter("Doctors.txt", append: true);
                    string jsonDoctor = JsonSerializer.Serialize(doctor);
                    writer.WriteLine(jsonDoctor);
                    status = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
            finally
            {
                if( writer != null ) 
                { 
                    writer.Close(); 
                }
            }
            return status;
        }
        public static List<Doctor> GetAllDoctorsFromDatabase()
        {
            List<Doctor> doctors = new List<Doctor>();
            StreamReader reader = null;
            try
            {
                reader = new StreamReader("Doctors.txt");
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    Doctor doctor = JsonSerializer.Deserialize<Doctor>(line);
                    doctors.Add(doctor);
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
            return doctors;
        }
        public static bool UpdateDoctorInDatabase(Doctor doctorToBeUpdated)
        {
            bool status = false;
            StreamReader reader = null;
            StreamWriter writer = null;
            List<Doctor> updatedDoctors = new List<Doctor>();

            try
            {
                if(isAlreadyPresent(doctorToBeUpdated)) 
                {
                    reader = new StreamReader("Doctors.txt");
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        Doctor doctor = JsonSerializer.Deserialize<Doctor>(line);
                        if (doctor.DoctorId != doctorToBeUpdated.DoctorId)
                        {
                            updatedDoctors.Add(doctor);
                        }
                        else
                        {
                            updatedDoctors.Add(doctorToBeUpdated);
                            status = true;
                        }
                    }
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

            if (status)
            {
                try
                {
                    writer = new StreamWriter("Doctors.txt");
                    if (writer != null)
                    {
                        foreach (Doctor doctor in updatedDoctors)
                        {
                            string jsonDoctor = JsonSerializer.Serialize(doctor);
                            writer.WriteLine(jsonDoctor);
                        }
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
        public static bool DeleteDoctorFromDatabase(int doctorId)
        {
            bool status = false;
            StreamReader reader = null;
            StreamWriter writer = null;
            List<Doctor> doctorsToKeep = new List<Doctor>();
            try
            {
                if(isAlreadyPresent(new Doctor { DoctorId = doctorId }))
                {
                    reader = new StreamReader("Doctors.txt");
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        Doctor doctor = JsonSerializer.Deserialize<Doctor>(line);
                        if (doctor.DoctorId != doctorId)
                        {
                            doctorsToKeep.Add(doctor);
                        }
                        else
                        {
                            status = true;
                            LogDeletedDoctor(doctor);
                        }
                    }
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
            if (status)
            {
                try
                {
                    writer = new StreamWriter("Doctors.txt");
                    if (writer != null)
                    {
                        foreach (Doctor doctor in doctorsToKeep)
                        {
                            string jsonPatient = JsonSerializer.Serialize(doctor);
                            writer.WriteLine(jsonPatient);
                        }
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
        public static List<Doctor> SearchDoctorsInDatabase(string specialization)
        {
            List<Doctor> doctors = new List<Doctor>();
            StreamReader streamReader = null;
            try
            {
                streamReader = new StreamReader("Doctors.txt");
                string line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    Doctor doctor = JsonSerializer.Deserialize<Doctor>(line);
                    if (doctor.Specialization == specialization)
                    {
                        doctors.Add(doctor);
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
            return doctors;
        }
    }
}
