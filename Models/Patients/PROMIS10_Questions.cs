using System.Collections.Generic;

namespace HospitalApp.Models.Patients
{
    public static class PROMIS10_Questions
    {
        public static string Question1 = "In the past 7 days, how often did you feel physically or emotionally exhausted?";
        public static string Question2 = "In the past 7 days, how often did you experience difficulty in concentrating or paying attention?";
        public static string Question3 = "In the past 7 days, how often did you feel worried or anxious?";
        public static string Question4 = "In the past 7 days, how often did you experience pain?";
        public static string Question5 = "In the past 7 days, how often did you feel down, depressed, or hopeless?";
        public static string Question6 = "In the past 7 days, how often did you experience feelings of anger or irritability?";
        public static string Question7 = "In the past 7 days, how often did you experience difficulty falling asleep or staying asleep?";
        
        public static List<string> QuestionsList = new List<string>
        {
            "In the past 7 days, how often did you feel physically or emotionally exhausted?",
            "In the past 7 days, how often did you experience difficulty in concentrating or paying attention?",
            "In the past 7 days, how often did you feel worried or anxious?",
            "In the past 7 days, how often did you experience pain?",
            "In the past 7 days, how often did you feel down, depressed, or hopeless?",
            "In the past 7 days, how often did you experience feelings of anger or irritability?",
            "In the past 7 days, how often did you experience difficulty falling asleep or staying asleep?"
        };
    }
}
