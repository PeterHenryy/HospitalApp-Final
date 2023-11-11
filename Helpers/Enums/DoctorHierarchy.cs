using System.Collections.Generic;

namespace HospitalApp.Helpers.Enums
{
    public static class DoctorHierarchy
    {
       
        public static Dictionary<string, string> DoctorsHierarchy = new Dictionary<string, string>
        {
            { "Specialist", "1" },
            { "Intermediate", "2" },
            { "Average", "3" }
        };
        public static Dictionary<int, string> DislpayDictionary = new Dictionary<int, string>
        {
            { 1, "Specialist" },
            { 2, "Intermediate" },
            { 3, "Average" }
        };
    }
}
