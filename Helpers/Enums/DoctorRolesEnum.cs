using System.Collections.Generic;

namespace HospitalApp.Helpers.Enums
{
    public static class DoctorRolesEnum
    {
        public static string Default = "Default";
        public static string Cardiovascular = "Cardiovascular";
        public static string Pulmonary = "Pulmonary";
        public static string Orthopedics = "Orthopedics";
        public static string Geriatrics = "Geriatrics";
        public static string Neurology = "Neurology";
        public static string Oncology = "Oncology";
        public static string Pediatrics = "Pediatrics";
        public static string Sports = "Sports";
        public static string Clinical_Cardiac_Electrophysiology = "Clinical_Cardiac_Electrophysiology";
        public static string Pediatric_Physical_Therapy = "Pediatric_Physical_Therapy";
        public static string Wound_Management = "Wound_Management";
        public static string Vestibular = "Vestibular";
        public static string Occupational_Therapy = "Occupational_Therapy";

		public static Dictionary<string, List<string>> RolesKeywords = new Dictionary<string, List<string>>
	    {
		    { DoctorRolesEnum.Cardiovascular, new List<string> { "Heart Health", "Cardiac Diseases", "Vascular System", "Blood Circulation", "Cardiovascular Surgery", "Cardiology", "Heart Conditions" } },
		    { DoctorRolesEnum.Pulmonary, new List<string> { "Respiratory", "Lung Diseases", "Pulmonary Function", "Breathing Disorders", "Respiratory Therapy", "Pulmonology", "Asthma" } },
		    { DoctorRolesEnum.Orthopedics, new List<string> { "Musculoskeletal System", "Orthopedic Surgery", "Bone Health", "Joint Disorders", "Orthopedic Injuries", "Sports Medicine", "Arthritis" } },
		    { DoctorRolesEnum.Geriatrics, new List<string> { "Aging Population", "Elderly Care", "Geriatric Medicine", "Age-Related Diseases", "Senior Health", "Dementia", "Long-Term Care" } },
		    { DoctorRolesEnum.Neurology, new List<string> { "Nervous System", "Neurological Disorders", "Brain Health", "Neurology Clinic", "Stroke", "Neurological Examinations", "Epilepsy" } },
		    { DoctorRolesEnum.Oncology, new List<string> { "Cancer Treatment", "Oncology Clinic", "Tumor Research", "Cancer Screening", "Chemotherapy", "Radiation Therapy", "Cancer Immunotherapy" } },
		    { DoctorRolesEnum.Pediatrics, new List<string> { "Child Health", "Pediatric Care", "Childhood Diseases", "Pediatric Immunization", "Developmental Pediatrics", "Pediatric Surgery", "Pediatric Dentistry" } },
		    { DoctorRolesEnum.Sports, new List<string> { "Sports Medicine", "Athletic Injuries", "Exercise Physiology", "Sports Rehabilitation", "Sports Nutrition", "Physical Therapy for Athletes", "Performance Enhancement" } },
		    { DoctorRolesEnum.Clinical_Cardiac_Electrophysiology, new List<string> { "Cardiac Electrophysiology", "Heart Rhythm Disorders", "Electrophysiology Studies", "Arrhythmia Treatment", "Ablation Procedures", "Cardiac Monitoring", "Implantable Devices" } },
		    { DoctorRolesEnum.Pediatric_Physical_Therapy, new List<string> { "Pediatric Physical Therapy", "Child Rehabilitation", "Motor Skills Development", "Pediatric Mobility", "Therapeutic Exercises for Children", "Pediatric Orthopedic Rehabilitation", "Developmental Delay Intervention" } },
		    { DoctorRolesEnum.Wound_Management, new List<string> { "Wound Care", "Chronic Wounds", "Wound Healing", "Advanced Wound Dressings", "Surgical Wound Management", "Infection Control in Wound Care", "Negative Pressure Wound Therapy" } },
		    { DoctorRolesEnum.Vestibular, new List<string> { "Vestibular System", "Balance Disorders", "Vestibular Rehabilitation", "Vertigo Treatment", "Dizziness Evaluation", "Inner Ear Disorders", "Vestibular Testing" } },
		    { DoctorRolesEnum.Occupational_Therapy, new List<string> { "Occupational Therapy", "Functional Rehabilitation", "Activities of Daily Living (ADL)", "Hand Therapy", "Assistive Devices", "Sensory Integration", "Work Rehabilitation" } },
        };
	}
}
