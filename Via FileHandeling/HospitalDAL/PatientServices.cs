using System;
using System.Collections.Generic;
using System.Diagnostics;

//using System.IdentityModel.Protocols.WSTrust;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

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
        public static bool isPatientIdPresent(int id)
        {
            List<Patient> Patients = GetAllPatientsFromDatabase();
            foreach (Patient pat in Patients)
            {
                if (pat.PatientId == id)
                {
                    return true;
                }
            }
            return false;
        }
        private static bool isAlreadyPresent(Patient patient)
        {
            bool status = false;
            List<Patient> patients = GetAllPatientsFromDatabase(); 
            foreach (Patient p in patients)
            {
                if (p.PatientId == patient.PatientId)
                {
                    status = true;
                    break;
                }
            }
            return status;
        }
        public static bool InsertPatient(Patient patient)
        {
            bool status = false;
            StreamWriter writer = null; 
            try
            { 
                if (!(isAlreadyPresent(patient)))
                {
                    writer = new StreamWriter("Patients.txt", append: true);
                    string jsonPatient = JsonSerializer.Serialize(patient);
                    writer.WriteLine(jsonPatient);
                    status = true;
                }
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
        public static List<Patient> SearchPatientsInDatabase(string name)
        {
            List<Patient> patients = new List<Patient>();
            StreamReader streamReader = null;
            try
            {
                streamReader = new StreamReader("Patients.txt");
                string line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    Patient patient = JsonSerializer.Deserialize<Patient>(line);
                    if (patient.Name == name)
                    {
                        patients.Add(patient);
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
            return patients;
        }
        public static bool DeletePatientFromDatabase(int patientId)
        {
            bool status = false;
            StreamReader reader = null;
            StreamWriter writer = null;
            List<Patient> patientsToKeep = new List<Patient>();

            try
            {
                if(isAlreadyPresent(new Patient { PatientId = patientId }))
                {
                    reader = new StreamReader("Patients.txt");
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        Patient patient = JsonSerializer.Deserialize<Patient>(line);
                        if (patient.PatientId != patientId)
                        {
                            patientsToKeep.Add(patient);
                        }
                        else
                        {
                            status = true;
                            LogDeletedPatient(patient);
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
                    writer = new StreamWriter("Patients.txt");
                    if (writer != null)
                    {
                        foreach (Patient patient in patientsToKeep)
                        {
                            string jsonPatient = JsonSerializer.Serialize(patient);
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
        public static bool UpdatePatientInDatabase(Patient patientToBeUpdated)
        {
            bool status = false;
            StreamReader reader = null;
            StreamWriter writer = null;
            List<Patient> updatedPatients = new List<Patient>(); 

            try
            {
                if(isAlreadyPresent(patientToBeUpdated)) 
                { 
                    reader = new StreamReader("Patients.txt");
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        Patient patient = JsonSerializer.Deserialize<Patient>(line);
                        if (patient.PatientId != patientToBeUpdated.PatientId)
                        {
                            updatedPatients.Add(patient);
                        }
                        else
                        {
                            updatedPatients.Add(patientToBeUpdated);
                        }
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
                if (reader != null)
                {
                    reader.Close();
                }
            }

            if (status)
            {
                try
                {
                    writer = new StreamWriter("Patients.txt");
                    if (writer != null)
                    {
                        foreach (Patient patient in updatedPatients)
                        {
                            string jsonPatient = JsonSerializer.Serialize(patient);
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
        public static List<Patient> GetAllPatientsFromDatabase()
        {
            List<Patient> patients = new List<Patient>(); 
            StreamReader reader = null; 
            try
            {
                reader = new StreamReader("Patients.txt");
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    Patient patient = JsonSerializer.Deserialize<Patient>(line);
                    patients.Add(patient);
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
            return patients; 
        }
    }
}
