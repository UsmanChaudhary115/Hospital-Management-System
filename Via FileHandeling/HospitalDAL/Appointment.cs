using System;
using System.Collections.Generic;
using System.Linq;
//using System.Runtime.Remoting.Lifetime;
using System.Text;
using System.Threading.Tasks;
//using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace HospitalDAL
{
    public class Appointment
    {
        private int appointmentId;
        public int AppointmentId
        { 
            get { return appointmentId; } 
            set 
            {
                if (value > 0) 
                {
                    appointmentId = value;
                }
            }
        }

        private int patientId;
        public int PatientId
        {
            get { return patientId; }
            set
            {
                if (value > 0)
                {
                    patientId = value;
                }
            }
        }

        private int doctorId;
        public int DoctorId
        {
            get { return doctorId; }
            set
            {
                if (value > 0)
                {
                    doctorId = value;
                }
            }
        }
        DateTime appointmentDate; 
        public DateTime AppointmentDate
        {
            get { return appointmentDate; }
            set
            {
                appointmentDate = value;
            }

        }
        public override string ToString()
        {
            return $"AppointmentId: {AppointmentId}, PatientId: {PatientId}, DoctorId: {DoctorId}, AppointmentDate: {AppointmentDate.ToString("yyyy-MM-dd")}";
        }
    }
}
