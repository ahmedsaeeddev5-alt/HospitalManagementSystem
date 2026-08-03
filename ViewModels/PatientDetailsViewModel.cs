namespace HospitalManagementSystem.ViewModels
{
    public class PatientDetailsViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; }
        public string Phone { get; set; } = string.Empty;
        public string? MedicalHistory { get; set; }

        public int TotalAppointments { get; set; }
        public int TotalBills { get; set; }
    }
}