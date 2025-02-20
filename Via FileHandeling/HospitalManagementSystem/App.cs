using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using HospitalDAL;

namespace HospitalConsoleApp
{
    internal class App
    {
        private static void displayAllPatients()
        {
            List<Patient> patients = PatientServices.GetAllPatientsFromDatabase();
            foreach (Patient pat in patients)
            {
                Console.WriteLine(pat);

            }
        }
        private static void displayAllDoctors()
        {
            List<Doctor> doctors = DoctorServices.GetAllDoctorsFromDatabase();
            foreach (Doctor doc in doctors)
            {
                Console.WriteLine(doc);

            }
        }
        private static void displayAllAppointments()
        {
            List<Appointment> appointments = AppointmentServices.GetAllAppointmentsFromDatabase();
            foreach (Appointment app in appointments)
            {
                Console.WriteLine(app);
            }
        }
        private static void showMenu()
        {
            Console.WriteLine("----Hospital Management System Menu----");
            Console.WriteLine("1. Add a new patient");
            Console.WriteLine("2. Update a patient");
            Console.WriteLine("3. Delete a patient (and save deleted record to history)");
            Console.WriteLine("4. Search for patients by name");
            Console.WriteLine("5. View all patients");
            Console.WriteLine("6. Add a new doctor");
            Console.WriteLine("7. Update a doctor");
            Console.WriteLine("8. Delete a doctor (and save deleted record to history)");
            Console.WriteLine("9. Search for doctors by specialization");
            Console.WriteLine("10. View all doctors");
            Console.WriteLine("11. Book an appointment");
            Console.WriteLine("12. View all appointments");
            Console.WriteLine("13. Search appointments by doctor or patient");
            Console.WriteLine("14. Cancel an appointment (and save deleted appointment to history)");
            Console.WriteLine("15. View history of deleted records (patients, doctors, or appointments)");
            Console.WriteLine("16. Exit the application");
        }
        private static void ExecuteChoice(int choice)
        {
            if (choice == 1)
            {
                Patient patient = new Patient();
                Console.Clear();
                do
                {
                    Console.Write("Enter Patient ID: ");
                    patient.PatientId = int.Parse(Console.ReadLine());
                } while (patient.PatientId <= 0 || PatientServices.isPatientIdPresent(patient.PatientId));

                Console.Write("Enter Patient Name: ");
                patient.Name = Console.ReadLine();

                do
                {
                    Console.Write("Enter Patient Email: ");
                    patient.Email = Console.ReadLine();
                } while (!(patient.Email.EndsWith("@gmail.com")));

                Console.Write("Enter Patient Disease: ");
                patient.Disease = Console.ReadLine();

                if (PatientServices.InsertPatient(patient))
                {
                    Console.WriteLine("Patient Added Successfully");
                }
                else
                {
                    Console.WriteLine("Failed!!!");
                }
                Console.WriteLine("Press Any Key to Continue");
                Console.ReadKey();
            }
            else if (choice == 2)
            {
                Console.Clear();
                displayAllPatients();
                Patient patient = new Patient();
                do
                {
                    Console.Write("Enter Patient ID: ");
                    patient.PatientId = int.Parse(Console.ReadLine());
                } while (patient.PatientId <= 0 || !PatientServices.isPatientIdPresent(patient.PatientId));

                Console.Write("Enter Patient Name: ");
                patient.Name = Console.ReadLine();

                do
                {
                    Console.Write("Enter Patient Email: ");
                    patient.Email = Console.ReadLine();
                } while (!(patient.Email.EndsWith("@gmail.com")));

                Console.Write("Enter Patient Disease: ");
                patient.Disease = Console.ReadLine();

                if (PatientServices.UpdatePatientInDatabase(patient))
                {
                    Console.WriteLine("Patient Updated Successfully");
                }
                else
                {
                    Console.WriteLine("Failed!!!");
                }
                Console.WriteLine("Press Any Key to Continue");
                Console.ReadKey();
            }
            else if (choice == 3)
            {
                Console.Clear();
                displayAllPatients();
                int patientIdToDelete;
                do
                {
                    Console.Write("Enter Patient ID: ");
                    patientIdToDelete = int.Parse(Console.ReadLine());
                } while (patientIdToDelete <= 0 || !PatientServices.isPatientIdPresent(patientIdToDelete));

                if (PatientServices.DeletePatientFromDatabase(patientIdToDelete))
                {
                    Console.WriteLine("Patient Deleted Successfully");
                }
                else
                {
                    Console.WriteLine("Failed!!!");
                }
                Console.WriteLine("Press Any Key to Continue");
                Console.ReadKey();
            }
            else if (choice == 4)
            {
                Console.Clear();
                Console.Write("Enter Patient Name to search: ");
                string patientNameToSearch = Console.ReadLine();

                List<Patient> foundPatients = PatientServices.SearchPatientsInDatabase(patientNameToSearch);

                Console.WriteLine("Search Results:");
                foreach (Patient patient in foundPatients)
                {
                    Console.WriteLine(patient.ToString());
                }
                Console.WriteLine("Press Any Key to Continue");
                Console.ReadKey();
            }
            else if (choice == 5)
            {
                Console.Clear();
                List<Patient> allPatients = PatientServices.GetAllPatientsFromDatabase();
                Console.WriteLine("All Patients:");
                foreach (Patient patient in allPatients)
                {
                    Console.WriteLine(patient);
                }
                Console.WriteLine("Press Any Key to Continue");
                Console.ReadKey();
            }
            else if (choice == 6)
            {
                Console.Clear();
                Doctor doctorToAdd = new Doctor();
                do
                {
                    Console.Write("Enter Doctor ID: ");
                    doctorToAdd.DoctorId = int.Parse(Console.ReadLine());
                } while (doctorToAdd.DoctorId <= 0 || DoctorServices.isDoctorIdPresent(doctorToAdd.DoctorId));

                Console.Write("Enter Doctor Name: ");
                doctorToAdd.Name = Console.ReadLine();

                Console.Write("Enter Doctor Specialization: ");
                doctorToAdd.Specialization = Console.ReadLine();

                if (DoctorServices.InsertDoctor(doctorToAdd))
                {
                    Console.WriteLine("Doctor Added Successfully");
                }
                else
                {
                    Console.WriteLine("Failed!!!");
                }
                Console.WriteLine("Press Any Key to Continue");
                Console.ReadKey();
            }
            else if (choice == 7)
            {
                Console.Clear();
                displayAllDoctors();
                Doctor doctorToUpdate = new Doctor();
                do
                {
                    Console.Write("Enter Doctor ID: ");
                    doctorToUpdate.DoctorId = int.Parse(Console.ReadLine());
                } while (doctorToUpdate.DoctorId <= 0 || !DoctorServices.isDoctorIdPresent(doctorToUpdate.DoctorId));

                Console.Write("Enter new Doctor Name: ");
                doctorToUpdate.Name = Console.ReadLine();

                Console.Write("Enter new Doctor Specialization: ");
                doctorToUpdate.Specialization = Console.ReadLine();

                if (DoctorServices.UpdateDoctorInDatabase(doctorToUpdate))
                {
                    Console.WriteLine("Doctor Updated Successfully");
                }
                else
                {
                    Console.WriteLine("Failed!!!");
                }
                Console.WriteLine("Press Any Key to Continue");
                Console.ReadKey();
            }
            else if (choice == 8)
            {
                Console.Clear();
                displayAllDoctors();
                int doctorIdToDelete;
                do
                {
                    Console.Write("Enter Doctor ID: ");
                    doctorIdToDelete = int.Parse(Console.ReadLine());
                } while (doctorIdToDelete <= 0 || !DoctorServices.isDoctorIdPresent(doctorIdToDelete));

                if (DoctorServices.DeleteDoctorFromDatabase(doctorIdToDelete))
                {
                    Console.WriteLine("Doctor Deleted Successfully");
                }
                else
                {
                    Console.WriteLine("Failed!!!");
                }
                Console.WriteLine("Press Any Key to Continue");
                Console.ReadKey();
            }
            else if (choice == 9)
            {
                Console.Clear();
                Console.Write("Enter Specialization to search: ");
                string specializationToSearch = Console.ReadLine();
                List<Doctor> foundDoctors = DoctorServices.SearchDoctorsInDatabase(specializationToSearch);
                Console.WriteLine("Search Results:");
                foreach (Doctor doctor in foundDoctors)
                {
                    Console.WriteLine(doctor.ToString());
                }
                Console.WriteLine("Press Any Key to Continue");
                Console.ReadKey();
            }
            else if (choice == 10)
            {
                Console.Clear();
                List<Doctor> allDoctors = DoctorServices.GetAllDoctorsFromDatabase();
                Console.WriteLine("All Doctors:");
                foreach (Doctor doctor in allDoctors)
                {
                    Console.WriteLine(doctor.ToString());
                }
                Console.WriteLine("Press Any Key to Continue");
                Console.ReadKey();
            }
            else if (choice == 11)
            {
                Console.Clear();
                Appointment appointment = new Appointment();

                do
                {
                    Console.Write("Enter Appointment ID: ");
                    appointment.AppointmentId = int.Parse(Console.ReadLine());
                } while (appointment.AppointmentId <= 0 || AppointmentServices.isAppointmentIdPresent(appointment.AppointmentId));
                do
                {
                    Console.Write("Enter Appointment Date (yyyy-mm-dd): ");
                    appointment.AppointmentDate = DateTime.Parse(Console.ReadLine());
                } while (appointment.AppointmentDate < DateTime.Now.Date);

                do
                {
                    Console.Write("Enter Patient ID: ");
                    appointment.PatientId = int.Parse(Console.ReadLine());
                } while (appointment.PatientId <= 0 || !(AppointmentServices.isPatientIdPresent(appointment.PatientId)));

                do
                {
                    Console.Write("Enter Doctor ID: ");
                    appointment.DoctorId = int.Parse(Console.ReadLine());

                } while (appointment.DoctorId <= 0 || !(AppointmentServices.isDoctorAvailable(appointment.DoctorId, appointment.AppointmentDate)));



                if (AppointmentServices.InsertAppointment(appointment))
                {
                    Console.WriteLine("Appointment Added Successfully");
                }
                else
                {
                    Console.WriteLine("Failed!!!");
                }
                Console.WriteLine("Press Any Key to Continue");
                Console.ReadKey();
            }
            else if (choice == 12)
            {
                Console.Clear();
                List<Appointment> allAppointments = AppointmentServices.GetAllAppointmentsFromDatabase();
                Console.WriteLine("All Appointments:");
                foreach (Appointment appointment in allAppointments)
                {
                    Console.WriteLine(appointment.ToString());
                }
                Console.WriteLine("Press Any Key to Continue");
                Console.ReadKey();
            }
            else if (choice == 13)
            {
                Console.Clear();
                int doctorIdForSearch, patientIdForSearch;
                do
                {
                    Console.Write("Enter Patient ID: ");
                    patientIdForSearch = int.Parse(Console.ReadLine());
                } while (patientIdForSearch <= 0 || !PatientServices.isPatientIdPresent(patientIdForSearch));

                do
                {
                    Console.Write("Enter Doctor ID: ");
                    doctorIdForSearch = int.Parse(Console.ReadLine());
                } while (doctorIdForSearch <= 0 || !DoctorServices.isDoctorIdPresent(doctorIdForSearch));

                List<Appointment> foundAppointments = AppointmentServices.SearchAppointmentsInDatabase(doctorIdForSearch, patientIdForSearch);

                Console.WriteLine("Search Results:");
                foreach (var appointment in foundAppointments)
                {
                    Console.WriteLine(appointment.ToString());
                }
                Console.WriteLine("Press Any Key to Continue");
                Console.ReadKey();
            }
            else if (choice == 14)
            {
                Console.Clear();
                displayAllAppointments();
                int appointmentIdToCancel;
                do
                {
                    Console.Write("Enter Appointment ID: ");
                    appointmentIdToCancel = int.Parse(Console.ReadLine());
                } while (appointmentIdToCancel <= 0 || !AppointmentServices.isAppointmentIdPresent(appointmentIdToCancel));

                if (AppointmentServices.DeleteAppointmentFromDatabase(appointmentIdToCancel))
                {
                    Console.WriteLine("Appointment Deleted Successfully");
                }
                else
                {
                    Console.WriteLine("Failed!!!");
                }
                Console.WriteLine("Press Any Key to Continue");
                Console.ReadKey();
            }
            else
            {
                Console.Clear();
                StreamReader streamReader = null;
                Console.WriteLine("Deleted Appointments:");
                try
                {
                    streamReader = new StreamReader("DeletedAppointments.txt");
                    string data = streamReader.ReadToEnd();
                    Console.WriteLine(data);
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

                Console.WriteLine("Deleted Patients:");
                try
                {
                    streamReader = new StreamReader("DeletedPatients.txt");
                    string data = streamReader.ReadToEnd();
                    Console.WriteLine(data);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
                finally
                {
                    streamReader.Close();
                }

                Console.WriteLine("Deleted Doctors:");
                try
                {
                    streamReader = new StreamReader("DeletedDoctors.txt");
                    string data = streamReader.ReadToEnd();
                    Console.WriteLine(data);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
                finally
                {
                    streamReader.Close();
                }
                Console.WriteLine("Press Any Key to Continue");
                Console.ReadKey();
            }
        }
        public static void startAPP()
        {
            int choice = 1;
            while (choice != 16)
            {
                Console.Clear();
                showMenu();
                Console.Write("Enter Choice: ");
                choice = int.Parse(Console.ReadLine());
                if (choice == 16)
                {
                    Console.WriteLine("Exiting!!!!");
                }
                else if (choice > 0 && choice < 16)
                {
                    ExecuteChoice(choice);
                }
                else
                {
                    Console.WriteLine("Wrong Input!!!");
                    Console.WriteLine("Press Any Key to Continue");
                    Console.ReadKey();
                }

            }
        }
    }
}
