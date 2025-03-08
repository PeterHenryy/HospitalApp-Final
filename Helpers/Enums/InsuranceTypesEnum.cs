using System;
using System.Collections.Generic;
using System.Linq;

namespace HospitalApp.Helpers.Enums
{
    public static class InsuranceTypesEnum
    {
        public static string Insurance1 = "(HMO)";
        public static string Insurance2 = "(PPO)";
        public static string Insurance3 = "(EPO)";
        public static string Insurance4 = "(POS)";
        public static string Insurance5 = "(HDHP) & (HSA)";
        public static string Insurance6 = "Catastrophic Health Insurance";
        public static string Insurance7 = "Medicare";
        public static string Insurance8 = "Medicaid";
        public static string Insurance9 = "(GOW)";
        public static string Insurance10 = "(GOW) & (DMI)";
        public static List<int> IdsForInsurances = new List<int>()
        {
            1,2,3,4,5,6,7,8,9,10
        };
        public static Dictionary<string, string> InsuranceCoverage = new Dictionary<string, string>
        {
            { "(HMO)", "10" },
            { "(PPO)", "20" },
            { "(EPO)", "30" },
            { "(POS)", "40" },
            { "(HDHP) & (HSA)", "50" },
            { "Catastrophic Health Insurance", "60" },
            { "Medicare", "70" },
            { "Medicaid", "80" },
            { "(GOW)", "90" },
            { "(GOW) & (DMI)", "100" },
        };
        public static decimal CalculatePercentage(decimal total, decimal percentage)
        {
            var amountOff = (total * percentage) / 100;
            return total - amountOff;
        }
        public static string GetInsuranceNameById(int Insuranceid)
        {
            if (Insuranceid == 0)
            {
                return "";
            }
            var name = InsuranceCoverage.ElementAt(Insuranceid - 1).Key.ToString();
            return name;
        }
    }
}
