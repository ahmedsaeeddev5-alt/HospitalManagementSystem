namespace HospitalManagementSystem.ViewModels
{
    public class AppointmentDetailsViewModel
    {
        public int Id { get; set; }

        public string PatientName { get; set; } = string.Empty;
        public string DoctorName { get; set; } = string.Empty;

        public DateTime Date { get; set; }
        public string Status { get; set; } = "Pending";

        public string? Notes { get; set; }
    }
}