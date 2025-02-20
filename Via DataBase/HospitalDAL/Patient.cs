using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;
//using Microsoft.Data.SqlClient;
using System.Data.SqlClient;

namespace HospitalDAL
{
    public class Patient
    {
        private int patientId;
        public int PatientId
        { 
            get { return patientId; } 
            set 
            {
                if (value > 0)
                {
                    { patientId = value; }
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
        private string email;
        public string Email
        {
            get { return email; }
            set 
            {
                if (value != null && value.Contains("@"))
                {
                    email = value;
                }
            }
        }
        private string disease;
        public string Disease
        {
            get { return disease; }
            set
            {
                if (value != null)
                {
                    disease = value;
                }
            }
        }
        public override string ToString()
        {
            return $"PatientId: {PatientId}, Name: {Name}, Email: {Email}, Disease: {Disease}";
        }
    }
}
