    using System;
    using System.Collections.Generic;
    using System.Linq;
    //using System.Runtime.Remoting.Lifetime;
    using System.Text;
    using System.Threading.Tasks;
    //using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

    namespace HospitalDAL
{
        public class Doctor
        {
            private int doctorId;
            public int DoctorId
            { 
                get { return doctorId; }
                set
                {
                    if (value > 0 && value != doctorId)
                    {
                        doctorId = value;
                    }
                }
            }
            private string name;
            public string Name 
            { 
                get { return name; }
                set
                {
                    if (value != null)
                    {
                        name = value;
                    }
                }
            }
            private string specialization;
            public string Specialization
            {
                get { return specialization; }
                set 
                {
                    if (value != null)
                    {
                        specialization = value;
                    }
                }
            }
            public override string ToString()
            {
                return $"DoctorId: {DoctorId}, Name: {Name}, Specialization: {Specialization}";
            }
        }
    }